using System.ComponentModel.DataAnnotations;

namespace DevMarketAPI.DTOs
{
    public class UpdateStudioProfileDto
    {
            public string? Name { get; set; }
            public string? LogoUrl { get; set; }
            public string? BackgroundPictureUrl { get; set; }
            public string? Abstract { get; set; }
            public string? Bio { get; set; }
            public string[]? settings { get; set; }

    }
}
