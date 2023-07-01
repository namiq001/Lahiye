using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;

namespace NOOKX_Project.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly NookxDbContext _context;

    public FooterViewComponent(NookxDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        Dictionary<string, Setting> setting = await _context.Settings.ToDictionaryAsync(x => x.Key);
        return View(setting);
    }
}
