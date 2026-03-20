using VaxTrack.BLL;
using VaxTrack_SDG_Project.VaxTrack.DAL;

namespace VaxTrack_SDG_Project.VaxTrack.UI
{
    public partial class LoginForm : Form
    {
        private Panel panelLogin;
        private Label lblHeader;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblRole;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private ComboBox cmbRole;
        private Button btnLogin;

        private AuthenticationService authService;

        public LoginForm()
        {
            InitializeComponent();
            BuildUI();

            authService = new AuthenticationService();
        }

        private void BuildUI()
        {
            this.Text = "VaxTrack Login";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // LOGIN PANEL
            panelLogin = new Panel();
            panelLogin.Size = new Size(350, 360);
            panelLogin.BackColor = Color.White;
            panelLogin.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panelLogin);

            // HEADER
            lblHeader = new Label();
            lblHeader.Text = "VaxTrack Login";
            lblHeader.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblHeader.Dock = DockStyle.Top;
            lblHeader.Height = 60;
            panelLogin.Controls.Add(lblHeader);

            // ROLE LABEL
            lblRole = new Label();
            lblRole.Text = "Select Role";
            lblRole.Location = new Point(30, 75);
            panelLogin.Controls.Add(lblRole);

            // ROLE COMBOBOX
            cmbRole = new ComboBox();
            cmbRole.Width = 280;
            cmbRole.Height = 20;
            cmbRole.Location = new Point(30, 100);
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRole.Items.Add("Admin");
            cmbRole.Items.Add("Doctor");
            cmbRole.SelectedIndex = 0;
            panelLogin.Controls.Add(cmbRole);

            // USERNAME LABEL
            lblUsername = new Label();
            lblUsername.Text = "Username";
            lblUsername.Location = new Point(30, 140);
            panelLogin.Controls.Add(lblUsername);

            // USERNAME TEXTBOX
            txtUsername = new TextBox();
            txtUsername.Width = 280;
            txtUsername.Location = new Point(30, 160);
            panelLogin.Controls.Add(txtUsername);

            // PASSWORD LABEL
            lblPassword = new Label();
            lblPassword.Text = "Password";
            lblPassword.Location = new Point(30, 200);
            panelLogin.Controls.Add(lblPassword);

            // PASSWORD TEXTBOX
            txtPassword = new TextBox();
            txtPassword.Width = 280;
            txtPassword.Location = new Point(30, 220);
            txtPassword.PasswordChar = '*';
            panelLogin.Controls.Add(txtPassword);

            // LOGIN BUTTON
            btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Width = 280;
            btnLogin.Height = 40;
            btnLogin.Location = new Point(30, 270);
            btnLogin.BackColor = Color.LightBlue;
            btnLogin.Click += BtnLogin_Click;
            panelLogin.Controls.Add(btnLogin);

            // CENTER PANEL
            this.Resize += CenterPanel;
            CenterPanel(null, null);
        }

        private void CenterPanel(object sender, EventArgs e)
        {
            panelLogin.Left = (this.ClientSize.Width - panelLogin.Width) / 2;
            panelLogin.Top = (this.ClientSize.Height - panelLogin.Height) / 2;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cmbRole.SelectedItem.ToString();

            bool success = authService.Login(username, password, role);

            if (!success)
            {
                MessageBox.Show("Invalid Username or Password");
                return;
            }

            MessageBox.Show("Login Successful");

            if (role == "Admin")
            {
                AdminRepository repo = new AdminRepository();
                int adminId = repo.GetAdminId(username, password);

                if (adminId != -1)
                {
                    AdminForm adminForm = new AdminForm(adminId); // pass ID (optional but recommended)
                    adminForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid Admin credentials.");
                }
            }
            else if (role == "Doctor")
            {
                DoctorRepository repo = new DoctorRepository();
                int doctorId = repo.GetDoctorId(username, password);

                if (doctorId != -1)
                {
                    DoctorForm doctorForm = new DoctorForm(doctorId);
                    doctorForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Doctor not found.");
                }
            }
        }
    }
}