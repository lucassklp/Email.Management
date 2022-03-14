using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Backend.Persistence;
using Email.Management.Domain;
using Email.Management.Dtos;
using Email.Management.Exceptions;
using Email.Management.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stubble.Core.Builders;

namespace Email.Management.Controllers
{
    [ApiController]
    [Route("api/templates")]
    [Authorize]
    public class TemplatesController : ControllerBase
    {
        private readonly DaoContext context;
        private readonly JwtUserDto user;
        private readonly EncryptionProvider encryption;
        private readonly IConfiguration configuration;
        public TemplatesController(DaoContext context, JwtUserDto user, EncryptionProvider encryption, IConfiguration configuration)
        {
            this.context = context;
            this.user = user;
            this.encryption = encryption;
            this.configuration = configuration;
        }

        [HttpGet("{id}")]
        public TemplateDto Get(long id)
        {
            return context.Set<Template>()
                .Where(x => x.Id == id && user.Id == x.User.Id)
                .OrderByDescending(x => x.Id)
                .ToList()
                .Select(x => new TemplateDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    Description = x.Description,
                    IsHtml = x.IsHtml,
                    Name = x.Name,
                    Subject = x.Subject,
                    MailId = x.MailId
                })
                .First();
        }

        [HttpGet]
        public PagedResultDto<Template> ListTemplates([FromQuery] int offset, [FromQuery] int size)
        {

            var query = context.Set<Template>()
                .Where(x => x.User.Id == user.Id);

            var total = query.Count();

            var result = query
                .OrderByDescending(x => x.Id)
                .Skip(offset)
                .Take(size)
                .Select(x => new Template
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
               .ToList();


            return new PagedResultDto<Template>
            {
                Total = total,
                Content = result
            };
        }

        [HttpPost]
        public TemplateDto Save([FromBody] TemplateDto templateDto)
        {
            Template template = new Template
            {
                Id = templateDto.Id,
                Content = templateDto.Content,
                Description = templateDto.Description,
                IsHtml = templateDto.IsHtml,
                Name = templateDto.Name,
                Subject = templateDto.Subject,
                MailId = templateDto.MailId,
                UserId = user.Id
            };

            context.Update(template);
            context.SaveChanges();

            templateDto.Id = template.Id;

            return templateDto;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = context.Set<Template>()
                .Where(x => x.Id == id)
                .Select(x => new Template
                {
                    Id = x.Id
                })
                .First();

            context.Remove(user);
            context.SaveChanges();

            return Ok();
        }

        [HttpPost("test")]
        [Authorize]
        public async Task<IActionResult> Test(TestTemplateDto template)
        {
            Mail mail;
            try
            {
                mail = context.Set<Mail>()
                    .Where(x => x.User.Id == user.Id && x.Id == template.MailId)
                    .First();
            }
            catch(Exception ex)
            {
                throw new BusinessException("mail-not-found", "The provided mail was not found" , ex);
            }

            var stubble = new StubbleBuilder().Build();
            var from = new MailAddress(mail.EmailAddress);
            var to = new MailAddress(template.Recipient.Email);
            var password = encryption.Decrypt(mail.Password, template.Secret);
            var subject = await stubble.RenderAsync(template.Subject, template.Recipient.Args);
            var body = await stubble.RenderAsync(template.Content, template.Recipient.Args);

            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = template.IsHtml
            };

            using var smtp = new SmtpClient(mail.Host, mail.Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = mail.EnableSsl,
                Credentials = new NetworkCredential(mail.EmailAddress, password)
            };

            await smtp.SendMailAsync(message);

            return Ok();
        }

        [HttpPost("send/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Send(long id, [FromBody] SendMailDto sendMail)
        {
            Template template;
            try
            {
                template = await context.Set<Template>()
                    .Include(x => x.Mail)
                    .FirstAsync(x => x.User.Token == sendMail.Token && x.Id == id);
            }
            catch
            {
                throw new TemplateNotFoundException();
            }

            var secret = configuration.GetValue<string>("PasswordSecret");
            var stubble = new StubbleBuilder().Build();
            var from = new MailAddress(template.Mail.EmailAddress, template.Mail.Name);
            var password = encryption.Decrypt(template.Mail.Password, secret);

            foreach (var recipient in sendMail.Recipients)
            {

                var subject = await stubble.RenderAsync(template.Subject, recipient.Args);
                var body = await stubble.RenderAsync(template.Content, recipient.Args);
                var to = new MailAddress(recipient.Email);

                var message = new MailMessage(from, to)
                {
                    Subject = subject,
                    Body = body,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = template.IsHtml
                };

                using var smtp = new SmtpClient(template.Mail.Host, template.Mail.Port)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    EnableSsl = template.Mail.EnableSsl,
                    Credentials = new NetworkCredential(template.Mail.EmailAddress, password)
                };
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch(Exception ex)
                {
                    throw new BusinessException("smtp-error", $"Something happened when sending the email to {recipient.Email} via SMTP: {ex.Message}", ex);
                }
            }

            return Ok();
        }
    }
}
