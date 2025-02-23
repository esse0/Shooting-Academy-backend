namespace ShootingAcademy.Models.DB.Base
{
    public interface IDbServiceBase<T>
    {
        public T Get();
        public void Post();
        public void Put();
        public void Delete();
    }
}
