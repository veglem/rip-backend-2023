using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Models;

public class UnivesityUnit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public bool IsDeleted { get; set; }

    // public int? ParrentUnit { get; set; }

    public string? Description { get; set; }

    // public virtual ICollection<UnivesityUnit> InverseParrentUnitNavigation { get; set; } = new List<UnivesityUnit>();

    // public virtual UnivesityUnit? ParrentUnitNavigation { get; set; }

    public virtual ICollection<DataAccess.Request> Requests { get; set; } = new List<DataAccess.Request>();

    public virtual ICollection<UniversityEmployee> UniversityEmployees { get; set; } = new List<UniversityEmployee>();
}
