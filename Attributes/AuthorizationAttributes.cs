using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Training_Management_System_ITI_Project.Models;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.enums;

namespace Training_Management_System_ITI_Project.Attributes
{

  public class MinimumRoleAttribute : Attribute, IAuthorizationFilter
  {
    private readonly UserRole _minimumRole;


    public MinimumRoleAttribute(UserRole minimumRole)
    {
      _minimumRole = minimumRole;
    }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      var userManager = context.HttpContext.RequestServices
          .GetService<UserManager<ApplicationUser>>();

      if (userManager == null)
      {
        context.Result = new ForbidResult();
        return;
      }

      var user = userManager.GetUserAsync(context.HttpContext.User).Result;
      if (user == null || !user.IsActive)
      {
        context.Result = new ForbidResult();
        return;
      }

      if ((int)user.Role < (int)_minimumRole)
      {
        context.Result = new ForbidResult();
        return;
      }
    }
  }


  public class RequireRoleAttribute : Attribute, IAuthorizationFilter
  {
    private readonly UserRole[] _allowedRoles;


    public RequireRoleAttribute(params UserRole[] allowedRoles)
    {
      _allowedRoles = allowedRoles;
    }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      var userManager = context.HttpContext.RequestServices
          .GetService<UserManager<ApplicationUser>>();

      if (userManager == null)
      {
        context.Result = new ForbidResult();
        return;
      }

      var user = userManager.GetUserAsync(context.HttpContext.User).Result;
      if (user == null || !user.IsActive)
      {
        context.Result = new ForbidResult();
        return;
      }

      if (!_allowedRoles.Contains(user.Role))
      {
        context.Result = new ForbidResult();
        return;
      }
    }
  }


  public class SuperAdminOnlyAttribute : MinimumRoleAttribute
  {
    public SuperAdminOnlyAttribute() : base(UserRole.SuperAdmin)
    {
    }
  }


  public class AdminOrAboveAttribute : MinimumRoleAttribute
  {
    public AdminOrAboveAttribute() : base(UserRole.Admin)
    {
    }
  }


  public class InstructorOrAboveAttribute : MinimumRoleAttribute
  {
    public InstructorOrAboveAttribute() : base(UserRole.Instructor)
    {
    }
  }


  public class ResourceOwnerOrAdminAttribute : Attribute, IAuthorizationFilter
  {
    private readonly string _userIdParameterName;


    public ResourceOwnerOrAdminAttribute(string userIdParameterName = "id")
    {
      _userIdParameterName = userIdParameterName;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      var userManager = context.HttpContext.RequestServices
          .GetService<UserManager<ApplicationUser>>();

      if (userManager == null)
      {
        context.Result = new ForbidResult();
        return;
      }

      var currentUser = userManager.GetUserAsync(context.HttpContext.User).Result;
      if (currentUser == null || !currentUser.IsActive)
      {
        context.Result = new ForbidResult();
        return;
      }

      if (currentUser.Role >= UserRole.Admin)
      {
        return;
      }

      var resourceUserId = context.RouteData.Values[_userIdParameterName]?.ToString() ??
                         context.HttpContext.Request.Query[_userIdParameterName].FirstOrDefault();

      if (string.IsNullOrEmpty(resourceUserId) || currentUser.Id != resourceUserId)
      {
        context.Result = new ForbidResult();
        return;
      }
    }
  }
}
