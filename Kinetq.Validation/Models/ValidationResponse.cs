namespace Kinetq.Validation.Models
{
    public class ValidationResponse
    {
        public IList<ValidationError> Errors { get; set; }
    }
}