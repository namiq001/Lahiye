using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels;
using NOOKX_Project.ViewModels.DetailVM;

namespace NOOKX_Project.Controllers;

public class ProductListController : Controller
{
    private readonly NookxDbContext _context;

    public ProductListController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search )
    {
        IQueryable<Product> query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.ProductName.ToLower().Contains(search.ToLower()));
        }

        SearchVM index = new SearchVM
        {
            Catagories = await _context.Catagories.ToListAsync(),
            Products = await query.ToListAsync(),
        };
        return View(index);
    }
}
