using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Order
{
    public string Id { get; set; } = null!;

    public string? Address { get; set; }

    public double? Amount { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? Phone { get; set; }

    public string? TransactStatusId { get; set; }

    public string? UserId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual TransactStatus? TransactStatus { get; set; }

    public virtual Customer? User { get; set; }
}
