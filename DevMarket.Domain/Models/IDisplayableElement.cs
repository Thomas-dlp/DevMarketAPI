namespace DevMarketAPI.Models
{
    public interface IDisplayableElement
    {
        Guid Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        string ImageUrl { get; set; }
    }
}
