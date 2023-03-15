using TeachersControl.Data;

namespace TeachersControl.Services
{
    public interface IAssignmentService
    {
        Task<List<Assignment>> GetAllAssignments();
        Task<Assignment> GetAssignment(string ID);
        Task<Assignment> AddAssignment(Assignment assignment);

        Task<Assignment> UpdateAssignment(Assignment assignment);
        Task<Assignment> DeleteAssignment(string ID);
    }
}