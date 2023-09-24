using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// �K�[ Cloudinary �t�m
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
builder.Services.AddSingleton(cloudinary); //���Ucloudinary�A��

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
        //�i�H�]�m��L�ѼơA�p��󧨸��|�B���@ID��
    };

    var uploadResult = cloudinary.Upload(uploadParams);

    //�B�z�W�ǵ��G�A����Ϥ�URKL�B���@ID��
    var imageUrl = uploadResult.SecureUrl.ToString();
    var publicId = uploadResult.PublicId;

    context.Response.WriteAsJsonAsync(new { ImageUrl = imageUrl, PublicId = publicId });
});

app.Run();
