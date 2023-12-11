namespace WebApi.AppServices.Contracts.Models.Request;

public class NewUnit
{
    public string? Name { get; set; }

    public string? ImgUrl { get; set; }

    public bool? IsDeleted { get; set; } = false;

    public int? ParrentUnit { get; set; }

    public string? Description { get; set; }
}
