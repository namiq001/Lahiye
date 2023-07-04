using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.DashBoardVM;
using NOOKX_Project.ViewModels.IconVM;
using System.Data;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class IconController : Controller
{
    private readonly NookxDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public IconController(NookxDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        List<Cantact> cantacts = await _context.Cantacts.ToListAsync();
        List<Service> services = await _context.Services.ToListAsync();
        DashVM dashVM = new DashVM
        {
            Cantacts = cantacts,
            Services = services
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
    public async Task<IActionResult> Create(CreateIconVM create)
    {
        if (!ModelState.IsValid)
        {
            return View(create);
        }
        Service service = new Service()
        {
            Title = create.Title,
            Description = create.Description,
            IconString = create.IconString,
        };

        _context.Services.Add(service);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        Service? service = await _context.Services.FindAsync(Id);
        if (service is null)
        {
            return NotFound();
        }
        _context.Services.Remove(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        Service? service = await _context.Services.FindAsync(Id);
        if (service is null)
        {
            return NotFound();
        }
        EditIconVM editIcon = new EditIconVM()
        {
            Title = service.Title,
            Descrition = service.Description,
            IconString =  service.IconString,
        };
        return View(editIcon);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int Id, EditIconVM editIcon)
    {
        Service? service = await _context.Services.FindAsync(Id);
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
        _context.Services.Update(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int id)
    {
        Service? service = await _context.Services.FindAsync(id);
        if (service  == null)
        {
            return NotFound();
        }

        DetailIconVM detailIcon = new DetailIconVM()
        {
            Title = service.Title,
            Description = service.Description,
            IconString = service.IconString,
        };
        return View(detailIcon);
    }
}
