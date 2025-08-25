using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories;
using Training_Management_System_ITI_Project.enums;
using Training_Management_System_ITI_Project.Repositories.Interfaces;

namespace Training_Management_System_ITI_Project
{

  public class Program
  {

    public static async Task Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      builder.Services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.AllowedUserNameCharacters =
                      "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = true;

        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedPhoneNumber = false;
      })
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders();

      builder.Services.ConfigureApplicationCookie(options =>
      {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.SlidingExpiration = true;
      });


      builder.Services.AddScoped<ICourseRepository, CourseRepository>();
      builder.Services.AddScoped<ISessionRepository, SessionRepository>();
      builder.Services.AddScoped<IUserRepository, UserRepository>();
      builder.Services.AddScoped<IGradeRepository, GradeRepository>(); var app = builder.Build();

      await InitializeDatabaseAsync(app);

      if (!app.Environment.IsDevelopment())
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

      await app.RunAsync();
    }


    private static async Task InitializeDatabaseAsync(WebApplication app)
    {
      using var scope = app.Services.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
      var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

      await context.Database.EnsureCreatedAsync();

      await CreateRolesAsync(roleManager);

      await CreateDefaultSuperAdminAsync(userManager);
    }


    private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
    {
      string[] roleNames = { "SuperAdmin", "Admin", "Instructor", "Trainee" };

      foreach (var roleName in roleNames)
      {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
          await roleManager.CreateAsync(new IdentityRole(roleName));
        }
      }
    }


    private static async Task CreateDefaultSuperAdminAsync(UserManager<ApplicationUser> userManager)
    {
      const string superAdminEmail = "superadmin@trainingms.com";
      const string superAdminPassword = "SuperAdmin123!";

      var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
      if (superAdmin == null)
      {
        superAdmin = new ApplicationUser
        {
          UserName = superAdminEmail,
          Email = superAdminEmail,
          FullName = "System Super Administrator",
          Role = UserRole.SuperAdmin,
          EmailConfirmed = true,
          IsActive = true
        };

        var result = await userManager.CreateAsync(superAdmin, superAdminPassword);
        if (result.Succeeded)
        {
          await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
          Console.WriteLine($"Default Super Admin created: {superAdminEmail}");
          Console.WriteLine($"Default Password: {superAdminPassword}");
          Console.WriteLine("Please change this password immediately after first login!");
        }
      }
    }
  }
}
