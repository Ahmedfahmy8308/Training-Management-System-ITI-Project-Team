using Training_Management_System_ITI_Project.enums;
using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories.Interfaces
{
  public interface IUserRepository : IRepository<ApplicationUser>
  {
    Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(UserRole role);
    Task<bool> IsEmailUniqueAsync(string email, string? excludeId = null);
    Task<ApplicationUser?> GetByStringIdAsync(string id);
  }
}
