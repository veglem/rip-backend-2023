using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Models;

public class UnivesityUnit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public int? ParrentUnit { get; set; }

    public string? Description { get; set; }

    public ICollection<UnivesityUnit> InverseParrentUnitNavigation { get; set; } = new List<UnivesityUnit>();

    public UnivesityUnit? ParrentUnitNavigation { get; set; }

    public ICollection<Request> Requests { get; set; } = new List<Request>();

    public ICollection<UniversityEmployee> UniversityEmployees { get; set; } = new List<UniversityEmployee>();
}
