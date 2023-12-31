﻿using NOOKX_Project.Models;

namespace NOOKX_Project.ViewModels.DetailVM;

public class ProductDetailVM
{
    public Product product { get; set; }
    public ProductComment productComment { get; set; }
    public List<ProductComment> productComments { get; set; }
    public List<Product> products { get; set; }
}
