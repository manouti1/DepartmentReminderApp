using System.ComponentModel.DataAnnotations;

namespace DepartmentReminderApp.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string Name { get; set; }

        [Display(Name = "Department Logo")]
        public string Logo { get; set; }

        public int? ParentDepartmentId { get; set; }

        public virtual Department? ParentDepartment { get; set; }

        public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();

    }
}
