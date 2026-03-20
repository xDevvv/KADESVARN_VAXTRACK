using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.BLL;
using VaxTrack_SDG_Project.VaxTrack.DAL;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

namespace VaxTrack_SDG_Project.VaxTrack.UI
{
    public partial class PatientForm : Form
    {
        private Panel mainPanel;
        private int doctorId;
        private int selectedPatientId = -1;

        private DataGridView dgvPatients;

        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtAge;
        private TextBox txtAddress;
        private TextBox txtContactNumber;
        private TextBox txtDoctorId;


        private DateTimePicker dtpBirthday;

        private ComboBox cmbVaccineId;
        private ComboBox cmbIsVaccinated;
        private ComboBox cmbGender;

        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;

        public PatientForm(int doctorId)
        {
            this.doctorId = doctorId;

            BuildUI();
            LoadPatients();
            LoadVaccines(cmbVaccineId);

            this.Resize += (s, e) => CenterPanel();
            CenterPanel();
        }

        private void BuildUI()
        {
            mainPanel = new Panel();
            mainPanel.Size = new Size(880, 620); // size of your whole UI block
            this.Controls.Add(mainPanel);

            this.Text = "Patient Management";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label title = new Label();
            title.Text = "Patient Management";
            title.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            title.Location = new Point(20, 20);
            title.AutoSize = true;

            // FIRST NAME
            Label lblFirst = new Label();
            lblFirst.Text = "First Name";
            lblFirst.Location = new Point(20, 80);
            lblFirst.Width = 70;

            txtFirstName = new TextBox();
            txtFirstName.Location = new Point(90, 80);
            txtFirstName.Width = 200;
            txtFirstName.BorderStyle = BorderStyle.FixedSingle;

            // LAST NAME
            Label lblLast = new Label();
            lblLast.Text = "Last Name";
            lblLast.Location = new Point(20, 120);
            lblLast.Width = 70;

            txtLastName = new TextBox();
            txtLastName.Location = new Point(90, 120);
            txtLastName.Width = 200;
            txtLastName.BorderStyle = BorderStyle.FixedSingle;

            // AGE
            Label lblAge = new Label();
            lblAge.Text = "Age";
            lblAge.Location = new Point(20, 160);
            lblAge.Width = 70;

            txtAge = new TextBox();
            txtAge.Location = new Point(90, 160);
            txtAge.Width = 200;
            txtAge.BorderStyle = BorderStyle.FixedSingle;

            // GENDER
            Label lblGender = new Label();
            lblGender.Text = "Gender";
            lblGender.Location = new Point(20, 200);
            lblGender.Width = 70;

            cmbGender = new ComboBox();
            cmbGender.Location = new Point(90, 200);
            cmbGender.Width = 200;
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");
            cmbGender.SelectedIndex = 0;

            // BIRTHDAY
            Label lblBirthday = new Label();
            lblBirthday.Text = "Birthday";
            lblBirthday.Location = new Point(20, 240);
            lblBirthday.Width = 70;

            dtpBirthday = new DateTimePicker();
            dtpBirthday.Format = DateTimePickerFormat.Custom;
            dtpBirthday.CustomFormat = "M/d/yyyy";
            dtpBirthday.Location = new Point(90, 240);
            dtpBirthday.Width = 200;


            // ADDRESS 
            Label lblAddress = new Label();
            lblAddress.Text = "Address";
            lblAddress.Location = new Point(350, 80);
            lblAddress.Width = 70;

            txtAddress = new TextBox();
            txtAddress.Location = new Point(420, 80);
            txtAddress.Width = 200;
            txtAddress.BorderStyle = BorderStyle.FixedSingle;

            // IS VACCINATED
            Label lblIsVaccinated = new Label();
            lblIsVaccinated.Text = "Vaccinated";
            lblIsVaccinated.Location = new Point(350, 120);
            lblIsVaccinated.Width = 70;

            cmbIsVaccinated = new ComboBox();
            cmbIsVaccinated.Location = new Point(420, 120);
            cmbIsVaccinated.Width = 200;
            cmbIsVaccinated.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbIsVaccinated.Items.AddRange(new string[]
            {
                "Yes",
                "No"
            });

            // DOCTOR ID
            Label lblDoctor = new Label();
            lblDoctor.Text = "Doctor ID";
            lblDoctor.Location = new Point(350, 160);
            lblDoctor.Width = 70;

            txtDoctorId = new TextBox();
            txtDoctorId.Location = new Point(420, 160);
            txtDoctorId.BorderStyle = BorderStyle.FixedSingle;
            txtDoctorId.Width = 200;
            txtDoctorId.ReadOnly = true;
            txtDoctorId.Text = doctorId.ToString();

            // CONTACT NUMBER
            Label lblContact = new Label();
            lblContact.Text = "Contact Number";
            lblContact.Location = new Point(350, 200);

            txtContactNumber = new TextBox();
            txtContactNumber.Location = new Point(450, 200);
            txtContactNumber.BorderStyle = BorderStyle.FixedSingle;
            txtContactNumber.Width = 170;

                // allow only numbers
                txtContactNumber.KeyPress += TxtContactNumber_KeyPress;

            // VACCINE TYPE
            Label lblVaccine = new Label();
            lblVaccine.Text = "Vaccine Type";
            lblVaccine.Location = new Point(350, 240);
            lblVaccine.Width = 80;

            cmbVaccineId = new ComboBox();
            cmbVaccineId.Location = new Point(430, 240);
            cmbVaccineId.Width = 190;
            cmbVaccineId.DropDownStyle = ComboBoxStyle.DropDownList;

            // BUTTONS
            btnAdd = new Button();
            btnAdd.Text = "Add";
            btnAdd.Location = new Point(780, 80);
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button();
            btnUpdate.Text = "Update";
            btnUpdate.Location = new Point(780, 120);
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Location = new Point(780, 160);
            btnDelete.Click += BtnDelete_Click;

            btnClear = new Button();
            btnClear.Text = "Clear";
            btnClear.Location = new Point(780, 200);
            btnClear.Click += BtnClear_Click;

            // DATAGRIDVIEW
            dgvPatients = new DataGridView();
            dgvPatients.Location = new Point(20, 300);
            dgvPatients.Size = new Size(840, 230);
            dgvPatients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPatients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPatients.ReadOnly = true;

            mainPanel.Controls.Add(title);
            mainPanel.Controls.Add(lblFirst);
            mainPanel.Controls.Add(txtFirstName);
            mainPanel.Controls.Add(lblLast);
            mainPanel.Controls.Add(txtLastName);
            mainPanel.Controls.Add(lblAge);
            mainPanel.Controls.Add(txtAge);
            mainPanel.Controls.Add(lblGender);
            mainPanel.Controls.Add(cmbGender);
            mainPanel.Controls.Add(lblAddress);
            mainPanel.Controls.Add(txtAddress);
            mainPanel.Controls.Add(lblIsVaccinated);
            mainPanel.Controls.Add(cmbIsVaccinated);
            mainPanel.Controls.Add(lblBirthday);
            mainPanel.Controls.Add(dtpBirthday);
            mainPanel.Controls.Add(lblDoctor);
            mainPanel.Controls.Add(txtDoctorId);
            mainPanel.Controls.Add(lblContact);
            mainPanel.Controls.Add(txtContactNumber);
            mainPanel.Controls.Add(lblVaccine);
            mainPanel.Controls.Add(cmbVaccineId);

            mainPanel.Controls.Add(btnAdd);
            mainPanel.Controls.Add(btnUpdate);
            mainPanel.Controls.Add(btnDelete);
            mainPanel.Controls.Add(btnClear);

            mainPanel.Controls.Add(dgvPatients);
        }

