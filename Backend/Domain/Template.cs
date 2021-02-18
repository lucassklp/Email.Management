using Backend.Domain;

namespace Email.Management.Domain
{
    public class Template
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsHtml { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public long MailId { get; set; }
        public Mail Mail { get; set; }
    }
}
