using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
				ViewData["InstructorId"] = id.Value;
				Instructor instructor = vm.Instructors
					.Where(i => i.Id == id.Value)
					.Single();
				vm.Courses = instructor.CourseAssignments
					.Select(i => i.Course);
			}

			if (courseId != null)
			{
				ViewData["CourseId"] = courseId.Value;
				vm.Enrollments = vm.Courses
					.Where(x => x.CourseID == courseId)
					.Single()
					.Enrollments;

			}

			return View();
		}
	}
}
