//using ContosoUniversity.Models;
//using System;
//using System.Linq;

//namespace ContosoUniversity.Data
//{
//    public static class DbInitializer
//    {
//        public static void Initialize(SchoolContext context)
//        {
//            context.Database.EnsureCreated();

//            // Look for any students.
//            if (context.Students.Any())
//            {
//                return;   // DB has been seeded
//            }

//            var students = new Student[]
//            {
//            new Student{FirstName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
//            new Student{FirstName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
//            new Student{FirstName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
//            new Student{FirstName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
//            new Student{FirstName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
//            new Student{FirstName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
//            new Student{FirstName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
//            new Student{FirstName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
//            };
//            foreach (Student s in students)
//            {
//                context.Students.Add(s);
//            }
//            context.SaveChanges();

//            var courses = new Course[]
//            {
//            new Course{CourseID=1050,Title="Chemistry",Credits=3},
//            new Course{CourseID=4022,Title="Microeconomics",Credits=3},
//            new Course{CourseID=4041,Title="Macroeconomics",Credits=3},
//            new Course{CourseID=1045,Title="Calculus",Credits=4},
//            new Course{CourseID=3141,Title="Trigonometry",Credits=4},
//            new Course{CourseID=2021,Title="Composition",Credits=3},
//            new Course{CourseID=2042,Title="Literature",Credits=4}
//            };
//            foreach (Course c in courses)
//            {
//                context.Courses.Add(c);
//            }
//            context.SaveChanges();

//            var enrollments = new Enrollment[]
//            {
//            new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
//            new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
//            new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
//            new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
//            new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
//            new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
//            new Enrollment{StudentID=3,CourseID=1050},
//            new Enrollment{StudentID=4,CourseID=1050},
//            new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
//            new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
//            new Enrollment{StudentID=6,CourseID=1045},
//            new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
//            };
//            foreach (Enrollment e in enrollments)
//            {
//                context.Enrollments.Add(e);
//            }
//            context.SaveChanges();
//        }
//    }
//}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = new Student[]
            {
                new Student { FirstName = "Carson",   LastName = "Alexander",
                    EnrollmentDate = DateTime.Parse("2010-09-01") },
                new Student { FirstName = "Meredith", LastName = "Alonso",
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Arturo",   LastName = "Anand",
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstName = "Gytis",    LastName = "Barzdukas",
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Yan",      LastName = "Li",
                    EnrollmentDate = DateTime.Parse("2012-09-01") },
                new Student { FirstName = "Peggy",    LastName = "Justice",
                    EnrollmentDate = DateTime.Parse("2011-09-01") },
                new Student { FirstName = "Laura",    LastName = "Norman",
                    EnrollmentDate = DateTime.Parse("2013-09-01") },
                new Student { FirstName = "Nino",     LastName = "Olivetto",
                    EnrollmentDate = DateTime.Parse("2005-09-01") }
            };

            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var instructors = new Instructor[]
            {
                new Instructor { FirstName = "Kim",     LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstName = "Fadi",    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstName = "Roger",   LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstName = "Candace", LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstName = "Roger",   LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12") }
            };

            foreach (Instructor i in instructors)
            {
                context.Instructor.Add(i);
            }
            context.SaveChanges();

            var departments = new Department[]
            {
                new Department { Name = "English",     Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId  = instructors.Single( i => i.LastName == "Abercrombie").Id },
                new Department { Name = "Mathematics", Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId  = instructors.Single( i => i.LastName == "Fakhouri").Id },
                new Department { Name = "Engineering", Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId  = instructors.Single( i => i.LastName == "Harui").Id },
                new Department { Name = "Economics",   Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId  = instructors.Single( i => i.LastName == "Kapoor").Id }
            };

            foreach (Department d in departments)
            {
                context.Department.Add(d);
            }
            context.SaveChanges();

            var courses = new Course[]
            {
                new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3,
                    DepartmentId = departments.Single( s => s.Name == "Engineering").Id
                },
                new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3,
                    DepartmentId = departments.Single( s => s.Name == "Economics").Id
                },
                new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3,
                    DepartmentId = departments.Single( s => s.Name == "Economics").Id
                },
                new Course {CourseID = 1045, Title = "Calculus",       Credits = 4,
                    DepartmentId = departments.Single( s => s.Name == "Mathematics").Id
                },
                new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4,
                    DepartmentId = departments.Single( s => s.Name == "Mathematics").Id
                },
                new Course {CourseID = 2021, Title = "Composition",    Credits = 3,
                    DepartmentId = departments.Single( s => s.Name == "English").Id
                },
                new Course {CourseID = 2042, Title = "Literature",     Credits = 4,
                    DepartmentId = departments.Single( s => s.Name == "English").Id
                },
            };

            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment {
                    InstructorId = instructors.Single( i => i.LastName == "Fakhouri").Id,
                    Location = "Smith 17" },
                new OfficeAssignment {
                    InstructorId = instructors.Single( i => i.LastName == "Harui").Id,
                    Location = "Gowan 27" },
                new OfficeAssignment {
                    InstructorId = instructors.Single( i => i.LastName == "Kapoor").Id,
                    Location = "Thompson 304" },
            };

            foreach (OfficeAssignment o in officeAssignments)
            {
                context.OfficeAssignment.Add(o);
            }
            context.SaveChanges();

            var courseInstructors = new CourseAssignment[]
            {
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id
                    },
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                    },
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                    },
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                    },
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Calculus" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id
                    },
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                    },
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Composition" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                    },
                new CourseAssignment {
                    CourseId = courses.Single(c => c.Title == "Literature" ).CourseID,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                    },
            };

            foreach (CourseAssignment ci in courseInstructors)
            {
                context.CourseAssignment.Add(ci);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    Grade = Grade.A
                },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                    Grade = Grade.C
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").Id,
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alonso").Id,
                    CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").Id,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseID = courses.Single(c => c.Title == "Chemistry").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Li").Id,
                    CourseID = courses.Single(c => c.Title == "Composition").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Justice").Id,
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                    Grade = Grade.B
                    }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                            s.Student.Id == e.StudentID &&
                            s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();
        }
    }
}