using System;
namespace Email.Management.Exceptions
{
    public class BusinessException : Exception
    {
        public string Token { get; set; }
        public BusinessException(string token, string message) : base(message)
        {
            this.Token = token;
        }

        public BusinessException(string token, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Token = token;
        }
    }
}
