using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Z75.Project_X
{
    public partial class Form1 : Form
    {
        private TcpClient client;

        int selectedIndex = 0;




        public Form1()
        {
            InitializeComponent();
            //listView1.Columns.Add("Date");
            //listView1.Columns.Add("ID");
            //listView1.Columns.Add("Faktura");
            //listView1.Columns.Add("PaymentYN");
            //listView1.Columns.Add("Assignet");
            //listView1.Columns.Add("Status");
            //listView1.Columns.Add("Note");
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Connect to the server
                ConnectToServer(8191);

                // Construct the message with Ime, Prezime, and Godini
                string message = $"{textBox1.Text}|{textBox2.Text}|{textBox3.Text}|{textBox8.Text}|{textBox5.Text}|{textBox6.Text}|{textBox7.Text}";

                // Send the message to the server
                SendDataToServer(message);

                // Close the client connection
                client.Close(); // Close the client connection here

                // Inform the user that the operation is completed
                MessageBox.Show("Data sent to the server and connection closed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {
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


        //private async void button4_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Connect to the server
        //        ConnectToServer(8191);

        //        // Send a request to the server to retrieve and sort data by "Ime"
        //        SendDataToServer("SortByID:" + textBox4.Text);


        //        // Clear the existing items in listView1
        //        listView1.Invoke((MethodInvoker)(() => listView1.Items.Clear())); // Clear the ListView

        //        // Receive data from the server
        //        string responseData = await ReceiveDataFromServer();

        //        // Parse the received data and populate the listView1
        //        if (!string.IsNullOrEmpty(responseData))
        //        {
        //            // Define delimiters for rows and items
        //            char rowDelimiter = '\n'; // Assuming rows are separated by newline
        //            char itemDelimiter = '|'; // Assuming items within a row are separated by '|'

        //            // Split the received data into rows
        //            string[] rows = responseData.Split(rowDelimiter);

        //            // Iterate through each row and add it to listView1
        //            foreach (string row in rows)
        //            {
        //                // Split the row into items
        //                string[] dataItems = row.Split(itemDelimiter);

        //                // Check if there are at least 3 items in the row
        //                if (dataItems.Length >= 3)
        //                {
        //                    // Create a new ListViewItem for the row
        //                    ListViewItem item = new ListViewItem(dataItems[0]);
        //                    item.SubItems.Add(dataItems[1]);
        //                    item.SubItems.Add(dataItems[2]);
        //                    item.SubItems.Add(dataItems[3]);
        //                    item.SubItems.Add(dataItems[4]);
        //                    item.SubItems.Add(dataItems[5]);
        //                    item.SubItems.Add(dataItems[6]);

        //                    // Add the ListViewItem to listView1
        //                    listView1.Invoke((MethodInvoker)(() => listView1.Items.Add(item))); // Invoke UI update
        //                }
        //                else
        //                {
        //                    MessageBox.Show("Data received succesfully.");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("No data received from the server.");
        //        }

        //        // Close the client connection
        //        client.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //}


        


       


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



        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selected = listView1.SelectedItems[0];
                // Access selected item and its subitems as needed
                string itemText = selected.Text;
                string subitemText = selected.SubItems[0].Text;
                int selectedIndex = listView1.SelectedIndices[0];
                //MessageBox.Show(selectedIndex.ToString());
                // Display the selected item information
                //MessageBox.Show($"Selected Item: {itemText}, Subitem: {subitemText}");

                textBox1.Text = selected.SubItems[0].Text;
                textBox2.Text = selected.SubItems[1].Text;
                textBox3.Text = selected.SubItems[2].Text;
                textBox5.Text = selected.SubItems[3].Text;
                textBox6.Text = selected.SubItems[4].Text;
                textBox7.Text = selected.SubItems[5].Text;
                textBox8.Text = selected.SubItems[6].Text;



            }
        }

        private void btnSabe_Click(object sender, EventArgs e)
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

                selected.SubItems[3].Text = textBox5.Text;
                selected.SubItems[4].Text = textBox6.Text;
                selected.SubItems[5].Text = textBox7.Text;
                selected.SubItems[6].Text = textBox8.Text;

               

            }
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;

            

        }

        

        

        

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the server IP address and port number
                string serverIp = "localhost";
                int serverPort = 8192;

                // Create a TcpClient to connect to the server
                TcpClient client = new TcpClient(serverIp, serverPort);
                

                // Get the network stream for reading and writing
                NetworkStream stream = client.GetStream();

                // Send a message to the server
                int message1 = selectedIndex + 1;
                string message2 = textBox1.Text;
                string message3 = textBox2.Text;
                string message4 = textBox3.Text;
                //string message5 = textBox4.Text;
                string message6 = textBox5.Text;
                string message7 = textBox6.Text;
                string message8 = textBox7.Text;
                string message9 = textBox8.Text;


                string krajno = message1 + "|" + message2 + "|" + message3 + "|" + message4 + "|" + message6 + "|" + message7 + "|" + message8 + "|" + message9;
                byte[] data = Encoding.ASCII.GetBytes(krajno);
                stream.Write(data, 0, data.Length);

                //// Receive a response from the server
                //data = new byte[1024];
                //int bytesRead = stream.Read(data, 0, data.Length);
                //string response = Encoding.ASCII.GetString(data, 0, bytesRead);
                //Console.WriteLine("Server Response: " + response);

                // Close the client connection
                client.Close();
                

            }
            catch
            {
                MessageBox.Show("Error: ");
            }
        }

        
    }
}

