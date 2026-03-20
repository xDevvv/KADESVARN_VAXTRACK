using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using VaxTrack_SDG_Project.VaxTrack.DAL;

public partial class ImportForm : Form
{
    private Button btnUpload;
    private Label lblTitle;
    private DataGridView dgvPreview;

    private List<ImportPatient> importedPatients = new List<ImportPatient>();

    public ImportForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        this.Text = "Import Patient Records (JSON)";
        this.Size = new Size(900, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        lblTitle = new Label()
        {
            Text = "Upload Patient JSON File",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Location = new Point(20, 15),
            AutoSize = true
        };

        btnUpload = new Button()
        {
            Text = "Upload JSON",
            Location = new Point(20, 60),
            Width = 150,
            BackColor = Color.DarkOrange,
            ForeColor = Color.White
        };
        btnUpload.Click += BtnUpload_Click;

        dgvPreview = new DataGridView()
        {
            Location = new Point(20, 110),
            Size = new Size(840, 320),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        this.Controls.Add(lblTitle);
        this.Controls.Add(btnUpload);
        this.Controls.Add(dgvPreview);
    }

    private async void BtnUpload_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Filter = "JSON files (*.json)|*.json"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            await LoadJsonAsync(openFileDialog.FileName);
        }
    }

    private async Task LoadJsonAsync(string filePath)
    {
        try
        {
            btnUpload.Enabled = false;
            btnUpload.Text = "Loading...";

            string json = await File.ReadAllTextAsync(filePath);

            importedPatients = JsonConvert.DeserializeObject<List<ImportPatient>>(json);

            dgvPreview.DataSource = importedPatients;

            btnUpload.Text = "Save to Database";
            btnUpload.Click -= BtnUpload_Click;
            btnUpload.Click += BtnSave_Click;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
        finally
        {
            btnUpload.Enabled = true;
        }
    }

    private async void BtnSave_Click(object sender, EventArgs e)
    {
        await SaveToDatabaseAsync();
    }

    private async Task SaveToDatabaseAsync()
    {
        try
        {
            btnUpload.Enabled = false;
            btnUpload.Text = "Saving...";

            await Task.Run(() =>
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    foreach (var p in importedPatients)
                    {
                        var cmd = new SQLiteCommand(@"
                            INSERT INTO patients 
                            (first_name, last_name, age, birth_date, gender, address, contact_number, doctor_id, vaccine_id, is_vaccinated, created_at)
                            VALUES
                            (@fn, @ln, @age, @bd, @gender, @addr, @contact, @doc, @vac, @vaccinated, @created)
                        ", conn);

                        cmd.Parameters.AddWithValue("@fn", p.FirstName);
                        cmd.Parameters.AddWithValue("@ln", p.LastName);
                        cmd.Parameters.AddWithValue("@age", p.Age);
                        cmd.Parameters.AddWithValue("@bd", p.BirthDate);
                        cmd.Parameters.AddWithValue("@gender", p.Gender);
                        cmd.Parameters.AddWithValue("@addr", p.Address);
                        cmd.Parameters.AddWithValue("@contact", p.ContactNumber);
                        cmd.Parameters.AddWithValue("@doc", p.DoctorID);
                        cmd.Parameters.AddWithValue("@vac", p.VaccineID);
                        cmd.Parameters.AddWithValue("@vaccinated", p.IsVaccinated);
                        cmd.Parameters.AddWithValue("@created", p.CreatedAt);

                        cmd.ExecuteNonQuery();
                    }
                }
            });

            MessageBox.Show("Import successful!");
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
        finally
        {
            btnUpload.Enabled = true;
            btnUpload.Text = "Upload JSON";
        }
    }
}

