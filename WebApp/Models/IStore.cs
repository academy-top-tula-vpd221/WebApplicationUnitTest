namespace WebApp.Models
{
    public interface IStore
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void Create(Employee employee);
    }
}
