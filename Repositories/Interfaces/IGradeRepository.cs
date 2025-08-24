using Training_Management_System_ITI_Project.Models;

namespace Training_Management_System_ITI_Project.Repositories.Interfaces
{
    public interface IGradeRepository : IRepository<Grade>
    {
        Task<IEnumerable<Grade>> GetGradesByTraineeAsync(string traineeId);
        Task<IEnumerable<Grade>> GetGradesBySessionAsync(int sessionId);
        Task<Grade?> GetGradeBySessionAndTraineeAsync(int sessionId, string traineeId);
        Task<IEnumerable<Grade>> GetGradesWithDetailsAsync();
    }
}
