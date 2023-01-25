

namespace LogicLayer.Services.Interfaces
{
    public interface IUserService
    {
        void InsertUser(Models.User newUser);
        Models.User GetUserByEmail(string candidateEmail);
        Models.User GetUserById(int id);
        Models.User GetCandidateUserByApplicationIdFromAppointment(int id);
        Models.User GetRecruiterUserByApplicationIdFromAppointment(int id);
        public List<Models.User> can();
        public List<Models.User> rec();
    }
}
