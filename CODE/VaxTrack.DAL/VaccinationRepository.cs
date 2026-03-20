using System.Data;
using System.Data.SQLite;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.DAL
{
    internal class VaccinationRepository
    {
        public DataTable GetVaccineDistribution(int doctorId)
        {
            DataTable table = new DataTable();
            try
            {
                string query = @"
                SELECT v.vaccine_name, COUNT(*) AS total
                FROM patients vs
                JOIN vaccines v ON vs.vaccine_id = v.vaccine_id
                WHERE vs.doctor_id = @doctorId
                GROUP BY v.vaccine_name
                ";

                using (var conn = Database.GetConnection())
                {
                    conn.Open(); // IMPORTANT

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorId", doctorId);

                        SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);


                        adapter.Fill(table);

                        return table;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return table;
            }
            
        }

        public DataTable GetVaccines()
        {
            DataTable table = new DataTable();
            try
            {
                using (SQLiteConnection conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT vaccine_id, vaccine_name FROM vaccines";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, conn))
                    {
                        adapter.Fill(table);
                    }
                }

                return table;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return table;
            }
            
        }

        public void AddVaccine(VaccineCreateDto dto)
        {
            string query = @"
                INSERT INTO vaccines (vaccine_name)
                VALUES (@name)
            ";

            using var conn = Database.GetConnection();
            conn.Open();

            using var cmd = new SQLiteCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", dto.VaccineName);

            cmd.ExecuteNonQuery();
        }
    }
}
