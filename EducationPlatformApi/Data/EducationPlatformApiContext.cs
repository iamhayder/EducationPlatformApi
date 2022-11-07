using Microsoft.EntityFrameworkCore;
using EducationPlatformApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace EducationPlatformApi.Data
{
    public class EducationPlatformApiContext : IdentityDbContext<ApplicationUser>
    {
        public EducationPlatformApiContext(DbContextOptions<EducationPlatformApiContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });
            modelBuilder.Entity<UserCourse>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserCourses)
                .HasForeignKey(bc => bc.UserId);
            modelBuilder.Entity<UserCourse>()
                .HasOne(bc => bc.Course)
                .WithMany(c => c.CourseUsers)
                .HasForeignKey(bc => bc.CourseId);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<UserCourse> UserCourses => Set<UserCourse>();
    }
}
