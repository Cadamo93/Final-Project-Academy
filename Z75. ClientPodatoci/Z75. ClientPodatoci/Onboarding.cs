using System;
using System.Data.SQLite;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Z75.ClientPodatoci
{
    public partial class Onboarding : Form
    {

        private TcpClient client;

        int selectedIndex = 0;


        public Onboarding()
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("ID");
            listView1.Columns.Add("Country");
            listView1.Columns.Add("Commision");
            listView1.Columns.Add("Acepting Company");
            listView1.Columns.Add("Acepting Product");


            FetchOnboardingDataFromServer();
        }

        private void FetchOnboardingDataFromServer()
        {
            try
            {
                // Connect to the server
                ConnectToServer(8090);

                // Send a specific request to the server to fetch Onboarding data
                SendDataToServer("GetOnboardingDataFromSQLite");

                // Receive the response from the server
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                int bytesRead = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                string responseData = Encoding.ASCII.GetString(receivedBuffer, 0, bytesRead);

                // Parse the response and populate the ListView
                PopulateListView(responseData);

                // Close the client connection
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PopulateListView(string responseData)
        {
            // Clear existing items in the ListView
            listView1.Items.Clear();

            // Split the response into rows
            string[] rows = responseData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            // Loop through rows and add them to the ListView
            foreach (string row in rows)
            {
                // Split the row into columns
                string[] columns = row.Split('|');

                // Create a ListViewItem and add columns
                ListViewItem item = new ListViewItem(columns);
                listView1.Items.Add(item);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate user input (you can add more validation as needed)
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                    string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                // Connect to the server
                ConnectToServer(8090);

                // Construct the message with Ime, Prezime, and Godini
                string message = $"{textBox1.Text}|{textBox2.Text}|{textBox3.Text}|{textBox4.Text}";

                // Send the message to the server
                SendDataToServer(message);

                // Close the client connection (if you're not planning to reuse it)
                client.Close();

                // Inform the user that the operation is completed
                MessageBox.Show("Data sent to the server and connection closed successfully.");


                FetchOnboardingDataFromServer();

                MessageBox.Show("gotovo.");


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
                MessageBox.Show("Error connecting to the server: " + ex.Message);
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
                MessageBox.Show("Error sending data to the server: " + ex.Message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem selected = listView1.SelectedItems[0];
                    // Access selected item and its subitems as needed

                    textBox1.Text = selected.SubItems[1].Text;
                    textBox2.Text = selected.SubItems[2].Text;
                    textBox3.Text = selected.SubItems[3].Text;
                    textBox4.Text = selected.SubItems[4].Text;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selected = listView1.SelectedItems[0];
                // Access selected item and its subitems as needed
                string itemText = selected.Text;
                string subitemText = selected.SubItems[0].Text;
                selectedIndex = listView1.SelectedIndices[0];
                //MessageBox.Show(selectedIndex.ToString());
                // Display the selected item information
                //MessageBox.Show($"Selected Item: {itemText}, Subitem: {subitemText}");

                selected.SubItems[1].Text = textBox1.Text;
                selected.SubItems[2].Text = textBox2.Text;
                selected.SubItems[3].Text = textBox3.Text;
                selected.SubItems[4].Text = textBox4.Text;



            }
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selected = listView1.SelectedItems[0];
                // Access selected item and its subitems as needed
                string itemText = selected.Text;
                string subitemText = selected.SubItems[0].Text;
                selectedIndex = listView1.SelectedIndices[0];
                //MessageBox.Show(selectedIndex.ToString());
                // Display the selected item information
                //MessageBox.Show($"Selected Item: {itemText}, Subitem: {subitemText}");

                selected.SubItems[1].Text = textBox1.Text;
                selected.SubItems[2].Text = textBox2.Text;
                selected.SubItems[3].Text = textBox3.Text;
                selected.SubItems[4].Text = textBox4.Text;



            }
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;

            
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the server IP address and port number
                string serverIp = "localhost";
                int serverPort = 8100;

                // Create a TcpClient to connect to the server
                TcpClient client = new TcpClient(serverIp, serverPort);


                // Get the network stream for reading and writing
                NetworkStream stream = client.GetStream();

                // Send a message to the server
                int message1 = selectedIndex + 1;
                string message2 = textBox1.Text;
                string message3 = textBox2.Text;
                string message4 = textBox3.Text;
                string message5 = textBox4.Text;

                


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

                FetchOnboardingDataFromServer();


            }
            catch
            {
                MessageBox.Show("Error: ");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    
}