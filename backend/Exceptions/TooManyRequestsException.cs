namespace AviTrack.Api.Exceptions;

public class TooManyRequestsException : Exception
{
    public TooManyRequestsException() 
        : base("Za dużo wywołań zewnętrznego API") { }
}