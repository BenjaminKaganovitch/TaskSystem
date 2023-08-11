using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SupportTicketSystem.Data;
using SupportTicketSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add Identity services to the container.
builder.Services.AddDbContext<SupportTicketSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SupportTicketSystemContext") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SupportTicketSystemContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

SeedData.Initialize(serviceProvider: app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Use authentication before authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

