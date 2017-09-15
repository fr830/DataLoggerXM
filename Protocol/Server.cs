using Npgsql;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using DataCenterEmu;
using System.Data;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Linq;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Web.Helpers;
using DataLogger.Data;

namespace Protocol
{
    public static class MyTcpListener
    {
        private static string _privateKey;
        private static string _publicKey;
        private const int BUFFER_SIZE = 1024;
        public static void Main1()
        {
            String msg = "\x02" + "BLVTRS0001" + "20160831152201" + "DATAM" + "\x03" + "\x0D";
            Console.WriteLine(msg);
            Console.WriteLine(CalculateChecksum(msg));
            Console.ReadLine();
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
            //Console.WriteLine(checksum);
            string hexString = String.Format("\\" + "x{0:X2}", checksum);
            //return hexString; //X2 de chuyen sang hexa
            return checksum.ToString("X2");
        }
        public static void sendMsg(NetworkStream nwStream, String msg)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(msg);
            String data = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            try
            {
                nwStream.Write(buffer, 0, buffer.Length);
                nwStream.Flush();
                Console.WriteLine("Sended: {0}", msg);
            }
            catch
            {
                Console.WriteLine("SENDING: " + "\"" + msg + "\"" + " BUT");
                Console.WriteLine("ERROR : CAN NOT LISTEN ANY CONNECT, CHECK CONNECT IN CENTER.");
            }
        }
        public static void sendByte(NetworkStream nwStream, byte[] msg)
        {
            //byte[] buffer = Encoding.ASCII.GetBytes(msg);
            //String data = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            try
            {
                nwStream.Write(msg, 0, msg.Length);
                nwStream.Flush();
                Console.WriteLine("Sended: {0}", _encoder.GetString(msg));
            }
            catch
            {
                Console.WriteLine("SENDING: " + "\"" + _encoder.GetString(msg) + "\"" + " BUT");
                Console.WriteLine("ERROR : CAN NOT LISTEN ANY CONNECT, CHECK CONNECT IN CENTER.");
            }
        }
        public static ASCIIEncoding _encoder = new ASCIIEncoding();
        //public static string Au(string _privateKey, string _publicKey, String enc, RSACryptoServiceProvider rsa)
        //{
        //    //var rsa = new RSACryptoServiceProvider();
        //    //_privateKey = rsa.ToXmlString(true);
        //    //_publicKey = rsa.ToXmlString(false);
        //    //var text = "Test1";
        //    //Console.WriteLine("RSA // Text to encrypt: " + text);
        //    //var enc = Encrypt(text);
        //    //Console.WriteLine("RSA // Encrypted Text: " + enc);
        //    var dec = Decrypt(enc, _privateKey, rsa);
        //    return dec;
        //    //Console.WriteLine("RSA // Decrypted Text: " + dec);
        //    // Console.ReadLine();
        //}
        //public static string Decrypt(string data, string _privateKey, RSACryptoServiceProvider rsa)
        //{
        //    //var rsa = new RSACryptoServiceProvider();
        //    var dataArray = data.Split(new char[] { ',' });
        //    byte[] dataByte = new byte[dataArray.Length];
        //    for (int i = 0; i < dataArray.Length; i++)
        //    {
        //        dataByte[i] = Convert.ToByte(dataArray[i]);
        //    }

