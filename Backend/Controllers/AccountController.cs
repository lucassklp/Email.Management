﻿using System;
using System.Linq;
using System.Text;
using Backend.Domain;
using Backend.Persistence;
using Email.Management.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Email.Management.Exceptions;
using Email.Management.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

namespace Email.Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly DaoContext context;
        public AccountController(IConfiguration configuration, DaoContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] CredentialDto credential)
        {
            User user;
            try
            {
                credential.Password = credential.Password.ToSha512();
                user = context.Set<User>()
                    .Where(x => x.Email == credential.Email && x.Password == credential.Password)
                    .Select(x => new User
                    {
                        Id = x.Id,
                        Email = x.Email
                    })
                    .First();
            }
            catch(Exception ex)
            {
                throw new InvalidCredentialException();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = new TokenDto();
            token.Token = tokenHandler.WriteToken(securityToken);

            return Ok(token);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterUserDto registrationUser)
        {
            var user = new User();
            user.Password = registrationUser.Password.ToSha512();
            user.Token = Guid.NewGuid();
            user.Email = registrationUser.Email;

            context.Add(user);
            context.SaveChanges();

            return Ok();
        }
    }
}
