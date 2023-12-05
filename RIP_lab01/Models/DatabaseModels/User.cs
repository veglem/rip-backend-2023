using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Passord { get; set; } = null!;

    public bool IsModerator { get; set; }

    public virtual ICollection<RectorOrder> RectorOrders { get; set; } = new List<RectorOrder>();
}
