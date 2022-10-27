using ContosoUniversity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContosoUniversity.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController
            // kirjutada nii sest kui tuleb suurem projekt ja rohkem contexte
            (
                SchoolContext context
            )
        {
            _context = context;
        }
        //asünkroonne lähenemine
        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ToListAsync();

            //peaks returnima vaate
            return View(courses);
        }
    }
}
