using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniBanking.API.Data;
using MiniBanking.API.DTOs;
using MiniBanking.API.Helpers;
using MiniBanking.API.Models;
using MiniBanking.API.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniBanking.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<object> Register(UserRegisterDto registerDto)
        {
            var userExists = await _context.Users
                .AnyAsync(x => x.Username == registerDto.Username);

            if (userExists)
                throw new Exception("Bu kullanıcı adı zaten mevcut.");

            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == registerDto.Email);

            if (emailExists)
                throw new Exception("Bu email zaten kayıtlı.");

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var account = new Account
            {
                UserID = user.UserID,
                Balance = 1000,
                AccountType = "Checking",
                IBAN = IbanGenerator.Generate()
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return new
            {
                message = "Kayıt başarılı!"
            };
        }

        public async Task<object?> Login(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == loginDto.Username);

            if (user == null)
                return null;

            bool passwordCorrect = BCrypt.Net.BCrypt.Verify(
                loginDto.Password,
                user.Password);

            if (!passwordCorrect)
                return null;

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new
            {
                token = jwt,
                username = user.Username,
                userId = user.UserID
            };
        }
    }
}