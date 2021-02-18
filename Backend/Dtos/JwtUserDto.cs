using System;
namespace Email.Management.Dtos
{
    public class JwtUserDto
    {
        public JwtUserDto(long id, string email)
        {
            Id = id;
            Email = email;
        }

        public string Email { get; private set; }
        public long Id { get; private set; }
    }
}
