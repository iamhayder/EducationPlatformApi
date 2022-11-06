using Microsoft.EntityFrameworkCore;
using EducationPlatformApi.Models;


namespace EducationPlatformApi.Data
{
    public class EducationPlatformApiContext: DbContext
    {
        public EducationPlatformApiContext(DbContextOptions<EducationPlatformApiContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Course> Courses => Set<Course>();

    }
}
