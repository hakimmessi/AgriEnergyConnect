using AgriEnergyConnect.Data.Data_Access;
using AgriEnergyConnect.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Web.Controllers
{

    [Authorize(Roles = "Employee")]
    public class EmployeesController : Controller
    {
        private readonly AgriEnergyConnectDbContext _context;

        public EmployeesController(AgriEnergyConnectDbContext context)
        {
            _context = context;
        }

        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFarmer(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                _context.Farmers.Add(farmer);
                await _context.SaveChangesAsync();
               // return RedirectToAction(nameof(ViewFarmers));
            }
            return View(farmer);
        }

        public async Task<IActionResult> ViewProducts(int farmerId)
        {
            var products = await _context.Products
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
            return View(products);
        }
    }

}