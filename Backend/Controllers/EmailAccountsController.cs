using System.Collections.Generic;
using System.Linq;
using Backend.Persistence;
using Email.Management.Domain;
using Email.Management.Dtos;
using Email.Management.Dtos.Output;
using Email.Management.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Email.Management.Controllers
{
    [ApiController]
    [Route("api/email-accounts")]
    [Authorize]
    public class EmailAccountsController : ControllerBase
    {
        private readonly DaoContext context;
        private readonly JwtUserDto user;
        private readonly EncryptionProvider encryption;
        private readonly IConfiguration configuration;
        public EmailAccountsController(DaoContext context, JwtUserDto user, EncryptionProvider encryption, IConfiguration configuration)
        {
            this.context = context;
            this.user = user;
            this.encryption = encryption;
            this.configuration = configuration;
        }

        [HttpDelete("{id}")]
        public void Delete(long id)
        {
            var item = context.Set<Mail>()
                .Where(x => x.UserId == user.Id && x.Id == id)
                .First();

            context.Remove(item);
            context.SaveChanges();
        }

        [HttpGet("{id}")]
        public MailDto Get(long id)
        {
            return context.Set<Mail>()
                .Where(x => x.UserId == user.Id && x.Id == id)
                .ToList()
                .Select(x => new MailDto
                {
                    Id = x.Id,
                    EmailAddress = x.EmailAddress,
                    EnableSsl = x.EnableSsl,
                    Host = x.Host,
                    Name = x.Name,
                    Port = x.Port
                })
                .First();
        }

        [HttpGet("list-all")]
        public List<SimpleMailOutputDto> ListAll()
        {
            return context.Set<Mail>()
                .Where(x => x.UserId == user.Id)
                .Select(x => new Mail
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList()
                .Select(x => new SimpleMailOutputDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
        }

        [HttpGet]
        public PagedResultDto<MailDto> GetTemplates([FromQuery] int offset, [FromQuery] int size)
        {
            var query = context.Set<Mail>()
                .Where(x => x.User.Id == user.Id);

            var total = query.Count();

            var result = query
                .OrderByDescending(x => x.Id)
                .Skip(offset)
                .Take(size)
                .Select(x => new MailDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    EmailAddress = x.EmailAddress,
                    EnableSsl = x.EnableSsl,
                    Host = x.Host,
                    Port = x.Port
                })
               .ToList();


            return new PagedResultDto<MailDto>
            {
                Total = total,
                Content = result
            };
        }

        [HttpPost]
        public MailDto Save([FromBody] InputMailDto mailDto)
        {
            var isUpdatePassword = (mailDto.Id != 0 && !string.IsNullOrEmpty(mailDto.Password))
                || mailDto.Id == 0;

            var secret = configuration.GetValue<string>("PasswordSecret");
            var encryptedPassword = encryption.Encrypt(mailDto.Password, secret);
            
            var mail = new Mail
            {
                Id = mailDto.Id,
                EmailAddress = mailDto.EmailAddress,
                EnableSsl = mailDto.EnableSsl,
                Host = mailDto.Host,
                Name = mailDto.Name,
                Password = isUpdatePassword ? encryptedPassword : string.Empty,
                Port = mailDto.Port,
                UserId = user.Id
            };

            context.Update(mail);
            context.Entry(mail).Property(x => x.Password).IsModified = isUpdatePassword;
            context.SaveChanges();

            return new MailDto
            {
                Id = mail.Id,
                EmailAddress = mail.EmailAddress,
                EnableSsl = mail.EnableSsl,
                Host = mail.Host,
                Name = mail.Name,
                Port = mail.Port
            };
        }
    }
}
