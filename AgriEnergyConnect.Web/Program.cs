using AgriEnergyConnect.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data.Data_Access;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var agriEnergyConnectDbConnectionString = builder.Configuration.GetConnectionString("AgriEnergyConnectDbConnection") ?? throw new InvalidOperationException("Connection string 'AgriEnergyConnectDbConnection' not found.");
builder.Services.AddDbContext<AgriEnergyConnectDbContext>(options =>
    options.UseSqlServer(agriEnergyConnectDbConnectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed the database with roles and an admin user.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await SeedRoles(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

/// <summary>
/// This method is responsible for seeding the roles and creating an admin user in the database.
/// It ensures that the required roles ("Farmer" and "Employee") exist in the system.
/// If the roles do not exist, it creates them using the RoleManager.
/// It also creates an admin user with the email "admin@admin.com" and assigns the "Employee" role to the admin user.
/// This method is called during application startup to ensure that the necessary roles and admin user are available in the database.
/// </summary>
/// <param name="userManager">The UserManager instance used for user management.</param>
/// <param name="roleManager">The RoleManager instance used for role management.</param>
/// <returns>A Task representing the asynchronous operation.</returns>
async Task SeedRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
{
    var roles = new[] { "Farmer", "Employee" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminUser = new IdentityUser
    {
        UserName = "admin@admin.com",
        Email = "admin@admin.com",
        EmailConfirmed = true
    };

    if (userManager.Users.All(u => u.Id != adminUser.Id))
    {
        var user = await userManager.FindByEmailAsync(adminUser.Email);
        if (user == null)
        {
            await userManager.CreateAsync(adminUser, "Password123!");
            await userManager.AddToRoleAsync(adminUser, "Employee");
        }
    }
}
