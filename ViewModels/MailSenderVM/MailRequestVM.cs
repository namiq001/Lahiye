﻿namespace NOOKX_Project.ViewModels.MailSenderVM;

public class MailRequestVM
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<IFormFile> Attachments { get; set; }

}
