using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class User
{
    private readonly WebContext _context;

    public User(WebContext context)
    {
        _context = context;

    }
    public User()
    {
        // Logic của hàm tạo không tham số, nếu cần
    }
    public string Id { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Image { get; set; }
    public User? TimKiem(string id)
    {
        if (_context == null)
        {
            // Handle the scenario where _context is null
            return null;
        }

        var user = _context.Users.Find(id);
        return user;
    }
}
