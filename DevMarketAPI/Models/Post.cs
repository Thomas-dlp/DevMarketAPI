namespace DevMarketAPI.Models
{
    public class Post :IDisplayableElement
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string StudiioId { get; set; }
    }
}
