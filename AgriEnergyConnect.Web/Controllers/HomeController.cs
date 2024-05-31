using AgriEnergyConnect.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AgriEnergyConnect.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Farmer")]
        public IActionResult FarmerHome()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        public IActionResult EmployeeHome()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

    }

}
