using Microsoft.Identity.Web;
using TeachersControl.Pages;
using static TeachersControl.Pages.TeachersData;

namespace TeachersControl.Services
{
    public class TeacherDataService : ITeacherDataService
    {
        private HttpClient _httpClient;
        private IHttpClientFactory _httpClientFactory;
        private ITokenAcquisition _tokenAcquisitionService;
        private List<Teacher> Teachers = new List<Teacher>();

        public TeacherDataService(IHttpClientFactory HttpClientFactory,
               ITokenAcquisition TokenAcquisitionService)
        {
            _httpClientFactory = HttpClientFactory;
            _tokenAcquisitionService = TokenAcquisitionService;
            
        }

        public async Task<List<TeachersData.Teacher>> GetAllTeachers() 
        {
            _httpClient = _httpClientFactory.CreateClient();

            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read.All", "Group.Read.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");

            // Since there's no context for persistence, teachers list must be cleared each time there is an interaction in the browser. Otherwise the list multiplies
            Teachers.Clear();
            var usersRequest = await _httpClient.GetAsync("https://graph.microsoft.com/v1.0/groups/666d64d9-49a8-4cf9-8dc3-ae752496da60/members?$select=displayName,mail");
            if (usersRequest.IsSuccessStatusCode)
            {
                var usersData = System.Text.Json.JsonDocument.Parse(await usersRequest.Content.ReadAsStreamAsync());
                var usersArray = usersData.RootElement.GetProperty("value").EnumerateArray();
                

                foreach (var u in usersArray)
                {
                    var teacher = new Teacher();
                    teacher.FullName = u.GetProperty("displayName").GetString();
                    teacher.Email = u.GetProperty("mail").GetString();
                    Teachers.Add(teacher);
                }

            }
            return Teachers;
        }
    }
}
