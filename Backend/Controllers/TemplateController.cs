using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Persistence;
using Email.Management.Domain;
using Email.Management.Dtos;
using Email.Management.Exceptions;
using Email.Management.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stubble.Core.Builders;

namespace Email.Management.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TemplateController : ControllerBase
    {
        private readonly DaoContext context;
        private readonly JwtUserDto user;
        private readonly EncryptionProvider encryption;
        public TemplateController(DaoContext context, JwtUserDto user, EncryptionProvider encryption)
        {
            this.context = context;
            this.user = user;
            this.encryption = encryption;
        }

        [HttpGet("List")]
        public List<Template> GetTemplates([FromQuery] int offset, [FromQuery] int size)
        {
            return context.Set<Template>()
                .Select(x => new Template
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
               .Where(x => x.User.Id == user.Id)
               .OrderByDescending(x => x.Id)
               .Skip(offset)
               .Take(size)
               .ToList();
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CreateTemplateDto templateDto)
        {
            Template template = new Template
            {
                Content = templateDto.Content,
                Description = templateDto.Description,
                IsHtml = templateDto.IsHtml,
                Name = templateDto.Name,
                Subject = templateDto.Subject,
                MailId = templateDto.MailId,
                UserId = user.Id
                
            };

            context.Add(template);
            context.SaveChanges();

            return Ok();
        }

        [HttpPost("Send/{id}")]
        public async Task<IActionResult> Send(long id, [FromBody] SendMailDto sendMail)
        {
            Template template;
            try
            {
                template = await context.Set<Template>()
                    .FirstAsync(x => x.User.Token == sendMail.Token && x.Id == id);
            }
            catch
            {
                throw new TemplateNotFoundException();
            }

            Mail mail;
            try
            {
                mail = await context.Set<Mail>()
                    .FirstAsync(x => x.Id == sendMail.MailId && x.User.Id == user.Id);
            }
            catch
            {
                throw new MailNotFoundException();
            }

            var stubble = new StubbleBuilder().Build();
            var subject = await stubble.RenderAsync(template.Subject, sendMail.Args);
            var body = await stubble.RenderAsync(template.Content, sendMail.Args);
            var from = new MailAddress(mail.EmailAddress, mail.Name);
            var password = encryption.Decrypt(mail.Password, sendMail.Secret);

            foreach (var recipient in sendMail.Recipients)
            {
                var to = new MailAddress(recipient);

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
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch(Exception ex)
                {
                    throw new BusinessException("smtp-error", $"Something happened when sending the email to {recipient} via SMTP: {ex.Message}", ex);
                }
            }

            return Ok();
        }
    }
}
