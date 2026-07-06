namespace ParkingLot.Api.Models;

public class SlotAvailability
{
    public SlotSize Size { get; init; }
    public int Total { get; init; }
    public int Available { get; init; }
}
