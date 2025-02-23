﻿using ShootingAcademy.Models.DB.ModelRole;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShootingAcademy.Models.DB.ModelUser.DTO
{
    public class CreateUserDTO
    {
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
        public required string Email { get; set; }
        [Required]
        public required string PasswordHash { get; set; }
        public Guid RoleId { get; set; }

        public static User ToUser(CreateUserDTO createUserDTO)
        {
            return new User
            {
                FirstName = createUserDTO.FirstName,
                SecoundName = createUserDTO.SecoundName,
                PatronymicName = createUserDTO.PatronymicName,
                Age = createUserDTO.Age,
                Grade = createUserDTO.Grade,
                Email = createUserDTO.Email,
                PasswordHash = createUserDTO.PasswordHash,
                RoleId = createUserDTO.RoleId,
            };
        }
    }
}