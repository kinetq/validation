using System.ComponentModel.DataAnnotations;
using Kinetq.Validation.Validators.Models;

namespace Kinetq.Validation.Exceptions
{
    public class ValidationsException : ValidationException
    {
        public readonly ValidationErrors ValidationErrors = new ValidationErrors();

        public ValidationsException()
        {

        }

        public ValidationsException(string name, string message)
        {
            ValidationErrors.Add(name, message);
        }

        public ValidationsException(ValidationErrors validationErrors)
        {
            ValidationErrors = validationErrors;
        }
    }
}