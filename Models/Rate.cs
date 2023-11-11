using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Rate
{
    public string Id { get; set; } = null!;

    public string? Comment { get; set; }

    public DateTime? RateDate { get; set; }

    public double? Rating { get; set; }

    public string? OrdersDetailsId { get; set; }

    public string? ProductId { get; set; }

    public string? UserId { get; set; }

    public virtual OrderDetail? OrdersDetails { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
