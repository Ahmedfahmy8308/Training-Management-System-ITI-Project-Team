using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.enums;

namespace Training_Management_System_ITI_Project.Models
{
  public class ApplicationUser : IdentityUser
  {

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 50 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    public UserRole Role { get; set; } = UserRole.Trainee;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Course> CoursesAsInstructor { get; set; } = new List<Course>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
  }
}
