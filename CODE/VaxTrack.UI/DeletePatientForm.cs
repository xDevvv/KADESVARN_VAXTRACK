using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.DAL;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.UI
{
    public partial class DeletePatientForm : Form
    {
        private int doctorId;

        private DateTimePicker dtpBirthdate;
        private Label lblTitle;
        private TextBox txtSearch;
        private DataGridView dgvPatients;
        private Label lblHint;


        private int selectedPatientId = -1;
        public DeletePatientForm(int doctorId)
        {
            this.doctorId = doctorId;
            InitializeUI();
            LoadPatients();
        }

        private void InitializeUI()
        {
            this.Text = "Deleting Patients";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // TITLE
            lblTitle = new Label
            {
                Text = "Deleting Patients",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // SEARCH
            Label lblSearch = new Label
            {
                Text = "Search Name:",
                Location = new Point(20, 80),
                Width = 80
            };
            txtSearch = new TextBox
            {
                Location = new Point(110, 75),
                Width = 290
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // DATA GRID
            dgvPatients = new DataGridView
            {
                Location = new Point(20, 130),
                Size = new Size(1040, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };
            dgvPatients.CellDoubleClick += DgvPatients_CellDoubleClick;

            // HINT
            lblHint = new Label
            {
                Text = "Tip: Double click a patient row to delete.",
                ForeColor = Color.Gray,
                Location = new Point(20, 440),
                AutoSize = true
            };

            this.Controls.AddRange(new Control[]
            {
                lblTitle, lblSearch, txtSearch, dgvPatients, lblHint,  dtpBirthdate
            });

        }
        private void LoadPatients()
        {
            PatientRepository repo = new PatientRepository();
            dgvPatients.DataSource = repo.GetPatientsToUpdate(doctorId);
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchPatients();
        }

        private void SearchPatients()
        {
            try
            {
                PatientRepository repo = new PatientRepository();
                DataTable table = repo.SearchPatients(doctorId, txtSearch.Text.Trim());
                dgvPatients.DataSource = table;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }

        }

        private void DgvPatients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                if (e.RowIndex < 0) return; // Ignore header row

                DataGridViewRow row = dgvPatients.Rows[e.RowIndex];
                int patientIdToDelete = Convert.ToInt32(row.Cells["PatientID"].Value);

                // Confirm deletion
                var result = MessageBox.Show(
                    "Are you sure you want to delete this patient?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result != DialogResult.Yes) return;

                DeletePatient(patientIdToDelete);
                dgvPatients.Rows.RemoveAt(e.RowIndex);

                MessageBox.Show("Patient deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting patient: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeletePatient(int patientId)
        {
            try
            {
                using var conn = Database.GetConnection();
                conn.Open();

                string query = "DELETE FROM patients WHERE patient_id = @id;";

                using var cmd = new SQLiteCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", patientId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
            
        }
    }
}
