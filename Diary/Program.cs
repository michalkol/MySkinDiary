using Diary.Data;
using Microsoft.EntityFrameworkCore;
using Diary.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Diary.Models;
using Microsoft.IdentityModel.Tokens;
using Diary.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Napojen� datab�ze pomoc� connection stringu v appsettings.json

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); //Služba pro připojení databáze


//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MonsterASP")));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); //DI pro Identity
builder.Services.AddScoped<RecordService>(); //Servisní třída
builder.Services.AddScoped<RecordServiceApi>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
});

// API
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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
// Aktivace Swaggeru
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseCors();
app.MapControllers();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//Autentizace pomocí Identity Framework
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Mapování endpointů
//app.MapRecordDTOEndpoints();

//await ImportDataAsync(app.Services);
app.Run();




//Manualní naplnění databáze dummy daty

static async Task ImportDataAsync(IServiceProvider services)
{
    var filePath = "E:\\Diary\\Diary\\bin\\Debug\\net8.0\\import_24_11_2024.json"; // Cesta k JSON souboru

    // Načtení dat z JSON souboru
    if (!File.Exists(filePath))
    {
        Console.WriteLine("Soubor neexistuje.");
        return;
    }

    var jsonData = await File.ReadAllTextAsync(filePath);
    var recordEntries = JsonSerializer.Deserialize<List<RecordEntry>>(jsonData);

    if (recordEntries == null || recordEntries.Count == 0)
    {
        Console.WriteLine("Soubor neobsahuje žádná data.");
        return;
    }

    // Získání služby DbContext
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Uložení dat do databáze
    await dbContext.RecordEntries.AddRangeAsync(recordEntries);
    await dbContext.SaveChangesAsync();

    Console.WriteLine("Data byla úspěšně importována!");
}
