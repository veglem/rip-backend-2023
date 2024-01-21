using WebApi.DataAccess;

namespace WebApi.AppServices.Contracts.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Passord { get; set; } = null!;

    public bool IsModerator { get; set; }

    public string? Fio { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ICollection<RectorOrder> RectorOrderCreators { get; } = new List<RectorOrder>();

    public virtual ICollection<RectorOrder> RectorOrderModerators { get; } = new List<RectorOrder>();
}
