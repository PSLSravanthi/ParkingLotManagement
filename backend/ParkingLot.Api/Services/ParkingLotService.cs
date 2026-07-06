using Microsoft.Extensions.Options;
using ParkingLot.Api.Models;

namespace ParkingLot.Api.Services;

/// <summary>
/// In-memory parking lot. A vehicle is assigned the smallest available slot that
/// can fit it (a motorcycle may fall back to a medium/large slot, a car to a large
/// slot, etc.) so the lot degrades gracefully instead of rejecting vehicles outright.
/// </summary>
public class ParkingLotService : IParkingLotService
{
    private readonly object _lock = new();
    private readonly List<ParkingSlot> _slots;
    private readonly Dictionary<int, Ticket> _tickets = new();
    private readonly ParkingLotOptions _options;
    private int _nextTicketId = 1;

    private static readonly SlotSize[] SizeOrder = { SlotSize.Small, SlotSize.Medium, SlotSize.Large };

    public ParkingLotService(IOptions<ParkingLotOptions> options)
    {
        _options = options.Value;
        _slots = BuildSlots(_options);
    }

    private static List<ParkingSlot> BuildSlots(ParkingLotOptions options)
    {
        var slots = new List<ParkingSlot>();
        var id = 1;

        for (var i = 0; i < options.SmallSlots; i++)
            slots.Add(new ParkingSlot { Id = id++, Size = SlotSize.Small });
        for (var i = 0; i < options.MediumSlots; i++)
            slots.Add(new ParkingSlot { Id = id++, Size = SlotSize.Medium });
        for (var i = 0; i < options.LargeSlots; i++)
            slots.Add(new ParkingSlot { Id = id++, Size = SlotSize.Large });

        return slots;
    }

    private static SlotSize MinimumSlotSizeFor(VehicleType vehicleType) => vehicleType switch
    {
        VehicleType.Motorcycle => SlotSize.Small,
        VehicleType.Car => SlotSize.Medium,
        VehicleType.Bus => SlotSize.Large,
        _ => throw new ArgumentOutOfRangeException(nameof(vehicleType), vehicleType, null)
    };

    private decimal HourlyRateFor(SlotSize size) => size switch
    {
        SlotSize.Small => _options.HourlyRateSmall,
        SlotSize.Medium => _options.HourlyRateMedium,
        SlotSize.Large => _options.HourlyRateLarge,
        _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
    };

    public IReadOnlyList<ParkingSlot> GetSlots()
    {
        lock (_lock)
        {
            return _slots
                .Select(s => new ParkingSlot { Id = s.Id, Size = s.Size, IsOccupied = s.IsOccupied, VehicleTicketId = s.VehicleTicketId })
                .ToList();
        }
    }

    public IReadOnlyList<SlotAvailability> GetAvailability()
    {
        lock (_lock)
        {
            return SizeOrder
                .Select(size => new SlotAvailability
                {
                    Size = size,
                    Total = _slots.Count(s => s.Size == size),
                    Available = _slots.Count(s => s.Size == size && !s.IsOccupied)
                })
                .ToList();
        }
    }

    public IReadOnlyList<Ticket> GetActiveTickets()
    {
        lock (_lock)
        {
            return _tickets.Values.Where(t => t.IsActive).OrderBy(t => t.EntryTime).ToList();
        }
    }

    public Ticket ParkVehicle(ParkVehicleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.VehicleNumber))
        {
            throw new ArgumentException("Vehicle number is required.", nameof(request));
        }

        lock (_lock)
        {
            var minimumSize = MinimumSlotSizeFor(request.VehicleType);
            var slot = _slots
                .Where(s => !s.IsOccupied && s.Size >= minimumSize)
                .OrderBy(s => s.Size)
                .FirstOrDefault();

            if (slot is null)
            {
                throw new ParkingLotFullException($"No available slot for vehicle type '{request.VehicleType}'.");
            }

            var ticket = new Ticket
            {
                Id = _nextTicketId++,
                VehicleNumber = request.VehicleNumber.Trim().ToUpperInvariant(),
                VehicleType = request.VehicleType,
                SlotId = slot.Id,
                EntryTime = DateTimeOffset.UtcNow
            };

            slot.IsOccupied = true;
            slot.VehicleTicketId = ticket.Id;
            _tickets[ticket.Id] = ticket;

            return ticket;
        }
    }

    public Ticket ExitVehicle(int ticketId)
    {
        lock (_lock)
        {
            if (!_tickets.TryGetValue(ticketId, out var ticket))
            {
                throw new TicketNotFoundException(ticketId);
            }

            if (!ticket.IsActive)
            {
                throw new TicketAlreadyClosedException(ticketId);
            }

            var slot = _slots.First(s => s.Id == ticket.SlotId);
            var exitTime = DateTimeOffset.UtcNow;
            var durationHours = Math.Max(1, Math.Ceiling((exitTime - ticket.EntryTime).TotalHours));

            ticket.ExitTime = exitTime;
            ticket.Fee = (decimal)durationHours * HourlyRateFor(slot.Size);

            slot.IsOccupied = false;
            slot.VehicleTicketId = null;

            return ticket;
        }
    }
}
