using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{

  public class Course
  {

    public int Id { get; set; }


    [Required(ErrorMessage = "Course name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Course name must be between 3 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    public string? InstructorId { get; set; }

    public virtual ApplicationUser? Instructor { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
  }
}
