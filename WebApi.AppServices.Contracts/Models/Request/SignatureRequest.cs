namespace WebApi.AppServices.Contracts.Models.Request;

public class SignatureRequest
{
    public int AccessToken { get; set; }
    public string Signature { get; set; } = "";
}
