using Kinetq.Validation.Interfaces;
using Kinetq.Validation.Models;
using Kinetq.Validation.Tests.Dtos;

namespace Kinetq.Validation.Tests.Validators;

public class AddressValidator: IValidator<AddressDto>
{
    private Func<string, string> _getName;

    public async Task Execute(AddressDto dto, ValidationErrors validationErrors)
    {
        if (string.IsNullOrEmpty(dto.Street))
        {
            validationErrors.Add(GetName(nameof(AddressDto.Street)), "Street needs to be supplied");
        }

        if (string.IsNullOrEmpty(dto.City))
        {
            validationErrors.Add(GetName(nameof(AddressDto.City)), "City needs to be supplied");
        }    
        
        if (string.IsNullOrEmpty(dto.Zipcode))
        {
            validationErrors.Add(GetName(nameof(AddressDto.Zipcode)), "Zipcode needs to be supplied");
        }
    }

    public int Order { get; }
    public IValidatorFactory ValidatorFactory { get; set; }

    public Func<string, int, string> GetNameWithIndex { get; set; }
    public Func<string, string> GetName { get; set; }
}