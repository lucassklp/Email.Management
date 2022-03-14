namespace Email.Management.Dtos
{
    public class TemplateDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsHtml { get; set; }
        public long MailId { get; set; }
    }
}
