using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Models
{

  public class Grade
  {
    public int Id { get; set; }

    [Required(ErrorMessage = "Session is required")]
    public int SessionId { get; set; }

    [Required(ErrorMessage = "Trainee is required")]
    public string TraineeId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Grade value is required")]
    [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
    public int Value { get; set; }

    public virtual Session Session { get; set; } = null!;

    public virtual ApplicationUser Trainee { get; set; } = null!;
  }
}
