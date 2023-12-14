using Microsoft.EntityFrameworkCore;
using UniversityAPI.Models.Tables;

namespace UniversityAPI.Models
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<CourseLecturer> CourseLecturers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          =>  optionsBuilder.UseSqlite(@"Data Source=Data/UniversityDb.db");
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
