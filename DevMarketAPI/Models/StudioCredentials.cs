namespace DevMarketAPI.Models
{
    public class StudioCredentials
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string email { get; set; }
        public string password { get; set; }

        
    }
}
