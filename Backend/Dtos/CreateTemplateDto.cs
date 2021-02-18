using System;
namespace Email.Management.Dtos
{
    public class CreateTemplateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsHtml { get; set; }
        public long MailId { get; set; }
    }
}
