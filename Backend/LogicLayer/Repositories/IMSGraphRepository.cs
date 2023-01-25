using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace LogicLayer.Repositories
{
    public interface IMSGraphRepository
    {
        public GraphServiceClient GetAuthenticatedGraphClient();
        public Task<string> GetAccessToken();
        public Task<Event> CreateEvent(Event @event);
        public void sendCandidateInvitation(Message message);
        public void SendEmail(Message message, bool saveToSentItems);
        public Task<ICalendarGetScheduleCollectionPage> GetAvailableTimes(List<String> schedules, DateTimeTimeZone startTime, DateTimeTimeZone endTime, int availabilityViewInterval);

    }
}
