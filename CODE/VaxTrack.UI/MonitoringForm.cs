using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.DAL;

public class MonitoringForm : Form
{
    private DataGridView dgvPatients;
    private Button btnRefresh;
    private Label lblTitle;
    private ComboBox cmbFilter;

    public MonitoringForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "Patient Vaccine Monitoring";
        this.Size = new Size(900, 500);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;

        lblTitle = new Label()
        {
            Text = "Overdue & Upcoming Vaccinations",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true
        };

        cmbFilter = new ComboBox()
        {
            Location = new Point(20, 60),
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbFilter.Items.AddRange(new string[] { "Overdue", "Today", "Next 7 Days" });
        cmbFilter.SelectedIndex = 0;

        btnRefresh = new Button()
        {
            Text = "Refresh",
            Location = new Point(240, 58),
            Width = 100,
            Height = 30,
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White
        };
        btnRefresh.Click += BtnRefresh_Click;

        dgvPatients = new DataGridView()
        {
            Location = new Point(20, 100),
            Size = new Size(840, 340),
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        this.Controls.Add(lblTitle);
        this.Controls.Add(cmbFilter);
        this.Controls.Add(btnRefresh);
        this.Controls.Add(dgvPatients);

        this.Load += MonitoringForm_Load;
    }

    private async void MonitoringForm_Load(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    private async void BtnRefresh_Click(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            btnRefresh.Enabled = false;
            btnRefresh.Text = "Loading...";

            DataTable dt = await GetPatientsAsync(cmbFilter.SelectedItem.ToString());
            dgvPatients.DataSource = dt;

            HighlightRows();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            btnRefresh.Enabled = true;
            btnRefresh.Text = "Refresh";
        }
    }

    private async Task<DataTable> GetPatientsAsync(string filter)
    {
        return await Task.Run(() =>
        {
            DataTable dt = new DataTable();

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = "";

                if (filter == "Overdue")
                {
                    query = @"
                        SELECT patient_id,
                               first_name || ' ' || last_name AS Name,
                               age, gender, contact_number, is_vaccinated
                        FROM patients
                        WHERE is_vaccinated = 0
                    ";
                }
                else if (filter == "Vaccinated")
                {
                    query = @"
                        SELECT patient_id,
                               first_name || ' ' || last_name AS Name,
                               age, gender, contact_number, is_vaccinated
                        FROM patients
                        WHERE is_vaccinated = 1
                    ";
                }
                else
                {
                    query = @"
                        SELECT patient_id,
                               first_name || ' ' || last_name AS Name,
                               age, gender, contact_number, is_vaccinated
                        FROM patients
                    ";
                }

                using (var cmd = new SQLiteCommand(query, conn))
                using (var adapter = new SQLiteDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        });
    }

    private void HighlightRows()
    {
        foreach (DataGridViewRow row in dgvPatients.Rows)
        {
            int isVaccinated = Convert.ToInt32(row.Cells["is_vaccinated"].Value);

            if (isVaccinated == 0)
            {
                row.DefaultCellStyle.BackColor = Color.LightCoral; // 🔴 Not vaccinated
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.LightGreen; // 🟢 Vaccinated
            }
        }
    }
}
