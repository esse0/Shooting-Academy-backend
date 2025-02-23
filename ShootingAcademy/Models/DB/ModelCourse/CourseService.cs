using Microsoft.EntityFrameworkCore;
using ShootingAcademy.Models.DB.ModelUser.DTO;
using ShootingAcademy.Models.DB.ModelUser;

namespace ShootingAcademy.Models.DB.ModelCourse
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Course> AddCourseAsync(CreateCourseDTO courseToCreate)
        {
            Course course = CreateCourseDTO.ToUser(courseToCreate);

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;
        }
        public async Task UpdateCourseAsync(UpdateUserDTO courseToUpdate)
        {
            Course course = UpdateUserDTO.ToUser(courseToUpdate);

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task<Course?> FindCourseByIdAsync(GetUserDto getCourseDto)
        {
            User? user = await _context.Courses.Where(x => x.Email == getUserDto.Email).AsNoTracking().FirstOrDefaultAsync();

            if (user == null) return null;

            return user;
        }

        public async Task<IEnumerable<GetCourseDto>> GetCourseAsync()
        {
            IEnumerable<User> Courses = await _context.Courses.AsNoTracking().ToListAsync();

            return Courses.Select(Course.ToGetUserDto);
        }
    }
}
