using NuGet.Common;

namespace NOOKX_Project.ViewModels.ProductVM;

public class CartVM
{
    public int Id { get; set; }
    public double Price { get; set; }
    public string ProductName { get; set; } = null!;
    public int Count { get; set; }
    public string ImageName { get; set; } = null!;
    public string CatagoryName { get; set; } = null!;
}
