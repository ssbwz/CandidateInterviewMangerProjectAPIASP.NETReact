using LogicLayer.Models;

namespace AppointmentAPI.DecoderService
{
    public interface IDecoderService
    {
        public DecryptedUser GetCredentials();
        public void Authorize();
        public void Authorize(string role);
        public void Authorize(List<string> roles);
    }
}
