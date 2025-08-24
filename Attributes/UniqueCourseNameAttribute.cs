using System.ComponentModel.DataAnnotations;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Attributes
{

  public class UniqueCourseNameAttribute : ValidationAttribute
  {

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      if (value == null || string.IsNullOrEmpty(value.ToString()))
        return ValidationResult.Success;

      var context = validationContext.GetService<ApplicationDbContext>();
      if (context == null)
        return ValidationResult.Success;

      var courseName = value.ToString()!;
      var course = validationContext.ObjectInstance as Course;


      var existingCourse = context.Courses
          .FirstOrDefault(c => c.Name.ToLower() == courseName.ToLower() &&
                              (course == null || c.Id != course.Id));

      if (existingCourse != null)
      {
        return new ValidationResult("A course with this name already exists.");
      }

      return ValidationResult.Success;
    }
  }
}
