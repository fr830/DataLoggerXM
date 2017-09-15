using DataLogger.Data;
using DataLogger.Entities;
using Microsoft.Win32;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;


namespace WinformProtocol
{
    public partial class Form1 : Form
    {
        TcpListener tcpListener = null;
        IPAddress localAddr;
        Int32 port;
        public static bool isStop;
        private bool allowshowdisplay = false;
        // The path to the key where Windows looks for startup applications
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        public Form1()
        {
            InitializeComponent();
            notifyIcon.Visible = true;
            this.allowshowdisplay = true;
            this.Show();
            // Check to see the current state (running at startup or not)
            if (rkApp.GetValue("Protocol") == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                chkRun.Checked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                chkRun.Checked = true;
            }
            this.Hide();
            this.allowshowdisplay = false;
        }
        public string TextLog
        {
            get { return textLog.Text; }
            set { textLog.Text = value; }
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.allowshowdisplay = false;
                this.Hide();
                notifyIcon.Visible = true;
            }
            else this.Show();
        }
        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay);
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.allowshowdisplay = true;
            notifyIcon.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            this.allowshowdisplay = true;
            notifyIcon.Visible = false;
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //station existedStationsSetting = new station_repository().get_info();
            port = 3001;
            textPort.Text = "3001";
            //textPort.Enabled = false;
            localAddr = IPAddress.Parse(Protocol.MyTcpListener.GetLocalIPAddress());
            textIP.Text = localAddr.ToString();
            btnListen.PerformClick();

            this.WindowState = FormWindowState.Normal;
            this.Hide();
        }

        async void btnListen_Click(object sender, EventArgs e)
        {
            textIP.Enabled = false;
            textPort.Enabled = false;
            btnListen.Enabled = false;
            btnRefesh.Enabled = false;
            //textLog.Enabled = false;
            Control control = new Control(this);
            //tcpListener = new TcpListener(localAddr, port);
            //tcpListener.Start();
            //Protocol.MyTcpListener.counter = 0;
            //try
            //{
            //    String data = null;
            //    TcpClient client = new TcpClient(AddressFamily.InterNetworkV6);
            //    client.Client.DualMode = true;
            //    textLog.Text = textLog.Text + "\n" + " >>> " + "Server Started";
            //    Protocol.MyTcpListener.counter = 0;
            //    await Task.Run(() =>
            //    {
            //        while (true)
            //        {
            //            Protocol.MyTcpListener.counter += 1;
            //            textLog.Text = textLog.Text + "\n" + " >> " + " While(true) ";
            //            client = tcpListener.AcceptTcpClient();
            //            textLog.Text = textLog.Text + "\n" + " >> " + client.Connected;
            //            textLog.Text = textLog.Text + "\n" + " >> " + "Client IP:" + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + " started!";
            //            Protocol.MyTcpListener.newThread = new Thread(() => Protocol.MyTcpListener.doChat(client, data, Protocol.MyTcpListener._encoder, Protocol.MyTcpListener.serialPortTN, Protocol.MyTcpListener.serialPortTP, Protocol.MyTcpListener.serialPortTOC, Protocol.MyTcpListener.serialPortSAMP, Protocol.MyTcpListener.objRelayIOControlGlobal, Protocol.MyTcpListener.objStationStatusGlobal, Protocol.MyTcpListener.objWaterSamplerGLobal, Protocol.MyTcpListener.objMeasuredDataGlobal));
            //            Protocol.MyTcpListener.newThread.Start();
            //            Thread.Sleep(1000);
            //        }
            //        tcpListener.Stop();

            //    });
            //}
            //catch (SocketException ex)
            //{
            //    Console.WriteLine(ex.StackTrace);
            //}
            //finally
            //{
            //    tcpListener.Stop();
            //}
            //textLog.Text = textLog.Text + "\n" + "End";
            control.StartListening(port,localAddr);
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Control control = new Control(this);
            TcpListener _listener;
            //try
            //{              
                _listener = control._listener;
                //_listener.Stop();
            //}
            //catch (Exception ex)
            //{
            //}
            isStop = true;
            //Environment.Exit(Environment.ExitCode);
            if (_listener != null)
            {
                _listener.Stop();
                Control.ContinueReclaim = false;
                Control.ThreadReclaim.Join();
                foreach (Object Client in Control.ClientSockets)
                {
                    ((ClientHandler)Client).Stop();
                }
            }
        }

        private void chkRun_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRun.Checked)
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("Protocol", Application.ExecutablePath.ToString());
    }
            else
            {
                // Remove the value from the registry so that the application doesn't start
               rkApp.DeleteValue("Protocol", false);
}
        }
    }
    public class Control
    {
        //static Form1 form1 = new Form1();
        public Form1 form1;
        public static ASCIIEncoding _encoder = new ASCIIEncoding();
        public const int BUFFER_SIZE = 1024;
        public static ArrayList ClientSockets;
        public static bool ContinueReclaim = true;
        public static Thread ThreadReclaim;
        public static int counter = 0;
        public static Thread newThread = null;
        public static TcpListener listener;

        //public static TcpListener tcpListener = null;
        public static System.IO.Ports.SerialPort serialPortTN;
        public static System.IO.Ports.SerialPort serialPortTOC;
        public static System.IO.Ports.SerialPort serialPortTP;
        public static System.IO.Ports.SerialPort serialPortSAMP;

        public static relay_io_control objRelayIOControlGlobal = new relay_io_control();
        public static station_status objStationStatusGlobal = new station_status();
        public static water_sampler objWaterSamplerGLobal = new water_sampler();
        public static measured_data objMeasuredDataGlobal = new measured_data();

        internal delegate void StringDelegate(string data, Form1 form);
        public Control()
        {
        }
        public Control(Form1 form1)
        {
            //Set the text of the textbox in the form1
            this.form1 = form1;
        }
        public TcpListener _listener
        {
            get { return listener; }
            set { listener = value; }
        }
        public static string CalculateChecksum(string dataToCalculate)
        {
            byte[] byteToCalculate = Encoding.ASCII.GetBytes(dataToCalculate);

            int checksum = 0;

            foreach (byte chData in byteToCalculate)
            {
                checksum += chData;
            }
            checksum &= 0xff;
            string hexString = String.Format("\\" + "x{0:X2}", checksum);
            return checksum.ToString("X2");
        }
        //public void setText(string text)
        //{
        //    if (form1.textLog.InvokeRequired)
        //    {
        //        StringDelegate d = new StringDelegate(setText);
        //        form1.textLog.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        form1.textLog.Text = text;
        //    }
        //}
        public void ClearTextBox(string value, Form1 form)
        {
            //if (form1.textLog.InvokeRequired)
            //{
            //    form1.Invoke(new Action<string>(AppendTextBox), new object[] { value });
            //    return;
            //}
            //form1.textLog.Text += value;

            //form1.textLog.Invoke((MethodInvoker)delegate
            //                    {
            //                        form1.textLog.Text += String.Format("{0} : {1}{2}",
            //                                            DateTime.Now,
            //                                            value,
            //                                            Environment.NewLine);
            //                    });

            if (form.textLog.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(ClearTextBox);
                form.textLog.Invoke(d, new object[] { "", form });
            }
            else
            {
                form.TextLog = "";
                form.textLog.SelectionStart = form.TextLog.Length;
                form.textLog.ScrollToCaret();
            }
        }
        public void AppendTextBox(string value, Form1 form)
        {
            //if (form1.textLog.InvokeRequired)
            //{
            //    form1.Invoke(new Action<string>(AppendTextBox), new object[] { value });
            //    return;
            //}
            //form1.textLog.Text += value;

            //form1.textLog.Invoke((MethodInvoker)delegate
            //                    {
            //                        form1.textLog.Text += String.Format("{0} : {1}{2}",
            //                                            DateTime.Now,
            //                                            value,
            //                                            Environment.NewLine);
            //                    });

            if (form.textLog.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(AppendTextBox);
                form.textLog.Invoke(d, new object[] { value, form });
            }
            else
            {
                form.TextLog = form.TextLog + value;
                form.textLog.SelectionStart = form.TextLog.Length;
                form.textLog.ScrollToCaret();
            }
        }
        public void sendMsg(NetworkStream nwStream, String msg)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(msg);
            String data = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            try
            {
                nwStream.Write(buffer, 0, buffer.Length);
                nwStream.Flush();
                form1.textLog.Text = form1.textLog.Text + Environment.NewLine + "Sended : " + msg;
            }
            catch
            {
                form1.textLog.Text = form1.textLog.Text + Environment.NewLine + "SENDING: " + "\"" + msg + "\"" + " BUT";
                form1.textLog.Text = form1.textLog.Text + Environment.NewLine + "ERROR : CAN NOT LISTEN ANY CONNECT, CHECK CONNECT IN CENTER.";
            }
        }
        public void sendByte(NetworkStream nwStream, byte[] msg, Form1 form)
        {
            //try
            //{
            //    nwStream.Write(msg, 0, msg.Length);
            //    nwStream.Flush();
            //    AppendTextBox(Environment.NewLine + " Sended : " + _encoder.GetString(msg), form);
            //}
            //catch
            //{
            //    AppendTextBox(Environment.NewLine + "SENDING: " + "\"" + _encoder.GetString(msg) + "\"" + " BUT", form);
            //    AppendTextBox(Environment.NewLine + "ERROR : CAN NOT LISTEN ANY CONNECT, CHECK CONNECT IN CENTER.", form);
            //}
            var new_msg = msg.TakeWhile((v, index) => msg.Skip(index).Any(w => w != 0x00)).ToArray();
            try
            {
                nwStream.Write(msg, 0, msg.Length);
                nwStream.Flush();
                AppendTextBox(Environment.NewLine + " Sended : " + _encoder.GetString(new_msg).Replace("\0", " "), form);
            }
            catch
            {
                AppendTextBox(Environment.NewLine + "SENDING: " + "\"" + _encoder.GetString(new_msg).Replace("\0", " ") + "\"" + " BUT", form);
                AppendTextBox(Environment.NewLine + "ERROR : CAN NOT LISTEN ANY CONNECT, CHECK CONNECT IN CENTER.", form);
            }
        }
        public string Encrypt(string data, string _publicKey, RSACryptoServiceProvider rsa)
        {
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            String encrypt = _encoder.GetString(encryptedByteArray);
            AppendTextBox(Environment.NewLine + "ENCRYPT : " + encrypt, form1);
            var length = encryptedByteArray.Count();
            var item = 0;
            var sb = new StringBuilder();
            foreach (var x in encryptedByteArray)
            {
                item++;
                sb.Append(x);
                if (item < length)
                    sb.Append(",");
            }
            return sb.ToString();
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }
        public static Boolean Timeout(TcpClient client)
        {
            if (client.ReceiveTimeout <= 60000) { return true; }
            else return false;

        }
        public static void requestInforSAMPLER(SerialPort com)
        {
            try
            {
                if (com.IsOpen)
                {
                    byte[] packet = new byte[11];
                    //Fill to packet
                    packet[0] = 0x02; // STX

                    packet[1] = 0x53; // 'S'
                    packet[2] = 0x41; // 'A'
                    packet[3] = 0x4D; // 'M'
                    packet[4] = 0x50; // 'P'
                    packet[5] = 0x31; // 1
                    packet[6] = 0x30; // 0
                    packet[7] = 0x03; // ETX
                    packet[8] = 0x39;
                    packet[9] = 0x37;
                    packet[10] = 0x0D;//
                    com.Write(packet, 0, 11);
                }
            }
            catch
            {

            }
        }
        public static void requestAutoSAMPLER(SerialPort com)
        {
            try
            {
                if (com.IsOpen)
                {
                    byte[] packet = new byte[11];
                    //Fill to packet
                    packet[0] = 0x02; // STX

                    packet[1] = 0x53; // 'S'
                    packet[2] = 0x41; // 'A'
                    packet[3] = 0x4D; // 'M'
                    packet[4] = 0x50; // 'P'
                    packet[5] = 0x30; // 0
                    packet[6] = 0x30; // 0
                    packet[7] = 0x03; // ETX
                    packet[8] = 0x39; // '9'
                    packet[9] = 0x36; // '6'
                    packet[10] = 0x0D;//
                    com.Write(packet, 0, 11);
                }
            }
            catch
            {

            }
        }
        public static string ConvertStr(String str, int i)
        {
            int j;
            //if (str.Length < i)
            //{
            //    j = i - str.Length;
            //    String pad = new String('*', j);
            //    str = pad + str;
            //}
            if (str.Length > i)
            {
                if (str.Contains("."))
                {
                    j = str.Length - i;
                    str = str.Substring(0, 9);
                }
            }
            return str;
        }
        public static String DateFormat(String date)
        {
            //yyyymmddHHmmss
            DateTime dateTime = DateTime.Parse(date);
            date = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
            date = date.Substring(6, 4) + date.Substring(0, 2) + date.Substring(3, 2) + date.Substring(11, 2) + date.Substring(14, 2) + date.Substring(17, 2);
            return date;
        }
        public static String _DateDeFormat(string date)
        {
            //date = date.Substring(0, 2) + "/" + date.Substring(2, 2) + "/" + date.Substring(4, 4) + " " + date.Substring(8, 2) + ":" + date.Substring(10, 2) + ":" + date.Substring(12, 2);
            //Console.WriteLine(date);
            //date = date.Substring(4, 4) + "-" + date.Substring(0, 2) + "-" + date.Substring(2, 2) + " " + date.Substring(8, 2) + ":" + date.Substring(10, 2) + ":" + date.Substring(12, 2);
            date = date.Substring(0, 4) + "-" + date.Substring(4, 2) + "-" + date.Substring(6, 2) + " " + date.Substring(8, 2) + ":" + date.Substring(10, 2) + ":" + date.Substring(12, 2);
            //DateTime dateValue = DateTime.ParseExact(date, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
            //date = date.Substring(0, 2) + date.Substring(3, 2) + date.Substring(6, 4) + date.Substring(11, 2) + date.Substring(14, 2) + date.Substring(17, 2);
            return date;
        }
        public static byte[] mergeArrayByte(byte[] output, byte[] subarray)
        {
            int i;
            for (i = 0; i < subarray.Length; i++)
            {
                output[i] = subarray[i];
            }
            return output;
        }
        public static String getMeasureTime(String table, String time)
        {
            String measuretime;
            try
            {
                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                //conn.Open();
                using (NpgsqlDBConnection db = new NpgsqlDBConnection())
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT " + time + " from " + table + " order by ID desc limit 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable data = new DataTable();
                            data.Load(dr); // Load 
                            string strvalue = "";
                            foreach (DataRow row in data.Rows)
                            {
                                strvalue = Convert.ToString(row["created"]);
                            }
                            strvalue = DateFormat(strvalue);
                            db.close_connection();
                            return strvalue;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return "ERROR";
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                return "ERROR";
            }
        }
        public static List<byte[]> DataDUMP(String date1, String date2, String table, String tablebinding)
        {
            DateTime dateValue;
            List<byte[]> lstData = new List<byte[]>();
            try
            {
                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                //conn.Open();
                using (NpgsqlDBConnection db = new NpgsqlDBConnection())
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * from " + table + " WHERE created < " + "\'" + date2 + "\'" + " AND created > " + "\'" + date1 + "\'" + "ORDER BY created ASC";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable data = new DataTable();
                            data.Load(dr); // Load data phu hop trong command DUMP
                            string sql_command1 = "SELECT * from " + tablebinding;
                            cmd.CommandText = sql_command1;
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            byte[] databyte = new byte[BUFFER_SIZE];
                            tbcode.Load(dr); // Load bang chua mapping cac truong
                            string strvalue = "";
                            byte[] clnnamevalue;
                            byte[] clnnamestatus;
                            byte[] code;
                            byte[] measuretime;
                            string[] strvalues = new string[data.Rows.Count];

                            byte[] countitem1 = new byte[2];
                            _encoder.GetBytes(ConvertStr(tbcode.Rows.Count.ToString(), 2)).CopyTo(countitem1, 0);

                            byte[] sql = null;

                            foreach (DataRow row1 in data.Rows)  // lay moi row trong data phu hop voi DUMP command
                            {
                                sql = null;
                                clnnamevalue = new byte[10];
                                clnnamestatus = new byte[2];
                                code = new byte[5];

                                //Console.WriteLine("\n ID \n" + Convert.ToString(row1["id"]));
                                byte[] _measuretime = _encoder.GetBytes(DateFormat(Convert.ToString(row1["created"])));
                                measuretime = new byte[14];
                                _measuretime.CopyTo(measuretime, 0);


                                if (sql == null)
                                {
                                    sql = measuretime.Concat(countitem1).ToArray();  //Measure time + count item
                                }
                                else
                                {
                                    sql = sql.Concat(measuretime).Concat(countitem1).ToArray();
                                }
                                //strvalue = strvalue + DateFormat(Convert.ToString(row1["created"])) + tbcode.Rows.Count.ToString() + "\\";
                                foreach (DataRow row2 in tbcode.Rows)
                                {

                                    byte[] _code = _encoder.GetBytes(Convert.ToString(row2["code"]));
                                    byte[] _clnnamevalue = _encoder.GetBytes(ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamevalue"])]), 10));
                                    byte[] _clnnamestatus = _encoder.GetBytes(ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamestatus"])]), 2));
                                    //strvalue = strvalue + Convert.ToString(row2["code"]);
                                    code = new byte[5];
                                    _code.CopyTo(code, 0);
                                    //strvalue = strvalue + ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamevalue"])]), 10);
                                    clnnamevalue = new byte[10];
                                    _clnnamevalue.CopyTo(clnnamevalue, 10 - _clnnamevalue.Length);
                                    //strvalue = strvalue + ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamestatus"])]), 2);
                                    clnnamestatus = new byte[2];
                                    _clnnamestatus.CopyTo(clnnamestatus, 2 - _clnnamestatus.Length);

                                    sql = sql.Concat(code).Concat(clnnamevalue).Concat(clnnamestatus).ToArray();

                                }
                                lstData.Add(sql);
                            }

                            db.close_connection();

                            return lstData;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        byte[] rt = _encoder.GetBytes("ERROR");
                        lstData.Add(rt);
                        return lstData;
                    }
                }
            }
            catch (Exception ex)
            {
                string[] Error = new String[1] { "error" };
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                byte[] rt = _encoder.GetBytes("ERROR");
                lstData.Add(rt);
                return lstData;
            }
        }
        public static byte[] DataSQL(String table, String tablebinding)
        {
            try
            {
                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                //conn.Open();
                using (NpgsqlDBConnection db = new NpgsqlDBConnection())
                {
                    if (db.open_connection())
                    {
                        string sql_command = "SELECT * from " + table + " order by ID desc limit 1";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable data = new DataTable();
                            byte[] databyte = new byte[BUFFER_SIZE];
                            // call load method of datatable to copy content of reader 
                            data.Load(dr); // Load 

                            string sql_command1 = "SELECT * from " + tablebinding;
                            cmd.CommandText = sql_command1;
                            //cmd = new NpgsqlCommand("SELECT * from " + tablebinding, conn);
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr);
                            string strvalue = "";
                            byte[] countitem = new byte[2];
                            countitem = _encoder.GetBytes(ConvertStr(tbcode.Rows.Count.ToString(), 2));
                            //byte[] sql = new byte[(5 + 10 + 2) * tbcode.Rows.Count + 2];
                            byte[] sql = countitem;
                            //sql = sql.Concat(countitem).ToArray();
                            byte[] clnnamevalue;
                            byte[] clnnamestatus;
                            byte[] code;
                            foreach (DataRow row in tbcode.Rows)
                            {
                                clnnamevalue = new byte[10];
                                clnnamestatus = new byte[2];
                                code = new byte[5];
                                byte[] _clnnamevalue = _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamevalue"])]), 10));
                                byte[] _clnnamestatus = _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamestatus"])]), 2));
                                byte[] _code = _encoder.GetBytes(Convert.ToString(row["code"]));
                                //strvalue = strvalue + Convert.ToString(row["code"]);
                                code = new byte[5];
                                _code.CopyTo(code, 0);
                                clnnamevalue = new byte[10];
                                _clnnamevalue.CopyTo(clnnamevalue, 10 - _clnnamevalue.Length);
                                clnnamestatus = new byte[2];
                                _clnnamestatus.CopyTo(clnnamestatus, 2 - _clnnamestatus.Length);
                                sql = sql.Concat(code).Concat(clnnamevalue).Concat(clnnamestatus).ToArray();
                            }
                            db.close_connection();
                            return sql;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        byte[] rt = _encoder.GetBytes("ERROR");
                        return rt;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
                byte[] rt = _encoder.GetBytes("ERROR");
                return rt;
            }
        }
        public static void UpdateData(String username, String newpass)
        {
            try
            {
                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
                //conn.Open();
                using (NpgsqlDBConnection db = new NpgsqlDBConnection())
                {
                    if (db.open_connection())
                    {
                        newpass = Crypto.HashPassword(newpass);
                        string sql_command = "UPDATE auth SET password = " + "\'" + newpass + "\'" + " WHERE user_name = " + "\'" + username + "\'";
                        //NpgsqlCommand cmd = new NpgsqlCommand("UPDATE auth SET password = " + "\'" + newpass + "\'" + " WHERE user_name = " + "\'" + username + "\'", conn);
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command;
                            cmd.ExecuteNonQuery();
                            db.close_connection();
                        }
                    }
                    else
                    {
                        db.close_connection();
                        //return "ERROR";
                    }
                }
            }
            catch (Exception ex)
            {
                string[] Error = new String[1] { "error" };
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
            }
        }
        #region command
        public void DATA(byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client)
        {

            if (_encoder.GetString(SubArray(buffer, j + 19, 1)).Equals("M"))
            {
                byte[] _STX = new byte[1];
                (new byte[] { 0x02 }).CopyTo(_STX, 0);
                string table = "data_5minute_values";
                string col = "created";
                String measuretime = getMeasureTime(table, col);
                byte[] _measuretime = new byte[14];
                _encoder.GetBytes(measuretime).CopyTo(_measuretime, 0);
                byte[] _Command = new byte[4];
                _encoder.GetBytes("DUMP").CopyTo(_Command, 0);
                byte[] _Param = new byte[1];
                String _param = "M";
                _encoder.GetBytes(_param).CopyTo(_Param, 0);
                byte[] _dcd = new byte[1];
                _encoder.GetBytes("1").CopyTo(_dcd, 0);
                byte[] _streamCode = new byte[11];
                _encoder.GetBytes(data.Substring(1, j)).CopyTo(_streamCode, 0);
                byte[] _header = _STX.Concat(_Command).Concat(_Param).Concat(_streamCode).Concat(_dcd).Concat(_measuretime).ToArray();
                byte[] _item = DataSQL("data_5minute_values", "databinding");
                byte[] _EOT = new byte[1];
                (new byte[] { 0x04 }).CopyTo(_EOT, 0);
                byte[] _ETX = new byte[1];
                (new byte[] { 0x03 }).CopyTo(_ETX, 0);
                String checksum = CalculateChecksum(_encoder.GetString(_header) + _encoder.GetString(_item) + _encoder.GetString(_ETX));
                byte[] _checksum = new byte[2];
                _encoder.GetBytes(checksum).CopyTo(_checksum, 0);
                byte[] _CR = new byte[1];
                (new byte[] { 0x0D }).CopyTo(_CR, 0);
                byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
                byte[] _sender = _header.Concat(_item).Concat(_tailer).ToArray();
                sendByte(nwStream, _sender, form1);
                buffer = new byte[BUFFER_SIZE];
                if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
                {
                    if (buffer[0] == 15 && Timeout(client))
                    {
                        sendByte(nwStream, _sender, form1);
                    }
                    else
                        if (buffer[0] == 6 && Timeout(client))
                    {
                        sendByte(nwStream, _EOT, form1);
                    }
                    else
                    {
                        sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
                    }
                }
                else
                {
                    sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
                }
            }
            else
                if (_encoder.GetString(SubArray(buffer, j + 19, 1)).Equals("H"))
            {
                byte[] _STX = new byte[1];
                (new byte[] { 0x02 }).CopyTo(_STX, 0);
                string table = "data_60minute_values";
                string col = "created";
                String measuretime = getMeasureTime(table, col);
                byte[] _measuretime = new byte[14];
                _encoder.GetBytes(measuretime).CopyTo(_measuretime, 0);
                byte[] _Command = new byte[4];
                _encoder.GetBytes("DUMP").CopyTo(_Command, 0);
                byte[] _Param = new byte[1];
                String _param = "H";
                _encoder.GetBytes(_param).CopyTo(_Param, 0);
                byte[] _dcd = new byte[1];
                _encoder.GetBytes("1").CopyTo(_dcd, 0);
                byte[] _streamCode = new byte[11];
                _encoder.GetBytes(data.Substring(1, j)).CopyTo(_streamCode, 0);
                byte[] _header = _STX.Concat(_Command).Concat(_Param).Concat(_streamCode).Concat(_dcd).Concat(_measuretime).ToArray();
                byte[] _item = DataSQL("data_60minute_values", "databinding");
                byte[] _EOT = new byte[1];
                (new byte[] { 0x04 }).CopyTo(_EOT, 0);
                byte[] _ETX = new byte[1];
                (new byte[] { 0x03 }).CopyTo(_ETX, 0);
                String checksum = CalculateChecksum(_encoder.GetString(_header) + _encoder.GetString(_item) + _encoder.GetString(_ETX));
                byte[] _checksum = new byte[2];
                _encoder.GetBytes(checksum).CopyTo(_checksum, 0);
                //String tailer = ETX + checksum + "\x0D";
                byte[] _CR = new byte[1];
                (new byte[] { 0x0D }).CopyTo(_CR, 0);
                byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
                byte[] _sender = _header.Concat(_item).Concat(_tailer).ToArray();
                sendByte(nwStream, _sender, form1);
                buffer = new byte[BUFFER_SIZE];
                if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
                {
                    if (buffer[0] == 15 && Timeout(client))
                    {
                        sendByte(nwStream, _sender, form1);
                    }
                    else
                        if (buffer[0] == 6 && Timeout(client))
                    {
                        sendByte(nwStream, _EOT, form1);
                    }
                    else
                    {
                        sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
                    }
                }
                else
                {
                    sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
                }
            }
            else
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR : " + "\" " + "DATA " + "\" " + "COMMAND"), form1);
            }

        }
        public void RDAT(byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client, relay_io_control objRelayIOControlGlobal, station_status objStationStatusGlobal, water_sampler objWaterSamplerGLobal, measured_data objMeasuredDataGlobal)
        {
            byte[] _STX = new byte[1];
            (new byte[] { 0x02 }).CopyTo(_STX, 0);
            String date = DateFormat(DateTime.Now.ToString());
            byte[] _date = new byte[14];
            _encoder.GetBytes(date).CopyTo(_date, 0);
            byte[] _Command = new byte[4];
            _encoder.GetBytes("RDAT").CopyTo(_Command, 0);
            byte[] _streamCode = new byte[11];
            _encoder.GetBytes(data.Substring(1, j)).CopyTo(_streamCode, 0);
            byte[] _dcd = new byte[1];
            _encoder.GetBytes("1").CopyTo(_dcd, 0);
            byte[] _header = _STX.Concat(_Command).Concat(_streamCode).Concat(_dcd).Concat(_date).ToArray();
            // String item;
            byte[] _ETX = new byte[1];
            (new byte[] { 0x03 }).CopyTo(_ETX, 0);
            #region item
            String mps_ph_value = ConvertStr(objMeasuredDataGlobal.MPS_pH.ToString(), 10);
            String mps_ph_status = ConvertStr(objMeasuredDataGlobal.MPS_pH_status.ToString(), 2);
            byte[] _mps_ph_code = new byte[5];
            byte[] _mps_ph_value = new byte[10];
            byte[] _mps_ph_status = new byte[2];
            _encoder.GetBytes("mpsph").CopyTo(_mps_ph_code, 0);
            _encoder.GetBytes(mps_ph_value).CopyTo(_mps_ph_value, 0);
            _encoder.GetBytes(mps_ph_status).CopyTo(_mps_ph_status, 0);
            byte[] _mps_ph = _mps_ph_code.Concat(_mps_ph_value).Concat(_mps_ph_status).ToArray();

            String mps_ec_value = ConvertStr(objMeasuredDataGlobal.MPS_EC.ToString(), 10);
            String mps_ec_status = ConvertStr(objMeasuredDataGlobal.MPS_EC_status.ToString(), 2);
            byte[] _mps_ec_code = new byte[5];
            byte[] _mps_ec_value = new byte[10];
            byte[] _mps_ec_status = new byte[2];
            _encoder.GetBytes("mpsec").CopyTo(_mps_ec_code, 0);
            _encoder.GetBytes(mps_ec_value).CopyTo(_mps_ec_value, 0);
            _encoder.GetBytes(mps_ec_status).CopyTo(_mps_ec_status, 0);
            byte[] _mps_ec = _mps_ec_code.Concat(_mps_ec_value).Concat(_mps_ec_status).ToArray();

            String mps_do_value = ConvertStr(objMeasuredDataGlobal.MPS_DO.ToString(), 10);
            String mps_do_status = ConvertStr(objMeasuredDataGlobal.MPS_DO_status.ToString(), 2);
            byte[] _mps_do_code = new byte[5];
            byte[] _mps_do_value = new byte[10];
            byte[] _mps_do_status = new byte[2];
            _encoder.GetBytes("mpsdo").CopyTo(_mps_do_code, 0);
            _encoder.GetBytes(mps_do_value).CopyTo(_mps_do_value, 0);
            _encoder.GetBytes(mps_do_status).CopyTo(_mps_do_status, 0);
            byte[] _mps_do = _mps_do_code.Concat(_mps_do_value).Concat(_mps_do_status).ToArray();

            String mps_orp_value = ConvertStr(objMeasuredDataGlobal.MPS_ORP.ToString(), 10);
            String mps_orp_status = ConvertStr(objMeasuredDataGlobal.MPS_ORP_status.ToString(), 2);
            byte[] _mps_orp_code = new byte[5];
            byte[] _mps_orp_value = new byte[10];
            byte[] _mps_orp_status = new byte[2];
            _encoder.GetBytes("mporp").CopyTo(_mps_orp_code, 0);
            _encoder.GetBytes(mps_orp_value).CopyTo(_mps_orp_value, 0);
            _encoder.GetBytes(mps_orp_status).CopyTo(_mps_orp_status, 0);
            byte[] _mps_orp = _mps_orp_code.Concat(_mps_orp_value).Concat(_mps_orp_status).ToArray();

            String mps_temp_value = ConvertStr(objMeasuredDataGlobal.MPS_Temp.ToString(), 10);
            String mps_temp_status = ConvertStr(objMeasuredDataGlobal.MPS_Temp_status.ToString(), 2);
            byte[] _mps_temp_code = new byte[5];
            byte[] _mps_temp_value = new byte[10];
            byte[] _mps_temp_status = new byte[2];
            _encoder.GetBytes("mtemp").CopyTo(_mps_temp_code, 0);
            _encoder.GetBytes(mps_temp_value).CopyTo(_mps_temp_value, 0);
            _encoder.GetBytes(mps_temp_status).CopyTo(_mps_temp_status, 0);
            byte[] _mps_temp = _mps_temp_code.Concat(_mps_temp_value).Concat(_mps_temp_status).ToArray();

            String mps_turbi_value = ConvertStr(objMeasuredDataGlobal.MPS_Turbidity.ToString(), 10);
            String mps_turbi_status = ConvertStr(objMeasuredDataGlobal.MPS_Turbidity_status.ToString(), 10);
            byte[] _mps_turbi_code = new byte[5];
            byte[] _mps_turbi_value = new byte[10];
            byte[] _mps_turbi_status = new byte[2];
            _encoder.GetBytes("turbi").CopyTo(_mps_turbi_code, 0);
            _encoder.GetBytes(mps_turbi_value).CopyTo(_mps_turbi_value, 0);
            _encoder.GetBytes(mps_turbi_status).CopyTo(_mps_turbi_status, 0);
            byte[] _mps_turbi = _mps_turbi_code.Concat(_mps_turbi_value).Concat(_mps_turbi_status).ToArray();

            String cln_tn_value = ConvertStr(objMeasuredDataGlobal.TN.ToString(), 10);
            String cln_tn_status = ConvertStr(objMeasuredDataGlobal.TN_status.ToString(), 2);
            byte[] _cln_tn_code = new byte[5];
            byte[] _cln_tn_value = new byte[10];
            byte[] _cln_tn_status = new byte[2];
            _encoder.GetBytes("clntn").CopyTo(_cln_tn_code, 0);
            _encoder.GetBytes(cln_tn_value).CopyTo(_cln_tn_value, 0);
            _encoder.GetBytes(cln_tn_status).CopyTo(_cln_tn_status, 0);
            byte[] _cln_tn = _cln_tn_code.Concat(_cln_tn_value).Concat(_cln_tn_status).ToArray();

            String cln_toc_value = ConvertStr(objMeasuredDataGlobal.TOC.ToString(), 10);
            String cln_toc_status = ConvertStr(objMeasuredDataGlobal.TOC_status.ToString(), 2);
            byte[] _cln_toc_code = new byte[5];
            byte[] _cln_toc_value = new byte[10];
            byte[] _cln_toc_status = new byte[2];
            _encoder.GetBytes("cltoc").CopyTo(_cln_tn_code, 0);
            _encoder.GetBytes(cln_toc_value).CopyTo(_cln_tn_value, 0);
            _encoder.GetBytes(cln_toc_status).CopyTo(_cln_tn_status, 0);
            byte[] _cln_toc = _cln_toc_code.Concat(_cln_toc_value).Concat(_cln_toc_status).ToArray();

            String cln_tp_value = ConvertStr(objMeasuredDataGlobal.TP.ToString(), 10);
            String cln_tp_status = ConvertStr(objMeasuredDataGlobal.TP_status.ToString(), 2);
            byte[] _cln_tp_code = new byte[5];
            byte[] _cln_tp_value = new byte[10];
            byte[] _cln_tp_status = new byte[2];
            _encoder.GetBytes("clntp").CopyTo(_cln_tn_code, 0);
            _encoder.GetBytes(cln_tp_value).CopyTo(_cln_tn_value, 0);
            _encoder.GetBytes(cln_tp_status).CopyTo(_cln_tn_status, 0);
            byte[] _cln_tp = _cln_tp_code.Concat(_cln_tp_value).Concat(_cln_tp_status).ToArray();

            String equiment_staus = ConvertStr(objWaterSamplerGLobal.equipment_status.ToString(), 2);
            byte[] _equiment_staus = new byte[2];
            _equiment_staus = _encoder.GetBytes(equiment_staus);

            String module_Door_value = ConvertStr(objStationStatusGlobal.module_Door.ToString(), 10);
            byte[] _module_Door_code = new byte[5];
            byte[] _module_Door_value = new byte[10];
            _encoder.GetBytes("mdoor").CopyTo(_module_Door_code, 0);
            _encoder.GetBytes(module_Door_value).CopyTo(_module_Door_value, 0);
            byte[] _module_Door = _module_Door_code.Concat(_module_Door_value).Concat(_equiment_staus).ToArray();

            String module_Fire_value = ConvertStr(objStationStatusGlobal.module_Fire.ToString(), 10);
            byte[] _module_Fire_code = new byte[5];
            byte[] _module_Fire_value = new byte[10];
            _encoder.GetBytes("mfire").CopyTo(_module_Fire_code, 0);
            _encoder.GetBytes(module_Fire_value).CopyTo(_module_Fire_value, 0);
            byte[] _module_Fire = _module_Fire_code.Concat(_module_Fire_value).Concat(_equiment_staus).ToArray();

            String module_Power_value = ConvertStr(objStationStatusGlobal.module_Power.ToString(), 10);
            byte[] _module_Power_code = new byte[5];
            byte[] _module_Power_value = new byte[10];
            _encoder.GetBytes("power").CopyTo(_module_Power_code, 0);
            _encoder.GetBytes(module_Power_value).CopyTo(_module_Power_value, 0);
            byte[] _module_Power = _module_Power_code.Concat(_module_Power_value).Concat(_equiment_staus).ToArray();

            String module_UP_value = ConvertStr(objStationStatusGlobal.module_UPS.ToString(), 10);
            byte[] _module_UP_code = new byte[5];
            byte[] _module_UP_value = new byte[10];
            _encoder.GetBytes("mdups").CopyTo(_module_UP_code, 0);
            _encoder.GetBytes(module_UP_value).CopyTo(_module_UP_value, 0);
            byte[] _module_UP = _module_UP_code.Concat(_module_UP_value).Concat(_equiment_staus).ToArray();

            String module_Flow_value = ConvertStr(objStationStatusGlobal.module_Flow.ToString(), 10);
            byte[] _module_Flow_code = new byte[5];
            byte[] _module_Flow_value = new byte[10];
            _encoder.GetBytes("mflow").CopyTo(_module_Flow_code, 0);
            _encoder.GetBytes(module_Flow_value).CopyTo(_module_Flow_value, 0);
            byte[] _module_Flow = _module_Flow_code.Concat(_module_Flow_value).Concat(_equiment_staus).ToArray();

            String module_PumpLAM_value = ConvertStr(objStationStatusGlobal.module_PumpLAM.ToString(), 10);
            byte[] _module_PumpLAM_code = new byte[5];
            byte[] _module_PumpLAM_value = new byte[10];
            _encoder.GetBytes("pplam").CopyTo(_module_PumpLAM_code, 0);
            _encoder.GetBytes(module_PumpLAM_value).CopyTo(_module_PumpLAM_code, 0);
            byte[] _module_PumpLAM = _module_PumpLAM_code.Concat(_module_PumpLAM_value).Concat(_equiment_staus).ToArray();

            String module_PumpLFLT_value = ConvertStr(objStationStatusGlobal.module_PumpLFLT.ToString(), 10);
            byte[] _module_PumpLFLT_code = new byte[5];
            byte[] _module_PumpLFLT_value = new byte[10];
            _encoder.GetBytes("plflt").CopyTo(_module_PumpLFLT_code, 0);
            _encoder.GetBytes(module_PumpLFLT_value).CopyTo(_module_PumpLFLT_value, 0);
            byte[] _module_PumpLFLT = _module_Door_code.Concat(_module_PumpLFLT_value).Concat(_equiment_staus).ToArray();

            String module_PumpLRS_value = ConvertStr(objStationStatusGlobal.module_PumpLRS.ToString(), 10);
            byte[] _module_PumpLRS_code = new byte[5];
            byte[] _module_PumpLRS_value = new byte[10];
            _encoder.GetBytes("pplrs").CopyTo(_module_PumpLRS_code, 0);
            _encoder.GetBytes(module_PumpLRS_value).CopyTo(_module_PumpLRS_value, 0);
            byte[] _module_PumpLRS = _module_PumpLRS_code.Concat(_module_PumpLRS_value).Concat(_equiment_staus).ToArray();

            String module_PumpRAM_value = ConvertStr(objStationStatusGlobal.module_PumpRAM.ToString(), 10);
            byte[] _module_PumpRAM_code = new byte[5];
            byte[] _module_PumpRAM_value = new byte[10];
            _encoder.GetBytes("ppram").CopyTo(_module_PumpRAM_code, 0);
            _encoder.GetBytes(module_PumpRAM_value).CopyTo(_module_PumpRAM_value, 0);
            byte[] _module_PumpRAM = _module_PumpRAM_code.Concat(_module_PumpRAM_value).Concat(_equiment_staus).ToArray();

            String module_PumpRFLT_value = ConvertStr(objStationStatusGlobal.module_PumpRFLT.ToString(), 10);
            byte[] _module_PumpRFLT_code = new byte[5];
            byte[] _module_PumpRFLT_value = new byte[10];
            _encoder.GetBytes("prflt").CopyTo(_module_PumpRFLT_code, 0);
            _encoder.GetBytes(module_PumpRFLT_value).CopyTo(_module_PumpRFLT_value, 0);
            byte[] _module_PumpRFLT = _module_PumpRFLT_code.Concat(_module_PumpRFLT_value).Concat(_equiment_staus).ToArray();

            String module_PumpRRS_value = ConvertStr(objStationStatusGlobal.module_PumpRRS.ToString(), 10);
            byte[] _module_PumpRRS_code = new byte[5];
            byte[] _module_PumpRRS_value = new byte[10];
            _encoder.GetBytes("pprrs").CopyTo(_module_PumpRRS_code, 0);
            _encoder.GetBytes(module_PumpRRS_value).CopyTo(_module_PumpRRS_value, 0);
            byte[] _module_PumpRRS = _module_PumpRRS_code.Concat(_module_PumpRRS_value).Concat(_equiment_staus).ToArray();

            String module_Temperature_value = ConvertStr(objStationStatusGlobal.module_Temperature.ToString(), 10);
            byte[] _module_Temperature_code = new byte[5];
            byte[] _module_Temperature_value = new byte[10];
            _encoder.GetBytes("mdtem").CopyTo(_module_Temperature_code, 0);
            _encoder.GetBytes(module_Temperature_value).CopyTo(_module_Temperature_value, 0);
            byte[] _module_Temperature = _module_Door_code.Concat(_module_Temperature_value).Concat(_equiment_staus).ToArray();

            String module_Humidity_value = ConvertStr(objStationStatusGlobal.module_Humidity.ToString(), 10);
            byte[] _module_Humidity_code = new byte[5];
            byte[] _module_Humidity_value = new byte[10];
            _encoder.GetBytes("mdhum").CopyTo(_module_Humidity_code, 0);
            _encoder.GetBytes(module_Humidity_value).CopyTo(_module_Humidity_value, 0);
            byte[] _module_Humidity = _module_Humidity_code.Concat(_module_Humidity_value).Concat(_equiment_staus).ToArray();
            #endregion item
            //item =      "mpsph" + mps_ph + mps_ph_status +
            //            "mpsec" + mps_ec + mps_ec_status +
            //            "mpsdo" + mps_do + mps_do_status +
            //            "turbi" + mps_turbidity + mps_turbidity_status +
            //            "mporp" + mps_orp + mps_orp_status +
            //            "mtemp" + mps_temp + mps_temp_status +
            //            "clntn" + tn + tn_status +
            //            "clntp" + tp + tp_status +
            //            "cltoc" + toc + toc_status +
            //            "power" + module_Power + equiment_staus +
            //            "mdups" + module_UP + equiment_staus +
            //            "mdoor" + module_Door + equiment_staus +
            //            "mfire" + module_Fire + equiment_staus +
            //            "mflow" + module_Flow + equiment_staus +
            //            "pplam" + module_PumpLAM + equiment_staus +
            //            "pplrs" + module_PumpLRS + equiment_staus +
            //            "plflt" + module_PumpLFLT + equiment_staus +
            //            "ppram" + module_PumpRAM + equiment_staus +
            //            "pprrs" + module_PumpRRS + equiment_staus +
            //            "prflt" + module_PumpRFLT + equiment_staus +
            //            "mdtem" + module_Temperature + equiment_staus +
            //            "mdhum" + module_Humidity + equiment_staus;
            byte[] _item = _mps_ph.Concat(_mps_ec).Concat(_mps_do).Concat(_mps_turbi).
                            Concat(_cln_tn).Concat(_cln_tp).Concat(_cln_toc).
                            Concat(_module_Power).Concat(_module_UP).Concat(_module_Door).Concat(_module_Fire).Concat(_module_Flow).
                            Concat(_module_Flow).Concat(_module_PumpLAM).Concat(_module_PumpLRS).Concat(_module_PumpLFLT).
                            Concat(_module_PumpRAM).Concat(_module_PumpRRS).Concat(_module_PumpRFLT).
                            Concat(_module_Temperature).Concat(_module_Humidity).ToArray();
            _item = _encoder.GetBytes("22").Concat(_item).ToArray();
            String checksum = CalculateChecksum(_encoder.GetString(_header) + _encoder.GetString(_item) + _encoder.GetString(_ETX));
            byte[] _checksum = new byte[2];
            _encoder.GetBytes(checksum).CopyTo(_checksum, 0);
            byte[] _CR = new byte[1];
            (new byte[] { 0x0D }).CopyTo(_CR, 0);
            byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
            byte[] _sender = _header.Concat(_item).Concat(_tailer).ToArray();
            sendByte(nwStream, _sender, form1);
            byte[] _EOT = new byte[1];
            (new byte[] { 0x04 }).CopyTo(_EOT, 0);
            buffer = new byte[BUFFER_SIZE];
            if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
            {
                if (buffer[0] == 6 && Timeout(client))
                {
                    sendByte(nwStream, _EOT, form1);
                    //sendMsg(nwStream, EOT1);
                    //Console.WriteLine("Sended: {0}", EOT1);
                }
                else
                {
                    sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
                    //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                    //Console.WriteLine("Sended: {0}", "ER");
                }
            }
            else
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
                //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
            }
        }
        public void DUMP(byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client)
        {
            List<byte[]> _dataarray = new List<byte[]>();
            string _param = "";
            if (_encoder.GetString(SubArray(buffer, j + 19, 1)).Equals("M"))
            {
                String date1 = _DateDeFormat(data.Substring(j + 20, 14));
                String date2 = _DateDeFormat(data.Substring(j + 34, 14));
                // Lay danh sach du lieu gui di
                _dataarray = DataDUMP(date1, date2, "data_5minute_values", "databinding");
                _param = "M";
            }
            else
                if (_encoder.GetString(SubArray(buffer, j + 19, 1)).Equals("H"))
            {
                String date1 = _DateDeFormat(data.Substring(j + 20, 14));
                String date2 = _DateDeFormat(data.Substring(j + 34, 14));
                // Lay danh sach du lieu gui di
                _dataarray = DataDUMP(date1, date2, "data_60minute_values", "databinding");
                _param = "H";
            }
            else
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR : " + "\" " + "DUMP" + "\" " + "COMMAND"),form1);
            }
            Boolean hasData = true;
            if (_dataarray.Count > 0)
            {
                for (int i = 0; i < _dataarray.Count; i++)
                {
                    byte[] _STX = new byte[1];
                    (new byte[] { 0x02 }).CopyTo(_STX, 0);
                    byte[] _Command = new byte[4];
                    _encoder.GetBytes("DUMP").CopyTo(_Command, 0);
                    byte[] _Param = new byte[1];
                    _encoder.GetBytes(_param).CopyTo(_Param, 0);
                    byte[] _streamCode = new byte[11];
                    _encoder.GetBytes(data.Substring(1, j)).CopyTo(_streamCode, 0);
                    byte[] _dcd = new byte[1];
                    _encoder.GetBytes("1").CopyTo(_dcd, 0);
                    byte[] _header = _STX.Concat(_Command).Concat(_Param).Concat(_streamCode).Concat(_dcd).ToArray();
                    byte[] _ETX = new byte[1];
                    (new byte[] { 0x03 }).CopyTo(_ETX, 0);
                    String checksum = CalculateChecksum(_encoder.GetString(_header) + _encoder.GetString(_dataarray[i]) + _encoder.GetString(_ETX));
                    byte[] _checksum = new byte[2];
                    _encoder.GetBytes(checksum).CopyTo(_checksum, 0);
                    byte[] _CR = new byte[1];
                    (new byte[] { 0x0D }).CopyTo(_CR, 0);
                    byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
                    byte[] _sender = _header.Concat(_dataarray[i]).Concat(_tailer).ToArray();
                    //List cac lenh truyen du lieu
                    _dataarray[i] = _sender;
                }

            }
            else
            {
                hasData = false;
            }
            byte[] _EOT = new byte[1];
            new byte[] { 0x04 }.CopyTo(_EOT, 0);
            byte[] _NAK = new byte[1];
            new byte[] { 0x15 }.CopyTo(_NAK, 0);
            int a = 0;
            int b = 0;
            //sendMsg(nwStream, dataarray[a]);

            //Dem du lieu gui di
            int ct = 0;
            int ctend = _dataarray.Count - 1;
            //a++;
            if (hasData == true) //co data khi goi DUMP
            {
                do
                {
                    if (b == 3)
                    {
                        sendByte(nwStream, _EOT, form1);
                        break;
                    }
                    if (ctend < 0)
                    {
                        sendByte(nwStream, _NAK, form1);
                        break;
                    }
                    buffer = new byte[BUFFER_SIZE];
                    sendByte(nwStream, _dataarray[ct], form1);
                    AppendTextBox(Environment.NewLine + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), form1);
                    if (nwStream.Read(buffer, 0, buffer.Length) != 0)
                    {
                        if (buffer[0] == 6 && ct == ctend)  //ACK - Va het du lieu truyen
                        {
                            b = 0;
                            sendByte(nwStream, _EOT, form1);
                            a = 0;
                            break;
                        }
                        else if (buffer[0] == 6 && ct < ctend) //Neu gui thanh cong tang bien công
                        {
                            ct++;
                            b = 0;
                        }
                        else if (buffer[0] == 15)  //NAK
                        {
                            b++;
                            sendByte(nwStream, _dataarray[ct], form1);
                        }
                        else
                        {
                            //int l = nwStream.Read(buffer,0,buffer.Length);
                            Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer, 0, nwStream.Read(buffer, 0, buffer.Length)));
                            sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT 1"), form1);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer, 0, nwStream.Read(buffer, 0, buffer.Length)));
                        //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                        sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT 2"), form1);
                        break;
                    }
                    //Bien dem so lenh truyen di
                } while (true);
            }
            else
            {
                sendByte(nwStream, _EOT, form1);   //ko co data khi goi DUMP
                //break;
            }
        }
        public void CCHK(NetworkStream nwStream)
        {

            byte[] _EOT = new byte[1];
            new byte[] { 0x04 }.CopyTo(_EOT, 0);
            sendByte(nwStream, _EOT, form1);
        }
        public void RCHK(SerialPort serialPortTN, SerialPort serialPortTP, SerialPort serialPortTOC, byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client)
        {

        }
        public void SAMP(SerialPort serialPortSAMP, byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client)
        {
            if (_encoder.GetString(SubArray(buffer, j + 26, 2)).Equals("00"))
            {
                requestAutoSAMPLER(serialPortSAMP);
                //Thread.Sleep(300);
                //requestInforSAMPLER(serialPortSAMP);
                byte[] _EOT = new byte[1];
                new byte[] { 0x04 }.CopyTo(_EOT, 0);
                sendByte(nwStream, _EOT, form1);
            }
            else if (_encoder.GetString(SubArray(buffer, j + 26, 2)).Equals("10"))
            {
                byte[] _STX = new byte[1];
                (new byte[] { 0x02 }).CopyTo(_STX, 0);
                //String header = STX + "SAMP" + data.Substring(1, j) + new user_repository().DateFormat(DateTime.Now.ToString()) + data.Substring(j + 19, 7);
                byte[] _Command = new byte[4];
                _encoder.GetBytes("SAMP").CopyTo(_Command, 0);
                byte[] _StationCode = new byte[11];
                _encoder.GetBytes(data.Substring(1, j)).CopyTo(_StationCode, 0);
                byte[] _Datetime = new byte[14];
                _encoder.GetBytes(DateFormat(DateTime.Now.ToString())).CopyTo(_Datetime, 0);
                byte[] _Analyze = new byte[7];
                _encoder.GetBytes(data.Substring(j + 19, 7)).CopyTo(_Analyze, 0);
                byte[] _header = _STX.Concat(_Command).Concat(_StationCode).Concat(_Datetime).Concat(_Analyze).ToArray();
                byte[] _item = new byte[65];
                byte[] _ETX = new byte[1];
                (new byte[] { 0x03 }).CopyTo(_ETX, 0);
                string checksum = CalculateChecksum(_encoder.GetString(_header) + _encoder.GetString(_item) + _encoder.GetString(_ETX));
                byte[] _checksum = new byte[2];
                _encoder.GetBytes(checksum).CopyTo(_checksum, 0);
                byte[] _CR = new byte[1];
                (new byte[] { 0x0D }).CopyTo(_CR, 0);
                byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
                byte[] _EOT = new byte[1];
                (new byte[] { 0x04 }).CopyTo(_EOT, 0);
                byte[] _sender = _header.Concat(_item).Concat(_tailer).ToArray();
                sendByte(nwStream, _sender, form1);
            }
            else
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR : " + "\" " + "SAMP" + "\" " + "COMMAND"), form1);
            }
        }
        public void INFO(byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client)
        {
            byte[] _STX = new byte[1];
            (new byte[] { 0x02 }).CopyTo(_STX, 0);
            byte[] _Command = new byte[4];
            _encoder.GetBytes("INFO").CopyTo(_Command, 0); ;
            byte[] _StationCode = new byte[11];
            _encoder.GetBytes(data.Substring(1, j)).CopyTo(_StationCode, 0); ;
            byte[] _Datetime = new byte[14];
            _encoder.GetBytes(DateFormat(DateTime.Now.ToString())).CopyTo(_Datetime, 0); ;
            byte[] _Analyze = new byte[7];
            _encoder.GetBytes(data.Substring(j + 19, 7)).CopyTo(_Analyze, 0); ;
            byte[] _header = _STX.Concat(_Command).Concat(_StationCode).Concat(_Datetime).Concat(_Analyze).ToArray();
            byte[] _itemcode = new byte[5];
            byte[] _addinfo = new byte[50];
            byte[] _item = _itemcode.Concat(_addinfo).ToArray();
            byte[] _ETX = new byte[1];
            (new byte[] { 0x03 }).CopyTo(_ETX, 0);
            string checksum = CalculateChecksum(_encoder.GetString(_header) + _encoder.GetString(_item) + _encoder.GetString(_ETX));
            byte[] _checksum = new byte[1];
            _encoder.GetBytes(checksum).CopyTo(_checksum, 0);
            byte[] _CR = new byte[1];
            (new byte[] { 0x0D }).CopyTo(_CR, 0);
            byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
            byte[] _EOT = new byte[1];
            (new byte[] { 0x04 }).CopyTo(_EOT, 0);
            byte[] _sender = _header.Concat(_item).Concat(_tailer).ToArray();
            sendByte(nwStream, _sender, form1);
            buffer = new byte[BUFFER_SIZE];
            if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
            {
                if (buffer[0] == 6 && Timeout(client))
                {
                    sendByte(nwStream, _EOT, form1);
                }
                else
                {
                    sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
                }
            }
            else
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"), form1);
            }
        }
        public void RSET(NetworkStream nwStream)
        {
            //Process[] GetPArry = Process.GetProcesses();
            //foreach (Process testProcess in GetPArry)
            //{
            //    string ProcessName = testProcess.ProcessName;
            //    //ProcessName = ProcessName.ToLower();
            //    if (ProcessName.CompareTo("DataLogger") == 0)
            //    {
            //        string fullPath = testProcess.MainModule.FileName;
            //        testProcess.Kill();
            //        Process.Start(fullPath);
            //    }
            //}
            //String EOT = "\x04";
            byte[] _EOT = new byte[1];
            (new byte[] { 0x04 }).CopyTo(_EOT, 0);
            sendByte(nwStream, _EOT, form1);
            String applicationName = "DataLogger";
            int pid = 0;
            // Wait for the process to terminate
            Process process = null;
            try
            {
                process = Process.GetProcessById(pid);
                process.WaitForExit(1000);
            }
            catch (ArgumentException ex)
            {
                // ArgumentException to indicate that the 
                // process doesn't exist?   LAME!!
            }
            Process.Start(applicationName, "");
        }
        public void SETP(String data, int j, NetworkStream nwStream, String username)
        {
            //String EOT = "\x04";
            byte[] _EOT = new byte[1];
            (new byte[] { 0x04 }).CopyTo(_EOT, 0);
            String newpass = data.Substring(j + 19, 10).TrimStart(new Char[] { '*' });
            UpdateData(username, newpass);
            //sendMsg(nwStream, EOT);
            sendByte(nwStream, _EOT, form1);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public short wYear;
            public short wMonth;
            public short wDayOfWeek;
            public short wDay;
            public short wHour;
            public short wMinute;
            public short wSecond;
            public short wMilliseconds;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);
        public T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        public void SETT(String data, int j, NetworkStream nwStream)
        {
            String date1 = data.Substring(j + 19, 14);
            String date = DateFormat(DateTime.Now.ToString());
            SYSTEMTIME st = new SYSTEMTIME
            {
                wYear = Convert.ToInt16(date1.Substring(4, 4)),
                wMonth = Convert.ToInt16(date1.Substring(0, 2)),
                wDay = Convert.ToInt16(date1.Substring(2, 2)),
                wHour = Convert.ToInt16(date1.Substring(8, 2)),
                wMinute = Convert.ToInt16(date1.Substring(10, 2)),
                wSecond = Convert.ToInt16(date1.Substring(12, 2))
            };
            if (SetSystemTime(ref st))
            {
                byte[] _EOT = new byte[1];
                (new byte[] { 0x04 }).CopyTo(_EOT, 0);
                sendByte(nwStream, _EOT, form1);
            }
            else
            {
                byte[] _NAK = new byte[1];
                (new byte[] { 0x15 }).CopyTo(_NAK, 0);
                sendByte(nwStream, _NAK, form1);
            }
        }
        #endregion command
        #region handler
        public static void Main1()
        {
            //StartListening();

            /////
            //TcpListener tcpListener = null;
            //Int32 port = 3001;
            //IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
            ////IPAddress localAddr = IPAddress.Parse("10.239.164.254");
            ////IPAddress localAddr = IPAddress.Parse("192.168.1.62");
            //// TcpListener server = new TcpListener(port);
            ////tcpListener = new TcpListener(localAddr, port);
            ////tcpListener.Start();
            //TcpListener tcpListener = new TcpListener(port);
            ////tcpListener.Server.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
            //tcpListener.Start();
            ////counter = 0;
            //Protocol_v2(serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, tcpListener, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
            ////Protocol(serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, tcpListener, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
        }
        public async void StartListening(int port, IPAddress localAddr)
        {
            ClientSockets = new ArrayList();
            ThreadReclaim = new Thread(new ThreadStart(Reclaim));
            ThreadReclaim.Start();
            Int32 Port = port;
            IPAddress LocalAddr = localAddr;
            listener = new TcpListener(LocalAddr, Port);
            try
            {
                listener.Start();
                await Task.Run(() =>
                {
                    int TestingCycle = 100;
                    int ClientNbr = 0;
                    // Start listening for connections.
                    AppendTextBox(Environment.NewLine + "Waiting for a connection...", form1);

                    while (TestingCycle > 0)
                    {
                        TcpClient handler;
                        if (Form1.isStop == true) { break; }
                        try
                        {
                            handler = listener.AcceptTcpClient();
                        }
                        catch (Exception ex) {
                            Console.WriteLine("Tcplistener not avaib");
                            break;
                        }
                        if (handler != null)
                        {
                            ClientNbr = ClientNbr + 1;
                            if (ClientNbr % 100 == 0)
                            {
                                ClearTextBox("",form1);
                            }
                            AppendTextBox(Environment.NewLine + "Client#" + ClientNbr + " accepted!", form1);
                            // An incoming connection needs to be processed.
                            lock (ClientSockets.SyncRoot)
                            {
                                int i = ClientSockets.Add(new ClientHandler(handler, form1));
                                ((ClientHandler)ClientSockets[i]).Start();
                            }
                            --TestingCycle;
                            if (TestingCycle == 0) TestingCycle = 100;
                        }
                        else break;
                    }

                    listener.Stop();

                    ContinueReclaim = false;
                    ThreadReclaim.Join();

                    foreach (Object Client in ClientSockets)
                    {
                        ((ClientHandler)Client).Stop();
                    }
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void Reclaim()
        {
            while (ContinueReclaim)
            {
                lock (ClientSockets.SyncRoot)
                {
                    for (int x = ClientSockets.Count - 1; x >= 0; x--)
                    {
                        Object Client = ClientSockets[x];
                        if (!((ClientHandler)Client).Alive)
                        {
                            ClientSockets.Remove(Client);
                            AppendTextBox(Environment.NewLine + "A client left", form1);
                        }
                    }
                }
                Thread.Sleep(200);
            }
        }
        public void Main2()
        {

            TcpListener tcpListener = null;
            Int32 port = 3001;
            IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
            //IPAddress localAddr = IPAddress.Parse("10.239.164.254");
            //IPAddress localAddr = IPAddress.Parse("192.168.1.62");
            tcpListener = new TcpListener(localAddr, port);
            //tcpListener.Start();
            ////TcpListener tcpListener = new TcpListener(port);
            ////tcpListener.Server.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
            tcpListener.Start();
            counter = 0;
            //Protocol_v2(serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, tcpListener, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
            Protocol(serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, tcpListener, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
        }
        public void Protocol(SerialPort serialPortTN, SerialPort serialPortTP, SerialPort serialPortTOC, SerialPort serialPortSAMP, TcpListener tcpListener, relay_io_control objRelayIOControlGlobal, station_status objStationStatusGlobal, water_sampler objWaterSamplerGLobal, measured_data objMeasuredDataGlobal)
        {
            try
            {
                String data = null;
                TcpClient client = new TcpClient(AddressFamily.InterNetworkV6);
                client.Client.DualMode = true;
                AppendTextBox(Environment.NewLine + " >>> " + "Server Started", form1);
                counter = 0;
                //server.Start();
                while (true)
                {
                    counter += 1;
                    //---incoming client connected---
                    AppendTextBox(Environment.NewLine + " >> " + "While(true)", form1);
                    client = tcpListener.AcceptTcpClient();
                    AppendTextBox(Environment.NewLine + " >> " + client.Connected, form1);
                    AppendTextBox(Environment.NewLine + " >> " + "Client IP:" + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + " started!", form1);
                    //Console.WriteLine(" >> " + "Client IP:" + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                    //handleClinet handleClinet = new handleClinet();
                    //NetworkStream nwStream = client.GetStream();
                    //startClient(client, Convert.ToString(counter), data, _privateKey, _publicKey, _encoder, serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
                    newThread = new Thread(() => doChat(client, data, _encoder, serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal));
                    //Console.WriteLine(" >> " + "Listening ...");
                    newThread.Start();
                    //client.Close();
                    //newThread.Abort();
                    Thread.Sleep(1000);
                }
                tcpListener.Stop();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                tcpListener.Stop();
            }
            AppendTextBox(Environment.NewLine + "End", form1);
            //Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }
        public void doChat(TcpClient client, String data, ASCIIEncoding _encoder, SerialPort serialPortTN, SerialPort serialPortTP, SerialPort serialPortTOC, SerialPort serialPortSAMP, relay_io_control objRelayIOControlGlobal, station_status objStationStatusGlobal, water_sampler objWaterSamplerGLobal, measured_data objMeasuredDataGlobal)
        {
            int requestCount = 0;
            //byte[] buffer = new byte[BUFFER_SIZE];
            string _privateKey;
            string _publicKey;
            requestCount = 0;

            while (true)
            {
                //if (client.Connected)
                //{
                try
                {
                    AppendTextBox(Environment.NewLine + " >> " + "Listening ...", form1);
                    requestCount = requestCount + 1;
                    //---get the incoming data through a network stream--- 
                    if (!client.Connected)
                    {
                        //Console.WriteLine(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + "not connected");
                        break;
                    }
                    NetworkStream nwStream = client.GetStream();
                    //client.ReceiveTimeout = 60000;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    byte[] sender = null;
                    //Translate data bytes to a ASCII string.
                    //buffer = new byte[BUFFER_SIZE];
                    //Console.WriteLine(nwStream.);
                    //int i = 0;
                    AppendTextBox(Environment.NewLine + "Client Connect" + client.Connected, form1);
                    int i = nwStream.Read(buffer, 0, buffer.Length);
                    // bug o dong 1224, nguyen nhan co the do client close truoc khi lay dc network strem tu client              
                    //if (i != 0)
                    while (i != 0)
                    {
                        data = null;
                        data = Encoding.ASCII.GetString(buffer, 0, i);
                        int flag = 0;
                        String stationid = "BLVTRS0001";
                        string[] separators = { "\\" };
                        try
                        {
                            String[] datas = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            String username = datas[1];
                            var rsa = new RSACryptoServiceProvider();
                            _privateKey = rsa.ToXmlString(true);
                            _publicKey = rsa.ToXmlString(false);
                            //Console.WriteLine(randomnumber.ToString());   
                            //foreach (var item in buffer)
                            //{ Console.Write("{0}", item + " "); }
                            AppendTextBox(Environment.NewLine + " Received: " + data, form1);
                            //Console.WriteLine("Length: {0}", data.Length);
                            //buffer.SubArray();
                            //if ((buffer[0] == 5 && datas[2].Remove(datas[2].Length - 1, 1).Equals(stationid)) || (buffer[0] == 5 && datas[2].Remove(datas[2].Length - 2, 2).Equals(stationid)) || (buffer[0] == 5 && datas[2].Equals(stationid)) || (buffer[0] == 2))
                            if (buffer[0] == 2)
                            {
                                //String msg = "\x06";
                                byte[] msg = new byte[] { 0x06 };
                                //msg = msg +"12345";
                                byte[] _publicKey_ = _encoder.GetBytes(_publicKey);
                                sender = msg.Concat(_publicKey_).ToArray();
                                sendByte(nwStream, sender, form1);
                                //Console.WriteLine("PUBLIC KEY :" + _publicKey);
                                //Console.WriteLine("ENCRYPT BYTE :" + MyTcpListener.Encrypt("123456", _publicKey, rsa));
                                //Console.WriteLine("\n\n\n" + Encrypt("123456", _publicKey, rsa));
                                buffer = new byte[BUFFER_SIZE];
                                try
                                {
                                    //buffer = new byte[BUFFER_SIZE];
                                    AppendTextBox(Environment.NewLine + "DataAvailable " + nwStream.DataAvailable, form1);

                                    i = nwStream.Read(buffer, 0, buffer.Length);
                                    if (i != 0)
                                    {
                                        AppendTextBox(Environment.NewLine + "DataAvailable " + nwStream.DataAvailable, form1);
                                        data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                                        //Console.WriteLine("Received ENCRYPT : {0}", data);
                                        //sendMsg(nwStream, _privateKey);
                                        byte[] encryptbyte = _encoder.GetBytes(data);
                                        var encryptedByteArray = Convert.FromBase64String(data);
                                        var length = encryptedByteArray.Count();
                                        var item = 0;
                                        var sb = new StringBuilder();
                                        foreach (var x in encryptedByteArray)
                                        {
                                            item++;
                                            sb.Append(x);
                                            if (item < length)
                                                sb.Append(",");
                                        }

                                        String encryptstring = sb.ToString();
                                        //Console.WriteLine("Received ENCRYPT BYTE : {0}", encryptstring);
                                        //if (new user_repository().Auth(username, encryptstring, _privateKey, _publicKey, rsa))
                                        if (true)
                                        {
                                            byte[] msg1 = new byte[] { 0x06 };
                                            sendByte(nwStream, msg1, form1);
                                            passingParamInit(serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, nwStream, buffer, data, client, username, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
                                            break;
                                        }
                                        else
                                        {
                                            sendByte(nwStream, new byte[] { 0x15 }, form1);
                                            //break;
                                        }
                                    }
                                    else
                                    {
                                        sendByte(nwStream, _encoder.GetBytes("TIME OUT"), form1);
                                        break;
                                    }
                                }
                                catch (Exception e)
                                {
                                    //Console.WriteLine(e.StackTrace);
                                    sendByte(nwStream, _encoder.GetBytes("ERROR : CONNECTION ERROR"), form1);
                                    break;
                                }
                            }
                            else
                            {
                                int j = nwStream.Read(buffer, 0, buffer.Length);
                                string error = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                                AppendTextBox(Environment.NewLine + "Received : " + data, form1);
                                sendByte(nwStream, _encoder.GetBytes("ERROR : FORMAT MESSAGE STATION ID"), form1);
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                            sendByte(nwStream, _encoder.GetBytes("ERROR "), form1);
                            break;
                        }
                    }
                    //else
                    //{
                    //    MyTcpListener.sendByte(nwStream, _encoder.GetBytes("ERROR : NO INPUT MESSAGE"));
                    //    break;

                    //}
                    AppendTextBox(Environment.NewLine + "nwStream Closed", form1);
                    nwStream.Close();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("ERROR : CAN NOT LISTEN FROM IP :" + ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString() + " CHECK CONNECT IN CENTER.");

                    Console.WriteLine(ex.StackTrace);

                    if (client == null)
                    {
                        break;
                    }
                    break;
                }
            }
            client.Close();
            //newThread.Interrupt();
            //if (!newThread.Join(999999999))
            //{ // or an agreed resonable time
            newThread.Abort();
            //}
            newThread = null;
            //client.Close();
        }
        public void passingParamInit(Form1 form,SerialPort serialPortTN, SerialPort serialPortTP, SerialPort serialPortTOC, SerialPort serialPortSAMP, NetworkStream nwStream, byte[] buffer, String data, TcpClient client, String username, relay_io_control objRelayIOControlGlobal, station_status objStationStatusGlobal, water_sampler objWaterSamplerGLobal, measured_data objMeasuredDataGlobal)
        {
            init( form,serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, nwStream, buffer, data, client, username, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
        }
        public void passingParamInit(SerialPort serialPortTN, SerialPort serialPortTP, SerialPort serialPortTOC, SerialPort serialPortSAMP, NetworkStream nwStream, byte[] buffer, String data, TcpClient client, String username, relay_io_control objRelayIOControlGlobal, station_status objStationStatusGlobal, water_sampler objWaterSamplerGLobal, measured_data objMeasuredDataGlobal)
        {
            init( null, serialPortTN, serialPortTP, serialPortTOC, serialPortSAMP, nwStream, buffer, data, client, username, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
        }
        public void init(Form1 form, SerialPort serialPortTN, SerialPort serialPortTP, SerialPort serialPortTOC, SerialPort serialPortSAMP, NetworkStream nwStream, byte[] buffer, String data, TcpClient client, String username, relay_io_control objRelayIOControlGlobal, station_status objStationStatusGlobal, water_sampler objWaterSamplerGLobal, measured_data objMeasuredDataGlobal)
        {
            //Control control = new
            int i;
            try
            {
                //buffer = new byte[BUFFER_SIZE];
                while
                ((i = nwStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    AppendTextBox(Environment.NewLine + " Listening COMMAND ...", form1);
                    try
                    {
                        int j = 10;
                        data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                        AppendTextBox(Environment.NewLine + " Received: " + data, form1);
                        int count = 1;
                        int choice = 0;

                        if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("DATA"))
                        {
                            DATA(buffer, data, j, nwStream, client);
                            break;
                        }
                        else
                            if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("RDAT"))
                        {
                            RDAT(buffer, data, j, nwStream, client, objRelayIOControlGlobal, objStationStatusGlobal, objWaterSamplerGLobal, objMeasuredDataGlobal);
                            break;
                        }
                        else
                                if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("DUMP"))
                        {
                            int countNAK = 0;
                            DUMP(buffer, data, j, nwStream, client);
                            break;
                        }
                        else
                                    if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("CCHK"))
                        {
                            //DataLogger.frmNewMain.requestAutoSAMPLER(serialPortSAMP);
                            String EOT = "\x04";
                            CCHK(nwStream);
                            //sendMsg(nwStream, EOT);
                            break;
                        }
                        else if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("RCHK"))
                        {
                            RCHK(serialPortTN, serialPortTP, serialPortTOC, buffer, data, j, nwStream, client);
                            break;
                        }
                        else if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("SAMP"))
                        {
                            SAMP(serialPortSAMP, buffer, data, j, nwStream, client);
                            break;
                        }
                        else if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("INFO"))
                        {
                            INFO(buffer, data, j, nwStream, client);
                            break;
                        }
                        else if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("RSET"))
                        {
                            RSET(nwStream);
                            //sendMsg(nwStream, EOT);
                            //Console.WriteLine("Sended: {0}", EOT);
                            break;
                        }
                        else if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("SETP"))
                        {
                            SETP(data, j, nwStream, username);
                            break;
                        }
                        else if (_encoder.GetString(SubArray(buffer,j + 15, 4)).Equals("SETT"))
                        {
                            SETT(data, j, nwStream);
                            break;
                        }
                    }
                    catch
                    {
                        sendByte(nwStream, _encoder.GetBytes("ERROR : FORMAT MESSAGE INIT"), form1);
                        //sendMsg(nwStream, "ERROR : FORMAT MESSAGE");
                        break;
                        //Console.WriteLine("Sended: {0}", "ERROR : FORMAT MESSAGE");
                    }
                    //}
                    //}
                }
            }
            catch (Exception e)
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR : RECV MESSAGE"), form1);
                //Console.WriteLine("Exception: {0}", e);
                //sendMsg(nwStream, "ERROR : RECV MESSAGE");
                //Console.WriteLine("Sended: {0}", "ERROR : RECV MESSAGE");
            }
        }  
        #endregion  handler
    }
    public class ClientHandler
    {
        TcpClient ClientSocket;
        bool ContinueProcess = false;
        Thread ClientThread;
        public Form1 form1;
        public ClientHandler()
        {
            //this.form1 = form1;
        }
        public ClientHandler(Form1 form1)
        {
            this.form1 = form1;
        }
        public ClientHandler(TcpClient ClientSocket)
        {
            this.ClientSocket = ClientSocket;
        }
        public ClientHandler(TcpClient ClientSocket,Form1 form1)
        {
            this.ClientSocket = ClientSocket;
            this.form1 = form1;
        }
        public void Start()
        {
            ContinueProcess = true;
            ClientThread = new Thread(new ThreadStart(Process));
            ClientThread.Start();
        }
        private void Process()
        {
            Control control = new Control(form1);
            // Incoming data from the client.
            string data = null;

            // Data buffer for incoming data.
            byte[] bytes;
            string _privateKey;
            string _publicKey;
            if (ClientSocket != null)
            {
                NetworkStream nwStream = ClientSocket.GetStream();
                //ClientSocket.ReceiveTimeout = 10000; // 1000 miliseconds

                while (ContinueProcess)
                {
                    bytes = new byte[ClientSocket.ReceiveBufferSize];
                    try
                    {
                        byte[] buffer = new byte[Control.BUFFER_SIZE];
                        byte[] sender = null;
                        //int BytesRead = nwStream.Read(bytes, 0, (int)ClientSocket.ReceiveBufferSize);
                        int BytesRead = nwStream.Read(buffer, 0, buffer.Length);
                        if (BytesRead > 0)
                        {
                            data = Encoding.ASCII.GetString(buffer, 0, BytesRead);
                            // Show the data on the console.
                            //Console.WriteLine("Text received : {0}", data);

                            // Echo the data back to the client.
                            //byte[] sendBytes = Encoding.ASCII.GetBytes(data);
                            //nwStream.Write(sendBytes, 0, sendBytes.Length);

                            //if (data == "quit") break;
                            //data = null;

                            if (data == "quit")
                                break;
                            #region process
                            int flag = 0;
                            String stationid = "BLVTRS0001";
                            string[] separators = { "\\" };
                            try
                            {
                                String[] datas = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                String username = datas[1];
                                var rsa = new RSACryptoServiceProvider();
                                _privateKey = rsa.ToXmlString(true);
                                _publicKey = rsa.ToXmlString(false);

                                control.AppendTextBox(Environment.NewLine + "Received: " + data, form1);
                                //buffer.SubArray();
                                if ((buffer[0] == 5 && datas[2].Remove(datas[2].Length - 1, 1).Equals(stationid)) || (buffer[0] == 5 && datas[2].Remove(datas[2].Length - 2, 2).Equals(stationid)) || (buffer[0] == 5 && datas[2].Equals(stationid)))
                                //if (buffer[0] == 2)
                                    {
                                    //String msg = "\x06";
                                    byte[] msg = new byte[] { 0x06 };
                                    //msg = msg +"12345";
                                    byte[] _publicKey_ = Control._encoder.GetBytes(_publicKey);
                                    sender = msg.Concat(_publicKey_).ToArray();
                                    control.sendByte(nwStream, sender, form1);
                                    //Console.WriteLine("PUBLIC KEY :" + _publicKey);
                                    //Console.WriteLine("ENCRYPT BYTE :" + MyTcpListener.Encrypt("123456", _publicKey, rsa));
                                    //Console.WriteLine("\n\n\n" + Encrypt("123456", _publicKey, rsa));
                                    buffer = new byte[Control.BUFFER_SIZE];
                                    try
                                    {
                                        //buffer = new byte[BUFFER_SIZE];
                                        control.AppendTextBox(Environment.NewLine + "DataAvailable " + nwStream.DataAvailable, form1);

                                        BytesRead = nwStream.Read(buffer, 0, buffer.Length);
                                        if (BytesRead != 0)
                                        {
                                            control.AppendTextBox(Environment.NewLine + "DataAvailable " + nwStream.DataAvailable, form1);
                                            data = System.Text.Encoding.ASCII.GetString(buffer, 0, BytesRead);
                                            //Console.WriteLine("Received ENCRYPT : {0}", data);
                                            //sendMsg(nwStream, _privateKey);
                                            byte[] encryptbyte = Control._encoder.GetBytes(data);
                                            var encryptedByteArray = Convert.FromBase64String(data);
                                            var length = encryptedByteArray.Count();
                                            var item = 0;
                                            var sb = new StringBuilder();
                                            foreach (var x in encryptedByteArray)
                                            {
                                                item++;
                                                sb.Append(x);
                                                if (item < length)
                                                    sb.Append(",");
                                            }

                                            String encryptstring = sb.ToString();
                                            //Console.WriteLine("Received ENCRYPT BYTE : {0}", encryptstring);
                                            //if (new user_repository().Auth(username, encryptstring, _privateKey, _publicKey, rsa))
                                            if (true)
                                            {
                                                byte[] msg1 = new byte[] { 0x06 };
                                                control.sendByte(nwStream, msg1, form1);
                                                //control.passingParamInit(form1,Control.serialPortTN, Control.serialPortTP, Control.serialPortTOC, Control.serialPortSAMP, nwStream, buffer, data, ClientSocket, username, Control.objRelayIOControlGlobal, Control.objStationStatusGlobal, Control.objWaterSamplerGLobal, Control.objMeasuredDataGlobal);
                                                control.init( null, Control.serialPortTN, Control.serialPortTP, Control.serialPortTOC, Control.serialPortSAMP, nwStream, buffer, data, ClientSocket, username, Control.objRelayIOControlGlobal, Control.objStationStatusGlobal, Control.objWaterSamplerGLobal, Control.objMeasuredDataGlobal);
                                                break;
                                            }
                                            else
                                            {
                                                control.sendByte(nwStream, new byte[] { 0x15 }, form1);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            control.sendByte(nwStream, Control._encoder.GetBytes("TIME OUT"), form1);
                                            break;
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        control.AppendTextBox(Environment.NewLine + ex.StackTrace, form1);
                                        control.sendByte(nwStream, Control._encoder.GetBytes("ERROR : CONNECTION ERROR"), form1);
                                        break;
                                    }
                                }
                                else
                                {
                                    int j = nwStream.Read(buffer, 0, buffer.Length);
                                    string error = System.Text.Encoding.ASCII.GetString(buffer, 0, BytesRead);
                                    control.AppendTextBox(Environment.NewLine + "Received : " + data, form1);
                                    control.sendByte(nwStream, Control._encoder.GetBytes("ERROR : FORMAT MESSAGE"), form1);
                                    break;
                                }
                            }
                            catch(Exception ex)
                            {
                                control.AppendTextBox(Environment.NewLine + ex.StackTrace, form1);
                                control.sendByte(nwStream, Control._encoder.GetBytes("ERROR : FORMAT MESSAGE"),form1);
                                break;
                            }
                            #endregion process

                        }
                    }
                    catch (Exception e)
                    {
                        control.AppendTextBox(Environment.NewLine + "Conection is broken !", form1);
                        Console.WriteLine(e.StackTrace);
                        //Console.ReadLine();
                        break;
                    }
                    //catch (IOException) {
                    //    Console.WriteLine("Conection is broken 1!");
                    //    Console.ReadLine();
                    //    break;
                    //} // Timeout
                    //catch (SocketException)
                    //{
                    //    Console.WriteLine("Conection is broken 2!");
                    //    break;
                    //}
                    Thread.Sleep(200);
                } // while ( ContinueProcess )
                nwStream.Close();
                ClientSocket.Close();
            }
        }  // Process()
        public void Stop()
        {
            ContinueProcess = false;
            if (ClientThread != null && ClientThread.IsAlive)
                ClientThread.Join();
        }
        public bool Alive
        {
            get
            {
                return (ClientThread != null && ClientThread.IsAlive);
            }
        }
    } // class ClientHandler 
}
