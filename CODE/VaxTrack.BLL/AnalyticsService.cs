using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using VaxTrack_SDG_Project.VaxTrack.DAL;


namespace VaxTrack_SDG_Project.VaxTrack.BLL
{
    internal class AnalyticsService
    {
        public void LoadPieChart(int doctorId, Panel container) 
        {
            // PIE CHART(Beside Tables)
            Chart pieChart = new Chart();
            pieChart.Size = new Size(350, 300);
            pieChart.Location = new Point(800, 230); // beside the tables
            pieChart.BackColor = Color.Transparent;

            Title chartTitle = new Title();
            chartTitle.Text = "Vaccine Distribution";
            chartTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            chartTitle.ForeColor = Color.Black;

            pieChart.Titles.Add(chartTitle);

            ChartArea area = new ChartArea();
            pieChart.ChartAreas.Add(area);
            area.BackColor = Color.Transparent;

            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;
            series["PieLabelStyle"] = "Outside";

            VaccinationRepository repo = new VaccinationRepository();
            DataTable vaccineData = repo.GetVaccineDistribution(doctorId);

            foreach (DataRow row in vaccineData.Rows)
            {
                string vaccine = row["vaccine_name"].ToString();
                int total = Convert.ToInt32(row["total"]);

                series.Points.AddXY(vaccine, total);
            }

            pieChart.Series.Add(series);

            Legend legend = new Legend();
            legend.Docking = Docking.Bottom;

            pieChart.Legends.Add(legend);

            container.Controls.Add(pieChart);

        }
    }
}
