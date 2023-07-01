using System.ComponentModel.DataAnnotations;

namespace NOOKX_Project.ViewModels.AccountVM;

public class RecetPasswordVM
{
    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;
    [Required]
    [DataType(DataType.Password), Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Token { get; set; } = null!;

}
