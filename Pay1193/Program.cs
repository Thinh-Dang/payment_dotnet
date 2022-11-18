using Microsoft.EntityFrameworkCore;
using Pay1193.Persistence;
using Pay1193.Services;
using Pay1193.Services.Implement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbContext ,ApplicationDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Pay1193"));
});

builder.Services.AddScoped<IEmployee, EmployeeService>();
builder.Services.AddScoped<IPayService, PayService>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<INationalInsuranceService, NationalInsureanceService>();
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
