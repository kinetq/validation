using Kinetq.Validation.Models;

namespace Kinetq.Validation.Interfaces
{
    public interface IValidatorFactory
    {
        Task Validate<T>(T dto, string name = null) where T : class;
        Task ValidateNested<T>(T dto, string? name = null, ValidationErrors validationErrors = null) where T : class;
    }
}