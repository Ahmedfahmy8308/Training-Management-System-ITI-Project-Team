using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Data
{

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Session> Sessions { get; set; }

    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Course>(entity =>
      {
        entity.HasKey(e => e.Id);

        entity.HasIndex(e => e.Name).IsUnique();
        entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Category).IsRequired();

        entity.HasOne(c => c.Instructor)
                    .WithMany(u => u.CoursesAsInstructor)
                    .HasForeignKey(c => c.InstructorId)
                    .OnDelete(DeleteBehavior.SetNull);
      });

      modelBuilder.Entity<Session>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.StartDate).IsRequired();
        entity.Property(e => e.EndDate).IsRequired();

        entity.HasOne(s => s.Course)
                    .WithMany(c => c.Sessions)
                    .HasForeignKey(s => s.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
      });

      modelBuilder.Entity<Grade>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Value).IsRequired();


        entity.HasOne(g => g.Session)
                    .WithMany(s => s.Grades)
                    .HasForeignKey(g => g.SessionId)
                    .OnDelete(DeleteBehavior.Cascade);


        entity.HasOne(g => g.Trainee)
                    .WithMany(u => u.Grades)
                    .HasForeignKey(g => g.TraineeId)
                    .OnDelete(DeleteBehavior.Cascade);

        entity.HasIndex(e => new { e.SessionId, e.TraineeId }).IsUnique();
      });
    }
  }
}
