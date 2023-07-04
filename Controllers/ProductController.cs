using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.DetailVM;
using NOOKX_Project.ViewModels.ProductVM;
using System.Text.Json;

namespace NOOKX_Project.Controllers;

public class ProductController : Controller
{
    private readonly NookxDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public ProductController(NookxDbContext context,UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> AddCart(int id)
    {
        AppUser member = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
        if(member is null)
        {
            Product? product = await _context
            .Products
            .Include(x => x.Catagories)
            .FirstAsync(x => x.Id == id);
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
        }
        else
        {
            CartBasketItem memberbasketitem = _context.CartBasketItems.FirstOrDefault(x => x.AppUserId == member.Id && x.ProductId == id);

            if (memberbasketitem != null)
            {
                memberbasketitem.Count++;
            }
            else
            {
                memberbasketitem = new CartBasketItem()
                {
                    AppUserId = member.Id,
                    ProductId = id,
                    Count = 1
                };

                _context.CartBasketItems.Add(memberbasketitem);
            }
        }
        await _context.SaveChangesAsync();


        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> GetCarts()
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
    public async Task<IActionResult> RemoveCart(int id)
    {
        AppUser member = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
        if (member == null)
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
        }
        else
        {
            CartBasketItem? basketItem = await _context.CartBasketItems.FirstOrDefaultAsync(x => x.ProductId == id);
            if (basketItem is null)
            {
                return NotFound();
            }

            _context.CartBasketItems.Remove(basketItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetCarts", "Product");

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
            ProductDetailVM productDetailVM = new ProductDetailVM
            {
                product = product,
                productComments = await _context.ProductComments.Include(x => x.appUser).ToListAsync(),
            };
            return View(productDetailVM);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ProductComment(ProductComment productComment)
    {
        Product product = _context.Products.FirstOrDefault(x => x.Id == productComment.ProductId);
        if (product == null) return View("Error");

        if (!ModelState.IsValid) return RedirectToAction("ViewProduct", new { id = product.Id });
        if (productComment.Rating < 1 || productComment.Rating > 5)
        {
            ModelState.AddModelError("Rating", "Error");
            return RedirectToAction("ViewProduct", new { id = product.Id });
        }
        AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

        ProductComment comment = new ProductComment
        {
            appUser = appUser,
            AppUserId = appUser.Id,
            ProductId = productComment.ProductId,
            product = product,
            Rating = productComment.Rating,
        };

        _context.ProductComments.Add(comment);
        _context.SaveChanges();
        return RedirectToAction(nameof(ViewProduct), new { id = product.Id });
    }
    public async Task<IActionResult> AddWishList(int id)
    {
        AppUser member = null;

        if(HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        if(member == null)
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
        }

        else
        {
            BasketItem memberbasketitem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == member.Id && x.ProductId == id);

            if(memberbasketitem != null)
            {
                memberbasketitem.Count++;
            }
            else
            {
                memberbasketitem = new BasketItem()
                {
                    AppUserId = member.Id,
                    ProductId = id,
                    Count = 1
                };

                _context.BasketItems.Add(memberbasketitem);
            }
        }
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> GetWishList()
    {
        List<Product> productList = new List<Product>();
        List<CartVM> cartVM = new List<CartVM>();
        CartVM cart = null;
        List<BasketItem> memberbasketItems = null;
        AppUser member = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        if(member == null)
        {
            
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
        }

        else
        {
            memberbasketItems = await _context.BasketItems.Include(x=>x.Product).Where(x => x.AppUserId == member.Id).ToListAsync();

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
    public async Task<IActionResult> RemoveCartWishList(int id)
    {
        AppUser member = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            member = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
        if(member == null)
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
        }
        else
        {
            BasketItem? basketItem = await _context.BasketItems.FirstOrDefaultAsync(x=>x.ProductId == id);
            if (basketItem is null) 
            {
                return NotFound();
            }

            _context.BasketItems.Remove(basketItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetWishList", "Product");

        }


        return RedirectToAction("GetWishList", "Product");
    }
}
