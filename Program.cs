using StudyTracker.Repositories;
using Microsoft.AspNetCore.Identity;
using StudyTracker.Services;
using StudyTracker.ViewModelBuilders;
using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Home/AccessDenied";  // или другой контроллер/действие
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<AssignmentRepository>();
builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<StudentCourseAssignmentRepository>();
builder.Services.AddScoped<StudentCourseAssignmentService>();
builder.Services.AddScoped<StudentAssignmentStatusRepository>();
builder.Services.AddScoped<StudentAssignmentStatusService>();
builder.Services.AddScoped<CoursesVmBuilder>();
builder.Services.AddScoped<CoursesWithStudentsVmBuilder>();
builder.Services.AddScoped<IReportService, ReportService>();


var app = builder.Build();
app.MapRazorPages();

var scope = app.Services.CreateScope();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
await CreateRolesAsync(roleManager);

async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Administrator", "Student" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

string adminEmail = "admin@example.com";
string adminPassword = "Admin@12345";

var adminUser = await userManager.FindByEmailAsync(adminEmail);

if (adminUser == null)
{
    adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
    var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
    if (createAdmin.Succeeded)
    {
        await userManager.AddToRoleAsync(adminUser, "Administrator");
    }
}
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

app.Run();
