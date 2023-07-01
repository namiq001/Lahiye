using System.ComponentModel.DataAnnotations;

namespace NOOKX_Project.ViewModels.AccountVM;

public class ForgotPasswordVM
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

}
