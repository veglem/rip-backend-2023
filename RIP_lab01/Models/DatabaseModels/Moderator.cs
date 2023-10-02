using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class Moderator
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<RectorOrder> RectorOrders { get; set; } = new List<RectorOrder>();

    public virtual User? User { get; set; }
}