        private void LoadPatients()
        {
            try
            {
                PatientRepository repo = new PatientRepository();
                DataTable table = repo.GetPatientsByDoctor(doctorId);

                dgvPatients.DataSource = table;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to load patients.");
            }

        }

        public static void LoadVaccines(ComboBox comboBox)
        {
            VaccinationRepository repo = new VaccinationRepository();
            DataTable vaccines = repo.GetVaccines();

            DataRow placeholder = vaccines.NewRow();
            placeholder["vaccine_id"] = DBNull.Value;
            placeholder["vaccine_name"] = "-- Select Vaccine --";
            vaccines.Rows.InsertAt(placeholder, 0); 

            comboBox.DataSource = vaccines;
            comboBox.DisplayMember = "vaccine_name";
            comboBox.ValueMember = "vaccine_id";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtAge.Text.Trim(), out int age))
                    throw new Exception("Invalid age input.");

                var dto = new PatientCreateDto
                {
                    firstName = txtFirstName.Text.Trim(),
                    lastName = txtLastName.Text.Trim(),
                    age = age,
                    birthDate = dtpBirthday.Value.ToString("yyyy-MM-dd"),
                    doctorId = this.doctorId,
                    gender = cmbGender.SelectedItem?.ToString() ?? "",
                    address = txtAddress.Text.Trim(),
                    contactNumber = txtContactNumber.Text.Trim(),
                    vaccineId = Convert.ToInt32(cmbVaccineId.SelectedValue),
                    isVaccinated = cmbIsVaccinated.SelectedItem?.ToString() == "Yes"
                };

                PatientService service = new PatientService();
                service.AddPatient(dto);

                LoadPatients();
                ClearFields();

                MessageBox.Show("Patient successfully added!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                UpdatePatientForm form = new UpdatePatientForm(doctorId);
                form.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to update");
            }

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeletePatientForm form = new DeletePatientForm(doctorId);
                form.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete");
            }

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtAge.Clear();
            txtAddress.Clear();
            txtAddress.Clear();
            txtContactNumber.Clear();
            cmbVaccineId.SelectedIndex = -1;
            cmbIsVaccinated.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            selectedPatientId = -1;
        }


        private void TxtContactNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void CenterPanel()
        {
            if (mainPanel != null)
            {
                mainPanel.Left = (this.ClientSize.Width - mainPanel.Width) / 2;
                mainPanel.Top = (this.ClientSize.Height - mainPanel.Height) / 2;
            }
        }
    }
}