using Microsoft.EntityFrameworkCore;
using Shawermawy.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "cart",
    pattern: "Cart",
    defaults: new { controller = "Cart", action = "Index" });

app.MapControllerRoute(
    name: "order",
    pattern: "Order",
    defaults: new { controller = "Order", action = "Index" });

app.MapControllerRoute(
    name: "review",
    pattern: "Review",
    defaults: new { controller = "Review", action = "Index" });

app.MapControllerRoute(
    name: "menu",
    pattern: "Menu",
    defaults: new { controller = "Menu", action = "Index" }); app.Run();