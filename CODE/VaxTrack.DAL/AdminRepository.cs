using System;
using System.Data.SQLite;

namespace VaxTrack_SDG_Project.VaxTrack.DAL
{
    public class AdminRepository
    {
        public int GetAdminId(string username, string password)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT admin_id, password 
                                 FROM Admins 
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
                                return Convert.ToInt32(reader["admin_id"]);
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

        public string GetPasswordHash(string username)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT password FROM Admins WHERE username=@username";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }
    }
}