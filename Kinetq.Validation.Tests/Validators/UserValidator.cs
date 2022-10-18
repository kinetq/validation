using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Models;
using Kinetq.Validation.Tests.Dtos;

namespace Kinetq.Validation.Tests.Validators;

public class UserValidator : IValidator<UserDto>
{
    public async Task Execute(UserDto dto, ValidationErrors validationErrors)
    {
        if (string.IsNullOrEmpty(dto.FirstName))
        {
            validationErrors.Add(GetName(nameof(UserDto.FirstName)), "First name needs to be supplied");
        }

        if (string.IsNullOrEmpty(dto.LastName))
        {
            validationErrors.Add(GetName(nameof(UserDto.LastName)), "Last name needs to be supplied");
        }

        if (dto.Roles != null)
        {
            for (var i = 0; i < dto.Roles.Count; i++)
            {
                if (dto.Roles[i] == null)
                {
                    validationErrors.Add(GetNameWithIndex(nameof(UserDto.Roles), i), "Role cannot be null");
                }
            }
        }
    }

    public int Order { get; }
    public IValidatorFactory ValidatorFactory { get; set; }
    public Func<string, int, string> GetNameWithIndex { get; set; }
    public Func<string, string> GetName { get; set; }
}