using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Tests.Dtos;
using Kinetq.Validation.Validators.Models;

namespace Kinetq.Validation.Tests.Validators;

public class SecondUserValidator : IValidator<UserDto>
{
    public async Task Execute(UserDto dto, ValidationErrors validationErrors)
    {
        await ValidatorFactory.ValidateNested(dto.Address, GetName(nameof(UserDto.Address), null), validationErrors);
    }

    public int Order => 1;
    public IValidatorFactory ValidatorFactory { get; set; }
    public Func<string, int?, string> GetName { get; set; }
}