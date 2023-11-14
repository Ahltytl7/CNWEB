using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Cart
{
    public string Id { get; set; } = null!;

    public double? Amount { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Customer? User { get; set; }
}
