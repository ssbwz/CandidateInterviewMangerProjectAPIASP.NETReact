using LogicLayer.Models;
using LogicLayer.Models.Enums;
using LogicLayer.Repositories;
using MySql.Data.MySqlClient;

using System.Diagnostics;
using System.Net;

namespace DataAccessLayer.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private MySqlDataReader reader;
        public DbMethods dbMethods { get; }

        public AppointmentRepository()
        {
            this.dbMethods = new DbMethods();
        }



        public GetAppointmentsResponse getAppointments()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("Select * FROM appointment;", dbMethods.conn);
                dbMethods.OpenConnection();
                GetAppointmentsResponse response = new GetAppointmentsResponse();
                List<Appointment> appointments = new List<Appointment>();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointment appointment = new Appointment();
                        appointment.Id = reader.GetInt32("id");
                        appointment.ApplicationId = reader.GetInt32("application_id");
                        appointment.StartDate = reader.GetDateTime("start_date");
                        appointment.EndDate = reader.GetDateTime("end_date");
                        appointment.Location = reader.GetString("location");
                        appointment.MSGraphId = reader.GetString("MSGraph_id");
                        appointments.Add(appointment);

                    }
                }
                response.Appointments = appointments;
                return response;
            }


            catch (Exception ex)
            {
                throw;           
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public GetAppointmentsResponse getAppointmentsByRecruiterId(int id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `appointment` JOIN applications on appointment.application_id = applications.id WHERE applications.recruiter_id = @id; ", dbMethods.conn);
                cmd.Parameters.AddWithValue("@id", id);
                dbMethods.OpenConnection();
                GetAppointmentsResponse response = new GetAppointmentsResponse();
                List<Appointment> appointments = new List<Appointment>();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointment appointment = new Appointment();
                        appointment.Id = reader.GetInt32("id");
                        appointment.ApplicationId = reader.GetInt32("application_id");
                        appointment.StartDate = reader.GetDateTime("start_date");
                        appointment.EndDate = reader.GetDateTime("end_date");
                        appointment.Location = reader.GetString("location");
                        appointment.MSGraphId = reader.GetString("MSGraph_id");
                        appointments.Add(appointment);

                    }
                }
                response.Appointments = appointments;
                return response;
            }


            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public GetAppointmentsResponse getAppointmentsFilterByDateAscending()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("Select * FROM appointment ORDER BY start_date ASC;", dbMethods.conn);
                dbMethods.OpenConnection();
                GetAppointmentsResponse response = new GetAppointmentsResponse();
                List<Appointment> appointments = new List<Appointment>();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointment appointment = new Appointment();
                        appointment.Id = reader.GetInt32("id");
                        appointment.ApplicationId = reader.GetInt32("application_id");
                        appointment.StartDate = reader.GetDateTime("start_date");
                        appointment.EndDate = reader.GetDateTime("end_date");
                        appointment.Location = reader.GetString("location");
                        appointment.MSGraphId = reader.GetString("MSGraph_id");
                        appointments.Add(appointment);
                    }
                }
                response.Appointments = appointments;
                return response;
            }


            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public GetAppointmentsResponse getAppointmentsFilterByDateDecending()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("Select * FROM appointment ORDER BY start_date DESC;", dbMethods.conn);
                dbMethods.OpenConnection();
                GetAppointmentsResponse response = new GetAppointmentsResponse();
                List<Appointment> appointments = new List<Appointment>();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointment appointment = new Appointment();
                        appointment.Id = reader.GetInt32("id");
                        appointment.ApplicationId = reader.GetInt32("application_id");
                        appointment.StartDate = reader.GetDateTime("start_date");
                        appointment.EndDate = reader.GetDateTime("end_date");
                        appointment.Location = reader.GetString("location");
                        appointment.MSGraphId = reader.GetString("MSGraph_id");
                        appointments.Add(appointment);
                    }
                }
                response.Appointments = appointments;
                return response;
            }


            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public GetAppointmentsResponse GetAppointmentsByRecruiterName(string name)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM `appointment` JOIN applications ON appointment.application_id = applications.id JOIN user ON applications.recruiter_id = user.id WHERE CONCAT(first_name, ' ', last_name) LIKE '%{name}%' ", dbMethods.conn);
                //cmd.Parameters.AddWithValue(appointment.Subject, id);
                dbMethods.OpenConnection();
                GetAppointmentsResponse response = new GetAppointmentsResponse();
                List<Appointment> appointments = new List<Appointment>();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointment getappointment = new Appointment();
                        getappointment.Id = reader.GetInt32("id");
                        getappointment.ApplicationId = reader.GetInt32("application_id");
                        getappointment.StartDate = reader.GetDateTime("start_date");
                        getappointment.EndDate = reader.GetDateTime("end_date");
                        getappointment.Location = reader.GetString("location");
                        getappointment.MSGraphId = reader.GetString("MSGraph_id");
                        appointments.Add(getappointment);

                    }
                }
                response.Appointments = appointments;
                return response;
            }


            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

     


      

        public AppointmentLink GetAppointmentLink(LinkValidation linkValidation)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT `id`, `usernameHashed`, `identifierHashed`, `status` " +
                    "FROM `appointments_links`" +
                    " WHERE identifierHashed = @IDENTIFIERHASHED AND usernameHashed = @USERNAMEHASHED"
                     , dbMethods.conn);

                cmd.Parameters.AddWithValue("USERNAMEHASHED", linkValidation.UsernameHashed);
                cmd.Parameters.AddWithValue("IDENTIFIERHASHED", linkValidation.IdentifierHashed);


                dbMethods.OpenConnection();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        string usernameHashed = reader.GetString("usernameHashed");
                        string identifierHashed = reader.GetString("identifierHashed");
                        Enum.TryParse(reader.GetString("status"), out LinkStatus status);

                        return new AppointmentLink(id, usernameHashed, identifierHashed, status);
                    }
                }

                return null;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't get appointment link", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public void SaveLinkHashes(AppointmentLink appointemntLink)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO `appointments_links` " +
                    "(`usernameHashed`, `identifierHashed`,`status`) " +
                    "VALUES (@USERNAMEHASHED,@IDENTIFIERHASHED,@STATUS)"
                     , dbMethods.conn);

                cmd.Parameters.AddWithValue("USERNAMEHASHED", appointemntLink.EmailHashed);
                cmd.Parameters.AddWithValue("IDENTIFIERHASHED", appointemntLink.IdentifierHashed);
                cmd.Parameters.AddWithValue("STATUS", appointemntLink.LinkStatus.ToString());

                dbMethods.OpenConnection();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                if (ex.Message.Contains("UX_usernameHashed_identifierHashed")) {
                    throw new ArgumentException("Username and identifier should be unique");
                }
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't inseart appointment link", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public int InsertAppointment(Appointment appointment)
        {
            try
            {
                string sql = "INSERT INTO appointment(MSGraph_Id,application_id, start_date, end_date, location) VALUES(@msGraphid, @application_id, @start_date, @end_date, @location);" +
                    "SELECT LAST_INSERT_ID();";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("msGraphid", appointment.MSGraphId);
                cmd.Parameters.AddWithValue("application_id", appointment.ApplicationId);
                cmd.Parameters.AddWithValue("start_date", appointment.StartDate);
                cmd.Parameters.AddWithValue("end_date", appointment.EndDate);
                cmd.Parameters.AddWithValue("location", appointment.Location);

                dbMethods.OpenConnection();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                throw new DALException("Couldn't insert appointment", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public void UpdateAppointmentLink(AppointmentLink appointmentLink)
        {
            try
            {
                string sql = "UPDATE `appointments_links` SET `status`=@LinkStatus WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("LinkStatus", appointmentLink.LinkStatus.ToString());
                cmd.Parameters.AddWithValue("id", appointmentLink.Id);

                dbMethods.OpenConnection();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                throw new DALException("Couldn't insert appointment", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }
    
        
    
        public Appointment GetSpecificAppointment(int appointment)
        {

            Appointment desiredAppointment;
            MySqlCommand cmd = new MySqlCommand("SELECT a.* FROM `appointment` AS a WHERE a.id = @APPOINTMENT", dbMethods.conn);
            try
            {
                desiredAppointment = new Appointment();
                cmd.Parameters.AddWithValue("@APPOINTMENT", appointment);
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    desiredAppointment.Id = Convert.ToInt32(reader["id"]);
                    desiredAppointment.ApplicationId = Convert.ToInt32(reader["application_id"]);
                    desiredAppointment.StartDate = Convert.ToDateTime(reader["start_date"]);
                    desiredAppointment.EndDate = Convert.ToDateTime(reader["end_date"]);
                    desiredAppointment.Location = Convert.ToString(reader["location"]);
                    desiredAppointment.MSGraphId = reader.GetString("MSGraph_id");
                }
            }
            catch (InvalidDataException de)
            {
                throw new InvalidDataException($"Error: Invalid data type has been passed: {de.Message} {de.StackTrace}");
            }
            catch (DALException)
            {
                throw new DALException("Error: Connecting to the data source failed, during the accessing of the appointment!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
            finally
            {
                dbMethods.CloseConnection();
            }

            try
            {
                return desiredAppointment;
            }
            catch (NullReferenceException)
            {
                throw new Exception("Error: Retrieval of the appointment's information failed!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        

        public Appointment UpdateSpecificAppointment(Appointment specificAppointment)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE appointment JOIN applications on appointment.application_id = applications.id SET applications.recruiter_id = @RECRUITER, appointment.MSGraph_id = @MSGRAPH WHERE appointment.id = @APPOINTMENT", dbMethods.conn);
            dbMethods.OpenConnection ();
            try
            {
                cmd.Parameters.AddWithValue("@RECRUITER", specificAppointment.RecruiterId);
                cmd.Parameters.AddWithValue("@MSGRAPH", specificAppointment.MSGraphId);
                cmd.Parameters.AddWithValue("@APPOINTMENT", specificAppointment.Id);
            }
            catch (InvalidDataException de)
            {
                throw new InvalidDataException($"Error: Invalid data type has been passed: {de.Message} {de.StackTrace}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                dbMethods.CloseConnection();
                return GetSpecificAppointment(specificAppointment.Id);
            }
            else
            {
                dbMethods.CloseConnection();
                throw new Exception("Update of the appointment was unsuccessful!");
            }
        }

        public bool DeleteSpecificAppointment(Appointment specificAppointment)
        {
            int result = 0;
            MySqlCommand cmd = new MySqlCommand("DELETE a.* FROM `appointment` as a WHERE a.id = @APPOINTMENT", dbMethods.conn);
            try
            {
                dbMethods.OpenConnection();
                cmd.Parameters.AddWithValue("@APPOINTMENT", specificAppointment.Id);
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message} {e.StackTrace}");
            }
            finally
            {
                dbMethods.CloseConnection();
            }
            if (result > 0)
            {              
               return true;
            }
            else
            {
                return false;
            }

        }

        public GetAppointmentsResponse GetAppointmentsBySubjectName(string name)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM `appointment` JOIN applications ON appointment.application_id = applications.id JOIN user ON applications.recruiter_id = user.id WHERE CONCAT(first_name, ' ', last_name) LIKE '%{name}%' ", dbMethods.conn);
                //cmd.Parameters.AddWithValue(appointment.Subject, id);
                dbMethods.OpenConnection();
                GetAppointmentsResponse response = new GetAppointmentsResponse();
                List<Appointment> appointments = new List<Appointment>();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointment getappointment = new Appointment();
                        getappointment.Id = reader.GetInt32("id");
                        getappointment.ApplicationId = reader.GetInt32("application_id");
                        getappointment.StartDate = reader.GetDateTime("start_date");
                        getappointment.EndDate = reader.GetDateTime("end_date");
                        getappointment.Location = reader.GetString("location");
                        getappointment.MSGraphId = reader.GetString("MSGraph_id");
                        appointments.Add(getappointment);

                    }
                }
                response.Appointments = appointments;
                return response;
            }


            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                dbMethods.CloseConnection();
            }

        }
    }
}
