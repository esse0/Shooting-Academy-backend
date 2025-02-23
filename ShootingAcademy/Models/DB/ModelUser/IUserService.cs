using ShootingAcademy.Models.DB.ModelUser.DTO;

namespace ShootingAcademy.Models.DB.ModelUser
{
    public interface IUserService
    {
        Task<GetUserDto> AddUserAsync(CreateUserDTO userToCreate);
        Task UpdateUserAsync(UpdateUserDTO userToUpdate);
        Task DeleteUserAsync(User user);
        Task<GetUserDto?> FindUserByIdAsync(int id);
        Task<IEnumerable <GetUserDto>> GetUserAsync();


    }
}
