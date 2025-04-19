namespace DevMarketAPI.Models
{
    public abstract class DisplayableElementReferenceBase
    { 
        public Guid DisplayableElementId { get; set; }
        public DisplayableElementType DisplayableElementType { get; set; }
        public int Order { get; set; }

    }

    public class DisplayableElementReference : DisplayableElementReferenceBase
    {

    }
    

    public class DisplayableElementReferenceLink : DisplayableElementReferenceBase
    {
        public Guid Id { get; set; }
        public Guid StudioId { get; set; }        // FK to parent
        public int Order { get; set; } // Order of display
    }


    public enum DisplayableElementType
    {
        Dev,
        Post,
        TradingStatus,
    }
}
