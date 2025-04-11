using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Entities;
using TechXpress.Data.AppContext;
using System;
using TechXpress.Data.Repositories.Implementation;
using TechXpress.Data.Repositories.Interfaces;
using TechXpress_E_commerce.Repositories;
using TechXpress.Services.Interfaces;
using TechXpress.Services.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using NETCore.MailKit.Extensions;
using System.Configuration;
using NETCore.MailKit.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//add identity service
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
 builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always; 
});


builder.Services.AddAuthentication()
    .AddCookie("AdminScheme", options =>
    {
        options.LoginPath = "/Admin/Admin/Login";
        options.LogoutPath = "/Admin/Admin/Logout";
        options.AccessDeniedPath = "/Admin/Admin/AccessDenied";
        options.Cookie.Name = "TechXpressAdmin";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });
 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IWishListRepository, WishListRepository>();


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();   
builder.Services.AddScoped<IPaymentService, StripePaymentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddMailKit(options =>
{
    options.UseMailKit(new NETCore.MailKit.Infrastructure.Internal.MailKitOptions
    {
        Server = builder.Configuration["EmailSettings:Server"],
        Port = int.Parse(builder.Configuration["EmailSettings:Port"]),
        SenderName = builder.Configuration["EmailSettings:SenderName"],
        SenderEmail = builder.Configuration["EmailSettings:SenderEmail"],
        Account = builder.Configuration["EmailSettings:Account"],
        Password = builder.Configuration["EmailSettings:Password"],
        Security = true
    });
});







var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();  
app.MapStaticAssets();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

//// Roles
//using (var scope = app.Services.CreateScope())
//{
//    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//    // Create Admin role if it doesn't exist
//    if (!await roleManager.RoleExistsAsync("Admin"))
//    {
//        await roleManager.CreateAsync(new IdentityRole("Admin"));
//    }

//    // Create default Admin user if it doesn't exist
//    var adminEmail = "admin@techxpress.com";
//    var adminUser = await userManager.FindByEmailAsync(adminEmail);

//    if (adminUser == null)
//    {
//        adminUser = new ApplicationUser
//        {
//            UserName = adminEmail,
//            Email = adminEmail,
//            EmailConfirmed = true,
//            FirstName = "Admin",
//            LastName = "User"
//        };

//        var result = await userManager.CreateAsync(adminUser, "Admin@123"); // هنا الباسورد عادي

//        if (result.Succeeded)
//        {
//            await userManager.AddToRoleAsync(adminUser, "Admin");
//        }
//    }
//}