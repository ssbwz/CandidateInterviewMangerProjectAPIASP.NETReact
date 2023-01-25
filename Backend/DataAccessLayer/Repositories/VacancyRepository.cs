using LogicLayer.Models;
using LogicLayer.Repositories;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        public VacancyRepository()
        {
            this.dbMethods = new DbMethods();
        }

        public DbMethods dbMethods { get; }

        public List<Vacancy> GetAllVacancies()
        {
            try
            {
                Vacancy desiredvacancy;
                List<Vacancy> vacancies = new List<Vacancy>();

                MySqlCommand cmd = new MySqlCommand("SELECT `id`, `title`, `location`, `meeting_location` " +
                    "FROM `vacancy`", dbMethods.conn);

                dbMethods.OpenConnection();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        desiredvacancy = new Vacancy();
                        desiredvacancy.Id = reader.GetInt32("id");
                        desiredvacancy.Title = reader.GetString("title");
                        desiredvacancy.Location = reader.GetString("location");
                        desiredvacancy.MeetingLocation = reader.GetString("meeting_location");

                        vacancies.Add(desiredvacancy);
                    }
                    return vacancies;
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't get list of all vacancies", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public Vacancy GetSpecificVacancy (int vacancy)
        {
            try
            {
                Vacancy desiredvacancy = new Vacancy();
              
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM vacancy WHERE id = @VACANCY", dbMethods.conn);
                cmd.Parameters.AddWithValue("@VACANCY", vacancy);

                dbMethods.OpenConnection();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        desiredvacancy.Id = reader.GetInt32("id");
                        desiredvacancy.Title = reader.GetString("title");
                        desiredvacancy.Location = reader.GetString("location");
                        desiredvacancy.MeetingLocation = reader.GetString("meeting_location");
                    }
                } 
                return desiredvacancy;
            }
            catch(MySqlException ex)
            {
                return null;
            }
            finally
            {
                dbMethods.CloseConnection();
            }

        }

        public void InsertVacancy(Vacancy vacancy)
        {
            try
            {
                string sql = "INSERT INTO vacancy(id,title, location, meeting_location) VALUES(@id,@title, @location, @meeting_location);";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("id", vacancy.Id);
                cmd.Parameters.AddWithValue("title", vacancy.Title);
                cmd.Parameters.AddWithValue("location", vacancy.Location);
                cmd.Parameters.AddWithValue("meeting_location", vacancy.MeetingLocation);

                dbMethods.OpenConnection();
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't insert vacancy", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public bool ExistsById(int id)
        {
            try
            {
                string sql = "SELECT COUNT(`id`) FROM `vacancy` WHERE id = @ID;";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("ID", id);

                dbMethods.OpenConnection();
         
                Int64 rowsAffected = (Int64)cmd.ExecuteScalar();
                return rowsAffected > 0;

            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't insert vacancy", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

    }
}
