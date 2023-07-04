using System.ComponentModel.DataAnnotations;

namespace NOOKX_Project.ViewModels.SendMessageVM;

public class SendVM
{
    [EmailAddress]
    public string EmailAddress { get; set; } = null!;
    public string Message { get; set; }
}
