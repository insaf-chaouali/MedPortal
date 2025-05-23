using System.Threading.Tasks;

using System.Threading.Tasks;
using projet_1.Models;

namespace projet_1.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest loginRequest);
        Task RegisterUserAsync(RegisterRequest registerRequest);
    }
}
