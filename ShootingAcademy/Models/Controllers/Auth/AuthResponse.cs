namespace ShootingAcademy.Models.Controllers.Auth
{
    public class AuthResponse
    {
        public string accessToken { get; set; } = null!;
        public string refreshToken { get; set; } = null!;
        public string role { get; set; } = null!;
        public string error { get; set; } = null!;
    }
}
