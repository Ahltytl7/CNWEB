﻿using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class User
{
    private readonly WebContext _context;
    /* private readonly IHostEnvironment _hostEnvironment;*/

    public User(WebContext context)
    {
        _context = context;
        /* _webHostEnvironment = hostEnvironment; */
    }
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Email { get; set; } = null!;

    public bool? Gender { get; set; }

    public string? Image { get; set; }

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? Status { get; set; }

    public string? Token { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Favoury> Favouries { get; set; } = new List<Favoury>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
    public string GetUserName(string userId)
    {
        
        return _context.Users.FirstOrDefault(c => c.Id == userId)?.Name;
    }
    public string GetUserEmail(string userId)
    {

        return _context.Users.FirstOrDefault(c => c.Id == userId)?.Email;
    }
}
