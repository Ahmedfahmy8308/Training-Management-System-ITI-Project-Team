using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories.Interfaces;

namespace Training_Management_System_ITI_Project.Controllers
{

  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly ICourseRepository _courseRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGradeRepository _gradeRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger,
        ICourseRepository courseRepository,
        ISessionRepository sessionRepository,
        IUserRepository userRepository,
        IGradeRepository gradeRepository,
        UserManager<ApplicationUser> userManager)
    {
      _logger = logger;
      _courseRepository = courseRepository;
      _sessionRepository = sessionRepository;
      _userRepository = userRepository;
      _gradeRepository = gradeRepository;
      _userManager = userManager;
    }


    public IActionResult Index()
    {
      if (User.Identity?.IsAuthenticated == true)
      {
        return RedirectToAction("Dashboard");
      }

      return View("Landing");
    }


    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
      var currentUser = await _userManager.GetUserAsync(User);

      ViewBag.TotalCourses = (await _courseRepository.GetAllAsync()).Count();
      ViewBag.TotalSessions = (await _sessionRepository.GetAllAsync()).Count();
      ViewBag.TotalUsers = (await _userRepository.GetAllAsync()).Count();
      ViewBag.TotalGrades = (await _gradeRepository.GetAllAsync()).Count();

      ViewBag.UserRole = currentUser?.Role.ToString();
      ViewBag.UserName = currentUser?.FullName;

      return View();
    }

    [AllowAnonymous]
    public IActionResult RegisterStudent()
    {
      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
