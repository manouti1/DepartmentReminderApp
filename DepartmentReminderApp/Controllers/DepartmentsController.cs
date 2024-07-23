using Microsoft.AspNetCore.Mvc;
using DepartmentReminderApp.Models;
using DepartmentReminderApp.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using DepartmentReminderApp.ViewModels;
using System.Linq;

namespace DepartmentReminderApp.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return View(departments);
        }

        // GET: Departments/Create
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentRepository.GetAllAsync();
            var viewModel = new DepartmentCreateViewModel
            {
                Departments = new SelectList(departments, "Id", "Name"),
                AvailableDepartments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList()
            };
            return View(viewModel);
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var department = new Department
                {
                    Name = viewModel.Name,
                    Logo = viewModel.Logo,
                    ParentDepartmentId = viewModel.ParentDepartmentId
                };

                await _departmentRepository.AddAsync(department);
                await _departmentRepository.SaveAsync();

                // Optionally, update sub-departments if needed
                foreach (var subDepartmentId in viewModel.SubDepartmentIds)
                {
                    var subDepartment = await _departmentRepository.GetByIdAsync(subDepartmentId);
                    if (subDepartment != null)
                    {
                        subDepartment.ParentDepartmentId = department.Id;
                        await _departmentRepository.UpdateAsync(subDepartment);
                    }
                }

                await _departmentRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            // Ensure Departments are re-populated if model state is not valid
            var departments = await _departmentRepository.GetAllAsync();
            viewModel.Departments = new SelectList(departments, "Id", "Name");
            viewModel.AvailableDepartments = departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
            return View(viewModel);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentRepository.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            var departments = await _departmentRepository.GetAllAsync();
            var availableDepartments = departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = department.SubDepartments.Any(sd => sd.Id == d.Id)
            }).ToList();

            var viewModel = new DepartmentCreateViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Logo = department.Logo,
                ParentDepartmentId = department.ParentDepartmentId,
                SubDepartmentIds = department.SubDepartments.Select(d => d.Id).ToList(),
                Departments = new SelectList(departments, "Id", "Name"),
                AvailableDepartments = availableDepartments
            };

            return View(viewModel);
        }


        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentCreateViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var department = await _departmentRepository.GetByIdAsync(viewModel.Id);
                if (department == null)
                {
                    return NotFound();
                }

                department.Name = viewModel.Name;
                department.ParentDepartmentId = viewModel.ParentDepartmentId;

                try
                {
                    await _departmentRepository.UpdateAsync(department);
                    await _departmentRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                foreach (var subDepartmentId in viewModel.SubDepartmentIds)
                {
                    var subDepartment = await _departmentRepository.GetByIdAsync(subDepartmentId);
                    if (subDepartment != null)
                    {
                        subDepartment.ParentDepartmentId = department.Id;
                        await _departmentRepository.UpdateAsync(subDepartment);
                    }
                }

                await _departmentRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            viewModel.Departments = new SelectList(await _departmentRepository.GetAllAsync(), "Id", "Name");
            return View(viewModel);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentRepository.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department != null)
            {
                await _departmentRepository.DeleteAsync(id);
                await _departmentRepository.SaveAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DepartmentExists(int id)
        {
            return (await _departmentRepository.GetByIdAsync(id)) != null;
        }
    }
}
