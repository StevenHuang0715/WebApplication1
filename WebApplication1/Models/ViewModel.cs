namespace WebApplication1.Models
{
    public class ViewModel
    {
        public List<ViewModelGroup>? Groups { get; set; }

        public List<ViewModelProduct>? Products { get; set; }

        public List<ViewModelGalleries>? Galleries { get; set; }
    }

    public class ViewModelGroup
    {
        public int GroupId { get; set; }

        public string? GroupName { get; set; }
    }

    public class ViewModelProduct
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Desc { get; set; }

        public int? GroupId { get; set; }

        public string? GroupName { get; set; }

        public int? Price { get; set; }

        public int? Stock { get; set; }

        public string? ImageUrl { get; set; }

        public string ImageBase64 { get; set; }
    }

    public class ViewModelGalleries
    {
        public int? Orders { get; set; }

        public string? ImageUrl { get; set; }
    }
}
