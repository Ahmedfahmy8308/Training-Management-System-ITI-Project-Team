using Microsoft.EntityFrameworkCore;
using Training_Management_System_ITI_Project.Data;
using Training_Management_System_ITI_Project.Models;
using Training_Management_System_ITI_Project.Repositories.Interfaces;

namespace Training_Management_System_ITI_Project.Repositories
{
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        public GradeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Grade?> GetGradeBySessionAndTraineeAsync(int sessionId, string traineeId)
        {
            return await _dbSet
                .Include(g => g.Session)
                .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .FirstOrDefaultAsync(g => g.SessionId == sessionId && g.TraineeId == traineeId);
        }

        public async Task<IEnumerable<Grade>> GetGradesBySessionAsync(int sessionId)
        {
            return await _dbSet
                .Include(g => g.Session)
                .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .Where(g => g.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetGradesByTraineeAsync(string traineeId)
        {
            return await _dbSet
                .Include(g => g.Session)
                .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .Where(g => g.TraineeId == traineeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Grade>> GetGradesWithDetailsAsync()
        {
            return await _dbSet
                .Include(g => g.Session)
                .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .ToListAsync();
        }

        public override async Task<Grade?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(g => g.Session)
                .ThenInclude(s => s.Course)
                .Include(g => g.Trainee)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
