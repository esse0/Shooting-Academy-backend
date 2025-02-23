using ShootingAcademy.Models.Controllers.Auth;
using ShootingAcademy.Models.DB.ModelUser.DTO;

namespace ShootingAcademy.Models.DB.ModelUser
{
    public interface IUserService
    {
        Task<User> AddUserAsync(CreateUserDTO userToCreate);
        Task UpdateUserAsync(UpdateUserDTO userToUpdate);
        Task DeleteUserAsync(User user);
        Task<User?> FindUserByIdAsync(GetUserDto getUserDto);
        Task<IEnumerable<GetUserDto>> GetUserAsync();
    }
}
