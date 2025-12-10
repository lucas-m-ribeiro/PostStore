namespace Poststore.Request
{
    public class ProductRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}