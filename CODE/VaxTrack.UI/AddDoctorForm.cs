using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.DAL;
using System.Text.RegularExpressions;

public class AddDoctorForm : Form
{
    private TextBox txtFullname;
    private TextBox txtEmail;
    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnSave;
    private Button btnCancel;

    public AddDoctorForm()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        
        this.Text = "Add Doctor";
        this.Size = new Size(400, 450);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.BackColor = Color.White;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // ===== Title =====
        Label lblTitle = new Label()
        {
            Text = "Create Doctor Account",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.FromArgb(40, 40, 40),
            Location = new Point(60, 20),
            AutoSize = true
        };

        // ===== Fullname =====

        Label lblFullname = new Label()
        {
            Text = "Fullname",
            Location = new Point(50, 80),
            Font = new Font("Segoe UI", 10)
        };

        txtFullname = new TextBox()
        {
            Location = new Point(50, 105),
            Width = 280,
            Font = new Font("Segoe UI", 10)
        };

        // ===== Email =====
        Label lblEmail = new Label()
        {
            Text = "Email",
            Location = new Point(50, 130),
            Font = new Font("Segoe UI", 10)
        };

        txtEmail = new TextBox()
        {
            Location = new Point(50, 155),
            Width = 280,
            Font = new Font("Segoe UI", 10)
        };

        txtEmail.TextChanged += (s, e) =>
        {
            if (IsValidEmail(txtEmail.Text))
            {
                txtEmail.BackColor = Color.White;
            }
            else
            {
                txtEmail.BackColor = Color.MistyRose; // light red
            }
        };

        //===== Username =====
        Label lblUsername = new Label()
        {
            Text = "Username",
            Location = new Point(50, 180),
            Font = new Font("Segoe UI", 10)
        };

        txtUsername = new TextBox()
        {
            Location = new Point(50, 205),
            Width = 280,
            Font = new Font("Segoe UI", 10)
        };

        // ===== Password =====
        Label lblPassword = new Label()
        {
            Text = "Password",
            Location = new Point(50, 230),
            Font = new Font("Segoe UI", 10)
        };

        txtPassword = new TextBox()
        {
            Location = new Point(50, 255),
            Width = 280,
            Font = new Font("Segoe UI", 10),
            PasswordChar = '●'
        };

        // ===== Save Button =====
        btnSave = new Button()
        {
            Text = "Save",
            Location = new Point(50, 320),
            Width = 130,
            Height = 35,
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Click += BtnSave_Click;

        // ===== Cancel Button =====
        btnCancel = new Button()
        {
            Text = "Cancel",
            Location = new Point(200, 320),
            Width = 130,
            Height = 35,
            BackColor = Color.Gray,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.Click += (s, e) => this.Close();

        // ===== Add Controls =====
        this.Controls.Add(lblTitle);
        this.Controls.Add(lblFullname);
        this.Controls.Add(txtFullname);
        this.Controls.Add(lblEmail);
        this.Controls.Add(txtEmail);
        this.Controls.Add(lblUsername);
        this.Controls.Add(txtUsername);
        this.Controls.Add(lblPassword);
        this.Controls.Add(txtPassword);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnCancel);
    }

    // ===== Save Logic =====
    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            MessageBox.Show("Please fill all fields.");
            return;
        }

        string hashedPassword = PasswordHasher.HashPassword(txtPassword.Text);

        using (var conn = Database.GetConnection())
        {
            conn.Open();

            string query = @"INSERT INTO doctors (username, password, full_name, email)
                             VALUES (@username, @password, @fullname, @email)";

            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                cmd.Parameters.AddWithValue("@password", hashedPassword);
                cmd.Parameters.AddWithValue("@fullname", txtFullname.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                try
                {
                    if (!IsValidEmail(txtEmail.Text))
                    {
                        MessageBox.Show("Please enter a valid email address.");
                        txtEmail.Focus();
                        return;
                    }
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Doctor added successfully!");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}