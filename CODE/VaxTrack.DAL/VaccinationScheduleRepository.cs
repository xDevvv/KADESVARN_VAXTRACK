using System.Data;
using System.Data.SQLite;
using VaxTrack_SDG_Project.VaxTrack.UI;

namespace VaxTrack_SDG_Project.VaxTrack.DAL
{
    internal class VaccinationScheduleRepository
    {

        public DataTable GetVaccinationsToday(int doctorId)
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT 
                        p.patient_id AS ""Patient ID"",
                        p.first_name || ' ' || p.last_name AS ""Patient Name"",
                        v.vaccine_name AS ""Vaccine"",
                        s.dosage AS ""Dosage"",
                        s.schedule_date AS ""Schedule Date""
                    FROM vaccination_schedule s
                    JOIN patients p ON s.patient_id = p.patient_id
                    JOIN vaccines v ON s.vaccine_id = v.vaccine_id
                    WHERE s.doctor_id = @doctorId
                    AND DATE(s.schedule_date) = DATE('now')
                    ORDER BY s.schedule_date";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorId", doctorId);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                        {

                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return table;
            }
            
          
        }

        public void CreateAppointment(int patientId, DateTime appointmentDate, string vaccineType)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = @"INSERT INTO Appointments (patient_id, appointment_date, vaccine_type, status) 
                             VALUES (@patientId, @date, @vaccine, 'Pending')";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@patientId", patientId);
                        cmd.Parameters.AddWithValue("@date", appointmentDate.ToString("yyyy-MM-dd HH:mm"));
                        cmd.Parameters.AddWithValue("@vaccine", vaccineType);
                        cmd.ExecuteNonQuery();
                    }
                }

            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
           
        }
    }
}