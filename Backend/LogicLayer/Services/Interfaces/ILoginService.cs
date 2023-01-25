using LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Interfaces
{
    public interface ILoginService
    {
        User LogInAttempt(LoginRequest credentials);
    }
}
