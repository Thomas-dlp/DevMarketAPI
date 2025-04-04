namespace DevMarketAPI.Models
{
    public interface IDisplayableElementReference
    {
        public Guid Id { get; set; }
        public DisplayableElementType Type { get; set; }
        public int Order { get; set; }


    }

    public enum DisplayableElementType
    {
        Dev,
        Post,
        TradingStatus,
    }
}
