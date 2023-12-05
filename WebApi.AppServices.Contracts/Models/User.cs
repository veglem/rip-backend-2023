using System;
using System.Collections.Generic;

namespace WebApi.DataAccess;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Passord { get; set; } = null!;

    public bool IsModerator { get; set; }

    public ICollection<RectorOrder> RectorOrderCreators { get; } = new List<RectorOrder>();

    public ICollection<RectorOrder> RectorOrderModerators { get; } = new List<RectorOrder>();
}
