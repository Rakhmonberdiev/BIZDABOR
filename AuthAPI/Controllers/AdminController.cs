using AuthAPI.Dtos;
using AuthAPI.Models;
using AuthAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{
    [Authorize(Policy = "AdminRole")]
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public AdminController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;

        }
        [HttpPost("newClient")]
        public async Task<ActionResult> NewClient(ClientDto clientDto)
        {
            try
            {
                if (await UserExists(clientDto.UserName))
                {
                    return BadRequest("Username is taken");
                }
                var user = _mapper.Map<AppUser>(clientDto);

                var result = await _userManager.CreateAsync(user, clientDto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                var roleResult = await _userManager.AddToRoleAsync(user, "Client");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
