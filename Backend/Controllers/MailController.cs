using System;
using System.Linq;
using Backend.Domain;
using Backend.Persistence;
using Email.Management.Domain;
using Email.Management.Dtos;
using Email.Management.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Email.Management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class MailController : ControllerBase
    {
        private readonly DaoContext context;
        private readonly JwtUserDto user;
        private readonly EncryptionProvider encryption;
        public MailController(DaoContext context, JwtUserDto user, EncryptionProvider encryption)
        {
            this.context = context;
            this.user = user;
            this.encryption = encryption;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] MailDto mailDto)
        {
            var secret = Guid.NewGuid().ToString();
            var user = context.Set<User>().First(x => x.Id == this.user.Id);
            var mail = new Mail
            {
                EmailAddress = mailDto.EmailAddress,
                EnableSsl = mailDto.EnableSsl,
                Host = mailDto.Host,
                Name = mailDto.Name,
                Password = encryption.Encrypt(mailDto.Password, secret),
                Port = mailDto.Port,
                User = user
            };

            context.Add(mail);
            context.SaveChanges();

            return Ok(new
            {
                secret
            });
        }
    }
}
