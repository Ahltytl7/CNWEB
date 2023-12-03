using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Role
{
    public Role()
    {
        Credentials = new HashSet<Credential>();
    }
    public string Id { get; set; } = null!;

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();
}
