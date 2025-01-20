using Microsoft.EntityFrameworkCore;
using TeaDemo.DataAccess;
using TeaDemo.DataAccess.Repository;
using TeaDemo.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using TeaDemo.Utility;
using TeaDemo.DataAccess.Repository.IRepository.TeaDemo.DataAccess.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// **服務註冊區域**
// 註冊 Controller 與 View 支援
builder.Services.AddControllersWithViews();

// 設定資料庫連接，使用 SQL Server 並連接應用程式的資料庫
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 設定 Identity，用於處理用戶驗證和授權
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true; // 要求帳號註冊後需驗證
})
    .AddEntityFrameworkStores<ApplicationDbContext>() // 使用 Entity Framework 作為存儲提供者
    .AddDefaultTokenProviders(); // 加入預設的 Token 提供者，用於密碼重設等操作

// 設定應用程式的 Cookie 行為
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login"; // 登入頁面路徑
    options.LogoutPath = $"/Identity/Account/Logout"; // 登出頁面路徑
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied"; // 無權限訪問時的路徑
});

// 註冊 Razor Pages 支援
builder.Services.AddRazorPages();

// 註冊依賴注入的服務
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // 注入 UnitOfWork，用於封裝資料庫操作
builder.Services.AddScoped<IEmailSender, EmailSender>(); // 注入 EmailSender，用於發送電子郵件



// 可以註冊初始化數據庫的服務（目前被註解）
// builder.Services.AddScoped<IDbInitializer, DbInitializer>();

var app = builder.Build();

// **中介軟體配置區域**
// 設定錯誤處理管道
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // 當應用程式非開發模式時，跳轉到錯誤頁面
    app.UseHsts(); // 使用 HSTS，強制 HTTPS，預設為 30 天
}

// 強制 HTTPS 重定向
app.UseHttpsRedirection();
app.UseStaticFiles(); // 啟用靜態文件支持

app.UseRouting(); // 啟用路由支持

// 啟用身份驗證與授權
app.UseAuthentication(); // 使用身份驗證
app.UseAuthorization(); // 使用授權

// 配置 Razor Pages 路由
app.MapRazorPages();

// 設定預設路由（包括區域路由）
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"); // 預設區域為 Customer，控制器為 Home，動作為 Index

// 應用程式執行
app.Run();

// **初始化數據庫方法**
// 此方法用於啟動時初始化數據庫（目前被註解）
// void SeedDatabase()
// {
//     using (var scope = app.Services.CreateScope())
//     {
//         var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
//         dbInitializer.Initialize(); // 執行數據庫初始化
//     }
// }
