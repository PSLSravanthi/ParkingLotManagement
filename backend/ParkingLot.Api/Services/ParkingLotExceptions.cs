namespace ParkingLot.Api.Services;

public class ParkingLotFullException : Exception
{
    public ParkingLotFullException(string message) : base(message) { }
}

public class TicketNotFoundException : Exception
{
    public TicketNotFoundException(int ticketId) : base($"Ticket {ticketId} was not found.") { }
}

public class TicketAlreadyClosedException : Exception
{
    public TicketAlreadyClosedException(int ticketId) : base($"Ticket {ticketId} has already been closed.") { }
}
