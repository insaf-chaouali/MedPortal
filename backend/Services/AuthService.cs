using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using projet_1.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using projet_1.Data;
using Microsoft.EntityFrameworkCore;

namespace projet_1.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Utilisateur> _passwordHasher;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, IPasswordHasher<Utilisateur> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponse> AuthenticateAsync(LoginRequest loginRequest)
        {
            // Retrieve the user by login
            var user = await _context.Utilisateurs.SingleOrDefaultAsync(u => u.Login == loginRequest.Login);

            if (user == null)
                return null;

            // Verify the password using PasswordHasher (ensure user.Password is hashed)
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginRequest.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new LoginResponse { Token = token, Role = user.Role, NameIdentifier = user.Id };
        }

        public async Task RegisterUserAsync(RegisterRequest request)
        {
            // Check if login exists
            var existingUser = await _context.Utilisateurs.AnyAsync(u => u.Login == request.Login);
            if (existingUser)
                throw new Exception("Login already exists");

            // Hash the password using PasswordHasher
            var hashedPassword = _passwordHasher.HashPassword(null, request.Password);

            var utilisateur = new Utilisateur
            {
                Login = request.Login,
                Password = hashedPassword,  // Store hashed password
                Email = request.Email,
                LastName = request.LastName,
                FirstName = request.FirstName,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                Phone = request.Phone,
                Address = request.Address,
                City = request.City,
                PostalCode = request.PostalCode,
                Role = request.Role,
                DateCreation = DateTime.Now
            };

            await _context.Utilisateurs.AddAsync(utilisateur);
            await _context.SaveChangesAsync();
        }

        private string GenerateJwtToken(Utilisateur user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key not configured")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
