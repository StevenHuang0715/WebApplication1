using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models
{
    public class AddProductModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Desc { get; set; }

        public int? GroupId { get; set; }

        public int? Price { get; set; }

        public int? Stock { get; set; }

        public IFormFile? Image { get; set; }

        public List<SelectListItem>? Groups { get; set; }

        public string? ImageUrl { get; set; }
    }
}
