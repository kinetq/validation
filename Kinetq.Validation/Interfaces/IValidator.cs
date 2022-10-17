using Kinetq.Validation.Validators.Models;

namespace Kinetq.Validation.Interfaces
{
    public interface IValidator<T> where T : class
    {
        Task Execute(T dto, ValidationErrors validationErrors);
        int Order { get; }
        IValidatorFactory ValidatorFactory { get; set; }
        Func<string, int?, string> GetName { get; set; }
    }
}