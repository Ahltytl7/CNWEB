using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Category
{
    private readonly WebContext _context;
    /* private readonly IHostEnvironment _hostEnvironment;*/

    public Category(WebContext context)
    {
        _context = context;
        /* _webHostEnvironment = hostEnvironment; */
    }
    public string Id { get; set; } = null!;

    public string Names { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public string GetCategoryName(string categoryId)
    {
        // Lấy tên danh mục dựa trên categoryId từ cơ sở dữ liệu hoặc các nguồn dữ liệu khác.
        // Ví dụ:
        return _context.Categories.FirstOrDefault(c => c.Id == categoryId)?.Names;
    }
}
