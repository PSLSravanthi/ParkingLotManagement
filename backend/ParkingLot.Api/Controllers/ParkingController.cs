using Microsoft.AspNetCore.Mvc;
using ParkingLot.Api.Models;
using ParkingLot.Api.Services;

namespace ParkingLot.Api.Controllers;

[ApiController]
[Route("api/parking")]
public class ParkingController : ControllerBase
{
    private readonly IParkingLotService _parkingLotService;

    public ParkingController(IParkingLotService parkingLotService)
    {
        _parkingLotService = parkingLotService;
    }

    [HttpGet("slots")]
    public ActionResult<IReadOnlyList<ParkingSlot>> GetSlots() => Ok(_parkingLotService.GetSlots());

    [HttpGet("availability")]
    public ActionResult<IReadOnlyList<SlotAvailability>> GetAvailability() => Ok(_parkingLotService.GetAvailability());

    [HttpGet("tickets/active")]
    public ActionResult<IReadOnlyList<Ticket>> GetActiveTickets() => Ok(_parkingLotService.GetActiveTickets());

    [HttpPost("park")]
    public ActionResult<Ticket> ParkVehicle([FromBody] ParkVehicleRequest request)
    {
        try
        {
            var ticket = _parkingLotService.ParkVehicle(request);
            return CreatedAtAction(nameof(GetActiveTickets), new { id = ticket.Id }, ticket);
        }
        catch (ParkingLotFullException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("exit/{ticketId:int}")]
    public ActionResult<Ticket> ExitVehicle(int ticketId)
    {
        try
        {
            var ticket = _parkingLotService.ExitVehicle(ticketId);
            return Ok(ticket);
        }
        catch (TicketNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (TicketAlreadyClosedException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
