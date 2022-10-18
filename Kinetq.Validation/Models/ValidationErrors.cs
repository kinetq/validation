using Kinetq.Validation.Helpers;

namespace Kinetq.Validation.Models
{
    public class ValidationErrors
    {
        public readonly IList<ValidationError> ErrorMessages = new List<ValidationError>();

        public void Add(string name, string message, string? errorCode = null)
        {
            ValidationError validationError = ErrorMessages.FirstOrDefault(x => x.Field.Equals(name));
            if (validationError != null)
            {
                validationError.Messages.Add(message);
            }
            else
            {
                validationError = new ValidationError()
                {
                    Field = name.FirstCharToLowerCase(),
                    ErrorCode = errorCode,
                    Messages = new List<string>() { message }
                };

                ErrorMessages.Add(validationError);
            }
        }
    }
}