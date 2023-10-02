using System;
using System.Collections.Generic;

namespace RIP_lab01;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Passord { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<Moderator> Moderators { get; set; } = new List<Moderator>();
}
