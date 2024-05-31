using AgriEnergyConnect.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AgriEnergyConnect.Data.Data_Access;
using AgriEnergyConnect.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity services
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Ensure roles are added
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add the custom DbContext
var agriEnergyConnectDbConnectionString = builder.Configuration.GetConnectionString("AgriEnergyConnectDbConnection") ?? throw new InvalidOperationException("Connection string 'AgriEnergyConnectDbConnection' not found.");
builder.Services.AddDbContext<AgriEnergyConnectDbContext>(options =>
    options.UseSqlServer(agriEnergyConnectDbConnectionString));

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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed the database with roles, users, and initial data.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<AgriEnergyConnectDbContext>();

        await SeedRolesAndUsers(userManager, roleManager, context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();

/// <summary>
/// This method is responsible for seeding the roles and creating users in the database.
/// It ensures that the required roles ("Farmer" and "Employee") exist in the system.
/// If the roles do not exist, it creates them using the RoleManager.
/// It also creates users with the appropriate roles and seeds initial products data.
/// </summary>
/// <param name="userManager">The UserManager instance used for user management.</param>
/// <param name="roleManager">The RoleManager instance used for role management.</param>
/// <param name="context">The DbContext instance used for data access.</param>
/// <returns>A Task representing the asynchronous operation.</returns>
async Task SeedRolesAndUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AgriEnergyConnectDbContext context)
{
    var roles = new[] { "Farmer", "Employee" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Create a default admin user
    var adminUser = new IdentityUser
    {
        UserName = "admin@agryenergyconnect.com",
        Email = "admin@agryenergyconnect.com",
        EmailConfirmed = true
    };

    if (userManager.Users.All(u => u.Id != adminUser.Id))
    {
        var user = await userManager.FindByEmailAsync(adminUser.Email);
        if (user == null)
        {
            await userManager.CreateAsync(adminUser, "EmployeeOn0512!");
            await userManager.AddToRoleAsync(adminUser, "Employee");
        }
    }

    // Create a default farmer user
    var farmerUser1 = new IdentityUser
    {
        UserName = "farmer1@agryenergyconnect.com",
        Email = "farmer1@agryenergyconnect.com",
        EmailConfirmed = true
    };

    if (userManager.Users.All(u => u.Id != farmerUser1.Id))
    {
        var user = await userManager.FindByEmailAsync(farmerUser1.Email);
        if (user == null)
        {
            await userManager.CreateAsync(farmerUser1, "Password123!");
            await userManager.AddToRoleAsync(farmerUser1, "Farmer");
        }
    }

    var farmerUser2 = new IdentityUser
    {
        UserName = "farmer2@agryenergyconnect.com",
        Email = "farmer2@agryenergyconnect.com",
        EmailConfirmed = true
    };

    if (userManager.Users.All(u => u.Id != farmerUser2.Id))
    {
        var user = await userManager.FindByEmailAsync(farmerUser2.Email);
        if (user == null)
        {
            await userManager.CreateAsync(farmerUser2, "Password123!");
            await userManager.AddToRoleAsync(farmerUser2, "Farmer");
        }
    }

    // Get the IDs of the created farmer users
    var farmer1 = await userManager.FindByEmailAsync(farmerUser1.Email);
    var farmer2 = await userManager.FindByEmailAsync(farmerUser2.Email);

    // Seed initial products
    SeedProducts(context, farmer1.Id, farmer2.Id);
}

/// <summary>
/// This method is responsible for seeding initial products into the database.
/// It ensures that some products are available for demonstration purposes.
/// </summary>
/// <param name="context">The DbContext instance used for data access.</param>
/// <param name="farmer1Id">The ID of the first farmer.</param>
/// <param name="farmer2Id">The ID of the second farmer.</param>
void SeedProducts(AgriEnergyConnectDbContext context, string farmer1Id, string farmer2Id)
{
    // Check if the database has been seeded
    if (context.Products.Any())
    {
        return;   // DB has been seeded
    }

    context.Products.AddRange(
        new Product
        {
            Name = "Apple",
            Category = "Fruit",
            ProductionDate = DateTime.Parse("2023-05-01"),
            FarmerId = int.Parse(farmer1Id)
        },
        new Product
        {
            Name = "Carrot",
            Category = "Vegetable",
            ProductionDate = DateTime.Parse("2023-04-15"),
            FarmerId = int.Parse(farmer2Id)
        }
    );

    context.SaveChanges();
}
