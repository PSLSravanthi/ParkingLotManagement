namespace ParkingLot.Api.Models;

public class Ticket
{
    public int Id { get; init; }
    public string VehicleNumber { get; init; } = string.Empty;
    public VehicleType VehicleType { get; init; }
    public int SlotId { get; init; }
    public DateTimeOffset EntryTime { get; init; }
    public DateTimeOffset? ExitTime { get; set; }
    public decimal? Fee { get; set; }

    public bool IsActive => ExitTime is null;
}
