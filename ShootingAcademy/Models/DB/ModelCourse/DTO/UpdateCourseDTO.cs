﻿using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelCourse.DTO
{
    public class UpdateCourseDTO
    {

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Duration { get; set; }

        public string Level { get; set; }

        public int Rate { get; set; }

        public int PeopleRateCount { get; set; }

        public bool IsClosed { get; set; }

        public Guid InstructorId { get; set; }

        
    }
}
