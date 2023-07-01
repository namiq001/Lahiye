using System.ComponentModel.DataAnnotations;

namespace NOOKX_Project.Models;

public class Product
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Product Name"), MaxLength(50, ErrorMessage = "Dont't play with Incpect")]
    public string ProductName { get; set; } = null!;
    public Double NewPrice { get; set; }
    public Double OldPrice { get; set; }
    public int Percentage { get; set; }
    public int CatagoryId { get; set; }
    public Catagory Catagories { get; set;}
    public string ProductImageUrl { get; set; } = null!;
}
