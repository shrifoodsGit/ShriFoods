using Microsoft.EntityFrameworkCore;
using ShriFoods.Model;
using ShriFoods.Pages;
using ShriFoods.Pages.Services;
using System.Text.Json;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddOutputCache();

//Database connection string
builder.Services.AddDbContext<FoodDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AzureSqlConnection")));

//Session
builder.Services.AddDistributedMemoryCache();//Required for Sesssion timeout
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);//Set Session timeout 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential= true;
});

//----------Cookies-------------------------
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


//--------------------Email Service------------------------------------------
var emailJson =
builder.Configuration["EmailSettingsJson"];
if (string.IsNullOrEmpty(emailJson))
{
    throw new Exception("EmailSettingsJson is missing in Azure App Settings");
}
var emailSettings =
JsonSerializer.Deserialize<EmailSettings>(emailJson);

builder.Services.AddScoped<EmailService>();

//Signleton for email 
builder.Services.AddSingleton(emailSettings);


//--------------------SmsService-----------------------------------------------
builder.Services.AddScoped<SmsService>();

//---------Pdf Service--------------------------------------------------- 
QuestPDF.Settings.License =
    LicenseType.Community;
builder.Services.AddScoped<PdfService>();


//-------------API Service------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Permissions-Policy"] = "geolocation=(), microphone=()";

    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self' https: data: 'unsafe-inline' 'unsafe-eval';";

    await next();
});

// Enable session middleware
app.UseSession(); 

//Number of Site visitors
app.UseMiddleware<TrackingMiddleware>();


app.UseCors("AllowAll");//Api service for App development 

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseHsts();
app.UseOutputCache();
app.MapRazorPages();
app.MapRazorPages();

app.Run();
