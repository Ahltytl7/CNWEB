using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class OrderDetail
{
    public string Id { get; set; } = null!;

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public string? OrderId { get; set; }

    public string? ProductId { get; set; }

    public double? Total { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
}
