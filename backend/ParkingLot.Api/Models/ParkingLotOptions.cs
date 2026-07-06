namespace ParkingLot.Api.Models;

public class ParkingLotOptions
{
    public const string SectionName = "ParkingLot";

    public int SmallSlots { get; set; } = 10;
    public int MediumSlots { get; set; } = 15;
    public int LargeSlots { get; set; } = 5;

    public decimal HourlyRateSmall { get; set; } = 1.0m;
    public decimal HourlyRateMedium { get; set; } = 2.0m;
    public decimal HourlyRateLarge { get; set; } = 4.0m;
}
