namespace CoreApp.Exceptions;

public class GateNotFoundException : Exception
{
    public GateNotFoundException(string message) : base(message)
    {
    }
}