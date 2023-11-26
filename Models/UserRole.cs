using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class UserRole
{
    private readonly WebContext _context;

    public UserRole(WebContext context)
    {
        _context = context;

    }
    public UserRole()
    {
        // Logic của hàm tạo không tham số, nếu cần
    }
    public string? UserId { get; set; }

    public string? RolesId { get; set; }

    public string? Note { get; set; }

    public virtual AppRole? Roles { get; set; }

    public virtual User? User { get; set; }
}
