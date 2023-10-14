using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class Service
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Stevenhuang1027SampleDbContext _dbContext;
        
        public Service(IMemoryCache memoryCache, Stevenhuang1027SampleDbContext dbContext)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }

        public List<Product> GetAllProducts()
        {
            if (!_memoryCache.TryGetValue("AllProducts", out List<Product> products))
            {
                // 如果快取中沒有資料，從資料庫中讀取資料
                products = _dbContext.Products.ToList();

                // 將結果存儲在快取中，設定過期時間
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12) // 這裡設定了12小時的過期時間
                };
                _memoryCache.Set("AllProducts", products, cacheEntryOptions);
            }
            return products;
        }

        public List<Gallery> GetAllGalleries()
        {
            if (!_memoryCache.TryGetValue("AllGalleries", out List<Gallery> galleries))
            {
                galleries = _dbContext.Galleries.ToList();

                // 將結果存儲在快取中，設定過期時間
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12) // 這裡設定了12小時的過期時間
                };
                _memoryCache.Set("AllGalleries", galleries, cacheEntryOptions);
            }
            return galleries;
        }

        public List<Group> GetAllGroups()
        {
            if (!_memoryCache.TryGetValue("AllGroups", out List<Group> groups))
            {
                groups = _dbContext.Groups.ToList();

                // 將結果存儲在快取中，設定過期時間
                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12) // 這裡設定了12小時的過期時間
                };
                _memoryCache.Set("AllGalleries", groups, cacheEntryOptions);
            }
            return groups;
        }

        /// <summary>
        /// 清空快取
        /// </summary>
        public void ClearCache()
        {
            _memoryCache.Remove("AllProducts");
            _memoryCache.Remove("AllGalleries");
            _memoryCache.Remove("AllGroups");
        }
    }
}
