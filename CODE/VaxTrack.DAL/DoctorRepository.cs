using System;
using VaxTrack_SDG_Project.VaxTrack.MODELS;
using System.Data.SQLite;

namespace VaxTrack_SDG_Project.VaxTrack.DAL
{
    public class DoctorRepository
    {
        public bool ValidateDoctor(string username, string hashedPassword)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"SELECT COUNT(*) 
                                 FROM Doctors 
                                 WHERE username = @username 
                                   AND password = @password";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", hashedPassword);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        public string GetPasswordHash(string username)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT password FROM Doctors WHERE username=@username";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        public int GetDoctorId(string username, string password)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT doctor_id, password 
                         FROM Doctors 
                         WHERE username = @username";

                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["password"].ToString();

                            if (PasswordHasher.VerifyPassword(password, storedHash))
                            {
                                return Convert.ToInt32(reader["doctor_id"]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
            return -1;

        }

        public void AddDoctor(Doctor dto)
        {
            string query = @"INSERT INTO doctors (username, password, full_name, email)
                         VALUES (@username, @password, @fullname, @email)";

            using var conn = Database.GetConnection();
            conn.Open();

            using var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@username", dto.Username);
            cmd.Parameters.AddWithValue("@password", dto.Password);
            cmd.Parameters.AddWithValue("@fullname", dto.FullName);
            cmd.Parameters.AddWithValue("@email", dto.Email);

            cmd.ExecuteNonQuery();
        }
    }
}