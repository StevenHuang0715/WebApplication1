using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Stevenhuang1027SampleDbContext _dbContext;
       
        public HomeController(ILogger<HomeController> logger, Stevenhuang1027SampleDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewModel model = new ViewModel();

            #region 組合類別
            model.Groups = new List<ViewModelGroup>();
            List<Group> allGroups = _dbContext.Groups.ToList();
            foreach (Group item in allGroups)
            {
                model.Groups.Add(new ViewModelGroup()
                {
                    GroupId = item.GroupId,
                    GroupName = item.GroupName
                });
            }
            #endregion

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

            #region 組合相片圖庫
            model.Galleries = new List<ViewModelGalleries>();
            List<Gallery> GalList = _dbContext.Galleries.OrderBy(g => g.Order).ToList();
            foreach (Gallery gallery in GalList)
            {
                model.Galleries.Add(
                    new ViewModelGalleries()
                    {
                        Orders = gallery.Order,
                        ImageString = Utility.ToBase64Image(gallery.Image),
                        Show = (bool)gallery.IsShow ? "1" : "0"
                    });
            }
            #endregion

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}