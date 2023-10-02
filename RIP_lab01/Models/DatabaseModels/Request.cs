using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class Request
{
    public int Id { get; set; }

    public int? UnitId { get; set; }

    public int? OrderId { get; set; }

    public virtual RectorOrder? Order { get; set; }

    public virtual UnivesityUnit? Unit { get; set; }
}
