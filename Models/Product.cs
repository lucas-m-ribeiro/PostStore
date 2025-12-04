namespace Poststore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; }= null!;
    }
}