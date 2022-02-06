using static Email.Management.Dtos.SendMailDto;

namespace Email.Management.Dtos
{
    public class TestTemplateDto : TemplateDto
    {
        public RecipientDto Recipient { get; set; }
        public string Secret { get; set; }
    }
}
