﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShootingAcademy.Models.DB
{
    public class Fraq
    {
        [Key]
        public Guid Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course Course { get; set; }
    }
}
