using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
	public class InstructorsController : Controller
	{
		private readonly SchoolContext _context;

		public InstructorsController
			(
			SchoolContext context
			)
		{
			_context = context;
		}
		public async Task<IActionResult> Index(int? id, int? courseId)
		{
			var vm = new InstructorsIndexData();

			vm.Instructors = await _context.Instructor
                .Include(i => i.OfficeAssignment)
				.Include(i => i.CourseAssignments)
					.ThenInclude(i => i.Course)
						.ThenInclude(i => i.Enrollments)
							.ThenInclude(i => i.Student)
				.Include(i => i.CourseAssignments)
					.ThenInclude(i => i.Course)
						.ThenInclude(i => i.Department)
				.AsNoTracking()
				.OrderBy(i => i.LastName)
				.ToListAsync();

			if(id != null)
			{
				ViewData["InstructorID"] = id.Value;
				Instructor instructor = vm.Instructors
					.Where(i => i.Id == id.Value)
					.Single();
				vm.Courses = instructor.CourseAssignments
					.Select(i => i.Course);
			}

			if (courseId != null)
			{
				ViewData["CourseID"] = courseId.Value;
				vm.Enrollments = vm.Courses
					.Where(x => x.CourseID == courseId)
					.Single()
					.Enrollments;

			}

			return View();
		}

		public async Task<IActionResult> Details(int? id)
		{

			var instructor = await _context.Instructor
				.FirstOrDefaultAsync(m => m.Id == id);

			if (instructor == null)
			{
				return NotFound();
			}
			return View(instructor);
		}

		[HttpGet]
		public IActionResult Create()
		{
			var instructor = new Instructor();
			instructor.CourseAssignments = new List<CourseAssignment>();

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Instructor instructor, string[] selectedCourses)
		{
			if(selectedCourses != null)
			{
				instructor.CourseAssignments = new List<CourseAssignment>();
				foreach (var course in selectedCourses)
				{
					var courseToAdd = new CourseAssignment
					{
						InstructorId = instructor.Id,
						CourseId = int.Parse(course)
					};
					instructor.CourseAssignments.Add(courseToAdd);
				}
			}
			if (ModelState.IsValid)
			{
				_context.Add(instructor);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			PopulateAssignedCourseData(instructor);
            return View(instructor);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var instructor = await _context.Instructor
				.Include(i => id.OfficeAssignment)
				.Include(i => i.CourseAssignments)
                    .ThenInclude(id => id.Course)
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.ID == id);

			if (instructor == null)
			{
				return NotFound();
			}

            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
		{
			if (id == null)
			{
				return NotFound();
			}
			var instructorToUpdate = await _context.Instructor
				.Include(i => i.OfficeAssignment)
				.Include(i => i.CourseAssignments)
					.ThenInclude(id => id.Course)
				.FirstOrDefaultAsync(s => s.Id == id);

			if (await TryUpdateModelAsync<Instructor>(
				instructorToUpdate,
				"",
				i => i.FirstName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
			{
				if (string.IsNullOrWhiteSpace(InstructorToUpdate.OfficeAssifnment?.Location))
				{
					instructorToUpdate.OfficeAssignment = null;
				}
				UpdateInstructorCourses(selectedCourses, instructorToUpdate);
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException)
				{
					ModelState.AddModelError("", "Unable to save changes. " +
						"Try again, and if the problem persists, " +
						"see your system administrator. ");
				}
				return RedirectToAction(nameof(Index));
			}
            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            PopulateAssignedCourseData(instructor);

			return ();
        }


		private void PopulateAssignedCourseData(Instructor instructor)
		{
			var allCourses = _context.Courses;
			var instructorCourses = new HashSet<int>
				(
					instructor.CourseAssignments.Select(c => c.CourseId)	
				);
			var vm = new List<AssignedCourseData>();
			foreach (var course in allCourses)
			{
				vm.Add(new AssignedCourseData
				{
					CourseID = course.CourseID,
					Title = course.Title,
					Assigned = instructorCourses.Contains(course.CourseID)
				});
			}
			ViewData["Courses"] = vm;
		}

		private void UpdateInstructorCourses(string[] seletedCourses, Instructor instructorToUpdate)
		{
			if (selectedCourses == null)
			{
				instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
				return;
			}

			var selectedCoursesHS = new HashSet<string>(selectedCourses);
			var instructorCourses = new HashSet<int>
				(instructorToUpdate.CourseAssignments
					.Select(c => c.Course.CourseID)
				);

			foreach (var course in _context.Courses)
			{
				if (selectedCoursesHS.Contains(course.CourseID.ToString()))
				{
					if(!instructorCourses.Contains(course.CourseID))
					{
						instructorToUpdate.CourseAssignments.Add(
							new CourseAssignment
							{
								InstructorId = instructorToUpdate.Id,
								CourseId = course.CourseID
							});
					}
					else
					{
						if(instructorCourses.Contains(course.CourseID))
						{
							CourseAssignment courseToRemove = instructorToUpdate.CourseAssignments
								.FirstOrDefault(c => c.CourseId == course.CourseID);
							_context.Remove(courseToRemove);
						}
					}
				}
			}
		}
	}
}
