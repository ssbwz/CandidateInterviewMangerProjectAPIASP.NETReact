using LogicLayer.Models;

namespace LogicLayer.Repositories
{
    public interface IUserRepository
    {
        public List<User> FindAllCandidatesByRole();
        public User GetSpecificRecruiter(int recruiter);
        public User GetSpecificCandidate(int candidate);
        public User GetSpecificRecruiterByEmail(string email);
        public User GetSpecificRecruiterByCredentials(string email, string password);
        public Role GetUserRole(int roleNumber);
        User GetCandidateUserByApplicationIdFromAppointment(int id);
        User GetRecruiterUserByApplicationIdFromAppointment(int id);
        void InsertUser(User user);
        User GetUserByEmail(string email);
        User GetUserById(int id);
        List<User> FindAllRecruitersByRole();
    }
}
