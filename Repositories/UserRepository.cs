using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.enums;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories.Interfaces;

namespace Training_Management_System_ITI_Project.Repositories
{
  public class UserRepository : Repository<ApplicationUser>, IUserRepository
  {
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(UserRole role)
    {
      return await _dbSet
          .Where(u => u.Role == role)
          .ToListAsync();
    }

    public async Task<bool> IsEmailUniqueAsync(string email, string? excludeId = null)
    {
      var query = _dbSet.Where(u => u.Email!.ToLower() == email.ToLower());

      if (!string.IsNullOrEmpty(excludeId))
      {
        query = query.Where(u => u.Id != excludeId);
      }

      return !await query.AnyAsync();
    }

    public async Task<ApplicationUser?> GetByStringIdAsync(string id)
    {
      return await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
    }
  }
}
