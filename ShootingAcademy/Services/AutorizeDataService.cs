namespace ShootingAcademy.Services
{
    public class AutorizeDataService
    {
        private readonly HttpContext context;

        public Guid UserGuid => Guid.Parse(context.User.Claims.ToArray()[0].Value);
        public bool IsAutorize => context.User.Claims.Any();
        public string Role => context.User.Claims.ToArray()[1].Value;

        public AutorizeDataService(HttpContext context)
        {
            this.context = context;
        }
    }
}
