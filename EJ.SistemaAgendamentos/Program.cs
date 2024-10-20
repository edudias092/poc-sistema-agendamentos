using EJ.SistemaAgendamentos.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(o => 
    o.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

builder.Services.AddIdentity<IdentityUser,IdentityRole>()
                .AddEntityFrameworkStores<MyDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRouting();

var app = builder.Build();

app.MapDefaultControllerRoute();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
