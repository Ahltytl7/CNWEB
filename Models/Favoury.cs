using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Favoury
{
    public string Id { get; set; } = null!;

    public string? UserId { get; set; }

    public string? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Customer? User { get; set; }
}
