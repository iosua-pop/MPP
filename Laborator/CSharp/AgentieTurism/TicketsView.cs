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
    public partial class TicketsView : Form
    {
        private readonly Service service;
        private readonly Flight selectedFlight;

        public TicketsView(Service service, Flight flight)
        {
            InitializeComponent();
            this.service = service;
            this.selectedFlight = flight;

            label3.Text = "Desinatie: " + flight.Destination;
            label4.Text = "Plecare: " + flight.DepartureDateTime.ToString();
            label5.Text = "Aeroport: " + flight.Airport;
            label6.Text = "Locuri disponibile: " + flight.AvailableSeats.ToString();
            numericUpDown1.Maximum = flight.AvailableSeats;
            numericUpDown1.Minimum = 1;
            numericUpDown1.Value = 1;

            button2.Click += BtnBuy_Click;
            button1.Click += BtnBack_Click;
        }

        private void BtnBuy_Click(object sender, EventArgs e)
        {
            try
            {

                string clientName = textBox1.Text;
                string tourists = textBox2.Text;
                string address = textBox3.Text;
                int seats = (int)numericUpDown1.Value;


                service.BuyTickets(selectedFlight, clientName, tourists, address, seats);

                MessageBox.Show("Bilet cumpărat cu succes!");
                service.NotifyObservers(new FlightEvent(FlightEvent.EventType.FLIGHTS_UPDATED));

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la cumpărare: " + ex.Message);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
