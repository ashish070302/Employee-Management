using EmployeeRepositories;
using EmployeeRepositories.Implementation;
using EmployeeRepositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Employee.UI")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddScoped<IStateRepo, StateRepo>();
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeRepo>();
builder.Services.AddScoped<IUtilityRepo, UtilityRepo>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60*1);
        
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Consider changing based on your HTTPS setup
        options.Cookie.SameSite = SameSiteMode.Strict; // Consider changing based on your application's requirements
        options.LoginPath = "/Account/LogIn"; // Set the login page path
        options.AccessDeniedPath = "/Account/AccessDenied"; // Set the access denied page path
    });
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(10);
	option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();