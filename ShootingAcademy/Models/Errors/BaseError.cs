﻿namespace ShootingAcademy.Models.Errors
{
    [Serializable]
    public class BaseError
    {
        public bool Error { get; set; }

        public string Message { get; set; }

        public bool Show { get; set; }
    }
}
