using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class ProductImage
{
    public string Id { get; set; } = null!;

    public string? Images { get; set; }

    public string? IdProduct { get; set; }

    public virtual Product? IdProductNavigation { get; set; }
}
