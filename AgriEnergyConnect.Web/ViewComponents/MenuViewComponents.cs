using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public class MenuViewComponent : ViewComponent
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public MenuViewComponent(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var role = "";
        if (_signInManager.IsSignedIn(HttpContext.User))
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (await _userManager.IsInRoleAsync(user, "Employee"))
            {
                role = "Employee";
            }
            else if (await _userManager.IsInRoleAsync(user, "Farmer"))
            {
                role = "Farmer";
            }
        }

        return View("Default", role);
    }
}
