using Kinetq.Validation.Dictionary;

namespace Kinetq.Validation.Validators.Models
{
    public class ValidationError
    {
        public string Field { get; set; }
        public IList<string> Messages { get; set; }
        public ErrorCode? ErrorCode { get; set; }
    }
}