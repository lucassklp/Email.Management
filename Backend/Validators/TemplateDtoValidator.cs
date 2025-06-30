using Email.Management.Dtos;
using FluentValidation;

namespace Email.Management.Validators;

public class TemplateDtoValidator:  AbstractValidator<TemplateDto>
{
    public TemplateDtoValidator()
    {
        RuleFor(x => x.ExternalId)
            .NotEmpty();
    }
}