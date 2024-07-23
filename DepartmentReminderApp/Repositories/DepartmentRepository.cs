using DepartmentReminderApp.Data;
using DepartmentReminderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DepartmentReminderApp.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync() ?? new List<Department>();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.ParentDepartment)
                .Include(d => d.SubDepartments)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await GetByIdAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Department>> GetParentDepartmentsAsync(int? parentId)
        {
            var parentDepartments = new List<Department>();
            while (parentId != null)
            {
                var parentDepartment = await _context.Departments.FindAsync(parentId);
                if (parentDepartment != null)
                {
                    parentDepartments.Insert(0, parentDepartment);
                    parentId = parentDepartment.ParentDepartmentId;
                }
                else
                {
                    parentId = null;
                }
            }
            return parentDepartments;
        }

        public async Task<List<Department>> GetSubDepartmentsAsync(int departmentId)
        {
            return await _context.Departments
                .Where(d => d.ParentDepartmentId == departmentId)
                .ToListAsync();
        }
    }

}
