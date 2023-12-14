using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models
{
    public class AddLecturerCourseModel
    {
        [Required]
        public string CourseId { get; set; }

        [Required]
        public string LecturerId { get; set; }
    }
}
