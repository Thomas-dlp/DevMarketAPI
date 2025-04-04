namespace DevMarketAPI.Models
{
    public class TradingStatus:IDisplayableElement
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string StudioId { get; set; }
    }
}
