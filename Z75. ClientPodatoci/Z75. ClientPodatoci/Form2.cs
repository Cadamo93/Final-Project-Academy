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
using System.IO;
using System.Data.SQLite;
using Newtonsoft.Json;

namespace Z75.Project_X
{
    public partial class Form2 : Form   
    {

        private TcpClient client;





        private int secRab = 0;
        private int minRab = 0;
        private int satRab = 0;

        private int secPauza = 0;
        private int minPauza = 0;
        private int satPauza = 0;

        private int secLunch = 0;
        private int minLunch = 0;
        private int satLunch = 0;


        public Form2()

        {
            InitializeComponent();
            //Da se zemi username od LogIn i da se prenesi vo ovaa forma za da mozi da se zacuva so ime vremeto na rabotenje
            usernameHidden.Text = Form3.message1;
            usernameHidden.Hide();
            dateTimePicker1.Hide();
            

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Date", 90);
            listView1.Columns.Add("ID", 50);
            listView1.Columns.Add("Faktura", 100);
            listView1.Columns.Add("PaymentYN", 80);
            listView1.Columns.Add("Assignet", 80);
            listView1.Columns.Add("Status", 100);
            listView1.Columns.Add("Note", 150);

            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.FullRowSelect = true;
            listView2.Columns.Add("ID", 50);
            listView2.Columns.Add("Country", 100);
            listView2.Columns.Add("Commision", 100);
            listView2.Columns.Add("Accepting New Company", 130);
            listView2.Columns.Add("Accepting New Product", 130);
            

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

                ResetTimer(timer1, ref secRab, ref minRab, ref satRab, tbRabota);
                ResetTimer(timer2, ref secPauza, ref minPauza, ref satPauza, tbPauza);
                ResetTimer(timer3, ref secLunch, ref minLunch, ref satLunch, tbRucek);


            }   
        catch
            {
                MessageBox.Show("Error: ");
            }

        }

       

        private void ResetTimer(Timer timer, ref int seconds, ref int minutes, ref int hours, System.Windows.Forms.TextBox textBox)
        {
            timer.Stop(); // Stop the timer
            seconds = 0; // Reset seconds
            minutes = 0; // Reset minutes
            hours = 0;   // Reset hours
            textBox.Text = "0 : 0 : 0"; // Reset the corresponding TextBox text
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

        
        private void ConnectToServer(int serverPort)
        {
            try
            {
                // Set the server IP address and port number
                string serverIp = "localhost";


                // Create a TcpClient to connect to the server
                client = new TcpClient(serverIp, serverPort);
                Console.WriteLine("Connected to server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SendDataToServer(string data)
        {
            try
            {
                // Get the network stream for writing
                NetworkStream stream = client.GetStream();

                // Send the data to the server
                byte[] dataBytes = Encoding.ASCII.GetBytes(data);
                stream.Write(dataBytes, 0, dataBytes.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending data: " + ex.Message);
            }
        }

        private async Task<string> ReceiveDataFromServer()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                return Encoding.ASCII.GetString(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error receiving data: " + ex.Message);
                return null;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            listView2.Hide();
            listView1.Show();
            try
            {
                // Connect to the server
                ConnectToServer(8191);

                // Send a request to the server to retrieve data from the SQLite table
                SendDataToServer("GetDataFromSQLite");

                listView1.Invoke((MethodInvoker)(() => listView1.Items.Clear()));

                // Receive data from the server
                string responseData = await ReceiveDataFromServer();

                // Close the client connection
                client.Close();

                // Parse the received data and populate the listView1
                if (!string.IsNullOrEmpty(responseData))
                {
                    // Define delimiters for rows and items
                    char rowDelimiter = '\n'; // Assuming rows are separated by newline
                    char itemDelimiter = '|'; // Assuming items within a row are separated by '|'

                    // Split the received data into rows
                    string[] rows = responseData.Split(rowDelimiter);

                    // Iterate through each row and add it to listView1
                    foreach (string row in rows)
                    {
                        // Split the row into items
                        string[] dataItems = row.Split(itemDelimiter);

                        // Check if there are at least 3 items in the row
                        if (dataItems.Length >= 3)
                        {
                            // Create a new ListViewItem for the row
                            ListViewItem item = new ListViewItem(dataItems[0]);
                            item.SubItems.Add(dataItems[1]);
                            item.SubItems.Add(dataItems[2]);
                            item.SubItems.Add(dataItems[3]);
                            item.SubItems.Add(dataItems[4]);
                            item.SubItems.Add(dataItems[5]);
                            item.SubItems.Add(dataItems[6]);

                            // Add the ListViewItem to listView1
                            listView1.Invoke((MethodInvoker)(() => listView1.Items.Add(item))); // Invoke UI update
                        }
                        else
                        {
                            MessageBox.Show("All data received.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No data received from the server.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnOnboarding_Click(object sender, EventArgs e)
        {
            Onboarding onboardin = new Onboarding();
            onboardin.Show();

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            listView1.Hide();
            listView2.Show();
            try
            {
                // Connect to the server
                ConnectToServer(8090);

                // Send a request to the server to retrieve data from the SQLite table
                SendDataToServer("GetOnboardingDataFromSQLite");

                listView2.Invoke((MethodInvoker)(() => listView2.Items.Clear()));

                // Receive data from the server
                string responseData = await ReceiveDataFromServer();

                // Close the client connection
                client.Close();

                // Parse the received data and populate the listView1
                if (!string.IsNullOrEmpty(responseData))
                {
                    // Define delimiters for rows and items
                    char rowDelimiter = '\n'; // Assuming rows are separated by newline
                    char itemDelimiter = '|'; // Assuming items within a row are separated by '|'

                    // Split the received data into rows
                    string[] rows = responseData.Split(rowDelimiter);

                    // Iterate through each row and add it to listView1
                    foreach (string row in rows)
                    {
                        // Split the row into items
                        string[] dataItems = row.Split(itemDelimiter);

                        // Check if there are at least 3 items in the row
                        if (dataItems.Length >= 3)
                        {
                            // Create a new ListViewItem for the row
                            ListViewItem item = new ListViewItem(dataItems[0]);
                            item.SubItems.Add(dataItems[1]);
                            item.SubItems.Add(dataItems[2]);
                            item.SubItems.Add(dataItems[3]);
                            item.SubItems.Add(dataItems[4]);
                           

                            // Add the ListViewItem to listView1
                            listView2.Invoke((MethodInvoker)(() => listView2.Items.Add(item))); // Invoke UI update
                        }
                        else
                        {
                            MessageBox.Show("All data received.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No data received from the server.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

       
    }
}
