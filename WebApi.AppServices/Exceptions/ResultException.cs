namespace WebApi.AppServices.Exceptions;

public class ResultException : Exception
{
    public ResultException(string message) : base(message){}
}
