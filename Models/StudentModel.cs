using System.ComponentModel.DataAnnotations;

namespace UniversityAPI.Models.Tables
{
    public class StudentModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        [Required]
        [Range(1, 10)]
        public int CourseYear { get; set;}
    }
}
