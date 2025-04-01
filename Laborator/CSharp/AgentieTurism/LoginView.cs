using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgentieTurism.Services;

namespace AgentieTurism
{
    public partial class LoginView : Form
    {
        private Service service;

        public LoginView(Service service)
        {
            InitializeComponent();
            this.service = service;

             button1.Click += BtnLogin_Click;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Completați toate câmpurile.");
                return;
            }

            try
            {
                var employee = service.LogIn(email, password);
                if (employee != null)
                {
                    MessageBox.Show("Autentificare reușită!");

                    FlightsView flightsView = new FlightsView(service);
                    flightsView.FormClosed += (s, args) => this.Close();
                    flightsView.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Email sau parolă greșită.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la autentificare: " + ex.Message);
            }
        }
    }
}