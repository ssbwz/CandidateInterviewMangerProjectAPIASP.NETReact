using LogicLayer.Models;

namespace LogicLayer.Repositories
{
    public interface IAppointmentRepository
    {
        public Appointment GetSpecificAppointment(int appointment);

        public Appointment UpdateSpecificAppointment(Appointment specificAppointment);

        public bool DeleteSpecificAppointment(Appointment specificAppointment);
        void SaveLinkHashes(AppointmentLink appointemntLink);
        AppointmentLink GetAppointmentLink(LinkValidation linkValidation);
        GetAppointmentsResponse getAppointments();
        GetAppointmentsResponse getAppointmentsFilterByDateAscending();
        GetAppointmentsResponse getAppointmentsFilterByDateDecending();
        GetAppointmentsResponse GetAppointmentsBySubjectName(string subject);
        int InsertAppointment(Appointment appointment);
        void UpdateAppointmentLink(AppointmentLink appointmentLink);
        GetAppointmentsResponse GetAppointmentsByRecruiterName(string name);        
        GetAppointmentsResponse getAppointmentsByRecruiterId(int id);
    }
}
