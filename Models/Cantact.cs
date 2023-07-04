using System.ComponentModel.DataAnnotations;

namespace NOOKX_Project.Models;

public class Cantact
{
    public int Id { get; set; }
    [StringLength(maximumLength: 255)]
    public string Message { get; set; } = null!;
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;  
    public DateTime? SendedDate { get; set; }
}
