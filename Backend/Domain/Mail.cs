using System.Collections.Generic;
using Backend.Domain;

namespace Email.Management.Domain
{
    public class Mail
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public List<Template> Templates { get; set; }
    }
}
