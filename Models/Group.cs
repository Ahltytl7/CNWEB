using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Group
{
    protected readonly WebContext _context;

    public Group(WebContext context)
    {
        _context = context;
    }
    public Group()
    {
        Credentials = new HashSet<Credential>();
        Members = new HashSet<Member>();
    }
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public virtual ICollection<Credential> Credentials { get; set; } = new List<Credential>();

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
    public int GetMemberCountForGroup(string groupId)
    {
        return _context.Members.Count(m => m.GroupId == groupId);
    }
}
