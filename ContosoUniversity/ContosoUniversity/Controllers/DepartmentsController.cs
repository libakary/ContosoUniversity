using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
	public class DepartmentsController : Controller
	{
		private readonly SchoolContext _context;

		public DepartmentsController
			(
				SchoolContext context
			)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			var result = await _context.Department
				.Include(d => d.Administrator)
				.ToListAsync();

			return View(result);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var department = await _context.Department
				.Include(i => i.Administrator)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.DepartmentID == id);

			ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "FullName",
				department.InstructorId);
			return View(department);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
		{
			if (id == null)
			{
				return NotFound();
			}

			var departmentToUpdate = await _context.Department
				.Include(i => i.Administrator)
				.FirstOrDefaultAsync(m => m.DepartmentID == id);

			if (departmentToUpdate == null)
			{
				Department deletedDepartment = new Department();
				await TryUpdateModelAsync(deletedDepartment);
				ModelState.AddModelError(string.Empty,
					"Unable to save changes. The department was deleted by another user.");
				ViewData["InstructorId"] = new SelectList(_context.Instructor,
					"Id", "FullName", deletedDepartment.InstructorId);
				return View(deletedDepartment);
			}

			_context.Entry(departmentToUpdate)
				.Property("RowVersion")
				.OriginalValue = rowVersion;

			if (await TryUpdateModelAsync<Department>(departmentToUpdate, "",
				s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorId))
			{
				try
				{
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (DbUpdateConcurrencyException ex)
				{
					var exceptionEntry = ex.Entries.Single();
					var clientValues = (Department)exceptionEntry.Entity;
					var databaseEntry = exceptionEntry.GetDatabaseValues();
					
					if (databaseEntry != null)
					{
						ModelState.AddModelError(string.Empty,
							"Unable to save changes. The department was deleted by another user.");
					}
					else
					{
						var databaseValues = (Department)databaseEntry.ToObject();

						if (databaseValues.Name != clientValues.Name)
						{
							ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}");
						}
                        if (databaseValues.Budget != clientValues.Budget)
                        {
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget}");
                        }
                        if (databaseValues.StartDate != clientValues.StartDate)
                        {
                            ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate}");
                        }
                        if (databaseValues.InstructorId != clientValues.InstructorId)
                        {
							Instructor databaseInstructor = await _context.Instructor
								.FirstOrDefaultAsync(i => i.Id == databaseValues.InstructorId);
                            ModelState.AddModelError("InstructorId", $"Current value: {databaseValues.InstructorId}");
                        }

						ModelState.AddModelError(string.Empty, "The record you attempted to edit "
							+ "was modified by another user after you got the original value. The "
							+ "edit operation was canceled and the current values in the database "
							+ "have been displayed. If you still want to edit this record, click "
							+ "the save button again. Otherwise click the back to list hyperlink.");
						departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;
						ModelState.Remove("RowVersion");
                    }
				}

			}
			ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id",
				"FullName", departmentToUpdate.InstructorId);

			return View(departmentToUpdate);
		}

		public async Task<IActionResult> Delete(int? id, bool? concurrencyError)
		{
			if (id == null)
			{
				return NotFound();
			}

			var department = await _context.Department
				.Include(i => i.Administrator)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.DepartmentID == id);

			if (department == null)
			{
				if (concurrencyError.GetValueOrDefault())
				{
					return RedirectToAction(nameof(Index));
				}
				return NotFound();
			}

			if (concurrencyError.GetValueOrDefault())
			{
				ViewData["ConcurrencyErrorMessage"] = "The record you attempted to edit "
				+ "was modified by another user after you got the original value. The "
				+ "edit operation was canceled and the current values in the database "
				+ "have been displayed. If you still want to edit this record, click "
				+ "the Back to List hyperlink.";

            }
			return View(department);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Department dep)
		{
			try
			{
				if (await _context.Department.AnyAsync(m => m.DepartmentID == dep.DepartmentID))
				{
					_context.Department.Remove(dep);
					await _context.SaveChangesAsync();
				}
				return RedirectToAction(nameof(Index));
			}
			catch (DbUpdateConcurrencyException)
			{
				return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = dep.DepartmentID });
			}
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			string query = "SELECT * FROM Department WHERE Id = {0}";
			var dep = await _context.Department
				.FromSqlRaw(query, id)
				.Include(d => d.Administrator)
				.AsNoTracking()
				.FirstOrDefaultAsync();

			if (dep == null)
			{
				return NotFound();
			}

			return View(dep);
		}

		public IActionResult Create()
		{
			ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "FullName");
			return View();
		}

		public async Task<IActionResult> Create(Department dep)
		{
			if (ModelState.IsValid)
			{
				_context.Add(dep);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["InstructorId"] = new SelectList(_context.Instructor, "Id", "FullName", dep.InstructorId);
			return View(dep);
		}
	}
}
