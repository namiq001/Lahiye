using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.ProductVM;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class ProductController : Controller
{
    private readonly NookxDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ProductController(IWebHostEnvironment environment, NookxDbContext context)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> products = await _context.Products.Include(x=>x.Catagories).ToListAsync();
        return View(products);
    }
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        CreateProductVM createProduct = new CreateProductVM()
        {
            Catagories = await _context.Catagories.ToListAsync(),
        };
        return View(createProduct);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductVM createProduct)
    {
        createProduct.Catagories = await _context.Catagories.ToListAsync();
        if (!ModelState.IsValid)
        {
            return View(createProduct);
        }
        string newFileName = Guid.NewGuid().ToString() + createProduct.Image.FileName;
        string path = Path.Combine(_environment.WebRootPath, "assets", "img", "shop", newFileName);
        using (FileStream stream = new FileStream(path, FileMode.CreateNew))
        {
            await createProduct.Image.CopyToAsync(stream);
        }

        Product product = new Product()
        {
            CatagoryId = createProduct.CatagoryId,
            ProductName = createProduct.ProductName,
            NewPrice = createProduct.NewPrice,
            OldPrice = createProduct.OldPrice,
            Percentage = createProduct.Percentage,
        };
        product.ProductImageUrl = newFileName;

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int Id)
    {
        Product? product = await _context.Products.FindAsync(Id);
        if (product is null) { return NotFound(); }
        string path = Path.Combine(_environment.WebRootPath, "assets", "img", "shop", product.ProductImageUrl);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        Product? product = await _context.Products.FindAsync(Id);
        if (product is null)
        {
            return NotFound();
        }
        EditProductVM editProduct = new EditProductVM()
        {
            ProductName = product.ProductName,
            CatagoryId = product.CatagoryId,
            NewPrice = product.NewPrice,
            OldPrice = product.OldPrice,
            Percentage = product.Percentage,
            Catagories = await _context.Catagories.ToListAsync(),
            ProductImageUrl = product.ProductImageUrl
        };
        return View(editProduct);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int Id, EditProductVM editProduct)
    {
        Product? product = await _context.Products.FindAsync(Id);
        if (product is null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            editProduct.Catagories = await _context.Catagories.ToListAsync();
            return View(editProduct);
        }
        if (editProduct.ProductImageUrl is not null)
        {
            string path = Path.Combine(_environment.WebRootPath, "assets", "img", "shop", product.ProductImageUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            string newFileName = Guid.NewGuid().ToString() + editProduct.Image.FileName;
            string newPath = Path.Combine(_environment.WebRootPath, "assets", "img", "shop", newFileName);
            using (FileStream stream = new FileStream(newPath, FileMode.CreateNew))
            {
                await editProduct.Image.CopyToAsync(stream);
            }
            product.ProductImageUrl = newFileName;
        }
        product.ProductName = editProduct.ProductName;
        product.CatagoryId = editProduct.CatagoryId;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
