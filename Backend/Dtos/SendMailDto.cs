using System;
using System.Collections.Generic;

namespace Email.Management.Dtos
{
    public class SendMailDto
    {
        public Guid Token { get; set; }
        public List<RecipientDto> Recipients { get; set; }

        public class RecipientDto
        {
            public string Email { get; set; }
            public Dictionary<string, string> Args { get; set; }
        }
    }
}
