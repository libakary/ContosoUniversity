using System.Collections;
using System.Collections.Generic;

namespace ContosoUniversity.Models
{
	public class InstructorsIndexData
	{
		public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<OfficeAssignment> OfficeAssignments { get; set; }
        public IEnumerable<CourseAssignment> CourseAssignments { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
