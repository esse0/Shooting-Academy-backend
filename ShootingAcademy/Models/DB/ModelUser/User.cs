﻿using ShootingAcademy.Models.DB.ModelUser.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB.ModelUser
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecoundName { get; set; }
        [Required]
        public string PatronymicName { get; set; }

        [Required]
        public int Age { get; set; }

        public string Grade { get; set; }
        public string Country { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public List<GroupMember> AthleteGroups { get; set; } = [];
        public List<Course> InstructoredCourses { get; set; } = [];
        public List<CourseMember> Courses { get; set; } = [];

        public static GetUserDto ToGetUserDto(User user)
        {
            return new GetUserDto
            {
                FirstName = user.FirstName,
                SecoundName = user.SecoundName,
                PatronymicName = user.PatronymicName,
                Age = user.Age,
                Grade = user.Grade,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                //RoleId = user.RoleId,
            };
        }
    }
}
