using MessagePack.Formatters;

namespace NOOKX_Project.ViewModels.SliderVM;

public class EditSliderVM
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? SliderImageUrl { get; set; }
    public IFormFile? Image { get; set; }


}
