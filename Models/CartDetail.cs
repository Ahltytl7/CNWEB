using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class CartDetail
{
    public string Id { get; set; } = null!;

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public string? CartId { get; set; }

    public string? ProductId { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Product? Product { get; set; }
}
