﻿namespace ShootingAcademy.Models.DB.ModelUser
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}
