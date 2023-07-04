using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.CatagotyVM;
using NOOKX_Project.ViewModels.DashBoardVM;
using NOOKX_Project.ViewModels.IconVM;
using System.Data;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class CatagoryController : Controller
{
    private readonly NookxDbContext _context;


    public CatagoryController(NookxDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Catagory> catagories = await _context.Catagories.ToListAsync();
        List<Cantact> cantacts = await _context.Cantacts.ToListAsync();
        DashVM dashVM = new DashVM
        {
            Catagories = catagories,
            Cantacts = cantacts,
        };
        return View(dashVM);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCatagoryVM createCatagory)
    {
        if(!ModelState.IsValid)
        {
            return View(createCatagory);
        }
       
        Catagory catagory = new Catagory()
        {
            CatagoryName = createCatagory.Name,
        };
         
        _context.Catagories.Add(catagory);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        Catagory? catagory = await _context.Catagories.FindAsync(Id);
        if (catagory is null)
        {
            return NotFound();
        }
        _context.Catagories.Remove(catagory);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        Catagory? catagory = await _context.Catagories.FindAsync(Id);
        if (catagory is null)
        {
            return NotFound();
        }
        EditCatagoryVM editCatagory = new EditCatagoryVM()
        {
            Name = catagory.CatagoryName
        };
        return View(editCatagory);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int Id, EditCatagoryVM editCatagory)
    {
        Catagory? catagory = await _context.Catagories.FindAsync(Id);
        if (catagory is null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(editCatagory);
        }
        catagory.CatagoryName = editCatagory.Name;
        _context.Catagories.Update(catagory);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int id)
    {
        Catagory? catagory = await _context.Catagories.FindAsync(id);
        if (catagory == null)
        {
            return NotFound();
        }

        DetailCatagoryVM detailCatagory = new DetailCatagoryVM()
        {
            Name = catagory.CatagoryName,
        };
        return View(detailCatagory);
    }
}
