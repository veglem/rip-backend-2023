namespace WebApi.AppServices.Contracts.Models.Responce;

public class GetUserInfo
{
    public string Username { get; set; } = null!;

    public bool IsModerator { get; set; }

    public string? Fio { get; set; }

    public string ImageUrl { get; set; } = null!;
}
