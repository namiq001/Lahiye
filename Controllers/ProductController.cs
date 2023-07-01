using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.ProductVM;
using System.Text.Json;

namespace NOOKX_Project.Controllers;

public class ProductController : Controller
{
    private readonly NookxDbContext _context;

    public ProductController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddCart(int id)
    {
        Product? product = await _context
            .Products
            .Include(x => x.Catagories)
            .FirstAsync(x=>x.Id==id);
        if (product == null)
        {
            return NotFound();
        }
        string? value = HttpContext.Request.Cookies["basket"];
        List<CartVM> cartsCookies = new List<CartVM>();
        if (value == null)
        {
            HttpContext
                .Response
                .Cookies
                .Append("basket", JsonSerializer.Serialize(cartsCookies));
        }
        else
        {
            cartsCookies = JsonSerializer.Deserialize<List<CartVM>>(value);
        }

        CartVM? cart = cartsCookies.Find(c => c.Id == id);
        if (cart == null)
        {
            cartsCookies.Add(new CartVM()
            {
                Id = id,
                Count = 1,
                ProductName = product.ProductName,
                Price = product.NewPrice,
                CatagoryName = product.Catagories.CatagoryName,
                ImageName = product.ProductImageUrl,
            });
        }
        else
        {
            cart.Count += 1;
        }

        HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(cartsCookies), new CookieOptions()
        {
            MaxAge = TimeSpan.FromDays(25)
        });
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> GetCarts()
    {
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
        return View(cartVM);
    }
    public async Task<IActionResult> Checkout()
    {
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
        return View(cartVM);
    }
    public async Task<IActionResult> RemoveCart(int id)
    {
        string? value = HttpContext.Request.Cookies["basket"];
        if (value == null) return NotFound();
        else
        {
            List<CartVM>? cartVM = JsonSerializer.Deserialize<List<CartVM>>(value);
            CartVM? cart = cartVM.FirstOrDefault(c => c.Id == id);
            if (cart is not null)
            {
                cartVM.Remove(cart);
            }
            HttpContext.Response.Cookies.Append("basket", JsonSerializer.Serialize(cartVM), new CookieOptions()
            {
                MaxAge = TimeSpan.FromMinutes(10)
            });
        }
        return RedirectToAction("GetCarts", "Product");
    }
    public IActionResult Address()
    {
        return View();
    }
    public IActionResult Delivery()
    {
        return View();
    }
    public IActionResult PayMent()
    {
        return View();
    }
    public async Task<IActionResult> ViewProduct(int id)
    {
        Product? product = await _context.Products.Include(x=>x.Catagories).FirstOrDefaultAsync(x=>x.Id==id);
        if(product is null)
        {
            return NotFound();
        }
        else
        {
            return View(product);
        }
    }



    public async Task<IActionResult> AddWishList(int id)
    {
        Product? product = await _context
            .Products
            .Include(x => x.Catagories)
            .FirstAsync(x => x.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        string? value = HttpContext.Request.Cookies["WishList"];
        List<CartVM> cartsCookies = new List<CartVM>();
        if (value == null)
        {
            HttpContext
                .Response
                .Cookies
                .Append("WishList", JsonSerializer.Serialize(cartsCookies));
        }
        else
        {
            cartsCookies = JsonSerializer.Deserialize<List<CartVM>>(value);
        }

        CartVM? cart = cartsCookies.Find(c => c.Id == id);
        if (cart == null)
        {
            cartsCookies.Add(new CartVM()
            {
                Id = id,
                Count = 1,
                ProductName = product.ProductName,
                Price = product.NewPrice,
                CatagoryName = product.Catagories.CatagoryName,
                ImageName = product.ProductImageUrl,
            });
        }
        else
        {
            cart.Count += 1;
        }

        HttpContext.Response.Cookies.Append("WishList", JsonSerializer.Serialize(cartsCookies), new CookieOptions()
        {
            MaxAge = TimeSpan.FromDays(25)
        });
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> GetWishList()
    {
        List<Product> productList = new List<Product>();
        List<CartVM> cartVM = new List<CartVM>();
        string value = HttpContext.Request.Cookies["WishList"];
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
        return View(cartVM);
    }
    public async Task<IActionResult> CheckoutWishList()
    {
        List<Product> productList = new List<Product>();
        List<CartVM> cartVM = new List<CartVM>();
        string value = HttpContext.Request.Cookies["WishList"];
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
        return View(cartVM);
    }
    public async Task<IActionResult> RemoveCartWishList(int id)
    {
        string? value = HttpContext.Request.Cookies["WishList"];
        if (value == null) return NotFound();
        else
        {
            List<CartVM>? cartVM = JsonSerializer.Deserialize<List<CartVM>>(value);
            CartVM? cart = cartVM.FirstOrDefault(c => c.Id == id);
            if (cart is not null)
            {
                cartVM.Remove(cart);
            }
            HttpContext.Response.Cookies.Append("WishList", JsonSerializer.Serialize(cartVM), new CookieOptions()
            {
                MaxAge = TimeSpan.FromMinutes(10)
            });
        }
        return RedirectToAction("GetWishList", "Product");
    }
}
