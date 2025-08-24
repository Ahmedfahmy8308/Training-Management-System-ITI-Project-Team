using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.Models;


namespace Training_Management_System_ITI_Project.Attributes
{

  public class UniqueEmailAttribute : ValidationAttribute
  {

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      if (value == null || string.IsNullOrEmpty(value.ToString()))
        return ValidationResult.Success;

      var userManager = validationContext.GetService<UserManager<ApplicationUser>>();
      if (userManager == null)
        return ValidationResult.Success;

      var email = value.ToString()!;
      var currentUser = validationContext.ObjectInstance as ApplicationUser;

      var existingUser = userManager.Users
          .FirstOrDefault(u => u.Email!.ToLower() == email.ToLower());

      if (existingUser != null && (currentUser == null || existingUser.Id != currentUser.Id))
      {
        return new ValidationResult("A user with this email address already exists.");
      }

      return ValidationResult.Success;
    }
  }
}
