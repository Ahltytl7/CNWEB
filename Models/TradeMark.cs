using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class TradeMark
{
    public string Id { get; set; } = null!;

    public string Names { get; set; } = null!;

    public string? Logo { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
