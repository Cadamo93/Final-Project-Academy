using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Z75.Project_X
{
    public partial class Form1 : Form
    {
        private string sqliteConnectionString = "Data Source=C:\\SQLite\\Payments.db;Version=3;";

        private ManualResetEvent stopThread = new ManualResetEvent(false);



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

            // Create the SQLite database
            CreateSQLiteDatabase();

            // Thread for handling incoming connections
            Thread thread = new Thread(ThreadProc)
            {
                IsBackground = true
            };
            thread.Start(); // Start the thread



            Thread thread1 = new Thread(t =>
            {
                ThreadProc1("Data Source= C:\\SQLite\\Final.db; Version = 3; New = True; Compress = True;", 8080);
            })
            {
                IsBackground = true
            };
            thread1.Start();

            Thread thread2 = new Thread(t =>
            {
                ThreadProc2("Data Source= C:\\SQLite\\TimeSheet.db; Version = 3; New = True; Compress = True;", 8070);
            })
            {
                IsBackground = true
            };
            thread2.Start();

            Thread thread3 = new Thread(t =>
            {
                ThreadProc3("Data Source= C:\\SQLite\\Payments.db; Version = 3; New = True; Compress = True;", 8192);

            })
            {
                IsBackground = true
            };
            thread3.Start();

            Thread thread5 = new Thread(t =>
            {
                ThreadProc5("Data Source= C:\\SQLite\\Onboarding.db; Version = 3; New = True; Compress = True;", 8090);
            })
            {
                IsBackground = true
            };
            thread5.Start();

            Thread thread6 = new Thread(t =>
            {
                ThreadProc6("Data Source= C:\\SQLite\\Onboarding.db; Version = 3; New = True; Compress = True;", 8100);
            })
            {
                IsBackground = true
            };
            thread6.Start();

            Thread thread7 = new Thread(t =>
            {
                ThreadProc7("Data Source= C:\\SQLite\\database\\Final1.db; Version = 3; New = True; Compress = True;", 8110);
            })
            {
                IsBackground = true
            };
            thread7.Start();


        }

        private void TransferData()
        {
            string sourceConnectionString = "Data Source=C:\\SQLite\\database\\Final1.db;";
            string destinationConnectionString = "Data Source=C:\\SQLite\\Final.db;";

            using (SQLiteConnection sourceConnection = new SQLiteConnection(sourceConnectionString))
            using (SQLiteConnection destinationConnection = new SQLiteConnection(destinationConnectionString))
            {
                try
                {
                    sourceConnection.Open();
                    destinationConnection.Open();

                    string query = "SELECT * FROM Final WHERE Usename NOT IN (SELECT Usename FROM Final)";

                    using (SQLiteCommand command = new SQLiteCommand(query, sourceConnection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string insertQuery = @"INSERT INTO Final (Usename, Password, Type) VALUES (@Usename, @Password, @Type)";

                                using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, destinationConnection))
                                {
                                    // Set parameters based on the retrieved data
                                    insertCommand.Parameters.AddWithValue("@Usename", reader["Usename"]);
                                    insertCommand.Parameters.AddWithValue("@Password", reader["Password"]);
                                    insertCommand.Parameters.AddWithValue("@Type", reader["Type"]);

                                    // Execute the insert command
                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    MessageBox.Show("Data transfer completed successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    sourceConnection.Close();
                    destinationConnection.Close();
                }
            }
        }

        public void ThreadProc1(string connectionString, int port)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip, port);
            TcpClient client = default(TcpClient);
            try
            {
                server.Start();
                //MessageBox.Show("Serverot e aktiviran");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            while (!stopThread.WaitOne(0))
            {

                
                    client = server.AcceptTcpClient();
                    byte[] receivedBuffer = new byte[1024];
                    NetworkStream stream = client.GetStream();
                    stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                    int count = Array.IndexOf<byte>(receivedBuffer, 0, 0);

                    string msg = Encoding.ASCII.GetString(receivedBuffer, 0, count);
                    byte[] sendData = Encoding.ASCII.GetBytes(msg);
                    int b = sendData.Length;

                
                    SQLiteConnection sqlite_conn;
                    sqlite_conn = CreateConnection(connectionString);

                    //CreateTable(sqlite_conn);

                    List<string> razdeleno = new List<string>();

                    razdeleno = ZboroviLista(msg);

                    SQLiteCommand cmd;
                    cmd = sqlite_conn.CreateCommand();




                    cmd.CommandText = "SELECT Usename, Password, Type FROM Final WHERE Usename = @user AND Password = @pass";



                    cmd.Parameters.AddWithValue("@user", razdeleno[0].Trim());
                    cmd.Parameters.AddWithValue("@pass", razdeleno[1].Trim());


                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Valid login, check the value of the third column
                        int type = reader.GetInt32(reader.GetOrdinal("Type"));

                        if (type == 1)
                        {
                            string response = "login";
                            byte[] responseData = Encoding.ASCII.GetBytes(response);
                            stream.Write(responseData, 0, responseData.Length);
                        }
                        else if (type == 2)
                        {
                            string response = "login2";
                            byte[] responseData = Encoding.ASCII.GetBytes(response);
                            stream.Write(responseData, 0, responseData.Length);
                        }
                    }
                    else
                    {
                        // Invalid login, send "incorrect" to the client
                        string response = "incorrect";
                        byte[] responseData = Encoding.ASCII.GetBytes(response);
                        stream.Write(responseData, 0, responseData.Length);
                    }


                    sqlite_conn.Close();

                



                //textBox1.Clear();
                //textBox1.Text = msg;

                //Messagex.Show(msg);
            }
        }

        public void ThreadProc2(string connectionString, int port)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip, port);
            TcpClient client = default(TcpClient);
            try
            {
                server.Start();
                //MessageBox.Show("Serverot e aktiviran");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            while (true)
            {
                client = server.AcceptTcpClient();
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                int count = Array.IndexOf<byte>(receivedBuffer, 0, 0);

                string msg = Encoding.ASCII.GetString(receivedBuffer, 0, count);
                byte[] sendData = Encoding.ASCII.GetBytes(msg);
                int b = sendData.Length;

                SQLiteConnection sqlite_conn;
                sqlite_conn = CreateConnection(connectionString);

                //CreateTable(sqlite_conn);

                List<string> razdeleno = new List<string>();

                razdeleno = ZboroviLista(msg);

                SQLiteCommand cmd;
                cmd = sqlite_conn.CreateCommand();

                



                if (msg == "GetTimeSheet")
                {
                    HandleTimeSheetDataRequest(client, sqlite_conn);
                }
                else
                {


                    cmd.CommandText = @"INSERT INTO TimeSheet(UserName, Date, VremeRabota, VremePauza, VremeRucek) VALUES (@UserName, @Date, @VremeRabota, @VremePauza, @VremeRucek)";



                    cmd.Parameters.AddWithValue("@UserName", razdeleno[0]);
                    cmd.Parameters.AddWithValue("@Date", razdeleno[1]);
                    cmd.Parameters.AddWithValue("@VremeRabota", razdeleno[2]);
                    cmd.Parameters.AddWithValue("@VremePauza", razdeleno[3]);
                    cmd.Parameters.AddWithValue("@VremeRucek", razdeleno[4]);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data imported successfully.");
                }



                


                sqlite_conn.Close();

                //textBox1.Clear();
                //textBox1.Text = msg;

                //Messagex.Show(msg);
            }
        }

        private void HandleTimeSheetDataRequest(TcpClient client, SQLiteConnection connection)
        {
            try
            {
                string selectDataQuery = "SELECT * FROM TimeSheet;";
                using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder responseBuilder = new StringBuilder();

                        // Loop through the database results and append them to the response string
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                responseBuilder.Append(reader[i].ToString());
                                if (i < reader.FieldCount - 1)
                                {
                                    responseBuilder.Append("|");
                                }
                            }
                            responseBuilder.AppendLine();
                        }

                        // Send the response back to the client
                        string responseData = responseBuilder.ToString();
                        byte[] responseBytes = Encoding.ASCII.GetBytes(responseData);
                        NetworkStream stream = client.GetStream();
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error handling Onboarding data request: " + ex.Message);
            }
        }

        static SQLiteConnection CreateConnection(string connectionString)
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection(connectionString);
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception)
            {
                Console.WriteLine("Problem so povrzuvanjeto so bazata");
            }
            return sqlite_conn;


        }

        //static void CreateTable(SQLiteConnection conn)
        //{

        //    SQLiteCommand sqlite_cmd;

        //    //proverka dali postoi tabela vo bazata
        //    string daliPostoiTabela = "SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'Final'";
        //    sqlite_cmd = conn.CreateCommand();
        //    sqlite_cmd.CommandText = daliPostoiTabela;
        //    var exists = sqlite_cmd.ExecuteReader();
        //    if (!exists.HasRows)
        //    {
        //        string createTable = "CREATE TABLE Final( Username VARCHAR(50), Password VARCHAR(50)))";
        //        sqlite_cmd = conn.CreateCommand();
        //        sqlite_cmd.CommandText = createTable;
        //        sqlite_cmd.ExecuteNonQuery();
        //    }

        //}

        


        static List<string> ZboroviLista(string str)
        {
            List<string> zborovi = new List<string>();
            string zbor = "";



            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != '|')
                {
                    zbor = zbor + str[i];
                    if (i == str.Length - 1)
                    {
                        zborovi.Add(zbor);
                        break;
                    }
                }
                else if (str[i] == '|')
                {
                    zborovi.Add(zbor);
                    zbor = "";

                }
            }


            return zborovi;

        }

        private void ThreadProc()
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip, 8191);
            TcpClient client = default(TcpClient);

            try
            {
                server.Start();
                //MessageBox.Show("Serverot e aktiviran");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            while (true)
            {
                client = server.AcceptTcpClient();
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                int bytesRead = stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                string msg = Encoding.ASCII.GetString(receivedBuffer, 0, bytesRead);

                if (msg == "GetDataFromSQLite")
                {
                    // Handle client's request to fetch data from SQLite database
                    HandleDataRequest(client);
                }
                else if (msg.StartsWith("SortByID:"))
                {
                    // Extract the value from the message sent by the client
                    string[] parts = msg.Split('|');
                    if (parts.Length == 2)
                    {
                        string textBox4Value = parts[1];
                        SearchID(client, textBox4Value);
                    }
                    
                }
               

                else
                {
                    // Split the received message into parts (Date, ID, >)
                    string[] messageParts = msg.Split('|'); // You can use any delimiter you want, like '|'

                    if (messageParts.Length == 7)
                    {
                        // Add the parts to the existing listView1 control
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add(messageParts[0]);
                        item.SubItems.Add(messageParts[1]);
                        item.SubItems.Add(messageParts[2]);
                        item.SubItems.Add(messageParts[3]);
                        item.SubItems.Add(messageParts[4]);
                        item.SubItems.Add(messageParts[5]);
                        item.SubItems.Add(messageParts[6]);

                        // Add the item to the existing listView1 control
                        listView1.Invoke((MethodInvoker)(() => listView1.Items.Add(item))); // Invoke UI update

                        // Update the SQLite database
                        UpdateSQLiteDatabase(messageParts[0], messageParts[1], messageParts[2], messageParts[3], messageParts[4], messageParts[5], messageParts[6]);
                    }
                }

                // Close the client connection after processing the message
                client.Close();
            }
        }






        private void ThreadProc3(string connectionString, int port)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip, port);
            TcpClient client = default(TcpClient);

            try
            {
                server.Start();
                //MessageBox.Show("Serverot e aktiviran");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            while (true)
            {
                client = server.AcceptTcpClient();
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                int count = Array.IndexOf<byte>(receivedBuffer, 0, 0);

                string msg = Encoding.ASCII.GetString(receivedBuffer, 0, count);



                SQLiteConnection sqlite_conn;
                sqlite_conn = CreateConnection(connectionString);

                //CreateTable(sqlite_conn);

                List<string> razdeleno = new List<string>();

                razdeleno = ZboroviLista(msg);

                                    

                SQLiteCommand cmd;
                cmd = sqlite_conn.CreateCommand();

                
                    int rowNumber = int.Parse(razdeleno[0]);
                    textBox1.Text = rowNumber.ToString();

                    string date = razdeleno[1];
                    string id = razdeleno[2];
                    string faktura = razdeleno[3];
                    string paymentYN = razdeleno[4];
                    string assigned = razdeleno[5];
                    string status = razdeleno[6];
                    string note = razdeleno[7];

                    

                    try
                    {

                    cmd.CommandText = @"UPDATE Payments SET Date = @Date, ID = @ID, Faktura = @Faktura, PaymentYN = @PaymentYN, Assignet = @Assignet, Status = @Status, Note = @Note WHERE RowNumber = " + "'" + rowNumber + "'";
                        
                            //cmd.Parameters.AddWithValue("@RowNumber", rowNumber);
                            cmd.Parameters.AddWithValue("@Date", date);
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@Faktura", faktura);
                            cmd.Parameters.AddWithValue("@PaymentYN", paymentYN);
                            cmd.Parameters.AddWithValue("@Assignet", assigned);
                            cmd.Parameters.AddWithValue("@Status", status);
                            cmd.Parameters.AddWithValue("@Note", note);
                            //cmd.Parameters.AddWithValue("@RowNumber", rowNumber);
                            cmd.ExecuteNonQuery();
                        

                        MessageBox.Show("Data updated successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating SQLite database: " + ex.Message);
                    }
                    finally
                    {
                        sqlite_conn.Close();
                    };
                
                

                client.Close();
            }
        }

        




        private void CreateSQLiteDatabase()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(sqliteConnectionString))
                {
                    connection.Open();
                    string createTableQuery = "CREATE TABLE IF NOT EXISTS Payments (RowNumber INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, ID TEXT, Faktura TEXT, PaymentYN TEXT, Assignet TEXT, Status TEXT, Note TEXT);";
                    using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating SQLite database: " + ex.Message);
            }
        }

        private void UpdateSQLiteDatabase(string date, string id, string faktura, string pay, string assigned, string status, string note)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(sqliteConnectionString))
                {
                    connection.Open();
                    string insertDataQuery = "INSERT INTO Payments (RowNumber, Date, ID, Faktura, PaymentYN, Assignet, Status, Note) VALUES (NULL, @Date, @ID, @Faktura, @PaymentYN, @Assignet, @Status, @Note);";
                    using (SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@Faktura", faktura);
                        command.Parameters.AddWithValue("@PaymentYN", pay);
                        command.Parameters.AddWithValue("@Assignet", assigned);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@Note", note);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating SQLite database: " + ex.Message);
            }
        }

        private void HandleDataRequest(TcpClient client)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(sqliteConnectionString))
                {
                    connection.Open();
                    string selectDataQuery = "SELECT RowNumber, Date, ID, Faktura, PaymentYN, Assignet, Status, Note FROM Payments;";
                    using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            StringBuilder responseBuilder = new StringBuilder();

                            // Loop through the database results and append them to the response string
                            while (reader.Read())
                            {
                                string date = reader["Date"].ToString();
                                string id = reader["ID"].ToString();
                                string faktura = reader["Faktura"].ToString();
                                string pay = reader["PaymentYN"].ToString();
                                string assigned = reader["Assignet"].ToString();
                                string status = reader["Status"].ToString();
                                string note = reader["Note"].ToString();

                                // Append the data items with '|' delimiter to create a row
                                string rowData = $"{date}|{id}|{faktura}|{pay}|{assigned}|{status}|{note}";

                                // Append the row data to the response with a newline delimiter
                                responseBuilder.AppendLine(rowData);
                            }

                            // Send the response back to the client
                            string responseData = responseBuilder.ToString();
                            byte[] responseBytes = Encoding.ASCII.GetBytes(responseData);
                            NetworkStream stream = client.GetStream();
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }


        private void SearchID(TcpClient client, string textBox4Value)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(sqliteConnectionString))
                {
                    connection.Open();
                    string selectDataQuery = "SELECT RowNumber, Date, ID, Faktura,  , Assignet, Status, Note FROM Payments WHERE ID = @textBox4Value ORDER BY ID;";
                    using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection))
                    {
                        // Add a parameter for textBox4Value
                        command.Parameters.AddWithValue("@textBox4Value", textBox4Value);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            StringBuilder responseBuilder = new StringBuilder();

                            // Loop through the sorted database results and append them to the response string
                            while (reader.Read())
                            {
                                string date = reader["Date"].ToString();
                                string id = reader["ID"].ToString();
                                string faktura = reader["Faktura"].ToString();
                                string pay = reader["PaymentYN"].ToString();
                                string assigned = reader["Assignet"].ToString();
                                string status = reader["Status"].ToString();
                                string note = reader["Note"].ToString();

                                // Append the data items with '|' delimiter to create a row
                                string rowData = $"{date}|{id}|{faktura}|{pay}|{assigned}|{status}|{note}";

                                // Append the row data to the response with a newline delimiter
                                responseBuilder.AppendLine(rowData);
                            }

                            // Send the response back to the client
                            string responseData = responseBuilder.ToString();
                            byte[] responseBytes = Encoding.ASCII.GetBytes(responseData);
                            NetworkStream stream = client.GetStream();
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        public static void CreateTable(SQLiteConnection conn, string tableName, List<string> columns)
        {
            SQLiteCommand sqlite_cmd;

            // Check if the table already exists in the database
            string doesTableExistQuery = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{tableName}'";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = doesTableExistQuery;
            var exists = sqlite_cmd.ExecuteReader();

            if (!exists.HasRows)
            {
                // If the table doesn't exist, create it with an auto-increment primary key
                string createTable = $"CREATE TABLE {tableName} ({string.Join(", ", columns)})";
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = createTable;
                sqlite_cmd.ExecuteNonQuery();
            }
        }


        public void ThreadProc5(string connectionString, int port)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip, port);
            TcpClient client = default(TcpClient);

            try
            {
                server.Start();
                // MessageBox.Show("Serverot e aktiviran");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            while (true)
            {
                client = server.AcceptTcpClient();
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                int count = Array.IndexOf<byte>(receivedBuffer, 0, 0);

                string msg = Encoding.ASCII.GetString(receivedBuffer, 0, count);

                SQLiteConnection sqlite_conn;
                sqlite_conn = CreateConnection(connectionString);

                // Define columns for the Onboarding table
                List<string> onboardingTableColumns = new List<string>
        {
            "ID INTEGER PRIMARY KEY AUTOINCREMENT",
            "Country TEXT",
            "Commision TEXT",
            "AceptingCompany TEXT",
            "AceptingProduct TEXT"
        };

                // Create the Onboarding table
                //CreateTable(sqlite_conn, "Onboarding", onboardingTableColumns);

                List<string> razdeleno = new List<string>();

                razdeleno = ZboroviLista(msg);

                SQLiteCommand cmd;
                cmd = sqlite_conn.CreateCommand();

                // Check if the request is to insert data into the Onboarding table
                if (razdeleno.Count == 4)
                {
                    cmd.CommandText = @"INSERT INTO Onboarding(Country, Commision, AceptingCompany, AceptingProduct) VALUES (@Country, @Commision, @AceptingCompany, @AceptingProduct)";

                    cmd.Parameters.AddWithValue("@Country", razdeleno[0]);
                    cmd.Parameters.AddWithValue("@Commision", razdeleno[1]);
                    cmd.Parameters.AddWithValue("@AceptingCompany", razdeleno[2]);
                    cmd.Parameters.AddWithValue("@AceptingProduct", razdeleno[3]);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data imported successfully.");
                }
                else if (msg == "GetOnboardingDataFromSQLite")
                {
                    // Handle the request to fetch data from the Onboarding table
                    HandleOnboardingDataRequest(client, sqlite_conn);
                }

                sqlite_conn.Close();

                client.Close();
            }
        }

        private void HandleOnboardingDataRequest(TcpClient client, SQLiteConnection connection)
        {
            try
            {
                string selectDataQuery = "SELECT * FROM Onboarding;";
                using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder responseBuilder = new StringBuilder();

                        // Loop through the database results and append them to the response string
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                responseBuilder.Append(reader[i].ToString());
                                if (i < reader.FieldCount - 1)
                                {
                                    responseBuilder.Append("|");
                                }
                            }
                            responseBuilder.AppendLine();
                        }

                        // Send the response back to the client
                        string responseData = responseBuilder.ToString();
                        byte[] responseBytes = Encoding.ASCII.GetBytes(responseData);
                        NetworkStream stream = client.GetStream();
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error handling Onboarding data request: " + ex.Message);
            }
        }

        private void ThreadProc6(string connectionString, int port)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip, port);
            TcpClient client = default(TcpClient);

            try
            {
                server.Start();
                //MessageBox.Show("Serverot e aktiviran");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            while (true)
            {
                client = server.AcceptTcpClient();
                byte[] receivedBuffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                int count = Array.IndexOf<byte>(receivedBuffer, 0, 0);

                string msg = Encoding.ASCII.GetString(receivedBuffer, 0, count);



                SQLiteConnection sqlite_conn;
                sqlite_conn = CreateConnection(connectionString);

                //CreateTable(sqlite_conn);

                List<string> razdeleno = new List<string>();

                razdeleno = ZboroviLista(msg);



                SQLiteCommand cmd;
                cmd = sqlite_conn.CreateCommand();


                int rowNumber = int.Parse(razdeleno[0]);
                textBox2.Text = rowNumber.ToString();

                string country = razdeleno[1];
                string commision = razdeleno[2];
                string aceptingCompany = razdeleno[3];
                string aceptingProduct = razdeleno[4];
                

                //Country, Commision, AceptingCompany, AceptingProduct



                try
                {

                    cmd.CommandText = @"UPDATE Onboarding SET Country = @Country, Commision = @Commision, AceptingCompany = @AceptingCompany, AceptingProduct = @AceptingProduct WHERE ID = " + "'" + rowNumber + "'";

                    //cmd.Parameters.AddWithValue("@RowNumber", rowNumber);
                    cmd.Parameters.AddWithValue("@Country", country);
                    cmd.Parameters.AddWithValue("@Commision", commision);
                    cmd.Parameters.AddWithValue("@AceptingCompany", aceptingCompany);
                    cmd.Parameters.AddWithValue("@AceptingProduct", aceptingProduct);
                    
                    cmd.ExecuteNonQuery();


                    MessageBox.Show("Data updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating SQLite database: " + ex.Message);
                }
                finally
                {
                    sqlite_conn.Close();
                };



                client.Close();
            }
        }


        public void ThreadProc7(string connectionString, int port)
        {
            IPAddress ip = Dns.GetHostEntry("localhost").AddressList[0];
            TcpListener server = new TcpListener(ip, port);
            TcpClient client = default(TcpClient);

            try
            {
                server.Start();
                // MessageBox.Show("Serverot e aktiviran");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return; // Exit the thread in case of an exception during server startup
            }

            while (true)
            {
                try
                {
                    client = server.AcceptTcpClient();
                    byte[] receivedBuffer = new byte[1024];
                    NetworkStream stream = client.GetStream();
                    stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                    int count = Array.IndexOf<byte>(receivedBuffer, 0, 0);

                    string msg = Encoding.ASCII.GetString(receivedBuffer, 0, count);

                    if (msg == "GetData")
                    {
                        // Handle data request and send the response to the client
                        HandleUsersDataRequest(client, connectionString);
                    }
                    else
                    {
                        // Process the received data as before
                        SQLiteConnection sqlite_conn = CreateConnection(connectionString);

                        List<string> razdeleno = ZboroviLista(msg);

                        using (SQLiteCommand cmd = sqlite_conn.CreateCommand())
                        {
                            cmd.CommandText = @"INSERT INTO Final(Usename, Password, Type) VALUES (@Usename, @Password, @Type)";
                            cmd.Parameters.AddWithValue("@Usename", razdeleno[0]);
                            cmd.Parameters.AddWithValue("@Password", razdeleno[1]);
                            cmd.Parameters.AddWithValue("@Type", int.Parse(razdeleno[2]));

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data imported successfully.");
                        }

                        sqlite_conn.Close();

                        TransferData();
                        MessageBox.Show("data sent");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.ToString()}");
                    // You might want to log the exception or handle it appropriately
                }
            }
        }

        private void HandleUsersDataRequest(TcpClient client, string connectionString)
        {
            try
            {
                using (SQLiteConnection sqlite_conn = CreateConnection(connectionString))
                {
                    string selectDataQuery = "SELECT * FROM Final;";
                    using (SQLiteCommand command = new SQLiteCommand(selectDataQuery, sqlite_conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            StringBuilder responseBuilder = new StringBuilder();

                            // Loop through the database results and append them to the response string
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    responseBuilder.Append(reader[i].ToString());
                                    if (i < reader.FieldCount - 1)
                                    {
                                        responseBuilder.Append("|");
                                    }
                                }
                                responseBuilder.AppendLine();
                            }

                            // Send the response back to the client
                            string responseData = responseBuilder.ToString();
                            byte[] responseBytes = Encoding.ASCII.GetBytes(responseData);
                            NetworkStream stream = client.GetStream();
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error handling data request: " + ex.Message);
            }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

       
    }
    
}

