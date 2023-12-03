using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class TransactStatus
{
    private readonly WebContext _context;
    /* private readonly IHostEnvironment _hostEnvironment;*/

    public TransactStatus(WebContext context)
    {
        _context = context;
        /* _webHostEnvironment = hostEnvironment; */
    }
    public string TransactStatusId { get; set; } = null!;

    public string? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public string GetTransName(string Id)
    {
        var transactStatus = _context.TransactStatuses.FirstOrDefault(c => c.TransactStatusId == Id);

        return transactStatus?.Status ?? "DefaultStatus"; // Sử dụng giá trị mặc định nếu là null
    }
}
