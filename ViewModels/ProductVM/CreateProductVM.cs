using NOOKX_Project.Models;

namespace NOOKX_Project.ViewModels.ProductVM;

public class CreateProductVM
{
    public string ProductName { get; set; } = null!;
    public double OldPrice { get; set; }
    public double NewPrice { get; set; }
    public int Percentage { get; set; }
    public int CatagoryId { get; set; }
    public IFormFile Image { get; set; } = null!;
    public List<Catagory>? Catagories { get; set; }
}
