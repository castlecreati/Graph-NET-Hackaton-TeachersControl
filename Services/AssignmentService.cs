using Microsoft.Graph;
using Microsoft.Identity.Web;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TeachersControl.Data;

namespace TeachersControl.Services
{
    public class AssignmentService : IAssignmentService
    {
        private HttpClient _httpClient;
        private IHttpClientFactory _httpClientFactory;
        private ITokenAcquisition _tokenAcquisitionService;
        private List<Assignment> assignments = new List<Assignment>();
        private Assignment assignment = new Assignment();

        public AssignmentService(IHttpClientFactory HttpClientFactory,
               ITokenAcquisition TokenAcquisitionService)
        {
            _httpClientFactory = HttpClientFactory;
            _tokenAcquisitionService = TokenAcquisitionService;

        }

        public async Task<List<Assignment>> GetAllAssignments()
        {
            _httpClient = _httpClientFactory.CreateClient();

            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read", "User.Read.All", "Group.Read.All", "Sites.Read.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");

            var listRequest = await _httpClient
                .GetAsync("https://graph.microsoft.com/v1.0/sites/90403554-37e2-45da-b2bf-fa5ec6f6925f/lists/a1ff6a07-15d9-466e-8bfd-6ed7158d469c/items?$expand=fields($select=id,Teacher0,Section,Subject)");

            if (listRequest.IsSuccessStatusCode)
            {
                assignments.Clear();
                var assignmentsData = System.Text.Json.JsonDocument.Parse(await listRequest.Content.ReadAsStreamAsync());
                var assignmentsArray = assignmentsData.RootElement.GetProperty("value").EnumerateArray();

                foreach (var c in assignmentsArray)
                {
                    var assignment = new Assignment();
                    var fields = c.GetProperty("fields");
                    assignment.Id = fields.GetProperty("id").GetString();
                    assignment.Teacher = fields.GetProperty("Teacher0").GetString();
                    assignment.Section = fields.GetProperty("Section").GetString();
                    assignment.Subject = fields.GetProperty("Subject").GetString();
                    assignments.Add(assignment);
                }
            }
            return assignments;
        }

        public async Task<Assignment> GetAssignment(string ID)
        {
            _httpClient = _httpClientFactory.CreateClient();

            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read", "User.Read.All", "Group.Read.All", "Sites.ReadWrite.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");

            string url = "https://graph.microsoft.com/v1.0/sites/90403554-37e2-45da-b2bf-fa5ec6f6925f/lists/a1ff6a07-15d9-466e-8bfd-6ed7158d469c/items/"+$"{ID}?$expand=fields($select=id,Teacher0,Section,Subject)";
            var listRequest = await _httpClient.GetAsync(url);

            if (listRequest.IsSuccessStatusCode)
            {

                var assignmentsData = System.Text.Json.JsonDocument.Parse(await listRequest.Content.ReadAsStreamAsync());
                var fields = assignmentsData.RootElement.GetProperty("fields");

                assignment = new Assignment()
                {
                    Id = fields.GetProperty("id").GetString(),
                    Teacher = fields.GetProperty("Teacher0").GetString(),
                    Section = fields.GetProperty("Section").GetString(),
                    Subject = fields.GetProperty("Subject").GetString()
                };

                Console.WriteLine(assignment);
                
            
            }
            return assignment;

        }
        public async Task<Assignment> AddAssignment(Assignment assignment)
        {
            _httpClient = _httpClientFactory.CreateClient();

            // get a token
            var token = await _tokenAcquisitionService.GetAccessTokenForUserAsync(new string[] { "User.Read", "User.Read.All", "Group.Read.All", "Sites.ReadWrite.All" });

            // make API call
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "Eventual");

            // Construir el cuerpo de la solicitud con los valores de los campos
            var requestBody = new
            {
                fields = new Dictionary<string, object>()
                {
                    {"Section", assignment.Section},
                    {"Subject", assignment.Subject},
                    {"Teacher0", assignment.Teacher}
                }
            };
            // Send HTTP request to Graph API

            var requestUrl = $"https://graph.microsoft.com/v1.0/sites/90403554-37e2-45da-b2bf-fa5ec6f6925f/lists/a1ff6a07-15d9-466e-8bfd-6ed7158d469c/items";
            //request.Content = new StringContent(JsonSerializer.Serialize(assignment), Encoding.UTF8, "application/json");
            //var response = await _httpClient.SendAsync(request); 
            var response = await _httpClient.PostAsync(requestUrl, new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();

         

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Assignment>(await response.Content.ReadAsStreamAsync());
            }

            return null;


        }
        public Task<Assignment> UpdateAssignment(Assignment assignment)
        {
            throw new NotImplementedException();
        }
        public Task<Assignment> DeleteAssignment(string ID)
        {
            throw new NotImplementedException();
        }
        
    }
}
