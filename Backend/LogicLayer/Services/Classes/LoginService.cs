using LogicLayer.Models;
using LogicLayer.Repositories;
using LogicLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Classes
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository userRepository;


        public LoginService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User LogInAttempt(LoginRequest credentials)
        {
            try
            {
                User recruiter = userRepository.GetSpecificRecruiterByCredentials(credentials.Email, credentials.Password);
                return recruiter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
