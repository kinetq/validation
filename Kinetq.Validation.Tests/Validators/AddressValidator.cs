using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Tests.Dtos;
using Kinetq.Validation.Validators.Models;

namespace Kinetq.Validation.Tests.Validators;

public class AddressValidator: IValidator<AddressDto>
{
    public async Task Execute(AddressDto dto, ValidationErrors validationErrors)
    {
        if (string.IsNullOrEmpty(dto.Street))
        {
            validationErrors.Add(GetName(nameof(AddressDto.Street), null), "Street needs to be supplied");
        }

        if (string.IsNullOrEmpty(dto.City))
        {
            validationErrors.Add(GetName(nameof(AddressDto.City), null), "City needs to be supplied");
        }    
        
        if (string.IsNullOrEmpty(dto.Zipcode))
        {
            validationErrors.Add(GetName(nameof(AddressDto.Zipcode), null), "Zipcode needs to be supplied");
        }
    }

    public int Order { get; }
    public IValidatorFactory ValidatorFactory { get; set; }
    public Func<string, int?, string> GetName { get; set; }
}