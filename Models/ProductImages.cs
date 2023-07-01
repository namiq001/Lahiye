namespace NOOKX_Project.Models;

public class ProductImages
{
    public int Id { get; set; }
    public string ImageName { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
