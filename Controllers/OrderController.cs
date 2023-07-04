using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.ProductVM;
using System.Text.Json;

namespace NOOKX_Project.Controllers;

public class OrderController : Controller
{
    private readonly NookxDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public OrderController(NookxDbContext context,UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> productList = new List<Product>();
        List<CartVM> cartVM = new List<CartVM>();
        List<CartBasketItem> memberbasketItems = null;
        CartVM cart = null;
        AppUser member = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
        if (member == null)
        {
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
        }
        else
        {
            memberbasketItems = await _context.CartBasketItems.Include(x => x.Product).Where(x => x.AppUserId == member.Id).ToListAsync();

            foreach (var item in memberbasketItems)
            {
                cart = new CartVM
                {
                    Id = item.Product.Id,
                    ProductName = item.Product.ProductName,
                    ImageName = item.Product.ProductImageUrl,
                    Price = item.Product.NewPrice,
                    Count = item.Count,
                };

                cartVM.Add(cart);

            }
        }

        return View(cartVM);
    }
}
