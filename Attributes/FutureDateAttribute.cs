using System.ComponentModel.DataAnnotations;

namespace Training_Management_System_ITI_Project.Attributes
{

  public class FutureDateAttribute : ValidationAttribute
  {

    public override bool IsValid(object? value)
    {
      if (value is DateTime dateTime)
      {
        return dateTime >= DateTime.Now.Date;
      }
      return false;
    }
  }
}
