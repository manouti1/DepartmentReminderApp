using DepartmentReminderApp.Services;
using Quartz;

namespace DepartmentReminderApp.Jobs
{
    public class ReminderJob : IJob
    {
        private readonly ReminderService _reminderService;

        public ReminderJob(ReminderService reminderService)
        {
            _reminderService = reminderService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _reminderService.CheckAndSendReminders();
        }
    }
}