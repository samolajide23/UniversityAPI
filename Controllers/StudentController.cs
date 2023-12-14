using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAPI.Models;
using UniversityAPI.Models.Tables;

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly UniversityDbContext _context;

        public StudentController(UniversityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetStudents")]
        public async Task<IActionResult> GetStudents()
        {
           var students = await _context.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("GetStudent/{studentId}")]
        public async Task<IActionResult> GetLecturer([FromRoute] string studentId)
        {
            var studentFound = await _context.Students.FindAsync(studentId);
            if (studentFound == null)
            {
                return NotFound();
            }

            return Ok(studentFound);
        }

        [HttpPost]
        [Route("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentModel model)
        {
            int studentCount = await _context.Students.CountAsync();
            var newStudent = new Student
            {
                StudentId = "S" + studentCount.ToString("D6"),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CourseYear = model.CourseYear,
            };

            await _context.Students.AddAsync(newStudent);
            await _context.SaveChangesAsync();
                
            return Ok(newStudent);
        }

        [HttpPut]
        [Route("UpdateStudent/{studentId}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] string studentId, [FromBody] StudentModel student)
        {
            var studentFound = await _context.Students.FindAsync(studentId);
            if (studentFound == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(student.FirstName) && studentFound.FirstName != student.FirstName)
            {
                studentFound.FirstName = student.FirstName;
            }
            else if (!string.IsNullOrEmpty(student.LastName) && studentFound.LastName != student.LastName)
            {
                studentFound.LastName = student.LastName;
            }
            if (!string.IsNullOrEmpty(student.Email) && studentFound.Email != student.Email)
            {
                studentFound.Email = student.Email;
            }
            if (!string.IsNullOrEmpty(student.PhoneNumber) && studentFound.PhoneNumber != student.PhoneNumber)
            {
                studentFound.PhoneNumber = student.PhoneNumber;
            }
            if ((student.CourseYear > 0) && studentFound.CourseYear != student.CourseYear)
            {
                studentFound.CourseYear = student.CourseYear;
            }

            _context.Entry(studentFound).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(studentFound);
        }
        
        [HttpDelete]
        [Route("RemoveStudent/{studentId}")]
        public async Task<IActionResult> RemoveStudent([FromRoute] string studentId)
        {
            var studentFound = await _context.Students.FindAsync(studentId);
            if (studentFound == null)
            {
                return NotFound();
            }

            var result = _context.Students.Remove(studentFound);

            if (result.State == EntityState.Deleted)
            { 
                var studentCourses = _context.StudentCourses.Where(x => x.StudentId == studentId).ToList();

                foreach (var sc in studentCourses)
                {
                   _context.StudentCourses.Remove(sc);
                }
            }
            await _context.SaveChangesAsync();

            return Ok("Student Removed");
        }
        
        [HttpPost]
        [Route("AddStudentToCourse")]
        public async Task<IActionResult> AddStudentToCourse([FromBody] AddStudentCourseModel model)
        {
            var studentFound = await _context.Students.FindAsync(model.StudentId);
            if (studentFound == null)
            {
                return NotFound("Student Not Found");
            }

            var courseFound = await _context.Courses.FindAsync(model.CourseId);
            if (courseFound == null)
            {
                return NotFound("Course Not Found");
            }

            var foundExisting = _context.StudentCourses.FirstOrDefaultAsync(x => x.CourseId == model.CourseId && x.StudentId == model.StudentId);
            if (foundExisting ! == null)
            {
                return Conflict("Student already assigned to course");
            }


            var studentCourse = new StudentCourse { 
            StudentId = model.StudentId,
            CourseId = model.CourseId,
            };

            await _context.StudentCourses.AddAsync(studentCourse);
            await _context.SaveChangesAsync();
            return Ok("Student Added to Course");
        }

        
    }
}
