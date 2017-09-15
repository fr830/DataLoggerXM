using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;

class TcpClientTest
{
    public static ASCIIEncoding _encoder = new ASCIIEncoding();
    private const int portNum = 3001;
    static public void Main()
    {
        
        TcpClient tcpClient = new TcpClient();
        try
        {
            tcpClient.Connect("192.168.1.87", portNum);
            
            //tcpClient.Connect("14.176.54.144", portNum);
            NetworkStream networkStream = tcpClient.GetStream();

            if (networkStream.CanWrite && networkStream.CanRead)
            {

                String DataToSend = "";

                while (DataToSend != "quit")
                {

                    byte[] _STX = new byte[1];
                    (new byte[] { 0x02 }).CopyTo(_STX, 0);
                    byte[] _ENQ = new byte[1];
                    (new byte[] { 0x05 }).CopyTo(_ENQ, 0);
                    byte[] _ACK = new byte[1];
                    (new byte[] { 0x06 }).CopyTo(_ACK, 0);
                    byte[] _Command = new byte[17];
                    _encoder.GetBytes("\\" + "admin"+ "\\" + "BLVTRS0002").CopyTo(_Command, 0);
                    //byte[] _Param = new byte[1];
                    //_encoder.GetBytes(_param).CopyTo(_Param, 0);
                    //byte[] _streamCode = new byte[11];
                    //_encoder.GetBytes(data.Substring(1, j)).CopyTo(_streamCode, 0);
                    //byte[] _dcd = new byte[1];
                    //_encoder.GetBytes("1").CopyTo(_dcd, 0);
                    //byte[] _header = _STX.Concat(_Command).ToArray();
                    //byte[] _ETX = new byte[1];
                    //(new byte[] { 0x03 }).CopyTo(_ETX, 0);
                    //String checksum = CalculateChecksum(_encoder.GetString(_header) + _encoder.GetString(_dataarray[i]) + _encoder.GetString(_ETX));
                    //byte[] _checksum = new byte[2];
                    //_encoder.GetBytes(checksum).CopyTo(_checksum, 0);
                    //byte[] _CR = new byte[1];
                    //(new byte[] { 0x0D }).CopyTo(_CR, 0);
                    //byte[] _tailer = _ETX.Concat(_checksum).Concat(_CR).ToArray();
                    byte[] _sender = _ENQ.Concat(_Command).ToArray();

                    Byte[] sendBytes = Encoding.ASCII.GetBytes(DataToSend);

                    networkStream.Write(_sender, 0, _sender.Length);
                    ///////////////////////////////////////////////////////////////////////////////

                    // Reads the NetworkStream into a byte buffer.
                    byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                    int BytesRead = networkStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);

                    // Returns the data received from the host to the console.
                    string returndata = Encoding.ASCII.GetString(bytes, 0, BytesRead);
                    Console.WriteLine("This is what the host returned to you: \r\n{0}", returndata);
                    DataToSend = "DUMP";
                    sendBytes = Encoding.ASCII.GetBytes(DataToSend);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    // Reads the NetworkStream into a byte buffer.
                    bytes = new byte[tcpClient.ReceiveBufferSize];
                    BytesRead = networkStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);

                    // Returns the data received from the host to the console.
                    returndata = Encoding.ASCII.GetString(bytes, 0, BytesRead);
                    Console.WriteLine("This is what the host returned to you: \r\n{0}", returndata);



                    byte[] _CommandSAMP = new byte[57];
                    _encoder.GetBytes("BLVTRS0002" + "20170523151212" + "DUMPM" + "20170520183012" + "20170528184512").CopyTo(_CommandSAMP, 0);

                    byte[] _ETX = new byte[1];
                    (new byte[] { 0x03 }).CopyTo(_ETX, 0);

                    string checksum = "69";
                    byte[] _checksum = new byte[2];
                    _encoder.GetBytes(checksum).CopyTo(_checksum, 0);

                    byte[] _CR = new byte[1];
                    (new byte[] { 0x0D }).CopyTo(_CR, 0);
                    //lenh cu the : STX + "BLVTRS0001" + "20170517121212" + "SAMP" + "1234567" + "10" + ETX + CHK + CR
                    byte[] _senderSAMP = _STX.Concat(_CommandSAMP).Concat(_ETX).Concat(_checksum).Concat(_CR).ToArray();
                    DataToSend = "DUMP";
                    sendBytes = Encoding.ASCII.GetBytes(DataToSend);
                    networkStream.Write(_senderSAMP, 0, _senderSAMP.Length);

                    // Reads the NetworkStream into a byte buffer.
                    bytes = new byte[tcpClient.ReceiveBufferSize];
                    BytesRead = networkStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);

                    // Returns the data received from the host to the console.
                    returndata = Encoding.ASCII.GetString(bytes, 0, BytesRead);
                    Console.WriteLine("This is what the host returned to you: \r\n{0}", returndata);

                    DataToSend = "DUMP";
                    sendBytes = Encoding.ASCII.GetBytes(DataToSend);
                    networkStream.Write(_ACK, 0, _ACK.Length);
                    Console.ReadLine();

                }
                networkStream.Close();
                tcpClient.Close();
            }
            else if (!networkStream.CanRead)
            {
                Console.WriteLine("You can not write data to this stream");
                tcpClient.Close();
            }
            else if (!networkStream.CanWrite)
            {
                Console.WriteLine("You can not read data from this stream");
                tcpClient.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }       // Main() 
} // class TcpClientTest {

