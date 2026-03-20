using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.DAL
{
    internal class PatientRepository
    {

        public DataTable GetPatientsByDoctor(int doctorId)
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = Database.GetConnection())
                {

                    conn.Open();

                    string query = @"
                        SELECT first_name || ' ' || last_name AS Patient, 
                        gender AS Gender, contact_number AS Contact
                        FROM patients WHERE doctor_id = @doctorId;";


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
        public int GetTotalPatients(int doctorId)
        {
            try{
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT COUNT(DISTINCT patient_id)
                         FROM patients
                         WHERE doctor_id = @doctorId";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorId", doctorId);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return 0;
            }
            
        }

        public int GetTotalVacinnatedPatient(int doctorId)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT COUNT(*)
                         FROM patients
                         WHERE doctor_id = @doctorId
                         AND is_vaccinated = 1";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorId", doctorId);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return 0;
            }
            
        }

        public int GetTodaysSchedule(int doctorId) 
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT COUNT(*)
                         FROM vaccination_schedule
                         WHERE doctor_id = @doctorId
                         AND DATE(schedule_date) = DATE('now')";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@doctorId", doctorId);
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return 0;
            }
            
        }

        public DataTable GetPatientsToUpdate(int doctorId)
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            p.patient_id AS PatientID,
                            p.first_name AS Firstname,
                            p.last_name AS Lastname,
                            p.age AS Age,
                            p.birth_date AS Birthdate,
                            p.gender AS Gender,
                            p.address AS Address,
                            v.vaccine_name AS VaccineName,
                            CASE 
                                WHEN p.is_vaccinated = 1 THEN 'Yes'
                                ELSE 'No'
                            END AS Vaccinated,
                            p.contact_number AS ContactNumber
                        FROM patients p
                        LEFT JOIN vaccines v ON p.vaccine_id = v.vaccine_id
                        WHERE p.doctor_id = @doctorId
                        ";

                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@doctorId", doctorId);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                    adapter.Fill(table);

                    return table;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return table;
            }
            
            
        }

        public void AddPatient(PatientCreateDto dto)
        {

            string query = @"
            INSERT INTO patients
                    (first_name ,last_name, age, birth_date, gender, address, doctor_id, contact_number,vaccine_id, is_vaccinated)
            VALUES  (@firstName,@lastName,@age,@birthDate,@gender, @address, @doctorId ,@contactNumber,@vaccineId,@isVaccinated)
        ";

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@firstName", dto.firstName);
                    cmd.Parameters.AddWithValue("@lastName", dto.lastName);
                    cmd.Parameters.AddWithValue("@age", dto.age);
                    cmd.Parameters.AddWithValue("@birthDate", dto.birthDate);
                    cmd.Parameters.AddWithValue("@gender", dto.gender);
                    cmd.Parameters.AddWithValue("@address", dto.address);
                    cmd.Parameters.AddWithValue("@doctorId", dto.doctorId);
                    cmd.Parameters.AddWithValue("@contactNumber", dto.contactNumber);
                    cmd.Parameters.AddWithValue("@vaccineId", dto.vaccineId);
                    cmd.Parameters.AddWithValue("@isVaccinated", dto.isVaccinated);

                    cmd.ExecuteNonQuery();
                }
            }
            
           
        }

        public void UpdatePatient(PatientUpdateDto dto)
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                // Get vaccine ID if needed
                int vaccineId = dto.vaccineId;


                string query = @"
                UPDATE patients
                SET first_name = @firstName,
                    last_name = @lastName,
                    age = @age,
                    gender = @gender,
                    birth_date = @birthDate,
                    address = @address,
                    vaccine_id = @vaccineId,
                    is_vaccinated = @isVaccinated,
                    contact_number = @contactNumber
                WHERE patient_id = @id
            ";

                using var cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", dto.patientId);
                cmd.Parameters.AddWithValue("@firstName", dto.firstName);
                cmd.Parameters.AddWithValue("@lastName", dto.lastName);
                cmd.Parameters.AddWithValue("@age", dto.age);
                cmd.Parameters.AddWithValue("@birthDate", dto.birthDate);
                cmd.Parameters.AddWithValue("@gender", dto.gender ?? "");
                cmd.Parameters.AddWithValue("@address", dto.address ?? "");
                cmd.Parameters.AddWithValue("@vaccineId", vaccineId);
                cmd.Parameters.AddWithValue("@isVaccinated", dto.isVaccinated ? 1 : 0);
                cmd.Parameters.AddWithValue("@contactNumber", dto.contactNumber);


                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
            
        }



        public DataTable SearchPatients(int doctorId, string search)
        {
            DataTable table = new DataTable();
            try
            {
                
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = @"
                    SELECT 
                        p.patient_id,
                        p.first_name,
                        p.last_name,
                        p.age,
                        v.vaccine_name,
                        p.is_vaccinated,
                        p.contact_number
                    FROM patients p
                    LEFT JOIN vaccines v ON p.vaccine_id = v.vaccine_id
                    WHERE p.doctor_id = @doctorId
                    AND (
                        p.first_name LIKE @search OR
                        p.last_name LIKE @search OR
                        p.contact_number LIKE @search
                    )
                    ";

                    SQLiteCommand cmd = new SQLiteCommand(query, conn);

                    cmd.Parameters.AddWithValue("@doctorId", doctorId);
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);

                    adapter.Fill(table);

                    return table;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return table;
            }
           
        }

        public DataRow GetPatientById(int patientId)
        {
            DataTable table = new DataTable();
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM patients WHERE patient_id = @id";

                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", patientId);

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
         
                    adapter.Fill(table);

                    return table.Rows[0];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return table.Rows[0];
            }
           
        }

        public bool IsPatientVaccinated(int patientId)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    // We check if there is at least one row in the Vaccinations table for this ID
                    string query = "SELECT COUNT(1) FROM Vaccinations WHERE patient_id = @id";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", patientId);

                        // ExecuteScalar returns the first column of the first row (the count)
                        long count = (long)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                return false;
            }
           
        }
    }
}
