using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.ViewModels.DashBoardVM;
using NOOKX_Project.ViewModels.SliderVM;
using System.Data;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class SliderController : Controller
{
    private readonly NookxDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public SliderController(NookxDbContext context,IWebHostEnvironment environment)
    {
        _environment = environment;
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        List<Cantact> cantacts = await _context.Cantacts.ToListAsync();
        List<Slider> sliders = await _context.Sliders.ToListAsync();
        DashVM dash = new DashVM
        {
            Cantacts = cantacts,
            Sliders = sliders
        };
        return View(dash);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateSliderVM createslider)
    {
        if (!ModelState.IsValid)
        {
            return View(createslider);
        }
        string newFileName = Guid.NewGuid().ToString() + createslider.Image.FileName;
        string path = Path.Combine(_environment.WebRootPath, "assets", "img", "slider", newFileName);
        using (FileStream stream = new FileStream(path, FileMode.CreateNew))
        {
            await createslider.Image.CopyToAsync(stream);
        }
        Slider slider = new Slider()
        {
            Title = createslider.Title,
            Description = createslider.Description
        };
        slider.SliderImageUrl = newFileName;

        _context.Sliders.Add(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int Id)
    {
        Slider? slider = await _context.Sliders.FindAsync(Id);
        if (slider is null)
        {
            return NotFound();
        }
        string path = Path.Combine(_environment.WebRootPath, "assets", "img", "slider", slider.SliderImageUrl);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        Slider? slider = await _context.Sliders.FindAsync(Id);
        if (slider is null)
        {
            return NotFound();
        }
        EditSliderVM editSlider = new EditSliderVM()
        {
            Title = slider.Title,
            Description = slider.Description,
            SliderImageUrl =  slider.SliderImageUrl,
        };
        return View(editSlider);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int Id, EditSliderVM editSlider)
    {
        Slider? slider = await _context.Sliders.FindAsync(Id);
        if (slider is null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View(editSlider);
        }
        if (editSlider.Image is not null)
        {
            string path = Path.Combine(_environment.WebRootPath, "assets", "img", "slider", slider.SliderImageUrl);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            string newFileName = Guid.NewGuid().ToString() + editSlider.Image.FileName;
            string newPath = Path.Combine(_environment.WebRootPath, "assets", "img", "slider", newFileName);
            using (FileStream stream = new FileStream(newPath, FileMode.CreateNew))
            {
                await editSlider.Image.CopyToAsync(stream);
            }
            slider.SliderImageUrl = newFileName;
        }
        slider.Title = editSlider.Title;
        slider.Description = editSlider.Description;
        _context.Sliders.Update(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int id)
    {
        Slider? slider = await _context.Sliders.FindAsync(id);
        if(slider is null)
        {
            return NotFound();
        }

        DetailSliderVM detailSlider = new DetailSliderVM()
        {
            Title = slider.Title,
            Description = slider.Description,
            SliderImageUrl = slider.SliderImageUrl,
        };

        return View(detailSlider);  
    }
}
