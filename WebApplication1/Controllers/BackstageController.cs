using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApplication1.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class BackstageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly YummyDbContext _dbContext;

        public BackstageController(ILogger<HomeController> logger, YummyDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            LoginModel allAccounts = Utility.getLoginModel();
            return View();
        }

        public IActionResult ProductList()
        {
            ProductListModel model = new ProductListModel();
            #region 組合商品
            model.Products = new List<ViewModelProduct>();
            List<Product> allProducts = _dbContext.Products.ToList();
            foreach (Product product in allProducts)
            {
                Group? group = _dbContext.Groups.SingleOrDefault(p => p.GroupId == product.GroupId);
                model.Products.Add(new ViewModelProduct()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Desc = product.Desc,
                    GroupId = product.GroupId,
                    GroupName = group?.GroupName,
                    Price = product.Price,
                    Stock = product.Stock,
                    ImageUrl = product.ImageUrl
                });
            }
            #endregion
            return View(model);
        }

        public IActionResult EditProduct(int ProdId)
        {
            EditProductModel model = new EditProductModel();
            Product? product = _dbContext.Products.SingleOrDefault(p => p.Id == ProdId);
            model.Id = product.Id;
            model.Name = product.Name;
            model.Desc = product.Desc;
            model.Price = product.Price;
            model.Stock = product.Stock;
            model.ImageUrl = product.ImageUrl;
            model.GroupId = product.GroupId;

            List<Group> GroupList = _dbContext.Groups.ToList();
            model.Groups = new List<SelectListItem>();
            foreach (Group group in GroupList)
            {
                model.Groups.Add(new SelectListItem()
                {
                    Text = group.GroupName,
                    Value = group.GroupId.ToString()
                });
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult EditProductPage02(EditProductModel Model, IFormFile myimg)
        {
            try
            {
                Product updateProd = new()
                {
                    Id = Model.Id,
                    Name = Model.Name,
                    Desc = Model.Desc,
                    GroupId = Model.GroupId,
                    Price = Model.Price,
                    Stock = Model.Stock,
                    ImageUrl = Model.ImageUrl
                };

                _dbContext.Products.Update(updateProd);
                _dbContext.SaveChanges();
                return RedirectToAction("ProductList");
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 登入頁
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult LoginPage1()
        {

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string Usr, string Pwd)
        {
            try
            {
                LoginModel LoginModel = Utility.getLoginModel();
                LoginInfo? member = LoginModel.allAccts.SingleOrDefault(p => p.Username == Usr);
                bool match = false;
                if (member != null)
                {
                    match = member.Password == Pwd;
                    if (match)
                    {
                        //驗證
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, Usr),
                            new Claim("FullName", Usr),
                           // new Claim(ClaimTypes.Role, "Administrator")
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return View("Index");
                    }
                }

                return View("LoginPage1");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("LoginPage1");
        }



    }
}

//更新資料庫指令：
//Scaffold-DbContext "Server=YummyDb.mssql.somee.com;Database=YummyDb;User ID=steven35741_SQLLogin_1;Password=91s79tpezy;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force