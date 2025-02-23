using ShootingAcademy.Models.DB.ModelCourse;
using ShootingAcademy.Models.DB.ModelCourse.DTO;

namespace ShootingAcademy.Models.DB.ModelCourse
{
    public interface ICourseService
    {
        Task<Course> AddCourseAsync(CreateCourseDTO courseToCreate);
        Task UpdateCourseAsync(UpdateCourseDTO courseToUpdate);
        Task DeleteCourseAsync(Course course);
        Task<Course?> FindCourseByIdAsync(GetCourseDto getCourseDto);
        Task<IEnumerable<GetCourseDto>> GetCourseAsync();
    }
}
