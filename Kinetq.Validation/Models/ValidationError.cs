namespace Kinetq.Validation.Models
{
    public class ValidationError
    {
        public string Field { get; set; }
        public IList<string> Messages { get; set; }
        public string? ErrorCode { get; set; }
    }
}