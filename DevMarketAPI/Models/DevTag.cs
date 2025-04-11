namespace DevMarketAPI.Models
{
    public class DevTag
    {
        public Guid DevId { get; set; }
        public Dev Dev { get; set; }

        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
