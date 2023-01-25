using LogicLayer.Repositories;
using LogicLayer.Services.Interfaces;

namespace LogicLayer.Services.Classes
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository { get; }
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void InsertUser(Models.User newUser)
        {
            if (GetUserByEmail(newUser.Email) != null)
            {
                return;
            }            
            userRepository.InsertUser(newUser);
        }

        public Models.User GetUserByEmail(string email)
        {
            return userRepository.GetUserByEmail(email);
        }

        public Models.User GetUserById(int id)
        {
            
            return userRepository.GetUserById(id);
            
        }
        public Models.User GetCandidateUserByApplicationIdFromAppointment(int id)
        {
            return this.userRepository.GetCandidateUserByApplicationIdFromAppointment(id);
        }

        public Models.User GetRecruiterUserByApplicationIdFromAppointment(int id)
        {
            return this.userRepository.GetRecruiterUserByApplicationIdFromAppointment(id);
        }

        public List<Models.User> can() => userRepository.FindAllCandidatesByRole();

        public List<Models.User> rec() => userRepository.FindAllRecruitersByRole();
    }
}
