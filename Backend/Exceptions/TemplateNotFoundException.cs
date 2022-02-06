namespace Email.Management.Exceptions
{
    public class TemplateNotFoundException : BusinessException
    {
        public TemplateNotFoundException() : base("template-not-found", "The template was not found for this user")
        {
        }
    }
}
