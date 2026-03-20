using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.DAL;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.UI
{
    public partial class UpdatePatientForm : Form
    {
        private int doctorId;

        private DateTimePicker dtpBirthdate;
        private Label lblTitle;
        private TextBox txtSearch;
        private DataGridView dgvPatients;
        private Label lblHint;

        // Patient detail controls
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtAge;
        private TextBox txtAddress;
        private TextBox txtContactNumber;
        private ComboBox cmbVaccinated;
        private ComboBox cmbVaccine;
        private ComboBox cmbGender;
        private Button btnUpdate;

        private int selectedPatientId = -1;

        public UpdatePatientForm(int doctorId)
        {


            this.doctorId = doctorId;
            InitializeUI();
            LoadPatients();
        }

        private void InitializeUI()
        {
            this.Text = "Updating Patients";
            this.Size = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            // TITLE
            lblTitle = new Label
            {
                Text = "Updating Patients",
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
                Text = "Tip: Double click a patient row to edit.",
                ForeColor = Color.Gray,
                Location = new Point(20, 440),
                AutoSize = true
            };

            // PATIENT DETAIL CONTROLS
            int firstLayerBaseY = 480;
            int secondLayerBaseY = 520; 
            txtFirstName = new TextBox { Location = new Point(20, firstLayerBaseY), Width = 150, PlaceholderText = "First Name" };
            txtLastName = new TextBox { Location = new Point(180, firstLayerBaseY), Width = 150, PlaceholderText = "Last Name" };
            txtAge = new TextBox { Location = new Point(340, firstLayerBaseY), Width = 50, PlaceholderText = "Age" };
            dtpBirthdate = new DateTimePicker
            {
                Location = new Point(400, firstLayerBaseY),
                Width = 120,
                Format = DateTimePickerFormat.Short
            };
            cmbGender = new ComboBox { Location = new Point(530, firstLayerBaseY), Width = 110 }; 
            cmbGender.Items.AddRange(new string[] { "Select Gender", "Male", "Female" });
            cmbGender.SelectedIndex = 0;
            txtAddress = new TextBox { Location = new Point(650, firstLayerBaseY), Width = 200, PlaceholderText = "Address" };
            cmbVaccine = new ComboBox
            {
                Location = new Point(20, secondLayerBaseY),
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            PatientForm.LoadVaccines(cmbVaccine);
            txtContactNumber = new TextBox { Location = new Point(180, secondLayerBaseY), Width = 150, PlaceholderText = "Contact No." };
            cmbVaccinated = new ComboBox
            {
                Location = new Point(340, secondLayerBaseY),
                Width = 100,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbVaccinated.Items.Add("Vaccinated?");
            cmbVaccinated.Items.AddRange(new string[] { "Yes", "No" });
            cmbVaccinated.SelectedIndex = 0;
            

            btnUpdate = new Button { Text = "Update", Location = new Point(880, firstLayerBaseY), Width = 80 };
            btnUpdate.Click += BtnUpdate_Click;

            // ADD CONTROLS
            this.Controls.AddRange(new Control[]
            {
                lblTitle, lblSearch, txtSearch, dgvPatients, lblHint,
                txtFirstName, txtLastName, txtAge, dtpBirthdate, cmbGender, txtAddress, cmbVaccine, cmbVaccinated, txtContactNumber, btnUpdate
            });
        }

        // ===============================
        // Load Patients
        // ===============================
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
            PatientRepository repo = new PatientRepository();
            DataTable table = repo.SearchPatients(doctorId, txtSearch.Text.Trim());
            dgvPatients.DataSource = table;
        }

        private void DgvPatients_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // header
            DataGridViewRow row = dgvPatients.Rows[e.RowIndex];
            selectedPatientId = Convert.ToInt32(row.Cells["PatientID"].Value);

            // Load patient data into detail fields
            LoadPatientData();
        }



        private void LoadPatientData()
        {
            try
            {
                DateTime dt;
                if (selectedPatientId == -1) return;

                PatientRepository repo = new PatientRepository();
                DataRow patient = repo.GetPatientById(selectedPatientId);


                string birthdateStr = patient["birth_date"].ToString();
                string isVaccinated = patient["is_vaccinated"].ToString();



                txtFirstName.Text = patient["first_name"].ToString();
                txtLastName.Text = patient["last_name"].ToString();
                txtAge.Text = patient["age"].ToString();
                dtpBirthdate.Value = DateTime.TryParse(birthdateStr, out dt) ? dt : DateTime.Today;
                cmbGender.Text = patient["gender"].ToString();
                txtAddress.Text = patient["address"].ToString();
                cmbVaccine.SelectedValue = patient["vaccine_id"] != DBNull.Value ? Convert.ToInt32(patient["Vaccine_id"]) : DBNull.Value;
                cmbVaccinated.SelectedIndex = isVaccinated == "1" ? 1 : isVaccinated == "0" ? 2 : 0;

                txtContactNumber.Text = patient["contact_number"].ToString();




                // Safe parsing for age
                int age = 0;
                int.TryParse(patient["Age"].ToString(), out age);
                txtAge.Text = age.ToString();
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
           
        }

        // ===============================
        // Update Patient
        // ===============================
        private void BtnUpdate_Click(object sender, EventArgs e)
        {

            if (selectedPatientId == -1)
            {
                MessageBox.Show("Please select a patient first.");
                return;
            }

            int age = 0;
            if (!int.TryParse(txtAge.Text.Trim(), out age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            PatientRepository repo = new PatientRepository();
            PatientUpdateDto dto = new PatientUpdateDto
            {
                patientId = selectedPatientId,
                firstName = txtFirstName.Text.Trim(),
                lastName = txtLastName.Text.Trim(),

                age = int.TryParse(txtAge.Text, out int parsedAge) ? parsedAge : 0,

                birthDate = dtpBirthdate.Value.ToString("yyyy-MM-dd"),

                gender = cmbGender.SelectedItem != null
                ? cmbGender.SelectedItem.ToString()
                : "",

                address = txtAddress.Text.Trim(),

                vaccineId = (cmbVaccine.SelectedValue == null || cmbVaccine.SelectedValue == DBNull.Value)
                ? 0
                : Convert.ToInt32(cmbVaccine.SelectedValue),

                isVaccinated = cmbVaccinated.SelectedIndex == 1,

                contactNumber = txtContactNumber.Text.Trim()
            };
            repo.UpdatePatient(dto);

            MessageBox.Show("Patient updated successfully!");
            LoadPatients();
        }
    }
}