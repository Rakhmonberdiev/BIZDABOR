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
        [HttpPut("edit/{id}")]
        public async Task<ActionResult> Update(Guid id,CategoryCreateUpdateDto dto)
        {
            try
            {
                var model = await _rep.GetCategoryById(id);
                if (model != null)
                {
                    var existingCategory = await _rep.ExistingCategory(dto.Title);
                    if (existingCategory) return BadRequest("A category with this name already exists.");
                    var modelToRepo = _mapper.Map(dto, model);
                    await _rep.Update(modelToRepo);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var model = await _rep.GetCategoryById(id);
                if (model != null)
                {
                    await _rep.Delete(model);
                    return Ok();
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
