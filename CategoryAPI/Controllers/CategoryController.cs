using AutoMapper;
using CategoryAPI.Dtos;
using CategoryAPI.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAPI.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryRep _rep;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRep rep, IMapper mapper)
        {
            _rep = rep;
            _mapper = mapper;
        }

        [HttpGet("AllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var models = await _rep.GetAllCategories();
            var modelsToReturn = _mapper.Map<IEnumerable<CategoryDto>>(models);
            return Ok(modelsToReturn);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(Guid id)
        {
            var model = await _rep.GetCategoryById(id);
            var modelToReturn = _mapper.Map<CategoryDto>(model);    
            return Ok(modelToReturn);

        }
    }
}
