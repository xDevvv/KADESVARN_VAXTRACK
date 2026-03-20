namespace VaxTrack_SDG_Project.VaxTrack.UI
{
    public partial class DashboardForm : Form
    {
        private Panel sidebar;
        private Panel mainPanel;
        private Label lblHeader;

        private Button btnPatients;
        private Button btnVaccines;
        private Button btnSchedule;
        private Button btnLogout;

        public DashboardForm()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "VaxTrack Dashboard";
            this.WindowState = FormWindowState.Maximized;

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
            btnPatients = CreateSidebarButton("Patients");
            btnPatients.Click += BtnPatients_Click;

            btnVaccines = CreateSidebarButton("Vaccines");
            btnVaccines.Click += BtnVaccines_Click;

            btnSchedule = CreateSidebarButton("Schedule");
            btnSchedule.Click += BtnSchedule_Click;

            btnLogout = CreateSidebarButton("Logout");
            btnLogout.Click += BtnLogout_Click;

            sidebar.Controls.Add(btnLogout);
            sidebar.Controls.Add(btnSchedule);
            sidebar.Controls.Add(btnVaccines);
            sidebar.Controls.Add(btnPatients);
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

        private void BtnPatients_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Open Patient Management");
        }

        private void BtnVaccines_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Open Vaccine Management");
        }

        private void BtnSchedule_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Open Vaccination Schedule");
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
