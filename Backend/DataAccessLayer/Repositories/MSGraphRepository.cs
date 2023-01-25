using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using Microsoft.Graph;
using LogicLayer.Models;
using LogicLayer.Repositories;

namespace DataAccessLayer.Repositories
{
    public class MSGraphRepository : IMSGraphRepository
    {
        /// <summary>
        /// An example of how to authenticate the Microsoft Graph SDK using the MSAL library
        /// </summary>
        /// <returns></returns>
        public GraphServiceClient GetAuthenticatedGraphClient()
        {
            AuthenticationConfig config = new AuthenticationConfig();

            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication
            bool isUsingClientSecret = IsAppUsingClientSecret(config);

            // Even if this is a console application here, a daemon application is a confidential client application
            IConfidentialClientApplication app;

            if (isUsingClientSecret)
            {
                // Even if this is a console application here, a daemon application is a confidential client application
                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            else
            {
                ICertificateLoader certificateLoader = new DefaultCertificateLoader();
                certificateLoader.LoadIfNeeded(config.Certificate);

                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithCertificate(config.Certificate.Certificate)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            app.AddInMemoryTokenCache();


            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator. 
            string[] scopes = new string[] { $"{config.ApiUrl}.default" }; // Generates a scope -> "https://graph.microsoft.com/.default"

            GraphServiceClient graphServiceClient =
                    new GraphServiceClient("https://graph.microsoft.com/V1.0/", new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                        AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                        // Add the access token in the Authorization header of the API request.
                        requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    }));

            return graphServiceClient;
        }


        public async Task<string> GetAccessToken()
        {
            AuthenticationConfig config = new AuthenticationConfig();

            // You can run this sample using ClientSecret or Certificate. The code will differ only when instantiating the IConfidentialClientApplication
            bool isUsingClientSecret = IsAppUsingClientSecret(config);

            // Even if this is a console application here, a daemon application is a confidential client application
            IConfidentialClientApplication app;

            if (isUsingClientSecret)
            {
                // Even if this is a console application here, a daemon application is a confidential client application
                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            else
            {
                ICertificateLoader certificateLoader = new DefaultCertificateLoader();
                certificateLoader.LoadIfNeeded(config.Certificate);

                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithCertificate(config.Certificate.Certificate)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();
            }

            app.AddInMemoryTokenCache();


            // With client credentials flows the scopes is ALWAYS of the shape "resource/.default", as the 
            // application permissions need to be set statically (in the portal or by PowerShell), and then granted by
            // a tenant administrator. 
            string[] scopes = new string[] { $"{config.ApiUrl}.default" }; // Generates a scope -> "https://graph.microsoft.com/.default"

            GraphServiceClient graphServiceClient =
                    new GraphServiceClient("https://graph.microsoft.com/V1.0/", new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                        AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                        // Add the access token in the Authorization header of the API request.
                        requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    }));

            return app.AcquireTokenForClient(scopes)
                   .ExecuteAsync().Result.AccessToken;
        }


        /// <summary>
        /// Checks if the sample is configured for using ClientSecret or Certificate. This method is just for the sake of this sample.
        /// You won't need this verification in your production application since you will be authenticating in AAD using one mechanism only.
        /// </summary>
        /// <param name="config">Configuration from appsettings.json</param>
        /// <returns></returns>
        private bool IsAppUsingClientSecret(AuthenticationConfig config)
        {
            string clientSecretPlaceholderValue = "[Enter here a client secret for your application]";

            if (!String.IsNullOrWhiteSpace(config.ClientSecret) && config.ClientSecret != clientSecretPlaceholderValue)
            {
                return true;
            }

            else if (config.Certificate != null)
            {
                return false;
            }

            else
                throw new Exception("You must choose between using client secret or certificate. Please update appsettings.json file.");
        }

        public async Task<Event> CreateEvent(Event @event)
        {   
            return await GetAuthenticatedGraphClient().Users["3d77407c-0a5d-4da9-997c-39127bd90f2e"].Events
                .Request()
                .AddAsync(@event);
        }

        public async void sendCandidateInvitation(Message message)
        {
            await GetAuthenticatedGraphClient().Users["3d77407c-0a5d-4da9-997c-39127bd90f2e"].SendMail(message, false).Request().PostAsync();
        }

        public async void SendEmail(Message message, bool saveToSentItems)
        {
            await GetAuthenticatedGraphClient().Users["3d77407c-0a5d-4da9-997c-39127bd90f2e"].SendMail(message, saveToSentItems).Request().PostAsync();
        }

        public async Task<ICalendarGetScheduleCollectionPage> GetAvailableTimes(List<string> schedules, DateTimeTimeZone startTime, DateTimeTimeZone endTime, int availabilityViewInterval)
        {
            return await GetAuthenticatedGraphClient().Users["3d77407c-0a5d-4da9-997c-39127bd90f2e"].Calendar
                .GetSchedule(schedules, endTime, startTime, availabilityViewInterval)
                .Request()
                .Header("Prefer", "outlook.timezone=\"Europe/Paris\"")
                .PostAsync();
        }
    }
}
