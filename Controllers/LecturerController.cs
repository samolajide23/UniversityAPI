using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityAPI.Models;
using UniversityAPI.Models.Tables;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly UniversityDbContext _context;

        public LecturerController(UniversityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetLecturers")]
        public async Task<IActionResult> GetLecturers()
        {
            var lecturers = await _context.Lecturers.ToListAsync();
            return Ok(lecturers);
        }

        [HttpGet("GetLecturer/{lecturerId}")]
        public async Task<IActionResult> GetLecturer([FromRoute] string lecturerId)
        {
            var lecturerFound = await _context.Lecturers.FindAsync(lecturerId);
            if (lecturerFound == null)
            {
                return NotFound();
            }

            return Ok(lecturerFound);
        }

        [HttpPost]
        [Route("AddLecturer")]
        public async Task<IActionResult> AddLecturer([FromBody] LecturerModel model)
        {
            int lecturerCount = await _context.Lecturers.CountAsync();
            var newLecturer = new Lecturer
            {
                LecturerId = "L" + lecturerCount.ToString("D3"),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            await _context.Lecturers.AddAsync(newLecturer);
            await _context.SaveChangesAsync();
            return Ok(newLecturer);
        }

        [HttpPut]
        [Route("UpdateLecturer/{lecturerId}")]
        public async Task<IActionResult> UpdateLecturer([FromRoute] string lecturerId, [FromBody] LecturerModel model)
        {
            var lecturerFound = await _context.Lecturers.FindAsync(lecturerId);
            if (lecturerFound == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.FirstName) && lecturerFound.FirstName != model.FirstName)
            {
                lecturerFound.FirstName = model.FirstName;
            }
            if (!string.IsNullOrEmpty(model.LastName) && lecturerFound.LastName != model.LastName)
            {
                lecturerFound.LastName = model.LastName;
            }
            if (!string.IsNullOrEmpty(model.Email) && lecturerFound.Email != model.Email)
            {
                lecturerFound.Email = model.Email;
            }
            if (!string.IsNullOrEmpty(model.PhoneNumber) && lecturerFound.PhoneNumber != model.PhoneNumber)
            {
                lecturerFound.PhoneNumber = model.PhoneNumber;
            }

            _context.Entry(lecturerFound).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(lecturerFound);
        }

        [HttpDelete]
        [Route("RemoveLecturer/{lecturerId}")]
        public async Task<IActionResult> RemoveLecturer([FromRoute] string lecturerId)
        {
            var lecturerFound = await _context.Lecturers.FindAsync(lecturerId);
            if (lecturerFound == null)
            {
                return NotFound();
            }

            var result = _context.Lecturers.Remove(lecturerFound);

            if (result.State == EntityState.Deleted)
            {
                var lecturerCourses = _context.CourseLecturers.Where(x => x.LecturerId == lecturerId).ToList();

                foreach (var lc in lecturerCourses)
                {
                    _context.CourseLecturers.Remove(lc);
                }
            }
            await _context.SaveChangesAsync();

            return Ok("Lecturer Removed");
        }

        [HttpPost]
        [Route("AddLecturerToCourse")]
        public async Task<IActionResult> AddLecturerToCourse([FromBody] AddLecturerCourseModel model)
        {
            var lecturerFound = await _context.Lecturers.FindAsync(model.LecturerId);
            if (lecturerFound == null)
            {
                return NotFound("Lecturer Not Found");
            }

            var courseFound = await _context.Courses.FindAsync(model.CourseId);
            if (courseFound == null)
            {
                return NotFound("Course Not Found");
            }

            var foundExisting = _context.CourseLecturers.FirstOrDefaultAsync(x => x.CourseId == model.CourseId && x.LecturerId == model.LecturerId);
            if (foundExisting != null)
            {
                return Conflict("Lecturer already teaches this course.");
            }

            var lecturerCourse = new CourseLecturer { 
            LecturerId = model.LecturerId,
            CourseId = model.CourseId,
            };

            await _context.CourseLecturers.AddAsync(lecturerCourse);
            await _context.SaveChangesAsync();
            return Ok("Lecturer Added to Course");
        }

        [HttpGet]
        [Route("CoursesForLecturer/{lecturerId}")]
        public async Task<IActionResult> GetCoursesForLecturer([FromRoute] string lecturerId)
        {
            var lecturerFound = await _context.Lecturers.FindAsync(lecturerId);
            if (lecturerFound == null)
            {
                return NotFound();
            }

            var courses = await _context.CourseLecturers.Where(x => x.LecturerId == lecturerFound.LecturerId).ToListAsync();

            return Ok(courses);
        }
    }
}
