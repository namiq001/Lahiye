using Org.BouncyCastle.Bcpg;

namespace NOOKX_Project.Models;

public class ProductComment
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public AppUser? appUser { get; set; }
    public int ProductId { get; set; }
    public Product? product { get; set; } 
    public byte Rating { get; set; }
}
