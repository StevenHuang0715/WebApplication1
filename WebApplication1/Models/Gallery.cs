﻿using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Gallery
{
    public int Id { get; set; }

    public int? Order { get; set; }

    public string? ImageUrl { get; set; }
}