using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.CantactIconVM;
using System.Data;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CantactMessageController : Controller
{
    private readonly NookxDbContext _context;

    public CantactMessageController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Cantact> cantacts = await _context.Cantacts.ToListAsync();
        return View(cantacts);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        Cantact? service = await _context.Cantacts.FindAsync(Id);
        if (service is null)
        {
            return NotFound();
        }
        _context.Cantacts.Remove(service);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int id)
    {
        Cantact? service = await _context.Cantacts.FindAsync(id);
        if (service == null)
        {
            return NotFound();
        }
        
        return View(service);
    }
}
