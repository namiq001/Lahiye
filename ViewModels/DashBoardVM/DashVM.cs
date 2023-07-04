using NOOKX_Project.Models;

namespace NOOKX_Project.ViewModels.DashBoardVM;

public class DashVM
{
    public List<Product>? Products { get; set; } 
    public List<Cantact>? Cantacts { get; set; } 
    public List<CantactIcon>? CantactIcons { get; set; } 
    public List<Catagory>? Catagories { get; set; } 
    public List<Service>? Services { get; set; } 
    public List<Setting>? Settings { get; set; } 
    public List<Slider>? Sliders { get; set; }
}
