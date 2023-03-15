using static TeachersControl.Pages.TeachersData;

namespace TeachersControl.Services
{
    public interface ITeacherDataService
    {
        Task<List<Teacher>> GetAllTeachers();
    }
}
