using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.DAL;

namespace VaxTrack_SDG_Project
{
    public class ScheduleForm : Form
    {
        private int _doctorId;
        private ComboBox cmbVaccineId;
        private DataGridView dgvPatients;
        private Button btnSchedule;
        private DateTimePicker dtpSchedule;
        private SQLiteConnection conn;
        private ComboBox lblIsVaccinated;
        private ComboBox? cmbIsVaccinated;

        public ScheduleForm(int doctorId)
        {
            this.Text = "Schedule Vaccination";
            this.Size = new Size(600, 400);

            InitializeComponents();
            InitializeDatabase();
            LoadUnscheduledPatients();
            _doctorId = doctorId;
        }

        private void InitializeComponents()
        {
            dgvPatients = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(540, 200),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            this.Controls.Add(dgvPatients);

            dtpSchedule = new DateTimePicker
            {
                Location = new Point(20, 240),
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today
            };

            this.Controls.Add(dtpSchedule);

            btnSchedule = new Button
            {
                Text = "Schedule Vaccination",
                Location = new Point(270, 240),
                Size = new Size(200, 30)
            };

            btnSchedule.Click += BtnSchedule_Click;
            this.Controls.Add(btnSchedule);

            Label lblVaccine = new Label();
            lblVaccine.Text = "Vaccine Type";
            lblVaccine.Location = new Point(20, 290);
            lblVaccine.Width = 80;

            cmbVaccineId = new ComboBox();
            cmbVaccineId.Location = new Point(20, 320);
            cmbVaccineId.Width = 190;
            cmbVaccineId.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVaccineId.Items.Insert(0, "-- Select Vaccination Status --");
            cmbVaccineId.SelectedIndex = 0;
            this.Controls.Add(cmbVaccineId);

            LoadVaccines();

            this.Controls.Add(lblVaccine);
            this.Controls.Add(cmbVaccineId);

        }

        private void InitializeDatabase()
        {
            var conn = Database.GetConnection();
            conn.Open();

            string createPatientTable = @"
            CREATE TABLE IF NOT EXISTS Patients (
                PatientID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Email TEXT,
                Vaccinated INTEGER DEFAULT 0,
                ScheduledDate TEXT
            );";

            using (SQLiteCommand cmd = new SQLiteCommand(createPatientTable, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        // Only load patients who are NOT vaccinated AND NOT scheduled
        private void LoadUnscheduledPatients()
        {
            string query = "SELECT patient_id, first_name, last_name FROM patients WHERE is_vaccinated = 0";

            var conn = Database.GetConnection();
            conn.Open();

            using (SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPatients.DataSource = dt;
            }
        }

        private void BtnSchedule_Click(object sender, EventArgs e)
        {
            if (dgvPatients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a patient first.");
                return;
            }

            int patientId = Convert.ToInt32(dgvPatients.SelectedRows[0].Cells[0].Value);

            DateTime selectedDate = dtpSchedule.Value;
            if (selectedDate < DateTime.Today)
            {
                MessageBox.Show("You cannot schedule a date in the past.");
                return;
            }

            if (cmbVaccineId.SelectedValue == null)
            {
                MessageBox.Show("Please select a vaccine.");
                return;
            }

            int vaccineId = Convert.ToInt32(cmbVaccineId.SelectedValue);

            if (vaccineId == 0)
            {
                MessageBox.Show("Please select a valid vaccine.");
                return;
            }

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string insertQuery = @"INSERT INTO vaccination_schedule
                    (patient_id, vaccine_id, doctor_id, schedule_date) 
                    VALUES (@patient_id, @vaccine_id, @doctor_id, @schedule_date)";

                        using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@patient_id", patientId);
                            cmd.Parameters.AddWithValue("@vaccine_id", vaccineId);
                            cmd.Parameters.AddWithValue("@doctor_id", _doctorId);
                            cmd.Parameters.AddWithValue("@schedule_date", selectedDate.ToString("yyyy-MM-dd"));
                            cmd.ExecuteNonQuery();
                        }


                        string updateQuery = @"UPDATE patients 
                                       SET is_vaccinated = 1 
                                       WHERE patient_id = @patient_id";

                        using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@patient_id", patientId);
                            cmd.ExecuteNonQuery();
                        }


                        transaction.Commit();

                        MessageBox.Show("Schedule created and patient updated!");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            LoadPatients();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            conn?.Close();
            base.OnFormClosing(e);
        }


        private void LoadVaccines()
        {
            using (var conn = Database.GetConnection())
            {
                string query = "SELECT vaccine_id, vaccine_name FROM vaccines";
                SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add placeholder
                DataRow row = dt.NewRow();
                row["vaccine_id"] = 0;
                row["vaccine_name"] = "-- Select Vaccine --";
                dt.Rows.InsertAt(row, 0);

                cmbVaccineId.DataSource = dt;
                cmbVaccineId.DisplayMember = "vaccine_name";
                cmbVaccineId.ValueMember = "vaccine_id";



            }
        }

        private void LoadPatients()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM patients WHERE is_vaccinated = 0";
                SQLiteDataAdapter da = new SQLiteDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPatients.DataSource = dt;
            }
        }

    }
}