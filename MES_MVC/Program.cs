using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Events;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Đăng ký MVC và JSON options
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Đăng ký các dịch vụ custom
builder.Services.AddService();
builder.Services.AddServiceERP();
builder.Services.AddContext();
builder.Services.AddRepository();
builder.Services.AddRepositoryERP();
builder.Services.AddHostedService<CleanupDownloadService>();

// Đăng ký DataProtection
var keyPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys");
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keyPath))
    .SetApplicationName("MESKeyShared");

// Đăng ký HttpContext và CookieHelper (đúng thứ tự)
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<CookieHelper>();

var app = builder.Build();

//đang ký cache và khởi tạo CacheHelper
using (var scope = app.Services.CreateScope())
{
    var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();
    CacheHelper.Initialize(cache);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware cấu hình ngôn ngữ và route data (dùng Cookie, không dùng Session)
app.Use(async (context, next) =>
{
    string cookieValue = string.Empty;

    // Thiết lập ngôn ngữ
    if (context.Request.Cookies.TryGetValue(GlobalHelper.Language, out cookieValue))
    {
        var culture = new System.Globalization.CultureInfo(cookieValue);
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
    }
    else
    {
        var defaultCulture = new System.Globalization.CultureInfo(GlobalHelper.LanguageCodeEN);
        System.Threading.Thread.CurrentThread.CurrentCulture = defaultCulture;
        System.Threading.Thread.CurrentThread.CurrentUICulture = defaultCulture;

        // Lưu vào Cookie nếu chưa có
        context.Response.Cookies.Append(GlobalHelper.Language, GlobalHelper.LanguageCodeEN, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        });
    }

    // Thiết lập menu cha
    if (!context.Request.Cookies.TryGetValue(GlobalHelper.MenuParent, out cookieValue))
    {
        cookieValue = "-1";
        context.Response.Cookies.Append(GlobalHelper.MenuParent, cookieValue, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        });
    }

    await next.Invoke();
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Sử dụng middleware xác thực người dùng
app.UseMiddleware<AuthMiddleware>();

// Định tuyến mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MAIN_Login}/{action=Index}/{id?}");

app.Run();
