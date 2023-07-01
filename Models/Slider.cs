using System.ComponentModel.DataAnnotations;

namespace NOOKX_Project.Models;

public class Slider
{
    public  int Id { get; set; }
    [Required(ErrorMessage = "Slider Title"),MaxLength(50,ErrorMessage = "Don't play with Incpect")]
    public string Title { get; set; } = null!;
    [Required(ErrorMessage = "Slider Description"), MaxLength(50, ErrorMessage = "Don't play with Incpect")]
    public string Description { get; set; } = null!;
    public string SliderImageUrl { get; set; } = null!;
}
