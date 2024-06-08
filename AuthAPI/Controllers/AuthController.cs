using AuthAPI.Dtos;
using AuthAPI.Models;
using AuthAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AuthController(UserManager<AppUser> userManager,ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            try
            {
                if (await UserExists(registerDto.UserName))
                {
                    return BadRequest("Username is taken");
                }
                var user = _mapper.Map<AppUser>(registerDto);

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }
                var generatedToken = await _tokenService.GetTokenAsync(user);
                return new UserDto
                {
                    UserName = registerDto.UserName,
                    Token = generatedToken
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
                [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
                if (user == null)
                {
                    return Unauthorized("Invalid username");
                }
                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!result)
                {
                    return Unauthorized("Invalid password");
                }
                var getToken = await _tokenService.GetTokenAsync(user);

                return new UserDto
                {
                    UserName = user.UserName,
                    Token = getToken
                };
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
       
        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x=>x.UserName == username.ToLower());
        }
    }
}
