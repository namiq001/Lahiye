using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.CantactIconVM;
using NOOKX_Project.ViewModels.DashBoardVM;
using NOOKX_Project.ViewModels.IconVM;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class CantactController : Controller
{
    private readonly NookxDbContext _context;

    public CantactController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<CantactIcon> cantactIcons = await _context.CantactIcons.ToListAsync();
        List<Cantact> cantacts = await _context.Cantacts.ToListAsync();
        DashVM dashVM = new DashVM
        {
            CantactIcons = cantactIcons,
            Cantacts = cantacts
        };
        return View(dashVM);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCantactVM create)
    {
        if (!ModelState.IsValid)
        {
            return View(create);
        }
        CantactIcon service = new CantactIcon()
        {
            Title = create.Title,
            Description = create.Description,
            IconString = create.IconString,
        };

        _context.CantactIcons.Add(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        CantactIcon? service = await _context.CantactIcons.FindAsync(Id);
        if (service is null)
        {
            return NotFound();
        }
        _context.CantactIcons.Remove(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        CantactIcon? service = await _context.CantactIcons.FindAsync(Id);
        if (service is null)
        {
            return NotFound();
        }
        EditCantactVM editIcon = new EditCantactVM()
        {
            Title = service.Title,
            Descrition = service.Description,
            IconString = service.IconString,
        };
        return View(editIcon);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int Id, EditCantactVM editIcon)
    {
        CantactIcon? service = await _context.CantactIcons.FindAsync(Id);
        if (service is null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(editIcon);
        }
        service.Title = editIcon.Title;
        service.Description = editIcon.Descrition;
        _context.CantactIcons.Update(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int id)
    {
        CantactIcon? service = await _context.CantactIcons.FindAsync(id);
        if (service == null)
        {
            return NotFound();
        }

        DetailCantactVM detailIcon = new DetailCantactVM()
        {
            Title = service.Title,
            Description = service.Description,
            IconString = service.IconString,
        };
        return View(detailIcon);
    }
}
