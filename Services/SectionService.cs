using Microsoft.Graph;
using Microsoft.Identity.Web;
using static TeachersControl.Pages.CalendarItems;
using Calendar = TeachersControl.Pages.SectionsData.Calendar;

namespace TeachersControl.Services
{
    public class SectionService : ISectionService
    {
        private HttpClient _httpClient;
        private IHttpClientFactory _httpClientFactory;
        private ITokenAcquisition _tokenAcquisitionService;
        private List<Calendar> calendars = new List<Calendar>();
        private List<string> subjects = new List<string>();
        private List<CalendarItem> calendarItems = new List<CalendarItem>();

        public SectionService(IHttpClientFactory HttpClientFactory,
               ITokenAcquisition TokenAcquisitionService)
        {
            _httpClientFactory = HttpClientFactory;
            _tokenAcquisitionService = TokenAcquisitionService;

        }

        public async Task<List<Calendar>> GetAllCalendars()
        {
            _httpClient = _httpClientFactory.CreateClient();

            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read", "User.Read.All", "Group.Read.All", "Sites.Read.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");

            var listRequest = await _httpClient.GetAsync("https://graph.microsoft.com/v1.0/sites/90403554-37e2-45da-b2bf-fa5ec6f6925f/lists?$select=id,name");

            if (listRequest.IsSuccessStatusCode)
            {
                calendars.Clear();
                var listsData = System.Text.Json.JsonDocument.Parse(await listRequest.Content.ReadAsStreamAsync());
                var listsArray = listsData.RootElement.GetProperty("value").EnumerateArray();

                foreach (var c in listsArray)
                {
                    var calendar = new Calendar();
                    calendar.Id = c.GetProperty("id").GetString();
                    calendar.Name = c.GetProperty("name").GetString();
                    if (calendar.Name.Contains("Section")) // We filter here because MSGraph doesn't have apropiate filters in sharepoint lists request
                        calendars.Add(calendar);
                }
            }
            return calendars;
        }
        public async Task<List<CalendarItem>> GetCalendarItemsByCalendar(string id)
        {
            _httpClient = _httpClientFactory.CreateClient();
            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read", "User.Read.All", "Group.Read.All", "Sites.Read.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");
            var Url = "https://graph.microsoft.com/v1.0/sites/90403554-37e2-45da-b2bf-fa5ec6f6925f/lists/"
                + $"{id}/items?$expand=fields($select=Title,field_1,field_2,field_3)";
            var listRequest = await _httpClient.GetAsync(Url);

            if (listRequest.IsSuccessStatusCode)
            {
                var getItems = System.Text.Json.JsonDocument.Parse(await listRequest.Content.ReadAsStreamAsync());
                var itemsArray = getItems.RootElement.GetProperty("value").EnumerateArray();
                calendarItems.Clear();
                foreach (var c in itemsArray)
                {

                    var calendarItem = new CalendarItem();
                    var fields = c.GetProperty("fields");
                    calendarItem.Clase = fields.GetProperty("Title").GetString();
                    calendarItem.StartTime = fields.GetProperty("field_1").GetDateTime();
                    calendarItem.FinishTime = fields.GetProperty("field_2").GetDateTime();
                    calendarItem.Subject = fields.GetProperty("field_3").GetString();
                    calendarItems.Add(calendarItem);

                }
            }
            return calendarItems;
        }
        public async Task<List<string>> GetDistinctSubjects()
        {
            _httpClient = _httpClientFactory.CreateClient();
            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read.All", "Group.Read.All", "Sites.Read.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");

            var listRequest = await _httpClient
            .GetAsync("https://graph.microsoft.com/v1.0/sites/90403554-37e2-45da-b2bf-fa5ec6f6925f/lists/e33e9693-f71b-4322-aedc-876ef83ed13f/items?$expand=fields($select=field_3)");

            if (listRequest.IsSuccessStatusCode)
            {
                var getItems = System.Text.Json.JsonDocument.Parse(await listRequest.Content.ReadAsStreamAsync());
                var itemsArray = getItems.RootElement.GetProperty("value").EnumerateArray();
                foreach (var c in itemsArray)
                {

                    //var subject = new List();
                    var fields = c.GetProperty("fields");
                    var subject = fields.GetProperty("field_3").GetString();
                    subjects.Add(subject);

                }
            }
            return subjects.Distinct().ToList();
        }

        public async Task<List<CalendarItem>> GetAllCalendarItems()
        {
            _httpClient = _httpClientFactory.CreateClient();
            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read", "User.Read.All", "Group.Read.All", "Sites.Read.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");
            var listRequest = await _httpClient
            .GetAsync("https://graph.microsoft.com/v1.0/sites/90403554-37e2-45da-b2bf-fa5ec6f6925f/lists/e33e9693-f71b-4322-aedc-876ef83ed13f/items?$expand=fields($select=Title,field_1,field_2,field_3)");

            if (listRequest.IsSuccessStatusCode)
            {
                var getItems = System.Text.Json.JsonDocument.Parse(await listRequest.Content.ReadAsStreamAsync());
                var itemsArray = getItems.RootElement.GetProperty("value").EnumerateArray();
                calendarItems.Clear();
                foreach (var c in itemsArray)
                {

                    var calendarItem = new CalendarItem();
                    var fields = c.GetProperty("fields");
                    calendarItem.Clase = fields.GetProperty("Title").GetString();
                    calendarItem.StartTime = fields.GetProperty("field_1").GetDateTime();
                    calendarItem.FinishTime = fields.GetProperty("field_2").GetDateTime();
                    calendarItem.Subject = fields.GetProperty("field_3").GetString();
                    calendarItems.Add(calendarItem);

                }
            }
            return calendarItems;

        }
    }
}
