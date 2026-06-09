using MiniBanking.API.DTOs;

namespace MiniBanking.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<object> Register(UserRegisterDto registerDto);
        Task<object?> Login(UserLoginDto loginDto);
    }
}