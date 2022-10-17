namespace Kinetq.Validation.Validators.Models
{
    public class ValidationResponse
    {
        public IList<ValidationError> Errors { get; set; }
    }
}