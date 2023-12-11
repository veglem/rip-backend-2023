namespace WebApi.AppServices.Contracts.Services.Validators;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message){}
}
