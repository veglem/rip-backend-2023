namespace WebApi.AppServices.Contracts.Models.Responce;

public class GetUnitResult
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public int Status { get; set; }

    // public int? ParrentUnit { get; set; }
    
    public string? Description { get; set; }

    // public ICollection<GetUnitResult> InverseParrentUnitNavigation { get; set; } = new List<GetUnitResult>();
}
