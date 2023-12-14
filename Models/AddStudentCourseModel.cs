using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models
{
    public class AddStudentCourseModel
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public string CourseId { get; set; }
    }
}
