using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class BackstageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Stevenhuang1027SampleDbContext _dbContext;

        public BackstageController(ILogger<HomeController> logger, Stevenhuang1027SampleDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
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
                    ImageString = Utility.ToBase64Image(product.Image)
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
            model.GroupId = product.GroupId;
            model.Image = product.Image;
            model.ImageString = product.Image != null ? Utility.ToBase64Image(product.Image) : "";

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
        public IActionResult EditProductPage02(EditProductModel Model, IFormFile chgImg)
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
                    Stock = Model.Stock
                };

                using (MemoryStream ms = new())
                {
                    if (chgImg != null)
                    {
						chgImg.CopyTo(ms);
						updateProd.Image = ms.ToArray();
					}
                    else
                    {
                        if (Model.Image != null)
                        {
                            updateProd.Image = Model.Image;
						}
					}
				}

				_dbContext.Products.Update(updateProd);
                _dbContext.SaveChanges();
                return RedirectToAction("ProductList");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult AddProduct()
        {
            AddProductModel model = new AddProductModel();
            model.Groups = new List<SelectListItem>();
            foreach (Group group in _dbContext.Groups)
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
        public IActionResult AddProduct02(AddProductModel Model)
        {
            try
            {
                Product prod = new Product()
                {
                    Name = Model.Name,
                    Desc = Model.Desc,
                    GroupId = Model.GroupId,
                    Price = Model.Price,
                    Stock = Model.Stock
                };

                if (Model.Image != null)
                {
                    using MemoryStream ms = new MemoryStream();
                    Model.Image.CopyTo(ms);
                    prod.Image = ms.ToArray();
                }

                _dbContext.Products.Add(prod);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return View("AddProduct02");
        }

        [HttpPost]
        public IActionResult DelProduct(int ProdId)
        {
            try
            {
                Product? delProd = _dbContext.Products.SingleOrDefault(p => p.Id == ProdId);
                if (delProd != null)
                {
                    _dbContext.Remove(delProd);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("ProductList");
        }

        public IActionResult GalleryList()
        {
            List<EditGalleryModel> model = new List<EditGalleryModel>();
            var GalList = _dbContext.Galleries.ToList();
            foreach (var item in GalList)
            {
                model.Add(new EditGalleryModel
                {
                    Id = item.Id,
                    Image = item.Image,
                    Order = item.Order,
                    ImageString = Utility.ToBase64Image(item.Image),
                    Show = item.IsShow == true ? "1" : "0"
                });
            }

            return View(model);
        }

		[HttpPost]
		public IActionResult EditGallery(int GalId)
		{
			Gallery? editGal = _dbContext.Galleries.SingleOrDefault(g => g.Id == GalId);

            EditGalleryModel model = new()
            {
                Id = editGal.Id,
                Order = editGal.Order,
                Image = editGal.Image,
                Show = editGal.IsShow == true ? "1" : "0"
            };

			return View(model);
		}

		[HttpPost]
		public IActionResult EditGallery02(EditGalleryModel Model, IFormFile chgImg)
		{
			try
			{
                Gallery updateGal = new()
                {
                    Id = Model.Id,
                    Order = Model.Order,
                    IsShow = Model.Show == "1"
                };

				using (MemoryStream ms = new())
				{
					if (chgImg != null)
					{
						chgImg.CopyTo(ms);
						updateGal.Image = ms.ToArray();
					}
					else
					{
						if (Model.Image != null)
						{
							updateGal.Image = Model.Image;
						}
					}
				}

				_dbContext.Galleries.Update(updateGal);
				_dbContext.SaveChanges();
				return RedirectToAction("GalleryList");
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
		public IActionResult LoginPage1()
        {
            return View();
        }
    }
}

//更新資料庫指令：
//Scaffold-DbContext "Server=sql.bsite.net\MSSQL2016;Database=stevenhuang1027_SampleDB;User ID=stevenhuang1027_SampleDB;Password=DBSamplePW;TrustServerCertificate=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
