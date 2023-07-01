using NOOKX_Project.Models;

namespace NOOKX_Project.ViewModels.ProductVM;

public class EditProductVM
{
    public string? ProductName { get; set; } 
    public string? ProductImageUrl { get; set; } 
    public double OldPrice { get; set; }
    public double NewPrice { get; set; }
    public int Percentage { get; set; }
    public int CatagoryId { get; set; }
    public IFormFile? Image { get; set; } 
    public List<Catagory>? Catagories { get; set; }

}
