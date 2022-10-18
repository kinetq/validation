using Kinetq.Validation.Models;

namespace Kinetq.Validation.Interfaces
{
    public interface IValidator<T> where T : class
    {
        Task Execute(T dto, ValidationErrors validationErrors);
        int Order { get; }
        IValidatorFactory ValidatorFactory { get; set; }
        Func<string, string> GetName { get; set; }
        Func<string, int, string> GetNameWithIndex { get; set; }
    }
}