using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models.Tables
{
    public class Course
    {
        [Key]
        public string CourseId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CourseName { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Capacity { get; set; }
    }
}
