using TeachersControl.Pages;
using static TeachersControl.Pages.CalendarItems;

namespace TeachersControl.Services
{
    public interface ISectionService
    {
        Task<List<SectionsData.Calendar>> GetAllCalendars();
        Task<List<string>> GetDistinctSubjects();
        Task<List<CalendarItem>> GetAllCalendarItems();
        Task<List<CalendarItem>> GetCalendarItemsByCalendar(string id);
    }
}