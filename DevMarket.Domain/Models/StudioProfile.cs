using System.ComponentModel.DataAnnotations;

namespace DevMarketAPI.Models
{
    public class StudioProfile 
    {
        [Key]
        public Guid Id { get; set; }

        
        [MaxLength(50)]
        public string? Name { get; set; }
        public string? LogoUrl { get; set; }
        public string? BackgroundPictureUrl { get; set; }
        public string? Abstract { get; set; }
        public string? Bio { get; set; }
        public string[]? settings { get; set; }
      

    }
}
