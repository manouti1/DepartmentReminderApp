using System.ComponentModel.DataAnnotations;

namespace DepartmentReminderApp.Models
{
    public class Reminder
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Date-Time")]
        public DateTime DateTime { get; set; }
        public bool Sent { get; set; }
    }
}
