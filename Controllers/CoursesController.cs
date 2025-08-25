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
  public class CoursesController : Controller
  {
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;


    public CoursesController(ICourseRepository courseRepository, IUserRepository userRepository)
    {
      _courseRepository = courseRepository;
      _userRepository = userRepository;
    }


    public async Task<IActionResult> Index(string searchTerm)
    {
      var viewModel = new CourseSearchViewModel
      {
        SearchTerm = searchTerm
      };

      if (string.IsNullOrEmpty(searchTerm))
      {
        viewModel.Courses = (await _courseRepository.GetCoursesWithInstructorAsync()).ToList();
      }
      else
      {
        viewModel.Courses = (await _courseRepository.SearchByNameOrCategoryAsync(searchTerm)).ToList();
      }

      return View(viewModel);
    }


    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var course = await _courseRepository.GetByIdAsync(id.Value);
      if (course == null)
      {
        return NotFound();
      }

      return View(course);
    }

    [InstructorOrAbove]
    public async Task<IActionResult> Create()
    {
      var viewModel = new CourseViewModel
      {
        AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList()
      };
      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [InstructorOrAbove]
    public async Task<IActionResult> Create(CourseViewModel viewModel)
    {
      if (ModelState.IsValid)
      {
        if (!await _courseRepository.IsNameUniqueAsync(viewModel.Name))
        {
          ModelState.AddModelError("Name", "A course with this name already exists.");
        }
        else
        {
          var course = new Course
          {
            Name = viewModel.Name,
            Category = viewModel.Category,
            InstructorId = viewModel.InstructorId
          };

          await _courseRepository.AddAsync(course);
          TempData["SuccessMessage"] = "Course created successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      viewModel.AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList();
      return View(viewModel);
    }

    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var course = await _courseRepository.GetByIdAsync(id.Value);
      if (course == null)
      {
        return NotFound();
      }

      var viewModel = new CourseViewModel
      {
        Id = course.Id,
        Name = course.Name,
        Category = course.Category,
        InstructorId = course.InstructorId,
        AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList()
      };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CourseViewModel viewModel)
    {
      if (id != viewModel.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        if (!await _courseRepository.IsNameUniqueAsync(viewModel.Name, viewModel.Id))
        {
          ModelState.AddModelError("Name", "A course with this name already exists.");
        }
        else
        {
          var course = await _courseRepository.GetByIdAsync(id);
          if (course == null)
          {
            return NotFound();
          }

          course.Name = viewModel.Name;
          course.Category = viewModel.Category;
          course.InstructorId = viewModel.InstructorId;

          await _courseRepository.UpdateAsync(course);
          TempData["SuccessMessage"] = "Course updated successfully!";
          return RedirectToAction(nameof(Index));
        }
      }

      viewModel.AvailableInstructors = (await _userRepository.GetUsersByRoleAsync(UserRole.Instructor)).ToList();
      return View(viewModel);
    }


    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var course = await _courseRepository.GetByIdAsync(id.Value);
      if (course == null)
      {
        return NotFound();
      }

      return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var result = await _courseRepository.DeleteAsync(id);
      if (result)
      {
        TempData["SuccessMessage"] = "Course deleted successfully!";
      }
      else
      {
        TempData["ErrorMessage"] = "Failed to delete course.";
      }

      return RedirectToAction(nameof(Index));
    }
  }
}
