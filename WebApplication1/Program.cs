using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// 添加 Cloudinary 配置
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();
var cloudinarySettings = configuration.GetSection("CloudinarySettings");
var account = new Account(
    cloudinarySettings["CloudName"],
    cloudinarySettings["ApiKey"],
    cloudinarySettings["ApiSecret"]
);

var cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary); //註冊cloudinary服務

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Stevenhuang1027SampleDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Stevenhuang1027SampleDB")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// cloudinary.
app.MapPost("/upload", (HttpContext context) =>
{
    var file = context.Request.Form.Files["file"];

    var uploadParams = new ImageUploadParams
    {
        File = new FileDescription(file.FileName, file.OpenReadStream())
        //可以設置其他參數，如文件夾路徑、公共ID等
    };

    var uploadResult = cloudinary.Upload(uploadParams);

    //處理上傳結果，獲取圖片URKL、公共ID等
    var imageUrl = uploadResult.SecureUrl.ToString();
    var publicId = uploadResult.PublicId;

    context.Response.WriteAsJsonAsync(new { ImageUrl = imageUrl, PublicId = publicId });
});

app.Run();
