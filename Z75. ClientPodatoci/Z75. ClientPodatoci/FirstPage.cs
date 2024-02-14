using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z75.ClientPodatoci;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Z75.Project_X
{
    public partial class FirstPage : Form   
    {

        private int secRab = 0;
        private int minRab = 0;
        private int satRab = 0;

        private int secPauza = 0;
        private int minPauza = 0;
        private int satPauza = 0;

        private int secLunch = 0;
        private int minLunch = 0;
        private int satLunch = 0;


        public FirstPage()

        {
            InitializeComponent();
            //Da se zemi username od LogIn i da se prenesi vo ovaa forma za da mozi da se zacuva so ime vremeto na rabotenje
            usernameHidden.Text = LogIn.message1;
            usernameHidden.Hide();
            dateTimePicker1.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void btnZacuvajVreme_Click(object sender, EventArgs e)
        {

            try
            {
                // Set the server IP address and port number
                string serverIp = "localhost";
                int serverPort = 8070;

                // Create a TcpClient to connect to the server
                TcpClient client = new TcpClient(serverIp, serverPort);
                Console.WriteLine("Connected to server.");

                // Get the network stream for reading and writing
                NetworkStream stream = client.GetStream();

                // Send a message to the server
                string message1 = usernameHidden.Text.ToString();
                string message2 = dateTimePicker1.Value.ToShortDateString();
                string message3 = tbRabota.Text;
                string message4 = tbPauza.Text;
                string message5 = tbRucek.Text;


                string krajno = message1 + "|" + message2 + "|" + message3 + "|" + message4 + "|" + message5;
                byte[] data = Encoding.ASCII.GetBytes(krajno);
                stream.Write(data, 0, data.Length);

                //// Receive a response from the server
                //data = new byte[1024];
                //int bytesRead = stream.Read(data, 0, data.Length);
                //string response = Encoding.ASCII.GetString(data, 0, bytesRead);
                //Console.WriteLine("Server Response: " + response);

                // Close the client connection
                client.Close();
                tbRabota.Text = "";
                tbPauza.Text = "";
                tbRucek.Text = "";

        }   
        catch
            {
                MessageBox.Show("Error: ");
            }

        }

        private void btnPocetokRab_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btnKrajRabota_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            secRab++;



            if (secRab == 60)
            {
                minRab++;
                secRab = 0;
                if (minRab == 60)
                {
                    satRab++;
                    minRab = 0;
                }
            }


            string sec = secRab.ToString();

            string min = minRab.ToString();

            string sat = satRab.ToString();

            string vreme = sat + " : " + min + " : " + sec;

            tbRabota.Text = vreme;
        }

        private void btnPocetokPauza_Click(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void btnKrajPauza_Click(object sender, EventArgs e)
        {
            timer2.Stop();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            secPauza++;



            if (secPauza == 60)
            {
                minPauza++;
                secPauza = 0;
                if (minPauza == 60)
                {
                    satPauza++;
                    minPauza = 0;
                }
            }


            string sec = secPauza.ToString();

            string min = minPauza.ToString();

            string sat = satPauza.ToString();

            string vreme = sat + " : " + min + " : " + sec;

            tbPauza.Text = vreme;
        }

        private void btnPocetokRucek_Click(object sender, EventArgs e)
        {
            timer3.Start();
        }

        private void btnKrajRucek_Click(object sender, EventArgs e)
        {
            timer3.Stop();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            secLunch++;



            if (secLunch == 60)
            {
                minLunch++;
                secLunch = 0;
                if (minLunch == 60)
                {
                    satLunch++;
                    minLunch = 0;
                }
            }


            string sec = secLunch.ToString();

            string min = minLunch.ToString();

            string sat = satLunch.ToString();

            string vreme = sat + " : " + min + " : " + sec;

            tbRucek.Text = vreme;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
