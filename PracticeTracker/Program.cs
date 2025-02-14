using App.Data.Context;
using App.Models;
using App.Repo;
using App.Security;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPasswordHasher<User>, BCryptPasswordHasher>();
builder.Services.AddScoped<ISqlDbContext, SqlDbContext>();
builder.Services.AddScoped<IPracticeSessionRepo, PracticeSessionRepo>();
builder.Services.AddScoped<ISetlistRepo, SetlistRepo>();
builder.Services.AddScoped<ISongRepo, SongRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=UserHub}/{id?}");

app.Run();
