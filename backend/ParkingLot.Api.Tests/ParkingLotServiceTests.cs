using Microsoft.Extensions.Options;
using ParkingLot.Api.Models;
using ParkingLot.Api.Services;
using Xunit;

namespace ParkingLot.Api.Tests;

public class ParkingLotServiceTests
{
    private static ParkingLotService CreateService(int small = 1, int medium = 1, int large = 1)
    {
        var options = Options.Create(new ParkingLotOptions
        {
            SmallSlots = small,
            MediumSlots = medium,
            LargeSlots = large,
            HourlyRateSmall = 1.0m,
            HourlyRateMedium = 2.0m,
            HourlyRateLarge = 4.0m
        });

        return new ParkingLotService(options);
    }

    [Fact]
    public void ParkVehicle_AssignsSmallestFittingSlot()
    {
        var service = CreateService();

        var ticket = service.ParkVehicle(new ParkVehicleRequest { VehicleNumber = "ab-123", VehicleType = VehicleType.Car });

        var slot = service.GetSlots().Single(s => s.Id == ticket.SlotId);
        Assert.Equal(SlotSize.Medium, slot.Size);
        Assert.True(slot.IsOccupied);
        Assert.Equal("AB-123", ticket.VehicleNumber);
    }

    [Fact]
    public void ParkVehicle_FallsBackToLargerSlot_WhenPreferredSizeIsFull()
    {
        var service = CreateService(small: 0, medium: 0, large: 1);

        var ticket = service.ParkVehicle(new ParkVehicleRequest { VehicleNumber = "M-1", VehicleType = VehicleType.Motorcycle });

        var slot = service.GetSlots().Single(s => s.Id == ticket.SlotId);
        Assert.Equal(SlotSize.Large, slot.Size);
    }

    [Fact]
    public void ParkVehicle_ThrowsWhenNoSlotFitsVehicle()
    {
        var service = CreateService(small: 1, medium: 0, large: 0);

        Assert.Throws<ParkingLotFullException>(() =>
            service.ParkVehicle(new ParkVehicleRequest { VehicleNumber = "C-1", VehicleType = VehicleType.Car }));
    }

    [Fact]
    public void ExitVehicle_FreesSlotAndCalculatesFee()
    {
        var service = CreateService();
        var ticket = service.ParkVehicle(new ParkVehicleRequest { VehicleNumber = "C-1", VehicleType = VehicleType.Car });

        var closedTicket = service.ExitVehicle(ticket.Id);

        Assert.NotNull(closedTicket.ExitTime);
        Assert.NotNull(closedTicket.Fee);
        Assert.True(closedTicket.Fee >= 2.0m);
        Assert.False(service.GetSlots().Single(s => s.Id == ticket.SlotId).IsOccupied);
    }

    [Fact]
    public void ExitVehicle_ThrowsForUnknownTicket()
    {
        var service = CreateService();

        Assert.Throws<TicketNotFoundException>(() => service.ExitVehicle(999));
    }

    [Fact]
    public void ExitVehicle_ThrowsWhenTicketAlreadyClosed()
    {
        var service = CreateService();
        var ticket = service.ParkVehicle(new ParkVehicleRequest { VehicleNumber = "C-1", VehicleType = VehicleType.Car });
        service.ExitVehicle(ticket.Id);

        Assert.Throws<TicketAlreadyClosedException>(() => service.ExitVehicle(ticket.Id));
    }

    [Fact]
    public void GetAvailability_ReflectsOccupiedSlots()
    {
        var service = CreateService(small: 2, medium: 1, large: 1);
        service.ParkVehicle(new ParkVehicleRequest { VehicleNumber = "M-1", VehicleType = VehicleType.Motorcycle });

        var availability = service.GetAvailability().Single(a => a.Size == SlotSize.Small);

        Assert.Equal(2, availability.Total);
        Assert.Equal(1, availability.Available);
    }
}
