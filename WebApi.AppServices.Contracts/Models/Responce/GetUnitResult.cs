namespace WebApi.AppServices.Models;

public class GetUnitResult
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int? ParrentUnit { get; set; }

    public ICollection<GetUnitResult> InverseParrentUnitNavigation { get; set; } = new List<GetUnitResult>();

    public GetUnitResult? ParrentUnitNavigation { get; set; }
}
