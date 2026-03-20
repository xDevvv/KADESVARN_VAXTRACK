using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VaxTrack_SDG_Project.VaxTrack.DAL
{
    public static class Database
    {
        private static string inputDataFolder = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, @"../../../../INPUT_DATA");

        private static string dbFile = Path.Combine(inputDataFolder, "vaxtrack.db");

        private static string connectionString = $"Data Source={dbFile};Version=3;";

        static Database()
        {

            using (var conn = GetConnection())
            {
                conn.Open();
                conn.Close();
            }
        }

        // ===== Helper Methods =====
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        // ===== Password hashing helper =====
        public static string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}