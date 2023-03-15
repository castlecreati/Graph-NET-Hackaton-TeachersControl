using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace TeachersControl.Services
{
    public class UserService : IUserService
    {
        User? user;
        private GraphServiceClient _graphServiceClient;
        private MicrosoftIdentityConsentAndConditionalAccessHandler _consentHandler;

        public UserService(GraphServiceClient GraphServiceClient,
                           MicrosoftIdentityConsentAndConditionalAccessHandler ConsentHandler)
        {
            _graphServiceClient = GraphServiceClient;
            _consentHandler = ConsentHandler;
        }
        public async Task<User> GetUserAsync()
        {
            try
            {
                user = await _graphServiceClient.Me.Request().GetAsync();
            }
            catch (Exception ex)
            {
                _consentHandler.HandleException(ex);
            }
            return user;
        }
    }
}
