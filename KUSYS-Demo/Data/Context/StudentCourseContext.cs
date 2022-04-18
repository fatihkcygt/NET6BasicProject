using KUSYS_Demo.Helpers;
using KUSYS_Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace KUSYS_Demo.Data.Context
{
    public class StudentCourseContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<User> Users { get; set; }

        public StudentCourseContext(DbContextOptions<StudentCourseContext> options) : base(options)
        {

        }
        public StudentCourseContext() : base()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }

    }
}
