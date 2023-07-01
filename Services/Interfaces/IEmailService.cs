using NOOKX_Project.ViewModels.MailSenderVM;

namespace NOOKX_Project.Services.Interfaces;

public interface IEmailService
{
    void SendMessage(MailRequestVM mailRequest);
}
