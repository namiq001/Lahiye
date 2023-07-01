using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NOOKX_Project.Models;
using NOOKX_Project.ViewModels.AccountVM;
using System.Net.Mail;
using System.Net;
using NOOKX_Project.ViewModels.MailSenderVM;
using NOOKX_Project.Services.Interfaces;

namespace NOOKX_Project.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailService _emailService;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager,IEmailService emailService)
    {
        _emailService = emailService;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM register)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        AppUser newUser = new AppUser()
        {
            Name = register.Name,
            Surname = register.Surname,
            UserName = register.UserName,
            Email = register.EmailAddress,
        };
        IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);
        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        IdentityResult role = await _userManager.AddToRoleAsync(newUser, "Admin");
        if (!role.Succeeded)
        {
            foreach (IdentityError error in role.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View(register);
            }
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        string link = Url.Action("ConfrimUser", "Account", new { email = register.EmailAddress, token = token }, HttpContext.Request.Scheme);

        MailMessage message = new MailMessage("knamiq605@gmail.com", newUser.Email)
        {
            Subject = "Confrimation email",
            Body = $"<a href = \"{link}\"> Click to confrim email.</a>",
            IsBodyHtml = true
        };

        SmtpClient smtpClient = new()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential("knamiq605@gmail.com", "oubcdycxxtsfcxij")
        };

        smtpClient.Send(message);

       

        return RedirectToAction(nameof(Login));
    }

    //Confrimation
    public async Task<IActionResult> ConfrimUser(string email, string token)
    {
        AppUser user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Incorrect confrim");
            return RedirectToAction("Index", "Home");
        }

        await _signInManager.SignInAsync(user, true);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        AppUser appUser = await _userManager.FindByEmailAsync(loginVM.EmailAdress);
        if (appUser is null)
        {
            ModelState.AddModelError("", "Invalid Password or Email");
            return View();
        }
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid Password or Email");
            return View();
        }
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> CreateRole()
    {
        IdentityRole role = new IdentityRole()
        {
            Name = "Admin"
        };
        await _roleManager.CreateAsync(role);
        return Json("Ok");
    }

    //Forgot Password
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPassword)
    {
        if (!ModelState.IsValid) return NotFound();

        AppUser exsistUser = await _userManager.FindByEmailAsync(forgotPassword.Email);

        if (exsistUser is null)
        {
            ModelState.AddModelError("Email", "Email isn't found");
            return View();
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);
        string link = Url.Action(nameof(ResetPassword), "Account", new { userId = exsistUser.Id, token = token }, HttpContext.Request.Scheme);

             _emailService.SendMessage(new MailRequestVM { ToEmail = forgotPassword.Email, Subject = "ResetPassword", Body = $"<a href=\"{link}\">Reset Password</a>" });

        return RedirectToAction(nameof(Login));

    }


    public async Task<IActionResult> ChangePassword(string username)
    {
        AppUser exsistUser = await _userManager.FindByNameAsync(username);

        if (exsistUser is null)
        {
            ModelState.AddModelError("Email", "Email isn't found");
            return View();
        }
        string token = await _userManager.GeneratePasswordResetTokenAsync(exsistUser);

        return RedirectToAction(nameof(ResetPassword), new { userId = exsistUser.Id, token = token });

    }

    //Reset Password
    public async Task<IActionResult> ResetPassword(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();

        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user is null) return NotFound();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(RecetPasswordVM resetPassword, string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            return BadRequest();

        if (!ModelState.IsValid)
            return View(resetPassword);

        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user is null)
            return NotFound();

        var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(resetPassword);
        }

        return RedirectToAction(nameof(Login));
    }
}
