using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class Status
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<RectorOrder> RectorOrders { get; set; } = new List<RectorOrder>();
}
