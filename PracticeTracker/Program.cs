using App.Data.Context;
using App.Models;
using App.Repo;
using App.Security;
using App.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISqlDbContext, SqlDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddScoped<UserRegistrationService>();
builder.Services.AddScoped<UserAuthenticationService>();
builder.Services.AddScoped<IPasswordHasher<User>, BCryptPasswordHasher>();


builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IPracticeSessionRepo, PracticeSessionRepo>();
builder.Services.AddScoped<ISetlistRepo, SetlistRepo>();
builder.Services.AddScoped<ISongRepo, SongRepo>();

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
app.UseAntiforgery();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=UserHub}/{id?}");

app.Run();
