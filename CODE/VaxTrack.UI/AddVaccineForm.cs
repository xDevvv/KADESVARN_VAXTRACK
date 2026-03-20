using VaxTrack_SDG_Project.VaxTrack.BLL;
using VaxTrack_SDG_Project.VaxTrack.MODELS;

public class AddVaccineForm : Form
{
    private TextBox txtVaccineName;
    private Button btnSave;
    private Button btnCancel;

    public AddVaccineForm()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        this.Text = "Add Vaccine";
        this.Size = new Size(400, 350);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.BackColor = Color.White;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // ===== Title =====
        Label lblTitle = new Label()
        {
            Text = "Create Vaccine Record",
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Color.FromArgb(40, 40, 40),
            Location = new Point(60, 20),
            AutoSize = true
        };

        // ===== Vaccine Name =====
        Label lblVaccineName = new Label()
        {
            Text = "Vaccine Name",
            Location = new Point(50, 70),
            Font = new Font("Segoe UI", 10)
        };

        txtVaccineName = new TextBox()
        {
            Location = new Point(50, 95),
            Width = 280,
            Font = new Font("Segoe UI", 10)
        };

        // ===== Save Button =====
        btnSave = new Button()
        {
            Text = "Save",
            Location = new Point(50, 260),
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
            Location = new Point(200, 260),
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
        this.Controls.Add(lblVaccineName);
        this.Controls.Add(txtVaccineName);
        this.Controls.Add(btnSave);
        this.Controls.Add(btnCancel);
    }

    // ===== Save Logic =====
    private void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtVaccineName.Text))
            {
                MessageBox.Show("Vaccine name is required.");
                txtVaccineName.Focus();
                return;
            }

            // Create DTO
            var dto = new VaccineCreateDto
            {
                VaccineName = txtVaccineName.Text.Trim()
            };

            // Call BLL
            VaccineService service = new VaccineService();
            service.AddVaccine(dto);

            MessageBox.Show("Vaccine added successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK; // important if used as modal
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}