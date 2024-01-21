namespace WebApi.AppServices.Contracts.Models.Request;

public class UpdateUserInfo
{
    public string? Fio { get; set; }
    
    public string? Username { get; set; }
    
    public string? NewPassword { get; set; }
    
    public string? OldPassword { get; set; }
}
