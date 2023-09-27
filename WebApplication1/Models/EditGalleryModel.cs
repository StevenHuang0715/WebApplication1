namespace WebApplication1.Models
{
    public class EditGalleryModel
    {
        public int Id { get; set; }

        public int? Order { get; set; }

        public byte[]? Image { get; set; }

        public string? ImageString { get; set; }

        public string? ImageUrl { get; set; }

        /// <summary>
        /// 0：不顯示，1：顯示
        /// </summary>
        public string? Show { get; set; }
    }
}
