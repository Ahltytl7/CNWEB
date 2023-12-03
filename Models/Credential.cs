using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Credential
{
    public string Id { get; set; } = null!;

    public string? GroupId { get; set; }

    public string? RoleId { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Role? Role { get; set; }
}
