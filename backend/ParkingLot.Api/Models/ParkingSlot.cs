namespace ParkingLot.Api.Models;

public class ParkingSlot
{
    public int Id { get; init; }
    public SlotSize Size { get; init; }
    public bool IsOccupied { get; set; }
    public int? VehicleTicketId { get; set; }
}
