using ShootingAcademy.Models.DB;

namespace ShootingAcademy.Models.Controllers.Analytic
{
    public class AnalyticResponse
    {
        public int lessonsCount { get; set; }
        public List<string> coursesByCategory { get; set; }
        public List<DB.Competition> competitions { get; set; }
        public Dictionary<string, float?> scoreByMonth { get; set; }
    }
}
