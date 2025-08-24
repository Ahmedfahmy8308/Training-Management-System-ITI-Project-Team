using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Attributes;

namespace Training_Management_System_ITI_Project.Models
{

  public class Session
  {

    public int Id { get; set; }

    [Required(ErrorMessage = "Course is required")]
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    [DataType(DataType.DateTime)]
    [FutureDate(ErrorMessage = "Start date cannot be in the past")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    [DataType(DataType.DateTime)]
    [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date")]
    public DateTime EndDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
  }
}
