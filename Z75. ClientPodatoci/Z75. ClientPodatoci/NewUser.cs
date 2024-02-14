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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Z75.ClientPodatoci
{
    public partial class NewUser : Form
    {

        private TcpClient client;
        public NewUser()
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Username");
            listView1.Columns.Add("Password");
            listView1.Columns.Add("Type");


        }

            private void btnAdd_Click(object sender, EventArgs e)
            {
                try
                {
                    // Validate user input (you can add more validation as needed)
                    if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                        string.IsNullOrWhiteSpace(textBox3.Text))
                    {
                        MessageBox.Show("Please fill in all fields.");
                        return;
                    }

                    // Connect to the server
                    ConnectToServer(8110);

                    // Construct the message with Ime, Prezime, and Godini
                    string message = $"{textBox1.Text}|{textBox2.Text}|{textBox3.Text}";

                    // Send the message to the server
                    SendDataToServer(message);

                    // Close the client connection (if you're not planning to reuse it)
                    client.Close();

                    // Inform the user that the operation is completed
                    MessageBox.Show("Data sent to the server and connection closed successfully.");



                


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }

            }

        


            private void ConnectToServer(int serverPort)
            {
                try
                {
                    // Set the server IP address and port number
                    string serverIp = "localhost";


                    // Create a TcpClient to connect to the server
                    client = new TcpClient(serverIp, serverPort);
                    MessageBox.Show("Connected to server.");
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

        //private async void btnShow_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Connect to the server
        //        ConnectToServer(8110);

        //        // Request data from the server
        //        SendDataToServer("GetData");

        //        // Receive data from the server
        //        string responseData = await ReceiveDataFromServer();

        //        // Close the client connection (if you're not planning to reuse it)
        //        client.Close();

        //        // Process and display the received data in the ListView
        //        UpdateListView(responseData);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //}

        private void UpdateListView(string data)
        {
            // Clear existing items in the ListView
            listView1.Items.Clear();

            // Split the received data into rows
            string[] rows = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                // Split each row into columns
                string[] columns = row.Split('|');

                // Create a ListViewItem and add it to the ListView
                ListViewItem item = new ListViewItem(columns);
                listView1.Items.Add(item);
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

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Connect to the server
                ConnectToServer(8110);

                // Request data from the server
                SendDataToServer("GetData");

                // Receive data from the server
                string responseData = await ReceiveDataFromServer();

                // Close the client connection (if you're not planning to reuse it)
                client.Close();

                // Process and display the received data in the ListView
                UpdateListView(responseData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }
    }
}
