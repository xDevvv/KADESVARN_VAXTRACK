using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using VaxTrack_SDG_Project.VaxTrack.DAL;
using VaxTrack_SDG_Project.VaxTrack.BLL;

namespace VaxTrack_SDG_Project.VaxTrack.UI
{
    public partial class DoctorForm : Form
    {
        private Panel sidebar;
        private Panel mainPanel;
        private Label lblHeader;

        private Button btnDashboard;
        private Button btnPatients;
        private Button btnVaccines;
        private Button btnAddSchedule;
        private Button btnImportData;
        private Button btnLogout;

        private int loggedDoctorId;

        private DataGridView dgvRecent;
        private DataGridView dgvSchedule;
        private DataGridView dgvPatients;


        public DoctorForm(int doctorId)
        {
            loggedDoctorId = doctorId;
            BuildUI();
            LoadDashboard();

        }

        private void BuildUI()
        {
            this.Text = "VaxTrack Dashboard";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;


            // HEADER
            lblHeader = new Label();
            lblHeader.Text = "VaxTrack Vaccination System";
            lblHeader.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblHeader.Dock = DockStyle.Top;
            lblHeader.Height = 60;
            lblHeader.BackColor = Color.LightBlue;

            this.Controls.Add(lblHeader);

            // SIDEBAR
            sidebar = new Panel();
            sidebar.Width = 200;
            sidebar.Dock = DockStyle.Left;
            sidebar.BackColor = Color.FromArgb(40, 40, 40);

            this.Controls.Add(sidebar);

            // MAIN PANEL
            mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.BackColor = Color.WhiteSmoke;

            this.Controls.Add(mainPanel);

            // BUTTONS
            btnDashboard = CreateSidebarButton("Dashboard");
            btnDashboard.Click += BtnDashboard_Click;

            btnPatients = CreateSidebarButton("Patients");
            btnPatients.Click += BtnPatients_Click;

            btnVaccines = CreateSidebarButton("Monitoring");
            btnVaccines.Click += BtnMonitoring_Click;

            btnAddSchedule = CreateSidebarButton("Add Schedule");
            btnAddSchedule.Click += BtnSchedule_Click;

            btnImportData = CreateSidebarButton("Import Data");
            btnImportData.Click += BtnUpImportData_click;

            btnLogout = CreateSidebarButton("Logout");
            btnLogout.Click += BtnLogout_Click;

            sidebar.Controls.Add(btnLogout);
            sidebar.Controls.Add(btnAddSchedule);
            sidebar.Controls.Add(btnImportData);
            sidebar.Controls.Add(btnVaccines);
            sidebar.Controls.Add(btnPatients);
            sidebar.Controls.Add(btnDashboard);
        }

        private Button CreateSidebarButton(string text)
        {
            Button btn = new Button();

            btn.Text = text;
            btn.Dock = DockStyle.Top;
            btn.Height = 60;
            btn.FlatStyle = FlatStyle.Flat;
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(60, 60, 60);
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.FlatAppearance.BorderSize = 0;

            return btn;
        }

        // DASHBOARD VIEW
        private void LoadDashboard()
        {
            mainPanel.Controls.Clear();

            // CARDS
            VaccinationScheduleRepository vacSchedRepo = new VaccinationScheduleRepository();
            PatientRepository patientRepo = new PatientRepository();

            int totalPatients = patientRepo.GetTotalPatients(loggedDoctorId);
            int totalVaccinated = patientRepo.GetTotalVacinnatedPatient(loggedDoctorId);
            int appointmentsToday = patientRepo.GetTodaysSchedule(loggedDoctorId);

            Panel cardPatients = CreateCard("Total Patients", totalPatients.ToString(), new Point(320, 80));
            Panel cardVaccinations = CreateCard("Total Vaccinated", totalVaccinated.ToString(), new Point(570, 80));
            Panel cardAppointments = CreateCard("Appointments Today", appointmentsToday.ToString(), new Point(820, 80));

            mainPanel.Controls.Add(cardPatients);
            mainPanel.Controls.Add(cardVaccinations);
            mainPanel.Controls.Add(cardAppointments);

            // LOAD PIE CHART
            AnalyticsService analytics = new AnalyticsService();
            analytics.LoadPieChart(loggedDoctorId, mainPanel);

            // RECENT VACCINATIONS
            Label lblRecent = new Label();
            lblRecent.Text = "Recent Vaccinations";
            lblRecent.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblRecent.Location = new Point(250, 200);
            lblRecent.AutoSize = true;

            dgvRecent = new DataGridView();
            dgvRecent.Location = new Point(250, 230);
            dgvRecent.Size = new Size(500, 200);
            dgvRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecent.ReadOnly = true;

            mainPanel.Controls.Add(lblRecent);
            mainPanel.Controls.Add(dgvRecent);

            LoadRecentVaccinations();




            // TODAY SCHEDULE
            Label lblSchedule = new Label();
            lblSchedule.Text = "Today's Schedule";
            lblSchedule.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblSchedule.Location = new Point(250, 450);
            lblSchedule.AutoSize = true;

            dgvSchedule = new DataGridView();
            dgvSchedule.Location = new Point(250, 480);
            dgvSchedule.Size = new Size(500, 200);
            dgvSchedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSchedule.ReadOnly = true;


            mainPanel.Controls.Add(lblSchedule);
            mainPanel.Controls.Add(dgvSchedule);

            LoadTodaysSchedule();


        }


        private void LoadTodaysSchedule()
        {
            try
            {
                VaccinationScheduleRepository repo = new VaccinationScheduleRepository();
                DataTable table = repo.GetVaccinationsToday(loggedDoctorId);
                dgvSchedule.DataSource = table;
            }
            catch(Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }

        }

        private void LoadRecentVaccinations()
        {
            try
            {
                PatientRepository repo = new PatientRepository();
                DataTable table = repo.GetPatientsByDoctor(loggedDoctorId);
                dgvRecent.DataSource = table;
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load recent vaccinations.");
            }
        }

        private Panel CreateCard(string title, string value, Point location)
        {
            Panel card = new Panel();
            card.Size = new Size(220, 100);
            card.Location = location;
            card.BackColor = Color.White;
            card.BorderStyle = BorderStyle.FixedSingle;

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTitle.Location = new Point(15, 15);
            lblTitle.AutoSize = true;

            Label lblValue = new Label();
            lblValue.Text = value;
            lblValue.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblValue.ForeColor = Color.DarkBlue;
            lblValue.Location = new Point(15, 40);
            lblValue.AutoSize = true;

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void BtnPatients_Click(object sender, EventArgs e)
        {
            PatientForm form = new PatientForm(loggedDoctorId);
            form.Show();
        }

        private void BtnMonitoring_Click(object sender, EventArgs e)
        {
            MonitoringForm form = new MonitoringForm();
            form.ShowDialog();
        }

        private void BtnSchedule_Click(object sender, EventArgs e)
        {
            ScheduleForm form = new ScheduleForm(loggedDoctorId);
            form.ShowDialog();
        }

        private void BtnUpImportData_click(object sender, EventArgs e)
        {
            ImportForm form = new ImportForm();
            form.ShowDialog();
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Show login form
            LoginForm login = new LoginForm();
            login.Show();
        }

    }
}