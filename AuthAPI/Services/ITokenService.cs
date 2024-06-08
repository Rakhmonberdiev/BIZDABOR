using AuthAPI.Models;

namespace AuthAPI.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(AppUser user);
    }
}
