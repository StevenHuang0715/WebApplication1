using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BackstageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyTestDb0813Context _dbContext;

        public BackstageController(ILogger<HomeController> logger, MyTestDb0813Context dbContext)
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
            model.Name= product.Name;
            model.Desc= product.Desc;
            model.Price= product.Price;
            model.Stock= product.Stock;
            model.ImageUrl= product.ImageUrl;
            model.GroupId= product.GroupId;

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
    }
}