        //    rsa.FromXmlString(_privateKey);
        //    try
        //    {
        //        var decryptedByte = rsa.Decrypt(dataByte, false);
        //        return _encoder.GetString(decryptedByte);
        //    }
        //    catch (Exception ex)
        //    {
        //        return "ERROR";
        //    }
        //}
        public static string Encrypt(string data, string _publicKey, RSACryptoServiceProvider rsa)
        {
            // var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_publicKey);
            var dataToEncrypt = _encoder.GetBytes(data);
            var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
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
        //public static Boolean Auth(String user, String recvstring, String _privateKey, String _publicKey, RSACryptoServiceProvider rsa)
        //{
        //    //Crypto.VerifyHashedPassword
        //    String table = "auth";
        //    using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //    {
        //        try
        //        {
        //            if (db.open_connection())
        //            {
        //                string sql_command = "SELECT password from " + table + " where user_name = " + "\'" + user + "\'";
        //                using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                {
        //                    cmd.CommandText = sql_command;
        //                    NpgsqlDataReader dr = cmd.ExecuteReader();
        //                    DataTable data = new DataTable();
        //                    // call load method of datatable to copy content of reader 
        //                    data.Load(dr); // Load 
        //                    String hashedpwd = data.Rows[0][0].ToString();
        //                    Boolean i = false;
        //                    //String privatekey = "9G&yH";
        //                    //var csp = new RSACryptoServiceProvider(2048);
        //                    //var privKey = csp.ExportParameters(true);
        //                    //csp.ImportParameters(privKey);
        //                    //byte[] ret = csp.Decrypt(recvstring, false);
        //                    //string decrypt = System.Text.Encoding.Unicode.GetString(ret);
        //                    String decrypt = Au(_privateKey, _publicKey, recvstring, rsa);
        //                    if (Crypto.VerifyHashedPassword(hashedpwd, decrypt))
        //                    {
        //                        i = true;
        //                    }
        //                    db.close_connection();
        //                    if (i)
        //                    {
        //                        Console.WriteLine("TRUE");
        //                        return true;
        //                    }
        //                    else Console.WriteLine("FALSE"); return false;
        //                }
        //            }
        //            else
        //            {
        //                db.close_connection();
        //                return false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string[] Error = new String[1] { "error" };
        //            Console.WriteLine("\nMessage ---\n{0}", ex.Message);
        //            Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
        //            return false;
        //        }
        //    }
        //}
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
        //public static void Main() {
        //    String mps_ph = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_pH.ToString(), 10);
        //    String mps_ph_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_pH_status.ToString(), 2);
        //    String mps_ec = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_EC.ToString(), 10);
        //    String mps_ec_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_EC_status.ToString(), 2);
        //    String mps_do = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_DO.ToString(), 10);
        //    String mps_do_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_DO_status.ToString(), 2);
        //    Console.WriteLine(mps_ph_status + mps_ec + mps_ec_status + mps_do + mps_do_status);
        //    Console.Read();
        //}
        public static Boolean Timeout(TcpClient client)
        {
            if (client.ReceiveTimeout <= 60000) { return true; }
            else return false;

        }
        public static void DATA(byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client)
        {

            if (_encoder.GetString(buffer.SubArray(j + 19, 1)).Equals("M"))
            {
                String STX = "\x02";
                byte[] _STX = new byte[] { 0x02 };
                string table = "data_5minute_values";
                string col = "created";
                String measuretime = new user_repository().getMeasureTime( table, col);
                byte[] _measuretime = _encoder.GetBytes(measuretime);
                String header = STX + "DATAM" + data.Substring(1, j) + "1" + measuretime;
                byte[] _header = _STX.Concat(_encoder.GetBytes("DATAM")).ToArray();
                //String item = DataSQL("data_5minute_values", "databinding");
                byte[] _item = new user_repository().DataSQL("data_5minute_values", "databinding");
                String ETX = "\x03";
                byte[] _ETX = new byte[] { 0x03 };
                String EOT = "\x04";
                byte[] _EOT = new byte[] { 0x04 };
                String checksum = CalculateChecksum(header + _encoder.GetString(_item) + ETX);
                byte[] _checksum = _encoder.GetBytes(checksum);
                String tailer = ETX + checksum + "\x0D";
                byte[] _tailer = _ETX.Concat(_checksum).Concat(new byte[] { 0x0D }).ToArray();
                //String sender = header + item + tailer;
                byte[] _sender = _header.Concat(_item).Concat(_tailer).ToArray();
                //sendMsg(nwStream, sender);
                sendByte(nwStream, _sender);
                //Console.WriteLine("Sended: {0}", sender);
                buffer = new byte[BUFFER_SIZE];
                if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
                {
                    if (buffer[0] == 15 && Timeout(client))
                    {
                        //sendMsg(nwStream, sender);
                        sendByte(nwStream, _sender);
                    }
                    else
                    if (buffer[0] == 6 && Timeout(client))
                    {
                        //sendMsg(nwStream, EOT);
                        //Console.WriteLine("Sended: {0}", EOT);
                        sendByte(nwStream, _EOT);
                    }
                    else
                    {
                        //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                        //Console.WriteLine("Sended: {0}", "ER");
                        sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
                    }
                }
                else
                {
                    sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
                    //Console.WriteLine("Sended: {0}", "ER");
                }
            }
            else
                if (data.Substring(j + 19, 1).Equals("H"))
            {
                String STX = "\x02";
                byte[] _STX = new byte[] { 0x02 };
                string table = "data_60minute_values";
                string col = "created";
                String measuretime = new user_repository().getMeasureTime( table, col);
                byte[] _measuretime = _encoder.GetBytes(measuretime);
                String header = STX + "DATAH" + data.Substring(1, j) + "1" + measuretime;
                byte[] _header = _STX.Concat(_encoder.GetBytes("DATAM")).ToArray();
                //String item = DataSQL("data_60minute_values", "databinding");
                byte[] _item = new user_repository().DataSQL("data_5minute_values", "databinding");
                String EOT = "\x04";
                byte[] _EOT = new byte[] { 0x04 };
                String ETX = "\x03";
                byte[] _ETX = new byte[] { 0x03 };
                String checksum = CalculateChecksum(header + _encoder.GetString(_item) + ETX);
                byte[] _checksum = _encoder.GetBytes(checksum);
                String tailer = ETX + checksum + "\x0D";
                byte[] _tailer = _ETX.Concat(_checksum).Concat(new byte[] { 0x0D }).ToArray(); //CR
                                                                                               //String sender = header + item + tailer;
                byte[] _sender = _header.Concat(_item).Concat(_tailer).ToArray();
                //byte[] sender1 = new byte[BUFFER_SIZE];
                //sender1[0] = Convert.ToByte(STX);
                //sender1[1] = Convert.ToByte("DATAM");
                //sendMsg(nwStream, sender);
                sendByte(nwStream, _sender);
                //Console.WriteLine("Sended: {0}", sender);
                buffer = new byte[BUFFER_SIZE];
                if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
                {
                    if (buffer[0] == 15 && Timeout(client))
                    {
                        //sendMsg(nwStream, sender);
                        sendByte(nwStream, _sender);
                    }
                    else
                        if (buffer[0] == 6 && Timeout(client))
                    {
                        sendByte(nwStream, _EOT);
                        //sendMsg(nwStream, EOT);
                        //Console.WriteLine("Sended: {0}", EOT);
                    }
                    else
                    {
                        //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                        //Console.WriteLine("Sended: {0}", "ER");
                        sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
                    }
                }
                else
                {
                    sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
                    //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                    //Console.WriteLine("Sended: {0}", "ER");
                }
            }
            else
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR : " + "\" " + "DATA " + "\" " + "COMMAND"));
                //sendMsg(nwStream, "ERROR : " + "\" " + "DATA " + "\" " + "COMMAND");
                //Console.WriteLine("Sended: {0}", "DATA");
            }

        }
        public static void DUMP(byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client)
        {
            if (data.Substring(j + 19, 1).Equals("M"))
            {
                String date1 = DateDeFormat(data.Substring(j + 20, 14));
                String date2 = DateDeFormat(data.Substring(j + 34, 14));
                byte[] _dataarray = new user_repository().DataDUMP(date1, date2, "data_5minute_values", "databinding");
                //String[] dataarray = DataDUMP(date1, date2, "data_5minute_values", "databinding");
                String STX = "\x02";
                byte[] _STX = new byte[] { 0x02 };
                String header = STX + "DUMPM" + data.Substring(1, j) + "1";
                byte[] _header = _STX.Concat(_encoder.GetBytes("DATAM")).Concat(_encoder.GetBytes(data.Substring(1, j))).Concat(_encoder.GetBytes("1")).ToArray();
                //String checksum = CalculateChecksum(header + item);                            
                String ETX = "\x03";
                byte[] _ETX = new byte[] { 0x03 };
                String checksum = CalculateChecksum(header + _encoder.GetString(_dataarray) + ETX);
                byte[] _checksum = _encoder.GetBytes(checksum);
                //String tailer = ETX + checksum + "\x0D";
                //String sender = header + item + tailer;
                String EOT = "\x04";
                byte[] _EOT = new byte[] { 0x04 };
                String CR = "\x04";
                byte[] _CR = new byte[] { 0x0D };
                byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
                byte[] _sender = _header.Concat(_dataarray).Concat(_tailer).ToArray();

                //for (int item = 0; item < dataarray.Length; item++)
                //{
                //    String checksum = "";
                //    checksum = CalculateChecksum(header + dataarray[item] + ETX);
                //    dataarray[item] = header + dataarray[item] + STX + checksum + ETX;
                //}
                int a = 0;
                int b = 0;
                //sendMsg(nwStream, dataarray[a]);
                sendByte(nwStream, _sender);
                //a++;
                do
                {
                    if (b == 3)
                    {
                        //sendMsg(nwStream, EOT);
                        sendByte(nwStream, _EOT);
                        //Console.WriteLine("Sended: {0}", EOT);
                        break;
                    }
                    buffer = new byte[BUFFER_SIZE];
                    if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
                    {
                        //if (buffer[0] == 6 && a < dataarray.Length && Timeout(client))
                        if (buffer[0] == 6 && Timeout(client))
                        {
                            b = 0;
                            //sendMsg(nwStream, dataarray[a]);
                            sendByte(nwStream, _sender);
                            //Console.WriteLine("Sended: {0}", dataarray[a]);
                            a++;
                        }
                        //else
                        //    if (buffer[0] == 6 && a == dataarray.Length && Timeout(client))
                        //{
                        //    sendMsg(nwStream, EOT);
                        //    //Console.WriteLine("Sended: {0}", EOT);
                        //    break;
                        //}
                        else
                        {
                            b++;
                            sendByte(nwStream, _sender);
                            //Console.WriteLine("Sended: {0}", dataarray[a]);
                        }
                    }
                    else
                    {
                        //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                        sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
                        break;
                    }

                } while (true);

            }
            else
                                    if (data.Substring(j + 19, 1).Equals("H"))
            {
                String date1 = DateDeFormat(data.Substring(j + 20, 14));
                String date2 = DateDeFormat(data.Substring(j + 34, 14));
                byte[] _dataarray = new user_repository().DataDUMP(date1, date2, "data_60minute_values", "databinding");
                //String[] dataarray = DataDUMP(date1, date2, "data_5minute_values", "databinding");
                String STX = "\x02";
                byte[] _STX = new byte[] { 0x02 };
                String header = STX + "DUMPH" + data.Substring(1, j) + "1";
                byte[] _header = _STX.Concat(_encoder.GetBytes("DATAM")).Concat(_encoder.GetBytes(data.Substring(1, j))).Concat(_encoder.GetBytes("1")).ToArray();
                //String checksum = CalculateChecksum(header + item);                            
                String ETX = "\x03";
                byte[] _ETX = new byte[] { 0x03 };
                String checksum = CalculateChecksum(header + _encoder.GetString(_dataarray) + ETX);
                byte[] _checksum = _encoder.GetBytes(checksum);
                //String tailer = ETX + checksum + "\x0D";
                //String sender = header + item + tailer;
                String EOT = "\x04";
                byte[] _EOT = new byte[] { 0x04 };
                String CR = "\x04";
                byte[] _CR = new byte[] { 0x0D };
                byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
                byte[] _sender = _header.Concat(_dataarray).Concat(_tailer).ToArray();

                //for (int item = 0; item < dataarray.Length; item++)
                //{
                //    String checksum = "";
                //    checksum = CalculateChecksum(header + dataarray[item] + ETX);
                //    dataarray[item] = header + dataarray[item] + STX + checksum + ETX;
                //}
                int a = 0;
                int b = 0;
                //sendMsg(nwStream, dataarray[a]);
                sendByte(nwStream, _sender);
                //a++;
                do
                {
                    if (b == 3)
                    {
                        //sendMsg(nwStream, EOT);
                        sendByte(nwStream, _EOT);
                        //Console.WriteLine("Sended: {0}", EOT);
                        break;
                    }
                    buffer = new byte[BUFFER_SIZE];
                    if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
                    {
                        //if (buffer[0] == 6 && a < dataarray.Length && Timeout(client))
                        if (buffer[0] == 6 && Timeout(client))
                        {
                            b = 0;
                            //sendMsg(nwStream, dataarray[a]);
                            sendByte(nwStream, _sender);
                            //Console.WriteLine("Sended: {0}", dataarray[a]);
                            a++;
                        }
                        //else
                        //    if (buffer[0] == 6 && a == dataarray.Length && Timeout(client))
                        //{
                        //    sendMsg(nwStream, EOT);
                        //    //Console.WriteLine("Sended: {0}", EOT);
                        //    break;
                        //}
                        else
                        {
                            b++;
                            sendByte(nwStream, _sender);
                            //Console.WriteLine("Sended: {0}", dataarray[a]);
                        }
                    }
                    else
                    {
                        //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                        sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
                        break;
                    }

                } while (true);
            }
            else
            {
                sendByte(nwStream, _encoder.GetBytes("ERROR : " + "\" " + "DUMP" + "\" " + "COMMAND"));
                //sendMsg(nwStream, "ERROR : " + "\" " + "DUMP " + "\" " + "COMMAND");
                //Console.WriteLine("Sended: {0}", "ERROR : " + "\" " + "DUMP " + "\" " + "COMMAND");
            }

        }
        public static void CCHK( NetworkStream nwStream) {
            String EOT = "\x04";
            byte[] _EOT = new byte[] { 0x04 };
            sendByte(nwStream, _EOT);
        }
        public static void RCHK() { }
        public static void SAMP() { }
        public static void INFO(byte[] buffer, String data, int j, NetworkStream nwStream, TcpClient client) {
            String STX = "\x02";
            byte[] _STX = new byte[] { 0x02 };
            String header = STX + "INFO" + data.Substring(1, j) + new user_repository().DateFormat(DateTime.Now.ToString()) + data.Substring(j + 19, 7);
            byte[] _heaer = _STX.Concat(_encoder.GetBytes("INFO")).Concat(_encoder.GetBytes(data.Substring(1, j))).Concat(_encoder.GetBytes(new user_repository().DateFormat(DateTime.Now.ToString()))).Concat(_encoder.GetBytes(data.Substring(j + 19, 7))).ToArray();
            String pad = new String('*', 50);
            byte[] _pad = new byte[50];
            String pad1 = new String('$', 5);
            byte[] _pad1 = new byte[5];
            String item = pad1 + pad;
            byte[] _item = _pad1.Concat(_pad).ToArray();
            String ETX = "\x03";
            byte[] _ETX = new byte[] { 0x03 };
            string checksum = CalculateChecksum(header + _encoder.GetString(_item) + ETX);
            byte[] _checksum = _encoder.GetBytes(checksum);
            String tailer = ETX + checksum + "\x0D";
            String CR = "\x0D";
            byte[] _CR = new byte[] { 0x03 };
            byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();

            String EOT = "\x04";
            byte[] _EOT = new byte[] { 0x04 };
            String sender = header + item + tailer;
            byte[] _sender = _heaer.Concat(_item).Concat(_tailer).ToArray();
            sendByte(nwStream, _sender);
            //sendMsg(nwStream, sender);
            //Console.WriteLine("Sended: {0}", sender);
            buffer = new byte[BUFFER_SIZE];
            if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
            {
                if (buffer[0] == 6 && Timeout(client))
                {
                    //sendMsg(nwStream, EOT);
                    sendByte(nwStream, _sender);
                    //Console.WriteLine("Sended: {0}", EOT);
                }
                else
                {
                    sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
                    //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                    //Console.WriteLine("Sended: {0}", "ER");
                }
            }
            else
            {
                //sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                sendByte(nwStream, _encoder.GetBytes("ERROR FORMAT OR TIMEOUT >60s"));
            }
        }
        public static void RSET(NetworkStream nwStream) {
            Process[] GetPArry = Process.GetProcesses();
            foreach (Process testProcess in GetPArry)
            {
                string ProcessName = testProcess.ProcessName;
                //ProcessName = ProcessName.ToLower();
                if (ProcessName.CompareTo("DataLogger") == 0)
                {
                    string fullPath = testProcess.MainModule.FileName;
                    testProcess.Kill();
                    Process.Start(fullPath);
                }
            }
            String EOT = "\x04";
            byte[] _EOT = new byte[] { 0x04 };
            sendByte(nwStream, _EOT);
        }
        public static void SETP(String data, int j, NetworkStream nwStream, String username) {
            String EOT = "\x04";
            byte[] _EOT = new byte[] { 0x04 };
            String newpass = data.Substring(j + 19, 10).TrimStart(new Char[] { '*' });
            new user_repository().UpdateData(username, newpass);
            //sendMsg(nwStream, EOT);
            sendByte(nwStream, _EOT);
        }
        public static void SETT(String data, int j, NetworkStream nwStream) {
            String date1 = data.Substring(j + 19, 14);
            String date = new user_repository().DateFormat(DateTime.Now.ToString());
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
                String EOT = "\x04";
                byte[] _EOT = new byte[] { 0x04 };
                //sendMsg(nwStream, EOT);
                sendByte(nwStream, _EOT);
            }
            else
            {
                String NAK = "\x15";
                byte[] _NAK = new byte[] { 0x15 };
                //sendMsg(nwStream, NAK);
                sendByte(nwStream, _NAK);
            }
        }
        public static void init(NetworkStream nwStream, byte[] buffer, String data, TcpClient client, String username)
        {
            int i;
            try
            {
                buffer = new byte[BUFFER_SIZE];
                while
                ((i = nwStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    Console.WriteLine("Listening COMMAND ...");
                    //Translate data bytes to a ASCII string.
                    //Console.WriteLine(buffer[0] + buffer[1]);
                    //data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                    //int flag = 0;
                    //String stationid = "BLVTRS0001";
                    //if (buffer[0] == 5 && Auth(data.Substring(1, 10).TrimStart(new Char[] { '*' }), data.Substring(11, 10).TrimStart(new Char[] { '*' })) && data.Substring(21, 10).Equals(stationid)) //ENQ
                    //{
                    //    Console.WriteLine("Received: {0}", data);
                    //    String msg = "\x06";
                    //    sendMsg(nwStream, msg);
                    //    //Console.WriteLine("Sended: {0}", "<ACK>");
                    //    //Console.WriteLine("Lenght: {0}", data.Length);
                    //    flag = 1;

                    //}
                    //else
                    //if (flag == 1)
                    //{
                    //    buffer = new byte[BUFFER_SIZE];
                    //    i = nwStream.Read(buffer, 0, buffer.Length);
                    try
                    {
                        int j = 10;
                        data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                        Console.WriteLine("Received: {0}", data);
                        int count = 1;
                        int choice = 0;

                        if (_encoder.GetString(buffer.SubArray(j + 15, 4)).Equals("DATA"))
                        {
                            DATA(buffer, data, j, nwStream, client);
                        }
                        else
                        #region RDAT
                    if (data.Substring(j + 15, 4).Equals("RDAT"))
                        {
                            /*
                            String STX1 = "\x02";
                            String measuretime1;
                            String header1 = STX1 + "RDAT" + data.Substring(1, j) + "1" + DateFormat(DateTime.Now.ToString());
                            String item1 = DataSQL("data_60minute_values", "databinding").Substring(0, 2);
                            String ETX1 = "\x03";
                            String mps_ph = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_pH.ToString(), 10);
                            String mps_ph_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_pH_status.ToString(), 2);
                            String mps_ec = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_EC.ToString(), 10);
                            String mps_ec_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_EC_status.ToString(), 2);
                            String mps_do = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_DO.ToString(), 10);
                            String mps_do_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_DO_status.ToString(), 2);
                            String mps_orp = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_ORP.ToString(), 10);
                            String mps_orp_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_ORP_status.ToString(), 2);
                            String mps_temp = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_Temp.ToString(), 10);
                            String mps_temp_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_Temp_status.ToString(), 2);
                            String mps_turbidity = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_Turbidity.ToString(), 10);
                            String mps_turbidity_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.MPS_Turbidity_status.ToString(), 10);
                            String tn = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.TN.ToString(), 10);
                            String tn_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.TN_status.ToString(), 2);
                            String toc = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.TOC.ToString(), 10);
                            String toc_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.TOC_status.ToString(), 2);
                            String tp = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.TP.ToString(), 10);
                            String tp_status = ConvertStr(DataLogger.frmNewMain.objMeasuredDataGlobal.TP_status.ToString(), 2);
                            String module_Door = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_Door.ToString(), 10);
                            String module_Fire = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_Fire.ToString(), 10);
                            String module_Power = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_Power.ToString(), 10);
                            String module_UP = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_UPS.ToString(), 10);
                            String module_Flow = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_Flow.ToString(), 10);
                            String module_PumpLAM = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_PumpLAM.ToString(), 10);
                            String module_PumpLFLT = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_PumpLFLT.ToString(), 10);
                            String module_PumpLRS = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_PumpLRS.ToString(), 10);
                            String module_PumpRAM = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_PumpRAM.ToString(), 10);
                            String module_PumpRFLT = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_PumpRFLT.ToString(), 10);
                            String module_PumpRRS = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_PumpRRS.ToString(), 10);
                            String module_Temperature = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_Temperature.ToString(), 10);
                            String module_Humidity = ConvertStr(DataLogger.frmNewMain.objStationStatusGlobal.module_Humidity.ToString(), 10);
                            String equiment_staus = ConvertStr(DataLogger.frmNewMain.objWaterSamplerGLobal.equipment_status.ToString(), 2);
                            item1 = item1 +
                                        "mpsec" + mps_ec + mps_ec_status +
                                        "mpsdo" + mps_do + mps_do_status +
                                        "turbi" + mps_turbidity + mps_turbidity_status +
                                        "mporp" + mps_orp + mps_orp_status +
                                        "mtemp" + mps_temp + mps_temp_status +
                                        "clntn" + tn + tn_status +
                                        "clntp" + tp + tp_status +
                                        "cltoc" + toc + toc_status +
                                        "power" + module_Power + equiment_staus +
                                        "mdups" + module_UP + equiment_staus +
                                        "mdoor" + module_Door + equiment_staus +
                                        "mfire" + module_Fire + equiment_staus +
                                        "mflow" + module_Flow + equiment_staus +
                                        "pplam" + module_PumpLAM + equiment_staus +
                                        "pplrs" + module_PumpLRS + equiment_staus +
                                        "plflt" + module_PumpLFLT + equiment_staus +
                                        "ppram" + module_PumpRAM + equiment_staus +
                                        "pprrs" + module_PumpRRS + equiment_staus +
                                        "prflt" + module_PumpRFLT + equiment_staus +
                                        "mdtem" + module_Temperature + equiment_staus +
                                        "mdhum" + module_Humidity + equiment_staus;
                            String checksum1 = CalculateChecksum(header1 + item1 + ETX1);
                            String tailer1 = ETX1 + checksum1 + "\x0D";
                            String sender1 = header1 + item1 + tailer1;
                            sendMsg(nwStream, sender1);
                            String EOT1 = "\x04";
                            //Console.WriteLine("Sended: {0}", sender1);
                            buffer = new byte[BUFFER_SIZE];
                            if (nwStream.Read(buffer, 0, buffer.Length) != 0 && Timeout(client))
                            {
                                if (buffer[0] == 6 && Timeout(client))
                                {
                                    sendMsg(nwStream, EOT1);
                                    //Console.WriteLine("Sended: {0}", EOT1);
                                    break;
                                }
                                else
                                {
                                    sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                                    //Console.WriteLine("Sended: {0}", "ER");
                                }
                            }
                            else
                            {
                                sendMsg(nwStream, "ERROR FORMAT OR TIMEOUT >60s");
                            }
                            */
                        }
                        else
                        #endregion RDAT
                        if (data.Substring(j + 15, 4).Equals("DUMP"))
                        {
                            int countNAK = 0;
                            DUMP(buffer, data, j, nwStream, client);
                        }
                        else
                        if (data.Substring(j + 15, 4).Equals("CCHK") && (data.Length == j + 30))
                        {
                            //DataLogger.frmNewMain.requestAutoSAMPLER(serialPortSAMP);
                            String EOT = "\x04";
                            CCHK(nwStream);
                            //sendMsg(nwStream, EOT);
                            break;
                        }
                        else
                        if (data.Substring(j + 15, 4).Equals("RCHK") && (data.Length == j + 30))
                        {
                            RCHK();
                        }
                        else
                        if (data.Substring(j + 15, 4).Equals("SAMP"))
                        {
                            /*
                            if (data.Substring(j + 26, 2).Equals("00"))
                            {
                                DataLogger.frmNewMain.doSAMP();
                                //String bottle_position = DataLogger.frmNewMain.objWaterSamplerGLobal.bottle_position.ToString();
                                //String status_info = DataLogger.frmNewMain.objWaterSamplerGLobal.door_status.ToString();
                                //String temp = DataLogger.frmNewMain.objWaterSamplerGLobal.refrigeration_Temperature.ToString();
                                //String type = "1";
                                String EOT = "\x04";
                                sendMsg(nwStream, EOT);
                                //Console.WriteLine("Sended: {0}", EOT);
                                break;
                            }
                            else if (data.Substring(j + 26, 2).Equals("10"))
                            {
                                String STX = "\x02";
                                String header = STX + "SAMP" + data.Substring(1, j) + DateFormat(DateTime.Now.ToString()) + data.Substring(j + 19, 7);
                                String bottle_position = DataLogger.frmNewMain.objWaterSamplerGLobal.bottle_position.ToString();
                                String status_info = DataLogger.frmNewMain.objWaterSamplerGLobal.door_status.ToString();
                                String temp = DataLogger.frmNewMain.objWaterSamplerGLobal.refrigeration_Temperature.ToString();
                                String type = "1";
                                String pad = new String('*', 50);
                                String item = temp + type + status_info + bottle_position + pad;
                                String ETX = "\x03";
                                string checksum = CalculateChecksum(header + item + ETX);
                                String tailer = ETX + checksum + "\x0D";

                                String EOT = "\x04";
                                String sender = header + item + tailer;
                                sendMsg(nwStream, sender);
                                //Console.WriteLine("Sended: {0}", sender);

                            }
                            else
                            {

                                sendMsg(nwStream, "ERROR : " + "\" " + "SAMPLE " + "\" " + "COMMAND");
                                //Console.WriteLine("Sended: {0}", "ERROR : " + "\" " + "SAMPLE " + "\" " + "COMMAND");
                                break;
                            }
                            */
                        }
                        else
                        if (data.Substring(j + 15, 4).Equals("INFO"))
                        {
                            INFO(buffer, data, j, nwStream, client);
                        }
                        else
                        if (data.Substring(j + 15, 4).Equals("RSET"))
                        {
                            RSET(nwStream);
                            //sendMsg(nwStream, EOT);
                            //Console.WriteLine("Sended: {0}", EOT);
                            break;
                        }
                        else
                        if (data.Substring(j + 15, 4).Equals("SETP"))
                        {
                            SETP(data,j, nwStream, username);
                            break;
                        }
                        else
                        if (data.Substring(j + 15, 4).Equals("SETT"))
                        {
                            SETT(data,j,nwStream);
                        }
                    }
                    catch
                    {
                        sendMsg(nwStream, "ERROR : FORMAT MESSAGE");
                        break;
                        //Console.WriteLine("Sended: {0}", "ERROR : FORMAT MESSAGE");
                    }
                    //}
                    //}
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: {0}", e);
                sendMsg(nwStream, "ERROR : RECV MESSAGE");
                //Console.WriteLine("Sended: {0}", "ERROR : RECV MESSAGE");
            }
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
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        public static void Main()
        //public static void Protocol()
        {
            TcpListener server = null;
            Thread.Sleep(800);
            try
            {
                Int32 port = 3001;
                IPAddress localAddr = IPAddress.Parse(GetLocalIPAddress());
                //IPAddress localAddr = IPAddress.Parse("10.239.164.254");
                //IPAddress localAddr = IPAddress.Parse("192.168.1.62");
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);
                String data = null;
                server.Start();
                while (true)
                {
                    Console.WriteLine("Listening...");
                    //---incoming client connected---
                    TcpClient client = server.AcceptTcpClient();
                    //---get the incoming data through a network stream---
                    NetworkStream nwStream = client.GetStream();
                    //client.ReceiveTimeout = 60000;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    byte[] sender = null;
                    //Translate data bytes to a ASCII string.
                    //buffer = new byte[BUFFER_SIZE];
                    int i = nwStream.Read(buffer, 0, buffer.Length);
                    if (i != 0)
                    {
                        data = null;
                        data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                        int flag = 0;
                        String stationid = "BLVTRS0001";
                        string[] separators = { "\\" };
                        String[] datas = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        String username = datas[1];
                        var rsa = new RSACryptoServiceProvider();
                        _privateKey = rsa.ToXmlString(true);
                        _publicKey = rsa.ToXmlString(false); ////
                                                             //Console.WriteLine(randomnumber.ToString());   
                        //foreach (var item in buffer)
                        //{ Console.Write("{0}", item + " "); }
                        Console.WriteLine("\n Received: {0}", data);
                        Console.WriteLine("Length: {0}", data.Length);
                        //buffer.SubArray();
                        if ((buffer[0] == 5 && datas[2].Remove(datas[2].Length - 2, 2).Equals(stationid)) || (buffer[0] == 5 && datas[2].Equals(stationid)))
                        {
                            //String msg = "\x06";
                            byte[] msg = new byte[] { 0x06 };
                            //msg = msg +"12345";
                            byte[] _publicKey_ = _encoder.GetBytes(_publicKey);
                            sender = msg.Concat(_publicKey_).ToArray();
                            sendByte(nwStream, sender);
                            Console.WriteLine("PUBLIC KEY :" + _publicKey);
                            Console.WriteLine("ENCRYPT :"+ Encrypt("123456",_publicKey,rsa));
                            //Console.WriteLine("\n\n\n" + Encrypt("123456", _publicKey, rsa));
                            buffer = new byte[BUFFER_SIZE];
                            try
                            {
                                buffer = new byte[BUFFER_SIZE];
                                i = nwStream.Read(buffer, 0, buffer.Length);
                                if (i != 0 && Timeout(client))
                                {
                                    data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                                    Console.WriteLine("Received Encrypt: {0}", data);
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
                                    //if (new user_repository().Auth(username, encryptstring, _privateKey, _publicKey, rsa))
                                    if(true)
                                    {
                                        //String msg1 = "\x06";
                                        //sendMsg(nwStream, msg1);
                                        byte[] msg1 = new byte[] { 0x06 };
                                        sendByte(nwStream, msg1);
                                        init(nwStream, buffer, data, client, username);
                                    }
                                    else
                                    {
                                        //sendMsg(nwStream, "\x15");
                                        //byte[] msg1 = new byte[] { 0x06 };
                                        sendByte(nwStream, new byte[] { 0x15 });
                                    }
                                }
                                else
                                {
                                    sendMsg(nwStream, "TIME OUT");
                                }
                            }
                            catch
                            {
                                sendMsg(nwStream, "ERROR : CONNECTION ERROR");
                            }
                        }
                        else
                        {
                            sendMsg(nwStream, "ERROR : FORMAT MESSAGE");
                        }
                    }
                    else
                    {
                        sendMsg(nwStream, "ERROR : NO INPUT MESSAGE");
                    }

                    client.Close();
                    Thread.Sleep(800);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
        //public static void UpdateData(String username, String newpass)
        //{
        //    using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //    {
        //        try
        //        {
        //            if (db.open_connection())
        //            {
        //                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
        //                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
        //                //conn.Open();
        //                newpass = Crypto.HashPassword(newpass);
        //                string sql_command = "UPDATE auth SET password = " + "\'" + newpass + "\'" + " WHERE user_name = " + "\'" + username + "\'";
        //                //NpgsqlCommand cmd = new NpgsqlCommand("UPDATE auth SET password = " + "\'" + newpass + "\'" + " WHERE user_name = " + "\'" + username + "\'", conn);
        //                using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                {
        //                    cmd.CommandText = sql_command;
        //                    cmd.ExecuteNonQuery();
        //                    db.close_connection();
        //                }
        //            }
        //            else
        //            {
        //                db.close_connection();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string[] Error = new String[1] { "error" };
        //            Console.WriteLine("\nMessage ---\n{0}", ex.Message);
        //            Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
        //        }
        //    }
        //}

        public static string[] SplitIntoChunks(this string toSplit, int chunkSize)
        {
            int stringLength = toSplit.Length;

            int chunksRequired = (int)Math.Ceiling((decimal)stringLength / (decimal)chunkSize);
            var stringArray = new string[chunksRequired];

            int lengthRemaining = stringLength;

            for (int i = 0; i < chunksRequired; i++)
            {
                int lengthToUse = Math.Min(lengthRemaining, chunkSize);
                int startIndex = chunkSize * i;
                stringArray[i] = toSplit.Substring(startIndex, lengthToUse);

                lengthRemaining = lengthRemaining - lengthToUse;
            }

            return stringArray;
        }
        public static String DateDeFormat(string date)
        {
            date = date.Substring(0, 2) + "/" + date.Substring(2, 2) + "/" + date.Substring(4, 4) + " " + date.Substring(8, 2) + ":" + date.Substring(10, 2) + ":" + date.Substring(12, 2);
            //Console.WriteLine(date);
            DateTime dateValue = DateTime.ParseExact(date, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);

            return date;
        }
        //public static string ConvertStr(String str, int i)
        //{
        //    int j;
        //    //if (str.Length < i)
        //    //{
        //    //    j = i - str.Length;
        //    //    String pad = new String('*', j);
        //    //    str = pad + str;
        //    //}
        //    if (str.Length > i)
        //    {
        //        if (str.Contains("."))
        //        {
        //            j = str.Length - i;
        //            str = str.Substring(0, 9);
        //        }
        //    }
        //    return str;
        //}
        //public static void Main() {
        //public static String getMeasureTime(String table, String time)
        //{
        //    String measuretime;
        //    //String time = "created";
        //    //String table = "data_5minute_values";
        //    using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //    {
        //        try
        //        {
        //            if (db.open_connection())
        //            {

        //                string sql_command = "SELECT " + time + " from " + table + " order by ID desc limit 1";
        //                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
        //                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
        //                //conn.Open();
        //                using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                {
        //                    cmd.CommandText = sql_command;
        //                    //NpgsqlCommand cmd = new NpgsqlCommand("SELECT " + time + " from " + table + " order by ID desc limit 1", conn);
        //                    NpgsqlDataReader dr = cmd.ExecuteReader();
        //                    DataTable data = new DataTable();
        //                    // call load method of datatable to copy content of reader 
        //                    data.Load(dr); // Load 
        //                                   //cmd = new NpgsqlCommand("SELECT * from " + tablebinding, conn);
        //                                   //dr = cmd.ExecuteReader();
        //                                   //DataTable tbcode = new DataTable();
        //                                   //tbcode.Load(dr);
        //                    string strvalue = "";
        //                    foreach (DataRow row in data.Rows)
        //                    {
        //                        strvalue = Convert.ToString(row["created"]);
        //                    }
        //                    strvalue = DateFormat(strvalue);
        //                    //Console.WriteLine(strvalue);
        //                    //Console.Read();
        //                    db.close_connection();
        //                    return strvalue;
        //                }
        //            }
        //            else
        //            {
        //                db.close_connection();
        //                return "ERROR";
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            Console.WriteLine("\nMessage ---\n{0}", ex.Message);
        //            Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
        //            return "ERROR";
        //        }
        //    }
        //}
        //public static String DateFormat(String date)
        //{
        //    //public static void Main(){     
        //    //String date = "8/21/2016 11:23:35 AM"; 
        //    DateTime dateTime = DateTime.Parse(date);
        //    date = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
        //    date = date.Substring(0, 2) + date.Substring(3, 2) + date.Substring(6, 4) + date.Substring(11, 2) + date.Substring(14, 2) + date.Substring(17, 2);
        //    //Console.WriteLine(date);
        //    //Console.Read();
        //    return date;

        //}
        public static byte[] mergeArrayByte(byte[] output, byte[] subarray)
        {

            int i;
            for (i = 0; i < subarray.Length; i++)
            {
                output[i] = subarray[i];
            }
            return output;
        }
        //public static void Main(){
        //public static byte[] DataDUMP(String date1, String date2, String table, String tablebinding)
        //{
        //    DateTime dateValue;
        //    //String date1 = "08052016121000";
        //    //String date2 = "08052016130000";
        //    //date1 = DateDeFormat(date1);
        //    //date2 = DateDeFormat(date2);
        //    //String table = "data_5minute_values";
        //    //String tablebinding = "databinding";
        //    //Console.WriteLine(date);
        //    //Console.Read();
        //    using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //    {
        //        try
        //        {
        //            if (db.open_connection())
        //            {
        //                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
        //                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
        //                //conn.Open();
        //                string sql_command = "SELECT * from " + table + " WHERE created < " + "\'" + date2 + "\'" + " AND created > " + "\'" + date1 + "\'" + "ORDER BY created ASC";
        //                //NpgsqlCommand cmd = new NpgsqlCommand("SELECT * from " + table + " WHERE created < " + "\'" + date2 + "\'" + " AND created > " + "\'" + date1 + "\'" + "ORDER BY created ASC", conn);
        //                using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                {
        //                    cmd.CommandText = sql_command;
        //                    NpgsqlDataReader dr = cmd.ExecuteReader();
        //                    DataTable data = new DataTable();
        //                    // call load method of datatable to copy content of reader 
        //                    data.Load(dr); // Load data phu hop trong command DUMP

        //                    string sql_command1 = "SELECT * from " + tablebinding;
        //                    //cmd = new NpgsqlCommand("SELECT * from " + tablebinding, conn);
        //                    cmd.CommandText = sql_command1;
        //                    dr = cmd.ExecuteReader();
        //                    DataTable tbcode = new DataTable();
        //                    byte[] databyte = new byte[BUFFER_SIZE];
        //                    tbcode.Load(dr); // Load bang chua mapping cac truong

        //                    string strvalue = "";

        //                    byte[] sql = null;

        //                    byte[] clnnamevalue = new byte[10];
        //                    byte[] clnnamestatus = new byte[2];
        //                    byte[] code = new byte[5];

        //                    byte[] countitem = new byte[2];
        //                    byte[] measuretime = new byte[14];
        //                    string[] strvalues = new string[data.Rows.Count];


        //                    countitem = _encoder.GetBytes(ConvertStr(data.Rows.Count.ToString(), 2));
        //                    //sql = sql.Concat(_encoder.GetBytes(DateFormat(Convert.ToString(row1["created"])))).Concat(_encoder.GetBytes(ConvertStr(data.Rows.Count.ToString(), 2))).ToArray();
        //                    foreach (DataRow row1 in data.Rows)  // lay moi row trong data phu hop voi DUMP command
        //                    {
        //                        //Console.WriteLine("\n ID \n" + Convert.ToString(row1["id"]));
        //                        measuretime = _encoder.GetBytes(DateFormat(Convert.ToString(row1["created"])));
        //                        sql = sql.Concat(measuretime).Concat(countitem).ToArray();  //Measure time + count item
        //                        //strvalue = strvalue + DateFormat(Convert.ToString(row1["created"])) + tbcode.Rows.Count.ToString() + "\\";
        //                        foreach (DataRow row2 in tbcode.Rows)
        //                        {

        //                            //strvalue = strvalue + Convert.ToString(row2["code"]);
        //                            code = _encoder.GetBytes(Convert.ToString(row2["code"]));
        //                            //strvalue = strvalue + ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamevalue"])]), 10);
        //                            clnnamevalue = _encoder.GetBytes(ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamevalue"])]), 10));
        //                            //strvalue = strvalue + ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamestatus"])]), 2);
        //                            clnnamestatus = _encoder.GetBytes(ConvertStr(Convert.ToString(row1[Convert.ToString(row2["clnnamestatus"])]), 2));

        //                            sql = sql.Concat(code).Concat(clnnamevalue).Concat(clnnamestatus).ToArray();
        //                            //Console.WriteLine(strvalue);
        //                            //strvalues[count] = strvalue;
        //                            //strvalue = "";
        //                        }
        //                    }
        //                    //strvalues = SplitIntoChunks(strvalue, 17 * (tbcode.Rows.Count) + 2 + 14);  //chia thanh moi string de nhet vao array ?
        //                    //foreach (string item in strvalues) { 

        //                    //}
        //                    //Console.WriteLine(strvalues[1]);
        //                    //Console.Read();
        //                    //strvalue = ConvertStr(tbcode.Rows.Count.ToString(), 2) + strvalue;
        //                    db.close_connection();
        //                    //return strvalues;
        //                    return sql;
        //                }
        //            }
        //            else
        //            {
        //                db.close_connection();
        //                string[] Error = new String[1] { "error" };
        //                //return Error;
        //                byte[] rt = _encoder.GetBytes("ERROR");
        //                return rt;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string[] Error = new String[1] { "error" };
        //            Console.WriteLine("\nMessage ---\n{0}", ex.Message);
        //            Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
        //            //return Error;
        //            byte[] rt = _encoder.GetBytes("ERROR");
        //            return rt;
        //        }
        //    }
        //}
        //public static byte[] DataSQL(String table, String tablebinding)
        //{
        //    using (NpgsqlDBConnection db = new NpgsqlDBConnection())
        //    {
        //        try
        //        {
        //            if (db.open_connection())
        //            {
        //                string sql_command = "SELECT * from " + table + " order by ID desc limit 1";
        //                //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";
        //                //NpgsqlConnection conn = new NpgsqlConnection(connstring);
        //                //conn.Open();
        //                //NpgsqlCommand cmd = new NpgsqlCommand("SELECT * from " + table + " order by ID desc limit 1", conn);
        //                using (NpgsqlCommand cmd = db._conn.CreateCommand())
        //                {
        //                    cmd.CommandText = sql_command;
        //                    NpgsqlDataReader dr = cmd.ExecuteReader();
        //                    DataTable data = new DataTable();
        //                    byte[] databyte = new byte[BUFFER_SIZE];
        //                    // call load method of datatable to copy content of reader 
        //                    data.Load(dr); // Load 

        //                    string sql_command1 = "SELECT * from " + tablebinding;
        //                    cmd.CommandText = sql_command1;
        //                    //cmd = new NpgsqlCommand("SELECT * from " + tablebinding, conn);
        //                    dr = cmd.ExecuteReader();
        //                    DataTable tbcode = new DataTable();
        //                    tbcode.Load(dr);

        //                    string strvalue = "";
        //                    byte[] sql = null;
        //                    byte[] clnnamevalue = new byte[10];
        //                    byte[] clnnamestatus = new byte[2];
        //                    sql = sql.Concat(_encoder.GetBytes(ConvertStr(tbcode.Rows.Count.ToString(), 2))).ToArray();

        //                    byte[] code = new byte[5];

        //                    foreach (DataRow row in tbcode.Rows)
        //                    {
        //                        //strvalue = strvalue + Convert.ToString(row["code"]);
        //                        code = _encoder.GetBytes(Convert.ToString(row["code"]));
        //                        clnnamevalue = _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamevalue"])]), 10));
        //                        clnnamestatus = _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamestatus"])]), 2));

        //                        sql = sql.Concat(code).Concat(clnnamevalue).Concat(clnnamestatus).ToArray();
        //                        //sql = sql.Concat(code).Concat(mergeArrayByte(clnnamevalue, _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamevalue"])]), 10)))).Concat(mergeArrayByte(clnnamestatus, _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamestatus"])]), 2)))).ToArray();

        //                        //strvalue = strvalue + ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamevalue"])]),10);
        //                        //sql = sql.Concat(mergeArrayByte(clnnamevalue,_encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamevalue"])]), 10)))).ToArray();
        //                        //strvalue = strvalue + ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamestatus"])]),2);
        //                        //sql = sql.Concat(mergeArrayByte(clnnamestatus, _encoder.GetBytes(ConvertStr(Convert.ToString(data.Rows[0][Convert.ToString(row["clnnamestatus"])]), 2)))).ToArray();

        //                    }
        //                    //Console.WriteLine(strvalue);
        //                    //Console.Read();
        //                    //sql = ConvertStr(tbcode.Rows.Count.ToString(), 2) + "\\" + strvalue + "\\";
        //                    db.close_connection();
        //                    return sql;
        //                }
        //            }
        //            else
        //            {
        //                db.close_connection();
        //                byte[] rt = _encoder.GetBytes("ERROR");
        //                return rt;
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            Console.WriteLine("\nMessage ---\n{0}", ex.Message);
        //            Console.WriteLine("\nMessage ---\n{0}", ex.StackTrace);
        //            //return "ERROR";
        //            byte[] rt = _encoder.GetBytes("ERROR");
        //            return rt;
        //        }
        //    }

        //}
    }
}
