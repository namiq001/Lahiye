using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace NOOKX_Project.Areas.Admin.Controllers;
[Area("Admin")]
public class DashBoardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
