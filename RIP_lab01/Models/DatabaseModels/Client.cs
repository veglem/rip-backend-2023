using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class Client
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
