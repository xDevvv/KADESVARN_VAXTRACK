using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.DAL;

namespace VaxTrack_SDG_Project.VaxTrack.UI
{
    public partial class AdminForm : Form
    {
        private int adminId;

        private DataGridView dgvDoctors;
        private Panel sidebar;
        private Panel mainPanel;

        public AdminForm(int adminId)
        {
            InitializeComponent();
            this.adminId = adminId;

            InitializeUI();
            LoadDashboard();
        }

        private void InitializeUI()
        {
            this.Text = "Admin Dashboard";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Sidebar
            sidebar = new Panel()
            {
                Width = 200,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(45, 45, 45)
            };

            // Main Panel
            mainPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Buttons
            Button btnDoctorMonitoring = CreateSidebarButton("Doctor Monitoring");
            Button btnAddDoctor = CreateSidebarButton("Add Doctor");
            Button btnAddVaccine = CreateSidebarButton("Add Vaccine");
            Button btnLogout = CreateSidebarButton("Logout");


            btnDoctorMonitoring.Click += BtnDoctorMonitoring_Click;
            btnAddDoctor.Click += BtnAddDoctor_Click;
            btnAddVaccine.Click += BtnAddVaccine_Click;
            btnLogout.Click += BtnLogout_Click;

            sidebar.Controls.Add(btnLogout);
            sidebar.Controls.Add(btnAddVaccine);
            sidebar.Controls.Add(btnAddDoctor);
            sidebar.Controls.Add(btnDoctorMonitoring);



            this.Controls.Add(mainPanel);
            this.Controls.Add(sidebar);

            mainPanel.Controls.Clear();

            
        }

        private void LoadDashboard()
        {
            mainPanel.Controls.Clear();

            Label lblTitle = new Label()
            {
                Text = "Doctor Management",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // ================= DataGridView =================
            DataGridView dgvDoctors = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(500, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
            };

            // Load doctors
            LoadDoctors(dgvDoctors);


            // ================= Add Controls =================
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(dgvDoctors);
        }

        // ================= Sidebar Button Style =================
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

        // ================= Load Doctors =================
        private void LoadDoctors(DataGridView dgv)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "SELECT doctor_id, username FROM Doctors";

                using (var adapter = new SQLiteDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgv.DataSource = dt;
                }
            }
        }

        // ================= Doctor Monitoring =================
        private void BtnDoctorMonitoring_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void BtnAddDoctor_Click(object sender, EventArgs e)
        {
            AddDoctorForm form = new AddDoctorForm();
            // Show as modal
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Reload dashboard automatically after adding new doctor
                LoadDashboard();
            }
        }

        private void BtnAddVaccine_Click(object sender, EventArgs e)
        {
            AddVaccineForm form = new AddVaccineForm();
            form.ShowDialog();
        }




        // ================= Logout =================
        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Show login form
            LoginForm login = new LoginForm();
            login.Show();
        }
    }
}