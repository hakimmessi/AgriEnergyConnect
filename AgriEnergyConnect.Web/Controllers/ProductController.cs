using AgriEnergyConnect.Data.Data_Access;
using AgriEnergyConnect.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Web.Controllers
{
    [Authorize(Roles = "Farmer,Employee")]
    public class ProductsController : Controller
    {
        private readonly AgriEnergyConnectDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AgriEnergyConnectDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
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
                try
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error adding product: {ProductName}", product.Name);
                    ModelState.AddModelError(string.Empty, "An error occurred while adding the product.");
                }
            }
            return View(product);
        }

        
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ViewProducts(int farmerId)
        {
            try
            {
                var products = await _context.Products
                    .Where(p => p.FarmerId == farmerId)
                    .ToListAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error viewing products for farmer ID: {FarmerId}", farmerId);
                return View(new List<Product>());
            }
        }
    }

}
