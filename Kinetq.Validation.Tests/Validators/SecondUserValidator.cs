using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Models;
using Kinetq.Validation.Tests.Dtos;

namespace Kinetq.Validation.Tests.Validators;

public class SecondUserValidator : IValidator<UserDto>
{
    public async Task Execute(UserDto dto, ValidationErrors validationErrors)
    {
        await ValidatorFactory.ValidateNested(dto.Address, GetName(nameof(UserDto.Address)), validationErrors);
    }

    public int Order => 1;
    public IValidatorFactory ValidatorFactory { get; set; }
    public Func<string, int, string> GetNameWithIndex { get; set; }
    public Func<string, string> GetName { get; set; }
}