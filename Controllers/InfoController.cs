using Microsoft.AspNetCore.Mvc;

namespace NOOKX_Project.Controllers;

public class InfoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
