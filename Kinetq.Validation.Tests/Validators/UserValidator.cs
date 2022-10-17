using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Tests.Dtos;
using Kinetq.Validation.Validators.Models;

namespace Kinetq.Validation.Tests.Validators;

public class UserValidator : IValidator<UserDto>
{
    public async Task Execute(UserDto dto, ValidationErrors validationErrors)
    {
        if (string.IsNullOrEmpty(dto.FirstName))
        {
            validationErrors.Add(GetName(nameof(UserDto.FirstName), null), "First name needs to be supplied");
        }

        if (string.IsNullOrEmpty(dto.LastName))
        {
            validationErrors.Add(GetName(nameof(UserDto.LastName), null), "Last name needs to be supplied");
        }
    }

    public int Order { get; }   
    public IValidatorFactory ValidatorFactory { get; set; }
    public Func<string, int?, string> GetName { get; set; }
}