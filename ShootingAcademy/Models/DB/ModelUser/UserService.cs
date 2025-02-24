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

        //public async Task<User> AddUserAsync(AuthRegisterRequest userToCreate)
        //{
        //    User user = CreateUserDTO.ToUser(userToCreate);

        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}
        //public async Task UpdateUserAsync(UpdateUserDTO userToUpdate)
        //{
        //    User user = UpdateUserDTO.ToUser(userToUpdate);

        //    _context.Users.Update(user);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteUserAsync(User user)
        //{
        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<User?> FindUserByIdAsync(AuthLoginRequest getUserDto)
        //{
        //    User? user = await _context.Users.Where(x => x.Email == getUserDto.Email && x.PasswordHash == getUserDto.password).AsNoTracking().FirstOrDefaultAsync();

        //    if (user == null) return null;
            
        //    return user;
        //}

        //public async Task<IEnumerable<GetUserDto>> GetUserAsync()
        //{
        //    IEnumerable<User> Users = await _context.Users.AsNoTracking().ToListAsync();

        //    return Users.Select(User.ToGetUserDto);
        //}

    }
}
