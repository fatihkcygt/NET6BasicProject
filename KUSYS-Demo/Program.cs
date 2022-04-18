using KUSYS_Demo.Data.Context;
using KUSYS_Demo.Data.Interfaces;
using KUSYS_Demo.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => {
                    options.LoginPath = "/login";
                });

builder.Services.AddDbContext<StudentCourseContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("KUSYS_DB"));
});

builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
