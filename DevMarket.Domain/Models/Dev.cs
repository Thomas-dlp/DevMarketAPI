﻿
namespace DevMarketAPI.Models
{
    public class Dev: IDisplayableElement
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Guid StudioId { get; set; }
        public List<DevTag> DevTags { get; set; }
    }
}
