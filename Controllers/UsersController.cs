using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.ViewModels;
using Training_Management_System_ITI_Project.Attributes;
using Training_Management_System_ITI_Project.enums;
using Training_Management_System_ITI_Project.Repositories.Interfaces;

namespace Training_Management_System_ITI_Project.Controllers
{

  [Authorize]
  [AdminOrAbove]
  public class UsersController : Controller
  {
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<IActionResult> Index(UserRole? filterByRole)
    {
      var viewModel = new UserListViewModel
      {
        FilterByRole = filterByRole
      };

      if (filterByRole.HasValue)
      {
        viewModel.Users = (await _userRepository.GetUsersByRoleAsync(filterByRole.Value)).ToList();
      }
      else
      {
        viewModel.Users = (await _userRepository.GetAllAsync()).ToList();
      }

      return View(viewModel);
    }

    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _userRepository.GetByIdAsync(id.Value);
      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }

    public IActionResult Create()
    {
      var viewModel = new UserViewModel();
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        if (!await _userRepository.IsEmailUniqueAsync(viewModel.Email))
        {
          ModelState.AddModelError("Email", "A user with this email already exists.");
        }
        else
        {
          var user = new ApplicationUser
          {
            FullName = viewModel.Name,
            Email = viewModel.Email,
            Role = viewModel.Role
          };

          await _userRepository.AddAsync(user);
          TempData["SuccessMessage"] = "User created successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      return View(viewModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _userRepository.GetByIdAsync(id.Value);
      if (user == null)
      {
        return NotFound();
      }

      var viewModel = new UserViewModel
      {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role
      };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel viewModel)
    {
      if (id != viewModel.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        if (!await _userRepository.IsEmailUniqueAsync(viewModel.Email, viewModel.Id))
        {
          ModelState.AddModelError("Email", "A user with this email already exists.");
        }
        else
        {
          var user = await _userRepository.GetByIdAsync(id);
          if (user == null)
          {
            return NotFound();
          }

          user.FullName = viewModel.Name;
          user.Email = viewModel.Email;
          user.Role = viewModel.Role;

          await _userRepository.UpdateAsync(user);
          TempData["SuccessMessage"] = "User updated successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      return View(viewModel);
    }

    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var user = await _userRepository.GetByIdAsync(id.Value);
      if (user == null)
      {
        return NotFound();
      }

      return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var result = await _userRepository.DeleteAsync(id);
      if (result)
      {
        TempData["SuccessMessage"] = "User deleted successfully!";
      }
      else
      {
        TempData["ErrorMessage"] = "Failed to delete user.";
      }

      return RedirectToAction(nameof(Index));
    }
  }
}
