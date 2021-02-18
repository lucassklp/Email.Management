using System;
using System.Collections.Generic;

namespace Email.Management.Dtos
{
    public class SendMailDto
    {
        public Guid Token { get; set; }
        public string Secret { get; set; }
        public long MailId { get; set; }
        public List<string> Recipients { get; set; }
        public Dictionary<string, string> Args { get; set; }
    }
}
