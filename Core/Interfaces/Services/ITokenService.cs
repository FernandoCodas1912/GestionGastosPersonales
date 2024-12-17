
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateTokenAsync(User user);
    }
}
