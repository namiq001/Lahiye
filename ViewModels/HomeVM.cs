using NOOKX_Project.Models;

namespace NOOKX_Project.ViewModels;

public class HomeVM
{
    public List<Catagory> Catagories { get; set; }
    public List<Product> Products { get; set; }
    public List<Service> Services { get; set; }
    public List<Slider> Sliders { get; set; }
}
