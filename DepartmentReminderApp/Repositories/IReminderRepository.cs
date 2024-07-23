using DepartmentReminderApp.Models;

namespace DepartmentReminderApp.Repositories
{
    public interface IReminderRepository
    {
        Task<IEnumerable<Reminder>> GetAllRemindersAsync();
        Task<Reminder> GetReminderByIdAsync(int id);
        Task AddReminderAsync(Reminder reminder);
        Task UpdateReminderAsync(Reminder reminder);
        Task DeleteReminderAsync(int id);
    }
}
