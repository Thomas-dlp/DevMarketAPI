using AutoMapper;
using DevMarketAPI.DTOs;
using DevMarketAPI.Models;
using Mapster;

namespace DevMarketAPI.Helpers
{
    public class StudioProfileMapping :Profile
    {
        public  StudioProfileMapping()
        {
            CreateMap<UpdateStudioProfileDto, StudioProfile>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());        }
    }
}

