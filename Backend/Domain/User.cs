using System;
using System.Collections.Generic;
using Email.Management.Domain;

namespace Backend.Domain
{
    public class User : IUser
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string Password { get; set; }

        public List<Template> Templates { get; set; }
        public List<Mail> Mails { get; set; }
    }
}