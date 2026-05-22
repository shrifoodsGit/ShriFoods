using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using ShriFoods.Model;
using ShriFoods.Pages;
using ShriFoods.Pages.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//--------------------------------------------------
// Services
//--------------------------------------------------

builder.Services.AddRazorPages();

builder.Services.AddControllers();

builder.Services.AddOutputCache();

//--------------------------------------------------
// Database
//--------------------------------------------------

builder.Services.AddDbContext<FoodDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureSqlConnection")));

//--------------------------------------------------
// Session
//--------------------------------------------------

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);

    options.Cookie.HttpOnly = true;

    options.Cookie.IsEssential = true;

    options.Cookie.SecurePolicy =
        CookieSecurePolicy.Always;
});

//--------------------------------------------------
// Secure Cookies
//--------------------------------------------------

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;

    options.Cookie.SecurePolicy =
        CookieSecurePolicy.Always;

    options.Cookie.SameSite =
        SameSiteMode.Lax;
});

//--------------------------------------------------
// Email Service
//--------------------------------------------------

var emailJson =
    builder.Configuration["EmailSettingsJson"];

EmailSettings? emailSettings = null;

if (!string.IsNullOrEmpty(emailJson))
{
    emailSettings =
        JsonSerializer.Deserialize<EmailSettings>(emailJson);

    builder.Services.AddSingleton(emailSettings);
}

builder.Services.AddScoped<EmailService>();

//--------------------------------------------------
// SMS Service
//--------------------------------------------------

builder.Services.AddScoped<SmsService>();

//--------------------------------------------------
// PDF Service
//--------------------------------------------------

QuestPDF.Settings.License =
    LicenseType.Community;

builder.Services.AddScoped<PdfService>();

//--------------------------------------------------
// CORS Policy
//--------------------------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowShriFoods",
        policy =>
        {
            policy.WithOrigins(
                    "https://shrifoods.in",
                    "https://www.shrifoods.in",
                    "https://shrifoods-dgb4dhbbhpeud7gd.canadacentral-01.azurewebsites.net")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

//--------------------------------------------------
// Production Error Handling
//--------------------------------------------------

if (!app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

//--------------------------------------------------
// Security Headers
//--------------------------------------------------

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Frame-Options"] =
        "DENY";

    context.Response.Headers["X-Content-Type-Options"] =
        "nosniff";

    context.Response.Headers["Referrer-Policy"] =
        "strict-origin-when-cross-origin";

    context.Response.Headers["Permissions-Policy"] =
        "geolocation=(), microphone=()";

    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self' https: data: 'unsafe-inline' 'unsafe-eval';";

    await next();
});

//--------------------------------------------------
// HTTPS
//--------------------------------------------------

app.UseHttpsRedirection();

//--------------------------------------------------
// Static Files
//--------------------------------------------------

app.UseStaticFiles();

//--------------------------------------------------
// Routing
//--------------------------------------------------

app.UseRouting();

//--------------------------------------------------
// CORS
//--------------------------------------------------

app.UseCors("AllowShriFoods");

//--------------------------------------------------
// Session
//--------------------------------------------------

app.UseSession();

//--------------------------------------------------
// Visitor Tracking Middleware
//--------------------------------------------------

app.UseMiddleware<TrackingMiddleware>();

//--------------------------------------------------
// Authorization
//--------------------------------------------------

app.UseAuthorization();

//--------------------------------------------------
// Output Cache
//--------------------------------------------------

app.UseOutputCache();

//--------------------------------------------------
// Controllers + Razor Pages
//--------------------------------------------------

app.MapControllers();

app.MapRazorPages();

//--------------------------------------------------
// Run Application
//--------------------------------------------------

app.Run();