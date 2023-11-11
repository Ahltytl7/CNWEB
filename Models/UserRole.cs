using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class UserRole
{
    public string? UserId { get; set; }

    public string? RolesId { get; set; }

    public virtual AppRole? Roles { get; set; }

    public virtual User? User { get; set; }
}
