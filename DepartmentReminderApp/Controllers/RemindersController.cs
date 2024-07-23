using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;
using DepartmentReminderApp.Models;
using DepartmentReminderApp.Repositories;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DepartmentReminderApp.Controllers
{
    public class RemindersController : Controller
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly IFluentEmail _fluentEmail;

        public RemindersController(IReminderRepository reminderRepository, IFluentEmail fluentEmail)
        {
            _reminderRepository = reminderRepository;
            _fluentEmail = fluentEmail;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _reminderRepository.GetAllRemindersAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var reminder = await _reminderRepository.GetReminderByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            return View(reminder);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DateTime")] Reminder reminder)
        {
            if (ModelState.IsValid)
            {
                await _reminderRepository.AddReminderAsync(reminder);

                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reminder = await _reminderRepository.GetReminderByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }
            return View(reminder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,DateTime")] Reminder reminder)
        {
            if (id != reminder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _reminderRepository.UpdateReminderAsync(reminder);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ReminderExists(reminder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reminder = await _reminderRepository.GetReminderByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            return View(reminder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reminderRepository.DeleteReminderAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ReminderExists(int id)
        {
            return await _reminderRepository.GetReminderByIdAsync(id) != null;
        }
    }
}
