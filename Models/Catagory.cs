using System.ComponentModel.DataAnnotations;

namespace NOOKX_Project.Models;

public class Catagory
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Catagory Name"), MaxLength(20, ErrorMessage = "Don't play with Incpect")]
    public string CatagoryName { get; set; } = null!;
    public List<Product> Products { get; set; }
}
