using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CNWEB.Models;

public partial class Product
{
    private readonly WebContext _context;

    public Product(WebContext context)
    {
        _context = context;
    }
    public Product()
    {
    }
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Discount { get; set; }

    public double? Price { get; set; }

    public DateTime? EnteredDate { get; set; }

    public int? Quantity { get; set; }

    public int? Sold { get; set; }

    public string? IdTradeMark { get; set; }

    public string? IdCategories { get; set; }

    public bool? Status { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual ICollection<Favoury> Favouries { get; set; } = new List<Favoury>();

    public virtual Category? IdCategoriesNavigation { get; set; }

    public virtual TradeMark? IdTradeMarkNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
    public string GetCategory(string id)
    {
        var categoryName = _context.Products
            .Where(p => p.Id == id)
            .Join(_context.Categories,
                product => product.IdCategories,
                category => category.Id,
                (product, category) => category.Names)
            .FirstOrDefault();

        return categoryName;
    }
    public string GetTradeMark(string id)
    {
        var categoryName = _context.Products
            .Where(p => p.Id == id)
            .Join(_context.TradeMarks,
                product => product.IdTradeMark,
                category => category.Id,
                (product, category) => category.Names)
            .FirstOrDefault();

        return categoryName;
    }
}
