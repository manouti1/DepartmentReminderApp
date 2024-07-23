using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DepartmentReminderApp.ViewModels
{
    public class DepartmentCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        public string Name { get; set; }

        [Display(Name = "Department Logo")]
        public string Logo { get; set; }

        [Display(Name = "Parent Department")]
        public int? ParentDepartmentId { get; set; }

        [Display(Name = "Sub-Departments")]
        public List<int> SubDepartmentIds { get; set; } = new List<int>();

        public SelectList? Departments { get; set; }

        public List<SelectListItem> AvailableDepartments { get; set; } = new List<SelectListItem>();
    }
}
