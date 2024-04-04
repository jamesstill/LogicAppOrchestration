namespace SourceApi.Models
{
    public class PayloadDto
    {
        public required string PayloadJSON { get; set; }
                
        public required string PayloadType { get; set; }

        public required string PayloadAction { get; set; }
    }
}
