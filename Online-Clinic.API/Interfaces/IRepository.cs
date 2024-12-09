namespace Online_Clinic.API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        int AddEntity(T entity);
        void UpdateEntity(int id, T entity);
        void DeleteEntity(int id);
        T GetEntity(int id, bool throwIfNotFound = true);
        List<T> GetEntities();
    }
}
