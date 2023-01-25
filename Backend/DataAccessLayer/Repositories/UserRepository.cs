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
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {
            this.dbMethods = new DbMethods();
        }
        MySqlDataReader reader;
        public DbMethods dbMethods { get; }

        public List<User> FindAllCandidatesByRole()
        {
            User requestedCandidate;
            List<User> can = new List<User>();
            try
            {
                string sql = "SELECT * FROM user  WHERE role = 3";
                dbMethods.OpenConnection();
                MySqlCommand command = new MySqlCommand(sql, dbMethods.conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    requestedCandidate = new User();
                    requestedCandidate.Id = Convert.ToInt32(reader["id"]);
                    requestedCandidate.FirstName = Convert.ToString(reader["first_name"]);
                    requestedCandidate.LastName = Convert.ToString(reader["last_name"]);
                    requestedCandidate.Email = Convert.ToString(reader["email"]);
                    requestedCandidate.Password = Convert.ToString(reader["password"]);
                    requestedCandidate.Role = Role.Candidate;


                    can.Add(requestedCandidate);
                }
                return can;
            }
            catch(Exception ex)
            {
                return can;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public User GetSpecificRecruiter(int recruiter)
        {
            User requestedRecruiter;
            MySqlCommand cmd = new MySqlCommand("SELECT u.* FROM `user` AS u WHERE u.id = @ID", dbMethods.conn);
            try
            {
                requestedRecruiter = new User();
                cmd.Parameters.AddWithValue("@ID", recruiter);
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    requestedRecruiter.Id = Convert.ToInt32(reader["id"]);
                    requestedRecruiter.FirstName = Convert.ToString(reader["first_name"]);
                    requestedRecruiter.LastName = Convert.ToString(reader["last_name"]);
                    requestedRecruiter.Email = Convert.ToString(reader["email"]);
                    requestedRecruiter.Password = Convert.ToString(reader["password"]);
                    requestedRecruiter.Role = Role.Recruiter;
                }
            }
            catch (InvalidDataException de)
            {
                throw new InvalidDataException($"Error: Invalid data type has been passed: {de.Message} {de.StackTrace}");
            }
            catch (DALException)
            {
                throw new DALException("Error: Connecting to the data source failed, during the accessing of the specific recruiter!");
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
                return requestedRecruiter;
            }
            catch (NullReferenceException)
            {
                throw new Exception("Error: Retrieval of the specific recruiter information failed!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public User GetSpecificCandidate(int candidate)
        {
            try
            {
                User requestedCandidate = new User();
                MySqlCommand cmd = new MySqlCommand("SELECT u.* FROM `user` AS u WHERE u.id = @ID", dbMethods.conn);
                cmd.Parameters.AddWithValue("@ID", candidate);
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    requestedCandidate.Id = Convert.ToInt32(reader["id"]);
                    requestedCandidate.FirstName = Convert.ToString(reader["first_name"]);
                    requestedCandidate.LastName = Convert.ToString(reader["last_name"]);
                    requestedCandidate.Email = Convert.ToString(reader["email"]);
                    requestedCandidate.Password = Convert.ToString(reader["password"]);
                    requestedCandidate.Role = Role.Candidate;
                }
                return requestedCandidate;
            }
            catch (Exception ex)
            {
                throw new DALException("Error: Connecting to the data source failed, during the accessing of the candidate by id!");
            }
            finally 
            {
                dbMethods.CloseConnection();
            }
        }

        public User GetSpecificRecruiterByEmail(string email)
        {
            User substitudeRecruiter;
            MySqlCommand cmd = new MySqlCommand("SELECT u.* FROM `user` as u WHERE u.email = @EMAIL", dbMethods.conn);
            try
            {
                substitudeRecruiter = new User();
                cmd.Parameters.AddWithValue("@EMAIL", email);
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    substitudeRecruiter.Id = Convert.ToInt32(reader["id"]);
                    substitudeRecruiter.FirstName = Convert.ToString(reader["first_name"]);
                    substitudeRecruiter.LastName = Convert.ToString(reader["last_name"]);
                    substitudeRecruiter.Email = Convert.ToString(reader["email"]);
                    substitudeRecruiter.Password = Convert.ToString(reader["password"]);
                    substitudeRecruiter.Role = Role.Recruiter;
                }
            }
            catch (InvalidDataException de)
            {
                throw new InvalidDataException($"Error: Invalid data type has been passed: {de.Message} {de.StackTrace}");
            }
            catch (DALException)
            {
                throw new DALException("Error: Connecting to the data source failed, during the accessing of the recruiter by email!");
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
                return substitudeRecruiter;
            }
            catch (NullReferenceException)
            {
                throw new Exception("Error: Retrieval of the recruiter's information by email failed!");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public User GetCandidateUserByApplicationIdFromAppointment(int id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM appointment JOIN applications on appointment.application_id = applications.id JOIN user on applications.candidate_id = user.id where applications.id = @id", dbMethods.conn);
                cmd.Parameters.AddWithValue("@id", id);
                dbMethods.OpenConnection();
                User user = new User();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user.FirstName = reader.GetString("first_name");
                        user.LastName = reader.GetString("last_name");
                        user.Email = reader.GetString("email");
                    }
                }
                return user;
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

        public User GetRecruiterUserByApplicationIdFromAppointment(int id)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM appointment JOIN applications on appointment.application_id = applications.id JOIN user on applications.recruiter_id = user.id where applications.id = @id", dbMethods.conn);
                cmd.Parameters.AddWithValue("@id", id);
                dbMethods.OpenConnection();
                User user = new User();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user.FirstName = reader.GetString("first_name");
                        user.LastName = reader.GetString("last_name");
                        user.Email = reader.GetString("email");
                    }
                }
                return user;
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

        public User GetUserByEmail(string email)
        {
            try
            {
                string sql = "SELECT * FROM user WHERE email = @email;";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("email", email);
                dbMethods.OpenConnection();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Enum.TryParse(reader.GetString("role"), out Role role);
                        if (role == Role.Recruiter)
                        {
                            return new User
                            {

                                Id = reader.GetInt32("id"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Email = reader.GetString("email"),
                                Password = reader.GetString("password"),
                                Role = role
                            };
                        }
                        else if (role == Role.Candidate) {
                            return new User
                            {

                                Id = reader.GetInt32("id"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Email = reader.GetString("email"),
                                Role = role
                            };
                        }
                       
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't get user by email", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public void InsertUser(User user)
        {
            try
            {
                string sql = "INSERT INTO user(first_name, last_name, email,password, role) VALUES(@first_name, @last_name, @email,@password, @role);";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("first_name", user.FirstName);
                cmd.Parameters.AddWithValue("last_name", user.LastName);
                cmd.Parameters.AddWithValue("email", user.Email);
                cmd.Parameters.AddWithValue("password", user.Password);
                cmd.Parameters.AddWithValue("role", user.Role);

                dbMethods.OpenConnection();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't insert user.", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                string sql = "SELECT * FROM user WHERE id = @id;";
                MySqlCommand cmd = new MySqlCommand(sql, dbMethods.conn);
                cmd.Parameters.AddWithValue("id", id);
                dbMethods.OpenConnection();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Enum.TryParse(reader.GetString("role"), out Role role);
                        if (role == Role.Recruiter)
                        {
                            return new User
                            {

                                Id = reader.GetInt32("id"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Email = reader.GetString("email"),
                                Password = reader.GetString("password"),
                                Role = role
                            };
                        }
                        else if (role == Role.Candidate)
                        {
                            return new User
                            {

                                Id = reader.GetInt32("id"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Email = reader.GetString("email"),
                                Role = role
                            };
                        }

                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw new DALException("Couldn't get user by email", ex);
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public List<User> FindAllRecruitersByRole()
        {
            User requestedRecruiter;
            List<User> can = new List<User>();
            try
            {
                string sql = "SELECT * FROM user  WHERE role = 2";
                dbMethods.OpenConnection();
                MySqlCommand command = new MySqlCommand(sql, dbMethods.conn);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    requestedRecruiter = new User();
                    requestedRecruiter.Id = Convert.ToInt32(reader["id"]);
                    requestedRecruiter.FirstName = Convert.ToString(reader["first_name"]);
                    requestedRecruiter.LastName = Convert.ToString(reader["last_name"]);
                    requestedRecruiter.Email = Convert.ToString(reader["email"]);
                    requestedRecruiter.Password = Convert.ToString(reader["password"]);
                    requestedRecruiter.Role = Role.Recruiter;


                    can.Add(requestedRecruiter);
                }
                return can;
            }
            catch (Exception ex)
            {
                return can;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }

        public User GetSpecificRecruiterByCredentials(string email, string password)
        {
            User user = new User();
            int roleNumber = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT u.* FROM `user` as u WHERE u.email = @EMAIL and u.password = @PASSWORD", dbMethods.conn);
                cmd.Parameters.AddWithValue("@EMAIL", email);
                cmd.Parameters.AddWithValue("@PASSWORD", password);
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user.Id = Convert.ToInt32(reader["id"]);
                    user.FirstName = Convert.ToString(reader["first_name"]);
                    user.LastName = Convert.ToString(reader["last_name"]);
                    user.Email = Convert.ToString(reader["email"]);
                    user.Password = Convert.ToString(reader["password"]);
                    roleNumber = Convert.ToInt32(reader["role"]);
                }
                if (user.Id == 0 || user.Id == null)
                {
                    throw new Exception("Wrong credentials!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Wrong credentials!");
            }
            finally
            {
                dbMethods.CloseConnection();
            }

            user.Role = GetUserRole(roleNumber);
            return user;
        }

        public Role GetUserRole(int roleNumber)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT r.* FROM `role` as r WHERE r.id = @ROLE", dbMethods.conn);
            cmd.Parameters.AddWithValue("@ROLE", roleNumber);
            string roleName = "";
            try
            {
                dbMethods.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roleName = Convert.ToString(reader["title"]);
                }
                dbMethods.CloseConnection();

                if (string.IsNullOrEmpty(roleName))
                {
                    throw new Exception("Ivalid role");
                }
                else
                {
                    switch (roleName)
                    {
                        case "admin": return Role.Admin;
                        case "recruiter": return Role.Recruiter;
                        case "candidate": return Role.Candidate;
                        default: throw new Exception("Ivalid role");
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            finally
            {
                dbMethods.CloseConnection();
            }
        }
    }
}
