using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgentieTurism.Models;
using AgentieTurism.Services;
using AgentieTurism.Utils;

namespace AgentieTurism
{
    public partial class FlightsView : Form, Observer<FlightEvent>
    {
        private readonly Service service;

        public FlightsView(Service service)
        {
            InitializeComponent();
            this.service = service;
            
            button1.Click += BtnSearch_Click;
            button2.Click += BtnDetails_Click;
            button3.Click += BtnLogout_Click;

            service.AddObserver(this);

            LoadAllFlights();
        }

        private void LoadAllFlights()
        {
            var flights = service.GetAllFlights();
            UpdateFlightTable(flights);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string destination = textBox1.Text.Trim();
            DateTime date = dateTimePicker1.Value;

            if (string.IsNullOrWhiteSpace(destination))
            {
                
                var flights = service.GetAllFlights();
                UpdateFlightTable(flights);
            }
            else
            { 
                var found = service.FindFlights(destination, date);
                UpdateFlightTable(found);
            }
        }


        private void BtnDetails_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selectează un zbor din listă.");
                return;
            }

            var row = dataGridView1.SelectedRows[0];

            try
            {
                int id = Convert.ToInt32(row.Cells[0].Value);
                string destination = row.Cells[1].Value.ToString();
                DateTime date = DateTime.Parse(row.Cells[2].Value.ToString());
                string airport = row.Cells[3].Value.ToString();
                int seats = Convert.ToInt32(row.Cells[4].Value);

                Flight selectedFlight = new Flight(destination, date, airport, seats);
                selectedFlight.Id = id;


                TicketsView ticketsView = new TicketsView(service, selectedFlight);
                ticketsView.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la deschiderea biletului: " + ex.Message);
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();

            LoginView loginView = new LoginView(service);
            loginView.Show();

            this.Close();
        }



        private void UpdateFlightTable(List<Flight> flights)
        {
            dataGridView1.Rows.Clear();
            foreach (var flight in flights)
            {
                dataGridView1.Rows.Add(
                    flight.Id,
                    flight.Destination,
                    flight.DepartureDateTime.ToString("yyyy-MM-dd HH:mm"),
                    flight.Airport,
                    flight.AvailableSeats
                );
            }
        }

        public void Update(FlightEvent ev)
        {
            if (ev.Type == FlightEvent.EventType.FLIGHTS_UPDATED)
            {
                LoadAllFlights();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            service.RemoveObserver(this);
        }
    }
}
