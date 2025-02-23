using ShootingAcademy.Models.Controllers.Auth;
using ShootingAcademy.Models.DB.ModelUser.DTO;

namespace ShootingAcademy.Models.DB.ModelUser
{
    public interface IUserService
    {
        Task<User> AddUserAsync(AuthRegisterRequest userToCreate);
        Task UpdateUserAsync(UpdateUserDTO userToUpdate);
        Task DeleteUserAsync(User user);
        Task<User?> FindUserByIdAsync(AuthLoginRequest getUserDto);
        Task<IEnumerable<GetUserDto>> GetUserAsync();
    }
}
