using System.Collections;
using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Models.Responce;

public class GetOrderResult
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string OrderBody { get; set; } = null!;

    public DateTime? CreationDate { get; set; }

    public DateTime? FormationDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Creator { get; set; } = null!;

    public string? Moderator { get; set; }

    public ICollection<GetUnitResult> Units { get; set; } =
        new List<GetUnitResult>();

    public int Status { get; set; }
}
