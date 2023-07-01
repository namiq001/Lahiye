using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.SettingVM;
using System.Data;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class SettingController : Controller
{
    readonly NookxDbContext _context;

    public SettingController(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Setting> settings = await _context.Settings.ToListAsync();
        return View(settings);
    }
    public async Task<IActionResult> Edit(int id)
    {
        Setting? setting = await _context.Settings.FindAsync(id);
        if (setting == null)
        {
            return NotFound();
        }
        EditSettingVM editVM = new EditSettingVM()
        {
            Value = setting.Value,
        };
        return View(editVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditSettingVM newSetting)
    {
        Setting? setting = await _context
            .Settings
            .Where(s => s.Id == id)
            .FirstOrDefaultAsync();
        if (setting == null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View();
        }

        setting.Value = newSetting.Value;
        _context.Settings.Update(setting);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
