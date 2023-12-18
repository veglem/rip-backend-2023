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

    public User Creator { get; set; } = null!;

    public User? Moderator { get; set; }

    public ICollection<int> Units { get; set; } =
        new List<int>();

    public string Status { get; set; } = null!;
}
