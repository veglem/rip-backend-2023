using System;
using System.Collections.Generic;

namespace WebApi.DataAccess;

public class Status
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<RectorOrder> RectorOrders { get; } = new List<RectorOrder>();
}
