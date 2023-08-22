using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Desc { get; set; }

    public int? GroupId { get; set; }

    public int? Price { get; set; }

    public int? Stock { get; set; }

    public string? ImageUrl { get; set; }

    public byte[]? Image { get; set; }
}
