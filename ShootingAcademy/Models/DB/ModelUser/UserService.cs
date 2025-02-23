using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.Controllers.Auth;
using ShootingAcademy.Models.DB.ModelUser.DTO;

namespace ShootingAcademy.Models.DB.ModelUser
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(CreateUserDTO userToCreate)
        {
            User user = CreateUserDTO.ToUser(userToCreate);

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task UpdateUserAsync(UpdateUserDTO userToUpdate)
        {
            User user = UpdateUserDTO.ToUser(userToUpdate);

            _context.users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> FindUserByIdAsync(GetUserDto getUserDto)
        {
            User? user = await _context.users.Where(x => x.Email == getUserDto.Email).AsNoTracking().FirstOrDefaultAsync();

            if (user == null) return null;
            
            return user;
        }

        public async Task<IEnumerable<GetUserDto>> GetUserAsync()
        {
            IEnumerable<User> users = await _context.users.AsNoTracking().ToListAsync();

            return users.Select(User.ToGetUserDto);
        }

    }
}
