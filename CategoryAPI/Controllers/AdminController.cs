using AutoMapper;
using CategoryAPI.Dtos;
using CategoryAPI.Models;
using CategoryAPI.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CategoryAPI.Controllers
{
    [Authorize(Policy = "AdminRole")]
    public class AdminController : BaseApiController
    {
        private readonly ICategoryRep _rep;
        private readonly IMapper _mapper;
        public AdminController(ICategoryRep rep,IMapper mapper)
        {
            _rep=rep;
            _mapper=mapper;
        }

        [HttpPost("add-category")]
        public async Task<ActionResult> Add(CategoryCreateUpdateDto dto)
        {
            try
            {
                if (dto != null)
                {
                    var existingCategory = await _rep.ExistingCategory(dto.Title);
                    if (existingCategory) return BadRequest("A category with this name already exists.");
                    var modelToRepo = _mapper.Map<Category>(dto);
                    await _rep.Create(modelToRepo);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
