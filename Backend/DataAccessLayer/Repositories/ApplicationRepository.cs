using LogicLayer.Models;
using LogicLayer.Models.Enums;
using LogicLayer.Repositories;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace DataAccessLayer.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private MySqlDataReader reader;
        public DbMethods dbMethods { get; }
        private IUserRepository userRepository { get; }
        private IVacancyRepository vacancyRepository { get; }
        public ApplicationRepository()
        {
            this.dbMethods = new DbMethods();
            userRepository = new UserRepository();
            vacancyRepository = new VacancyRepository();
        }


        public Application GetSpecificApplication(int application)
        {
            Application desiredApplication;
            MySqlCommand cmd = new MySqlCommand("SELECT a.* FROM `applications` AS a WHERE a.id = @APPLICATION", dbMethods.conn);
            try
            {
                desiredApplication = new Application();
                cmd.Parameters.AddWithValue("@APPLICATION", application);
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    User candidate = userRepository.GetSpecificCandidate(Convert.ToInt32(reader["candidate_id"]));
                    Vacancy vacancy = vacancyRepository.GetSpecificVacancy(Convert.ToInt32(reader["vacancy_id"]));
                    desiredApplication.Id = Convert.ToInt32(reader["id"]);
                    desiredApplication.Candidate = candidate;
                    desiredApplication.JobVacancy = vacancy;
                    desiredApplication.VacancyId = vacancy.Id;
                    desiredApplication.CandidateId = candidate.Id;
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
                return desiredApplication;
            }
            catch (NullReferenceException)
            {
                throw new Exception("Error: Retrieval of the application's information failed!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public List<Application> GetApplicationssByVacancy(int vacancy)
        {
            List<Application> desiredApplications = new List<Application>();
            try
            {
                Application desiredApplication;
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM applications WHERE vacancy_id = @VACANCY", dbMethods.conn);
                cmd.Parameters.AddWithValue("@VACANCY", vacancy);
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    desiredApplication = new Application();
                    User candidate = userRepository.GetSpecificCandidate(Convert.ToInt32(reader["candidate_id"]));
                    Vacancy desiredvacancy = vacancyRepository.GetSpecificVacancy(vacancy);
                    desiredApplication.Id = Convert.ToInt32(reader["id"]);
                    desiredApplication.Candidate = candidate;
                    desiredApplication.JobVacancy = desiredvacancy;
                    desiredApplications.Add(desiredApplication);
                }
                return desiredApplications;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return desiredApplications;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public void InsertApplication(Application application)
        {
            try
            {
                string sql = "INSERT INTO applications(vacancy_id, candidate_id,recruiter_id) VALUES(@vacancy_id, @candidate_id,@recruiter_id);";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("vacancy_id", application.VacancyId);
                cmd.Parameters.AddWithValue("candidate_id", application.CandidateId);
                cmd.Parameters.AddWithValue("recruiter_id", application.RecruiterId);

                dbMethods.OpenConnection();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't insert application", ex);              
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public Application GetApplicationByCandidateIdAndVacancyId(int candidateId, int vacancyId)
        {
            try
            {
                string sql = "SELECT * FROM applications WHERE vacancy_id = @vacancy_id AND candidate_id = @candidate_id";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("vacancy_id", vacancyId);
                cmd.Parameters.AddWithValue("candidate_id", candidateId);

                dbMethods.OpenConnection();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Application application = new Application()
                        {
                            VacancyId = reader.GetInt32("vacancy_id"),
                            CandidateId = reader.GetInt32("candidate_id"),
                            Id = reader.GetInt32("id"),
                            RecruiterId = reader.GetInt32("recruiter_id")
                        };
                        return application;
                    }
                }
                return null;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't get application", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

    }
}

