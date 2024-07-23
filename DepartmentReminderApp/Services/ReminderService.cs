using DepartmentReminderApp.Data;
using FluentEmail.Core;

namespace DepartmentReminderApp.Services
{
    public class ReminderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFluentEmail _fluentEmail;
        private readonly ILogger<ReminderService> _logger;

        public ReminderService(ApplicationDbContext context, IFluentEmail fluentEmail, ILogger<ReminderService> logger)
        {
            _context = context;
            _fluentEmail = fluentEmail;
            _logger = logger;
        }

        public async Task CheckAndSendReminders()
        {
            var now = DateTime.Now;
            var reminders = _context.Reminders
                .Where(r => r.DateTime <= now && !r.Sent)
                .ToList();

            foreach (var reminder in reminders)
            {
                var email = _fluentEmail
                    .To("mohammad.anouti@gmail.com") // Ensure you have the recipient email field in your Reminder model
                    .Subject(reminder.Title)
                    .Body($"Reminder: {reminder.Title} is set for {reminder.DateTime}");

                try
                {
                    await email.SendAsync();
                    reminder.Sent = true;
                    _context.Reminders.Update(reminder);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while sending reminder email for {Title} scheduled at {DateTime}.", reminder.Title, reminder.DateTime);
                }
            }
        }
    }
}