﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models
{
    public class EditProductModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Desc { get; set; }

        public int? GroupId { get; set; }

        public int? Price { get; set; }

        public int? Stock { get; set; }

        public byte[]? Image { get; set; }

        public string? ImageString { get; set; }

        public string? ImageUrl{ get; set; }


        public List<SelectListItem>? Groups { get; set; }
    }
}
