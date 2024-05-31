using AgriEnergyConnect.Data.Data_Access;
using AgriEnergyConnect.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AgriEnergyConnect.Web.Controllers
{
    [Authorize(Roles = "Farmer, Employee")]
    public class ProductsController : Controller
    {
        private readonly AgriEnergyConnectDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(AgriEnergyConnectDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Farmer")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
                if (farmer == null)
                {
                    // Handle case where no farmer is associated with the user
                    return NotFound();
                }

                product.FarmerId = farmer.Id;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ViewProducts));
            }
            return View(product);
        }

        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> ViewProducts()
        {
            var userId = _userManager.GetUserId(User);
            var products = await _context.Products
              //  .Where(p => p.FarmerId == userId)
                .ToListAsync();
            return View(products);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ViewAllProducts(int farmerId)
        {
            var products = await _context.Products
                .Where(p => p.FarmerId == farmerId)
                .ToListAsync();
            return View(products);
        }
    }


}
