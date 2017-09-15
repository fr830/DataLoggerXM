using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTcpListener
{
    class CCHK
    {
        private int _rx_write = 0;
        private int _rx_counter = 0;
        private byte[] _rx_buffer = new byte[2048];

        private byte[] _receive_buffer = new byte[2048];
        private int _buffer_counter = 0;
        private const int PACKET_LENGTH = 100;
        private void serialPort_DataReceived(SerialPort serialPort)
        {
            try
            {

                if (!serialPort.IsOpen)
                    return;
                int bytes = serialPort.BytesToRead;
                byte[] buffer = new byte[bytes];
                serialPort.Read(buffer, 0, bytes);
                //Console.Write("buffer \t"+ _encoder.GetString(buffer) + "\n");
                //Console.Write("buffer.length \t" + buffer.Length + "\n");
                //for (int i = 0; i < bytes; i++)
                //{
                //    TOC_rx_buffer[TOC_rx_write++] = buffer[i];
                //}
                if (_rx_buffer == null)
                {
                    _rx_buffer = buffer;
                }
                else _rx_buffer = _rx_buffer.Concat(buffer).ToArray();
                //Console.Write("TOC \t"+_encoder.GetString(TOC_rx_buffer) + "\n");
                _rx_counter += bytes;
                if (_rx_buffer.Length == PACKET_LENGTH)
                {
                    //Console.Write("TRUE1");
                    ProcessData("");
                }
                if (_rx_buffer != null)
                {
                    if (_rx_buffer.Length > PACKET_LENGTH)
                    {
                        _rx_buffer = null;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void ProcessData(string text)
        {
            try
            {
                    ParseData(_rx_buffer);
                    //TN_rx_counter = 0;
                    //TN_rx_write = 0;
            }
            catch //(Exception ex)
            {

            }
            finally
            {

            }
        }
        private string ParseData(byte[] text)
        {
            int bufferIndex = 0;
            string result = "";

            try
            {
                if (text.Length >= PACKET_LENGTH)
                {
                    // MessageBox.Show(Encoding.UTF8.GetString(text, 0, text.Length));
                    //for (int i = 0; i < text.Length - PACKET_LENGTH; i++)
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (text[i] == 0x02 &&      // STX
                            text[i + 1] == 0x43 &&    // 'C'
                            text[i + 1] == 0x43 &&      // 'C'
                            text[PACKET_LENGTH + i - 1] == 0x0D && // CR
                            text[PACKET_LENGTH + i - 4] == 0x03) // ETX
                        {
                            // process data
                            string strDateTime = Encoding.UTF8.GetString(text, i + 5, 14);
                            string strN = Encoding.UTF8.GetString(text, i + 19, 2);
                            string strParameter = Encoding.UTF8.GetString(text, i + 21, 3);

                            string strStatus = Encoding.UTF8.GetString(text, i + 24, 2);
                            string strValueB = Encoding.UTF8.GetString(text, i + 26, 10);
                            string strValueA = Encoding.UTF8.GetString(text, i + 36, 10);
                            // add info 46,50 ----- 96
                            //txta.Text = strValueA;
                            //txtb.Text = strValueB;

                            DateTime dt = DateTime.ParseExact(strDateTime.Trim(), "yyyyMMddHHmmss", null);
                            //txtDate.Text = dt.ToString("dd-MM-yyyy");
                            //txtTime.Text = dt.ToString("HH:mm:ss");
                            result = string.Format("DateTime: {0}; Number Parameter: {1}; Parameter: {2}; Value A: {3}; Value B: {4}; Addition Info: {5}", strDateTime, strN, strParameter, strValueA, strValueB, strStatus);
                            //MessageBox.Show(result);
                            bufferIndex = i + PACKET_LENGTH;
                            i = i + PACKET_LENGTH - 1;
                        }
                    }
                }
                else
                {
                }
                //Reset buffer
                _buffer_counter = 0;
                Array.Clear(_receive_buffer, 0, _receive_buffer.Length);

                _rx_write = 0;
                _rx_counter = 0;
                _rx_buffer = null;

                _receive_buffer = new byte[2048];
                _buffer_counter = 0;
            }
            catch //(Exception ex)
            {
                //MessageBox.Show("0002," + ex.Message);
            }

            if (result == "")
                return result;
            return result;
        }
        private static void requestInfor(SerialPort com)
        {
            if (com.IsOpen)
            {
                byte[] packet = new byte[9];
                //Fill to packet
                packet[0] = 0x02;//STX
                packet[1] = 0x43;//C
                packet[2] = 0x43;//C
                packet[3] = 0x48;//H
                packet[4] = 0x4B;//K
                packet[5] = 0x03;//ETX
                packet[6] = 0x31;//CHK
                packet[7] = 0x3E;//CHK
                packet[8] = 0x0D;//CR

                com.Write(packet, 0, 9);
            }
        }

        private static void requestInforRCHK(SerialPort com)
        {

        }
    }
}
