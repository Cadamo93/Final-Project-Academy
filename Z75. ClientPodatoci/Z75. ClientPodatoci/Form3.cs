using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z75.Project_X;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Z75.ClientPodatoci
{
    public partial class Form3 : Form
    {
        static public string message1;

        private TcpListener server;
      

        private ManualResetEvent stopThread = new ManualResetEvent(false);



        public void ThreadProc1()
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            server = new TcpListener(ip, 8085);
            TcpClient client = default(TcpClient);
            try
            {
                server.Start();
                // MessageBox.Show("Tredot za poraki e aktiven");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return; // Exit the thread in case of an exception
            }

            while (!stopThread.WaitOne(0))
            {
                if (server.Pending())
                {
                    client = server.AcceptTcpClient();
                    byte[] receivedBuffer = new byte[1024];
                    NetworkStream stream = client.GetStream();
                    stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                    int count = Array.IndexOf<byte>(receivedBuffer, 0, 0);

                    string msg = Encoding.ASCII.GetString(receivedBuffer, 0, count);
                    byte[] sendData = Encoding.ASCII.GetBytes(msg);
                    int b = sendData.Length;

                    textBox1.Clear();
                    textBox1.Text = msg;

                    //Messagex.Show(msg);
                }
            }

            // Clean up resources before exiting the thread
            server.Stop();
        }



        public Form3()
        {
            InitializeComponent();

            Thread threadClient = new Thread(ThreadProc1)
            {
                IsBackground = true
            };
            threadClient.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Set the server IP address and port number
                string serverIp = "localhost";
                int serverPort = 8080;

                // Create a TcpClient to connect to the server
                TcpClient client = new TcpClient(serverIp, serverPort);
                Console.WriteLine("Connected to server.");

                // Get the network stream for reading and writing
                NetworkStream stream = client.GetStream();

                // Send a message to the server
                message1 = textBox1.Text.ToString();
                string message2 = textBox2.Text.ToString();
                

                string krajno = message1 + "|" + message2;
                byte[] data = Encoding.ASCII.GetBytes(krajno);
                stream.Write(data, 0, data.Length);

                // Receive a response from the server
                data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine("Server Response: " + response);

                // Close the client connection
                client.Close();

                // Handle the server response
                if (response == "login2")
                {
                    
                    message1 = textBox1.Text.ToString();
                    MessageBox.Show("Login successful!");
                    // Perform actions for a successful login
                    Form2 form2 = new Form2();
                    form2.Show();
                }
                else if (response == "login")
                {

                    message1 = textBox1.Text.ToString();
                    MessageBox.Show("Login successful!");
                    // Perform actions for a successful login
                    Manager manager = new Manager();
                    manager.Show();
                }
                else if (response == "incorrect")
                {
                    MessageBox.Show("Incorrect username or password.");
                    // Perform actions for an incorrect login
                }
                else
                {
                    MessageBox.Show("Unexpected response from server.");
                    // Handle other responses as needed
                }

                stopThread.Set();
            }
            catch
            {
                MessageBox.Show("Error: ");
            }
        }
    }
}
