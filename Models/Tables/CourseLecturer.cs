using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityAPI.Models.Tables
{
    public class CourseLecturer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Courses")]
        public string CourseId { get; set; }

        [Required]
        [ForeignKey("Lecturers")]
        public string LecturerId { get; set; }
    }
}