using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using NOOKX_Project.NOOKXDataBase;
using NOOKX_Project.Services.Interfaces;
using NOOKX_Project.ViewModels.AccountVM;
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using System.Net;
using NOOKX_Project.ViewModels.SendMessageVM;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NOOKX_Project.Controllers;

public class ContactController : Controller
{
    private readonly NookxDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailService _emailService;

    public ContactController(NookxDbContext context
        , UserManager<AppUser> userManager
        , SignInManager<AppUser> signInManager
        , RoleManager<IdentityRole> roleManager
        , IEmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(SendVM send)
    {
        if(!ModelState.IsValid)
        {
            return View(send);
        }

        Cantact cantact = new Cantact
        {
            EmailAddress = send.EmailAddress,
            Message = send.Message,
            SendedDate = DateTime.UtcNow.AddHours(4),
        };

        _context.Cantacts.Add(cantact);
        await _context.SaveChangesAsync();  

        return RedirectToAction(nameof(Index));
    }
}
