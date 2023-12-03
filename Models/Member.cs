using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Member
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? LoginName { get; set; }

    public string? Password { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? Status { get; set; }

    public string? GroupId { get; set; }

    public string? Image { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual Group? Group { get; set; }
}
