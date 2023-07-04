using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.DashBoardVM;
using System.Data; 

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class DashBoardController : Controller
{
    private readonly NookxDbContext _context;

    public DashBoardController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Setting> settings = await _context.Settings.ToListAsync();

        List<Cantact> cantacts = await _context.Cantacts.ToListAsync();

        DashVM dashVM = new DashVM
        {
            Settings = settings,
            Cantacts = cantacts,
        };
        return View(dashVM);
    }
}
