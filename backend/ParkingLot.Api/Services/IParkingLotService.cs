using ParkingLot.Api.Models;

namespace ParkingLot.Api.Services;

public interface IParkingLotService
{
    IReadOnlyList<ParkingSlot> GetSlots();

    IReadOnlyList<SlotAvailability> GetAvailability();

    IReadOnlyList<Ticket> GetActiveTickets();

    Ticket ParkVehicle(ParkVehicleRequest request);

    Ticket ExitVehicle(int ticketId);
}
