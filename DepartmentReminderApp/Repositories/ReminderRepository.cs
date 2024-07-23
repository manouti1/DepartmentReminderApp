using DepartmentReminderApp.Data;
using DepartmentReminderApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DepartmentReminderApp.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly ApplicationDbContext _context;

        public ReminderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reminder>> GetAllRemindersAsync()
        {
            return await _context.Reminders.ToListAsync();
        }

        public async Task<Reminder> GetReminderByIdAsync(int id)
        {
            return await _context.Reminders.FindAsync(id);
        }

        public async Task AddReminderAsync(Reminder reminder)
        {
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReminderAsync(Reminder reminder)
        {
            _context.Entry(reminder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReminderAsync(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder != null)
            {
                _context.Reminders.Remove(reminder);
                await _context.SaveChangesAsync();
            }
        }
    }
}
