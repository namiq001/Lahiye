namespace NOOKX_Project.ViewModels.SliderVM;

public class CreateSliderVM
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
}
