namespace ParkingLot.Api.Models;

public class ParkVehicleRequest
{
    public string VehicleNumber { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
}
