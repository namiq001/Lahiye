using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;

namespace NOOKX_Project.Controllers;

public class ProductListController : Controller
{
    private readonly NookxDbContext _context;

    public ProductListController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> products = await _context.Products.Include(x => x.Catagories).ToListAsync();

        return View(products);
    }
}
