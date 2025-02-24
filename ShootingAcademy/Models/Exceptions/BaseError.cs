namespace ShootingAcademy.Models.Exceptions
{
    public class BaseError
    {
        public bool Error { get; set; }

        public string Message { get; set; }

        public string Code { get; set; }

        public bool Show { get; set; }
    }
}
