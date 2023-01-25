using LogicLayer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services.Interfaces
{
    public interface IAppointmentService
    {
        public CreateAppointmentResponse GenerateCreateAppointmentLink(CreateAppointmentLinkRequest request);
        IsAppointmentHashVaildResponse IsAppointmentHashVaild(LinkValidation linkValidation);
        CreateAppoinementResponse CreateAppointment(CreateAppointmentRequest request);
        GetAppointmentCreationInfoResponse GetAppointmentCreationInfo(string usernameHashed, string identifierHashed);
        Task<List<AvailableTime>> GetAvailableTimesAsync(string email, DateTime date);
        void SendEmail(Email email);
        void CancelAnEvent(CancelAppointmentRequest request);
        GetAppointmentsResponse GetAllAppointments();
       // GetAppointmentsResponse GetAllAppointmentsByRecruiterId(int id);
        GetAppointmentsResponse GetAllAppointmentsByDateAscending();
        GetAppointmentsResponse GetAllAppointmentsByDateDecending();
        public GetSpecificAppointmentResponse GetSpecificAppointment(int appointment);
        public GetSpecificAppointmentResponse ChangeRecruiter(string email, int appointmentID);
        public bool DeleteSpecificAppointment(int appointmentID);
        GetAppointmentsResponse GetAppointmentsByRecruiterName(string name);
        GetAppointmentsResponse GetAppointmentsByRecruiterId(int id);
        public Task<List<User>> GetAvailableRecruiters(DateTime startDate);
    }
}
