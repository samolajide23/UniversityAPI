using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAPI.Models;
using UniversityAPI.Models.Tables;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UniversityDbContext _context;

        public CourseController(UniversityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetCourses")]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            return Ok(courses);
        }

        [HttpGet("GetCourse/{courseId}")]
        public async Task<IActionResult> GetCourse([FromRoute] string courseId)
        {
            var courseFound = await _context.Courses.FindAsync(courseId);
            if (courseFound == null)
            {
                return NotFound();
            }

            return Ok(courseFound);
        }

        [HttpPost]
        [Route("AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] CourseModel model)
        {
            int courseCount = await _context.Courses.CountAsync();
            var newCourse = new Course
            {
                CourseId = "C" + courseCount.ToString("D3"),
                CourseName = model.CourseName,
                Description = model.Description,
                Capacity = model.Capacity,
            };

            await _context.Courses.AddAsync(newCourse);
            await _context.SaveChangesAsync();
            return Ok(newCourse);
        }

        [HttpPut]
        [Route("UpdateCourse/{courseId}")]
        public async Task<IActionResult> UpdateLecturer([FromRoute] string courseId, [FromBody] CourseModel model)
        {
            var courseFound = await _context.Courses.FindAsync(courseId);
            if (courseFound == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.CourseName) && courseFound.CourseName != model.CourseName)
            {
                courseFound.CourseName = model.CourseName;
            }
            if (!string.IsNullOrEmpty(model.Description) && courseFound.Description != model.Description)
            {
                courseFound.Description = model.Description;
            }
            if ((model.Capacity > 0) && courseFound.Capacity != model.Capacity)
            {
                courseFound.Capacity = model.Capacity;
            }

            _context.Entry(courseFound).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(courseFound);
        }

        [HttpDelete]
        [Route("RemoveCourse/{courseId}")]
        public async Task<IActionResult> RemoveCourse([FromRoute] string courseId)
        {
            var courseFound = await _context.Courses.FindAsync(courseId);
            if (courseFound == null)
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();

            return Ok("Student Removed");
        }

        [HttpGet]
        [Route("StudentsForCourses/{CourseId}")]
        public async Task<IActionResult> GetStudentsForCourse([FromRoute] string courseId)
        {
            var courseFound = await _context.Courses.FindAsync(courseId);
            if (courseFound == null)
            {
                return NotFound();
            }

            var students = await _context.StudentCourses.Where(x => x.CourseId == courseFound.CourseId).ToListAsync();

            return Ok(students);
        }

        [HttpGet]
        [Route("LecturersForCourses/{CourseId}")]
        public async Task<IActionResult> GetLecturerForCourse([FromRoute] string courseId)
        {
            var courseFound = await _context.Courses.FindAsync(courseId);
            if (courseFound == null)
            {
                return NotFound();
            }

            var lecturers = await _context.CourseLecturers.Where(x => x.CourseId == courseFound.CourseId).ToListAsync();
           
            return Ok(lecturers);
        }
    }
}
