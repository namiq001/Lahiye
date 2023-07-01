using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels;
using NOOKX_Project.ViewModels.ProductVM;
using System.Text.Json;

namespace NOOKX_Project.Controllers;

public class HomeController : Controller
{
    private readonly NookxDbContext _context;

    public HomeController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        List<Product> products = await _context.Products.Include(x => x.Catagories).ToListAsync();
        List<Catagory> catagories = await _context.Catagories.ToListAsync();
        List<Service> services = await _context.Services.ToListAsync();
        List<Slider> sliders = await _context.Sliders.ToListAsync();

        HomeVM homeVM = new HomeVM()
        {
            Catagories = catagories,
            Products = products,
            Services = services,
            Sliders = sliders,
        };

        List<Product> productList = new List<Product>();
        List<CartVM> cartVM = new List<CartVM>();
        string value = HttpContext.Request.Cookies["basket"];
        if (value is null)
        {
            cartVM = null;
        }
        else
        {

            cartVM = JsonSerializer.Deserialize<List<CartVM>>(value);
            foreach (var item in cartVM)
            {
                Product? product = await _context.Products.Include(c => c.Catagories).FirstOrDefaultAsync();
                productList.Add(product);
            }
        }

        ViewBag.CartVM = cartVM;

        return View(homeVM);
    }
    public async Task<IActionResult> Product(int id)
    {
        Product? products = await _context.Products.Include(c => c.Catagories).FirstAsync(p => p.Id == id);
        return View(products);
    }
    public IActionResult GetSession()
    {
        return Json(HttpContext.Session.Get("test"));
    }
    public IActionResult GetCookies()
    {
        return Json(HttpContext.Request.Cookies["test"]);
    }

}