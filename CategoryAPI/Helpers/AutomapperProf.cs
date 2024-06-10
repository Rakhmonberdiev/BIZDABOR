using AutoMapper;
using CategoryAPI.Dtos;
using CategoryAPI.Models;

namespace CategoryAPI.Helpers
{
    public class AutomapperProf : Profile
    {
        public AutomapperProf()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
