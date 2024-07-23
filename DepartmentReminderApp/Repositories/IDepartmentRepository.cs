using DepartmentReminderApp.Models;

namespace DepartmentReminderApp.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);
        Task SaveAsync();
        Task<List<Department>> GetParentDepartmentsAsync(int? parentId);
        Task<List<Department>> GetSubDepartmentsAsync(int departmentId);
    }
}
