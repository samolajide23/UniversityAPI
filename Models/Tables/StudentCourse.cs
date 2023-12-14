using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models.Tables
{
    public class StudentCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Students")]
        public string StudentId { get; set; }

        [Required]
        [ForeignKey("Courses")]
        public string CourseId { get; set; }
    }
}
