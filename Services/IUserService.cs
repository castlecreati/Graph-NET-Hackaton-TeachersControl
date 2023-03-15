using Microsoft.Graph;

namespace TeachersControl.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync();
    }
}
