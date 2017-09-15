using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using DataLogger.Entities;
using System.Globalization;
using DataLogger.Data;
using System.Diagnostics;
using System.IO;
using Excel = ClosedXML.Excel;
//Microsoft.Office.Interop.Excel;
using System.Reflection;
using DataLogger.Utils;
using System.Resources;
using System.Net.Sockets;
using System.Net;
using WinformProtocol;
using Npgsql;

namespace DataLogger
{
    public partial class frmNewMain : Form
    {
        LanguageService lang = new LanguageService(typeof(frmNewMain).Assembly);
        public static string language_code = "en";

        public bool is_close_form = false;

        public static TcpListener tcpListener = null;
        public static DateTime datetime00;
        private System.Threading.Timer tmrThreadingTimer;
        private System.Threading.Timer tmrThreadingTimer_HeadingTime;
        private System.Threading.Timer tmrThreadingTimerStationStatus;
        private System.Threading.Timer tmrThreadingTimerFor5Minute;
        private System.Threading.Timer tmrThreadingTimerFor60Minute;
        private System.Threading.Timer tmrThreadingTimerForFTP;

        public CalculationDataValue objCalCulationDataValue5Minute = new CalculationDataValue();
        public CalculationDataValue objCalCulationDataValue60Minute = new CalculationDataValue();

        public const int TRANSACTION_ADD_NEW = 1;
        public const int TRANSACTION_UPDATE = 2;

        public const int PERIOD_CHECK_COMMUNICATION_ERROR = 35;

        public const string ADAM_4050 = "ADAM4050";
        public const string ADAM_4051 = "ADAM4051";
        public const string ADAM_4017_1 = "ADAM40171";
        public const string ADAM_4017_2 = "ADAM40172";
        //public static int _GROUP = 1;

        public const string STATUS_ERROR = "Error";
        public const string STATUS_Normal = "Normal";
        public const string STATUS_WARNING = "Warning";
        public const string STATUS_MEASURING = "Measuring";
        public const string STATUS_CALIBRATE = "Calibrate";

        public const int INT_STATUS_NORMAL = 0;
        public const int INT_STATUS_MEASURING_STOP = 1;
        public const int INT_STATUS_EMPTY_SAMPLER_RESERVOIR = 2;
        public const int INT_STATUS_CALIBRATING = 3;
        public const int INT_STATUS_MAINTENANCE = 4;
        public const int INT_STATUS_COMMUNICATION_ERROR = 5;
        public const int INT_STATUS_INSTRUMENT_ERROR = 6;

        // global dataValue
        int countingRequest = 0;
        public int firstTimeForIOControl = 0;

        public static measured_data objMeasuredDataGlobal = new measured_data();

        data_value obj5MinuteDataValue = new data_value();
        data_value obj60MinuteDataValue = new data_value();

        // delegate used for Invoke
        internal delegate void StringDelegate(string data);
        internal delegate void HeadingTimerDelegate(string data);
        private delegate void ProcessDataCallback(string text);
        internal delegate void SetHeadingLoginNameDelegate(string data);

        public Boolean ADAM4050Sign = false;
        public Boolean ADAM4051Sign = false;
        public Boolean ADAM4017Sign = false;
        // ADAM 4050
        private int ADAM405x_rx_write = 0;
        private int ADAM405x_rx_counter = 0;
        //private byte[] ADAM405x_rx_buffer = null;
        private byte[] ADAM405x_rx_buffer = new byte[2048];

        private const int ADAM405x_PACKET_LENGTH = 8;
        private const int ADAM_TEMP_HUMIDITY_PACKET_LENGTH = 58;
        private const string ADAM_TEMP_HUMIDITY = "";
        private byte[] ADAM405x_receive_buffer = new byte[2048];
        private int ADAM405x_buffer_counter = 0;

        private readonly data_5minute_value_repository db5m = new data_5minute_value_repository();
        private readonly data_60minute_value_repository db60m = new data_60minute_value_repository();
        private readonly maintenance_log_repository _maintenance_logs = new maintenance_log_repository();

        public static Form1 protocol;
        public static Boolean isSamp;
        #region Form event
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        public frmNewMain()
        {
            InitializeComponent();
        }
        private void frmNewMain_Load(object sender, EventArgs e)
        {
            //frmConfiguration.protocol = new Form1(this.serialPortSAMP);
            
            //frmConfiguration.protocol.Show();          

            GlobalVar.maintenanceLog = new maintenance_log();

            backgroundWorkerMain.RunWorkerAsync();
            initUserInterface();
            tmrThreadingTimer = new System.Threading.Timer(new TimerCallback(tmrThreadingTimer_TimerCallback), null, System.Threading.Timeout.Infinite, 2000);
            tmrThreadingTimer.Change(0, 2000);
            tmrThreadingTimer_HeadingTime = new System.Threading.Timer(new TimerCallback(tmrThreadingTimer_HeadingTime_TimerCallback), null, System.Threading.Timeout.Infinite, 700);
            tmrThreadingTimer_HeadingTime.Change(0, 700);
            tmrThreadingTimerStationStatus = new System.Threading.Timer(new TimerCallback(tmrThreadingTimerStationStatus_TimerCallback), null, System.Threading.Timeout.Infinite, 2000);
            tmrThreadingTimerStationStatus.Change(0, 2000);
            tmrThreadingTimerFor5Minute = new System.Threading.Timer(new TimerCallback(tmrThreadingTimerFor5Minute_TimerCallback), null, System.Threading.Timeout.Infinite, 50000);
            //tmrThreadingTimerFor5Minute.Change(0, 2500);
            tmrThreadingTimerFor5Minute.Change(0, 50000);
            tmrThreadingTimerFor60Minute = new System.Threading.Timer(new TimerCallback(tmrThreadingTimerFor60Minute_TimerCallback), null, System.Threading.Timeout.Infinite, 2000);
            //tmrThreadingTimerFor60Minute.Change(0, 3000);
            tmrThreadingTimerFor60Minute.Change(0, 240000);
            tmrThreadingTimerForFTP = new System.Threading.Timer(new TimerCallback(tmrThreadingTimerForFTP_TimerCallback), null, 1000 * 60, Timeout.Infinite);
            tmrThreadingTimerForFTP.Change(0, 1000 * 60 * 60 * 2);
            //tmrThreadingTimerForFTP.Change(0, 1000);
            initConfig(true);
            Thread.Sleep(500);
            frmConfiguration.protocol = new Form1(this);
        }
        private void initConfig(bool isConfigCOM = false)
        {
            GlobalVar.stationSettings = new station_repository().get_info();
            GlobalVar.moduleSettings = new module_repository().get_all();

            label9.Text = Convert.ToString(GlobalVar.stationSettings.station_name);

            for (int i = 1; i <= GlobalVar.moduleSettings.Count(); i++)
            {
                foreach (var item in GlobalVar.moduleSettings)
                {
                    string currentvar = "var" + i.ToString();
                    string currentlabelname = "txt" + currentvar;
                    string currentlabelunit = "txt" + currentvar + "Unit";
                    if (item.item_name.Equals(currentvar))
                    {
                        ClearLabel(this, item.display_name, currentlabelname);
                        ClearLabel(this, item.unit, currentlabelunit);
                    }
                }
            }

            if (init(isConfigCOM))
            {
            }
            else
            {
                if (!serialPortADAM.IsOpen)
                {
                    serialPortADAM.PortName = "COM100";
                }
                MessageBox.Show(lang.getText("please_check_system"));
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                tcpListener.Stop();
            }
            catch (Exception ex)
            {
            }
            this.Close();
        }
        #endregion
        private void backgroundWorkerMain_DoWork(object sender, DoWorkEventArgs e)
        {
            station existedStationsSetting = new station_repository().get_info();
            if (existedStationsSetting == null)
            {

            }
            else
            {


            }
        }
        #region initial method
        private bool init(bool isConfigCOM = false)
        {
            try
            {
                GlobalVar.moduleSettings = new module_repository().get_all();
                for (int i = 1; i <= GlobalVar.moduleSettings.Count(); i++)
                {
                    foreach (var item in GlobalVar.moduleSettings)
                    {
                        string currentvar = "var" + i.ToString();
                        string currentlabelname = "txt" + currentvar;
                        string currentlabelunit = "txt" + currentvar + "Unit";
                        string currentlabelvalue = "txt" + currentvar + "Value";
                        if (item.item_name.Equals(currentvar))
                        {
                            ClearTextbox(this, "---", currentlabelvalue);
                        }
                    }
                }
                if (isConfigCOM)
                {

                    if (serialPortADAM.IsOpen)
                        serialPortADAM.Close();

                    if (Convert.ToString(GlobalVar.stationSettings.module_comport) != "")
                    {
                        serialPortADAM.PortName = GlobalVar.stationSettings.module_comport;
                        serialPortADAM.Open();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private void initUserInterface()
        {

            switch_language();
        }
        private void switch_language()
        {
            lang.nextLanguage();
            switch (lang.CurrentLanguage.Language)
            {
                case ELanguage.English:
                    language_code = "en";
                    this.btnMonthlyReport.BackgroundImage = global::DataLogger.Properties.Resources.MonthlyReportButton;
                    break;
                case ELanguage.Vietnamese:
                    language_code = "vi";
                    this.btnMonthlyReport.BackgroundImage = global::DataLogger.Properties.Resources.MonthlyReportButton;
                    break;
                default:
                    break;
            }
            this.btnLanguage.BackgroundImage = lang.CurrentLanguage.Icon;
            // heading menu
            lang.setText(lblHeaderNationName, "main_menu_language");
            lang.setText(lblMainMenuTitle, "main_menu_title");
            settingForLoginStatus();
            // left menu buttong
            lang.setText(lblThaiNguyenStation, "thai_nguyen_station_text", EAlign.Center);
            lang.setText(lblAutomaticMonitoring, "automatic_monitoring_text", EAlign.Center);
            lang.setText(lblSurfaceWaterQuality, "surface_water_quality_text", EAlign.Center);
            // control panel
            lang.setText(this, "data_logger_system");
        }
        #endregion
        #region Comport receive
        public delegate void DataReceivedEventHandler(object sender, ReceivedEventArgs e);
        public event DataReceivedEventHandler DataReceived;
        private void serialPortADAM_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //MessageBox.Show("testeset");
                if (!serialPortADAM.IsOpen)
                    return;
                int bytes = serialPortADAM.BytesToRead;

                byte[] buffer = new byte[bytes];
                serialPortADAM.Read(buffer, 0, bytes);
                //Console.WriteLine(serialPortADAM.PortName);
                //string raw_data1 = Encoding.UTF8.GetString(buffer);
                for (int i = 0; i < bytes; i++)
                {
                    ADAM405x_rx_buffer[ADAM405x_rx_write++] = buffer[i];
                    if (ADAM405x_rx_write >= 2048)
                        ADAM405x_rx_write = 0;
                }
                ADAM405x_rx_counter = ADAM405x_rx_counter + bytes;
                var new_data = ADAM405x_rx_buffer.TakeWhile((v, index) => ADAM405x_rx_buffer.Skip(index).Any(w => w != 0x00)).ToArray();
                //string raw_data2 = Encoding.UTF8.GetString(ADAM405x_rx_buffer);
                //Console.WriteLine("ADAM405x_rx_counter : " + ADAM405x_rx_counter);
                //Console.WriteLine("data : " + new_data.Length + " : " + Encoding.UTF8.GetString(new_data));
                ProcessDataADAM("");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        #endregion

        #region ComPort Process data
        private void setText(string text)
        {
            if (this.txtData.InvokeRequired)
            {
                StringDelegate d = new StringDelegate(setText);
                this.txtData.Invoke(d, new object[] { text });
            }
            else
            {
                txtData.Text = text;
            }
        }
        private void setTextHeadingTimer(string text)
        {
            if (this.txtData.InvokeRequired)
            {
                HeadingTimerDelegate d = new HeadingTimerDelegate(setTextHeadingTimer);
                this.lblHeadingTime.Invoke(d, new object[] { text });
            }
            else
            {
                lblHeadingTime.Text = text;
            }
        }
        private void setTextHeadingLogin(string text)
        {
            if (this.txtData.InvokeRequired)
            {
                SetHeadingLoginNameDelegate d = new SetHeadingLoginNameDelegate(setTextHeadingLogin);
                this.lblLoginDisplayName.Invoke(d, new object[] { text });
            }
            else
            {
                lblLoginDisplayName.Text = text;
            }
        }

        public static ASCIIEncoding _encoder = new ASCIIEncoding();
        private void ProcessDataADAM(string text)
        {
            try
            {
                if (this.txtData.InvokeRequired)
                {
                    ProcessDataCallback d = new ProcessDataCallback(ProcessDataADAM);
                    this.txtData.Invoke(d, new object[] { text });
                }
                else
                {
                    string temp1 = ADAMParseData(ADAM405x_rx_buffer);
                }
            }
            catch
            {

            }
        }
        public void writeLog(string content, string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    File.Create(filename);
                }

                TextWriter twr = new StreamWriter(filename, true);
                DateTime dt = new DateTime();
                dt = DateTime.Now;
                twr.Write(dt.ToString() + " : ");
                twr.WriteLine(content);
                twr.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: -" + ex.Message);
            }
        }
        private string ADAMParseData(byte[] text)
        {
            //int bufferIndex = 0;
            //tmrThreadingTimerStationStatus.Change(Timeout.Infinite, Timeout.Infinite);
            string result = "";
            try
            {
                int j = 0;
                //Console.Write(station_status_data_type +"\n");
                if (station_status_data_type_4017.Equals(ADAM_4017_1) || station_status_data_type_4017.Equals(ADAM_4017_2))
                {
                    //Console.WriteLine(text.Length);
                    if (text.Length >= ADAM_TEMP_HUMIDITY_PACKET_LENGTH)
                    {
                        for (int i = 0; i < text.Length - ADAM_TEMP_HUMIDITY_PACKET_LENGTH; i++)
                        {
                            j = i;
                            if (text[j] == 0x3E &&
                                text[j + ADAM_TEMP_HUMIDITY_PACKET_LENGTH - 1] == 0x0D
                                )
                            {
                                // process data
                                string raw_data = Encoding.UTF8.GetString(SubArray(text, i, ADAM_TEMP_HUMIDITY_PACKET_LENGTH));
                                result = String.Format("{0}-{1}", station_status_data_type_4017, string.Format("raw_data: {0}", raw_data));

                                //string[] str_input = raw_data.Split('+');
                                raw_data = raw_data.Split('>')[1];
                                string[] str_input = new string[8];
                                for (int k = 0; k < 8; k++)
                                {
                                    str_input[k] = raw_data.Substring(k * 7, 7);
                                }
                                //foreach (string str in str_input) {
                                //    Console.WriteLine(str);
                                //}
                                IEnumerable<module> ADAM4017Modules = null;
                                GlobalVar.moduleSettings = new module_repository().get_all();
                                if (station_status_data_type_4017.Equals(ADAM_4017_1))
                                {
                                    //station_status_data_type == ADAM_4017_1;
                                    ADAM4017Modules = GlobalVar.moduleSettings.Where(m => m.module_id == CommonInfo.INT_ADAM_4017_1);
                                    Console.WriteLine("ADAM 1");
                                }
                                if (station_status_data_type_4017.Equals(ADAM_4017_2))
                                {
                                    ADAM4017Modules = GlobalVar.moduleSettings.Where(m => m.module_id == CommonInfo.INT_ADAM_4017_2);
                                    Console.WriteLine("ADAM 2");
                                }
                                int indexvar1 = 1;
                                int indexvar2 = 1;
                                int indexvar3 = 1;
                                int indexvar4 = 1;
                                int indexvar5 = 1;
                                int indexvar6 = 1;
                                int indexvar7 = 1;
                                int indexvar8 = 1;
                                int indexvar9 = 1;
                                int indexvar10 = 1;
                                int indexvar11 = 1;
                                int indexvar12 = 1;
                                int indexvar13 = 1;
                                int indexvar14 = 1;
                                int indexvar15 = 1;
                                int indexvar16 = 1;
                                int indexvar17 = 1;
                                int indexvar18 = 1;

                                Double dec_var1 = -1;
                                Double dec_var2 = -1;
                                Double dec_var3 = -1;
                                Double dec_var4 = -1;
                                Double dec_var5 = -1;
                                Double dec_var6 = -1;
                                Double dec_var7 = -1;
                                Double dec_var8 = -1;
                                Double dec_var9 = -1;
                                Double dec_var10 = -1;
                                Double dec_var11 = -1;
                                Double dec_var12 = -1;
                                Double dec_var13 = -1;
                                Double dec_var14 = -1;
                                Double dec_var15 = -1;
                                Double dec_var16 = -1;
                                Double dec_var17 = -1;
                                Double dec_var18 = -1;


                                Double dec_raw_var1;
                                Double dec_raw_var2;
                                Double dec_raw_var3;
                                Double dec_raw_var4;
                                Double dec_raw_var5;
                                Double dec_raw_var6;
                                Double dec_raw_var7;
                                Double dec_raw_var8;
                                Double dec_raw_var9;
                                Double dec_raw_var10;
                                Double dec_raw_var11;
                                Double dec_raw_var12;
                                Double dec_raw_var13;
                                Double dec_raw_var14;
                                Double dec_raw_var15;
                                Double dec_raw_var16;
                                Double dec_raw_var17;
                                Double dec_raw_var18;

                                //Console.Write("Chuan bi vao vong for \n");
                                foreach (module itemModule in ADAM4017Modules)
                                {
                                    //Console.Write("Da vao vao vong for , chuan bi vao switch \n");
                                    switch (itemModule.item_name.ToLower().Trim())
                                    {
                                        case "var1":
                                            indexvar1 = itemModule.channel_number;
                                            dec_raw_var1 = Convert.ToDouble(str_input[indexvar1]);
                                            //dec_var1 = (Double)(dec_raw_var1 - itemModule.input_min) * (Double)(itemModule.output_max / (itemModule.input_max - itemModule.input_min)) + itemModule.output_min + itemModule.off_set;
                                            dec_var1 = Calculator(dec_raw_var1, itemModule);
                                            break;
                                        case "var2":
                                            indexvar2 = itemModule.channel_number;
                                            dec_raw_var2 = Convert.ToDouble(str_input[indexvar2]);
                                            dec_var2 = Calculator(dec_raw_var2, itemModule);
                                            break;
                                        case "var3":
                                            indexvar3 = itemModule.channel_number;
                                            dec_raw_var3 = Convert.ToDouble(str_input[indexvar3]);
                                            dec_var3 = Calculator(dec_raw_var3, itemModule);
                                            break;
                                        case "var4":
                                            indexvar4 = itemModule.channel_number;
                                            dec_raw_var4 = Convert.ToDouble(str_input[indexvar4]);
                                            dec_var4 = Calculator(dec_raw_var4, itemModule);
                                            break;
                                        case "var5":
                                            indexvar5 = itemModule.channel_number;
                                            dec_raw_var5 = Convert.ToDouble(str_input[indexvar5]);
                                            dec_var5 = Calculator(dec_raw_var5, itemModule);
                                            break;
                                        case "var6":
                                            indexvar5 = itemModule.channel_number;
                                            dec_raw_var6 = Convert.ToDouble(str_input[indexvar6]);
                                            dec_var6 = Calculator(dec_raw_var6, itemModule);
                                            break;
                                        case "var7":
                                            indexvar7 = itemModule.channel_number;
                                            dec_raw_var7 = Convert.ToDouble(str_input[indexvar7]);
                                            dec_var7 = Calculator(dec_raw_var7, itemModule);
                                            break;
                                        case "var8":
                                            indexvar8 = itemModule.channel_number;
                                            dec_raw_var8 = Convert.ToDouble(str_input[indexvar8]);
                                            dec_var8 = Calculator(dec_raw_var8, itemModule);
                                            break;
                                        case "var9":
                                            indexvar9 = itemModule.channel_number;
                                            dec_raw_var9 = Convert.ToDouble(str_input[indexvar9]);
                                            dec_var9 = (Double)(dec_raw_var9 - itemModule.input_min) * (Double)(itemModule.output_max / (itemModule.input_max - itemModule.input_min)) + itemModule.output_min + itemModule.off_set;
                                            break;
                                        case "var10":
                                            indexvar10 = itemModule.channel_number;
                                            dec_raw_var10 = Convert.ToDouble(str_input[indexvar10]);
                                            dec_var10 = Calculator(dec_raw_var10, itemModule);
                                            break;
                                        case "var11":
                                            indexvar11 = itemModule.channel_number;
                                            dec_raw_var11 = Convert.ToDouble(str_input[indexvar11]);
                                            dec_var11 = Calculator(dec_raw_var11, itemModule);
                                            break;
                                        case "var12":
                                            indexvar12 = itemModule.channel_number;
                                            dec_raw_var12 = Convert.ToDouble(str_input[indexvar12]);
                                            dec_var12 = Calculator(dec_raw_var12, itemModule);
                                            break;
                                        case "var13":
                                            indexvar13 = itemModule.channel_number;
                                            dec_raw_var13 = Convert.ToDouble(str_input[indexvar13]);
                                            dec_var13 = Calculator(dec_raw_var13, itemModule);
                                            break;
                                        case "var14":
                                            indexvar14 = itemModule.channel_number;
                                            dec_raw_var14 = Convert.ToDouble(str_input[indexvar14]);
                                            dec_var14 = Calculator(dec_raw_var14, itemModule);
                                            break;
                                        case "var15":
                                            indexvar15 = itemModule.channel_number;
                                            dec_raw_var15 = Convert.ToDouble(str_input[indexvar15]);
                                            dec_var15 = Calculator(dec_raw_var15, itemModule);
                                            break;
                                        case "var16":
                                            indexvar16 = itemModule.channel_number;
                                            dec_raw_var16 = Convert.ToDouble(str_input[indexvar16]);
                                            dec_var16 = Calculator(dec_raw_var16, itemModule);
                                            break;
                                        case "var17":
                                            indexvar17 = itemModule.channel_number;
                                            dec_raw_var17 = Convert.ToDouble(str_input[indexvar17]);
                                            dec_var17 = Calculator(dec_raw_var17, itemModule);
                                            break;
                                        case "var18":
                                            indexvar18 = itemModule.channel_number;
                                            dec_raw_var18 = Convert.ToDouble(str_input[indexvar18]);
                                            dec_var18 = Calculator(dec_raw_var18, itemModule);
                                            break;
                                        default:
                                            break;
                                    }
                                    //Console.Write("Da thoat vong switch \n");
                                }
                                if (dec_var1 != -1)
                                {
                                    objMeasuredDataGlobal.var1 = dec_var1;
                                    Console.WriteLine("objMeasuredDataGlobal.var1 :" + objMeasuredDataGlobal.var1);
                                }
                                if (dec_var2 != -1)
                                {
                                    objMeasuredDataGlobal.var2 = dec_var2;
                                    Console.WriteLine("objMeasuredDataGlobal.var2 :" + objMeasuredDataGlobal.var2);
                                }
                                if (dec_var3 != -1)
                                {
                                    objMeasuredDataGlobal.var3 = dec_var3;
                                    Console.WriteLine("objMeasuredDataGlobal.var3 :" + objMeasuredDataGlobal.var3);
                                }
                                if (dec_var4 != -1)
                                {
                                    objMeasuredDataGlobal.var4 = dec_var4;
                                    Console.WriteLine("objMeasuredDataGlobal.var4 :" + objMeasuredDataGlobal.var4);
                                }
                                if (dec_var5 != -1)
                                {
                                    objMeasuredDataGlobal.var5 = dec_var5;
                                    Console.WriteLine("objMeasuredDataGlobal.var5 :" + objMeasuredDataGlobal.var5);
                                }
                                if (dec_var6 != -1)
                                {
                                    objMeasuredDataGlobal.var6 = dec_var6;
                                    Console.WriteLine("objMeasuredDataGlobal.var6 :" + objMeasuredDataGlobal.var6);
                                }
                                if (dec_var7 != -1)
                                {
                                    objMeasuredDataGlobal.var7 = dec_var7;
                                    Console.WriteLine("objMeasuredDataGlobal.var7 :" + objMeasuredDataGlobal.var7);
                                }
                                if (dec_var8 != -1)
                                {
                                    objMeasuredDataGlobal.var8 = dec_var8;
                                    Console.WriteLine("objMeasuredDataGlobal.var8 :" + objMeasuredDataGlobal.var8);
                                }
                                if (dec_var9 != -1)
                                {
                                    objMeasuredDataGlobal.var9 = dec_var9;
                                    Console.WriteLine("objMeasuredDataGlobal.var9 :" + objMeasuredDataGlobal.var9);
                                }
                                if (dec_var10 != -1)
                                {
                                    objMeasuredDataGlobal.var10 = dec_var10;
                                    Console.WriteLine("objMeasuredDataGlobal.var10 :" + objMeasuredDataGlobal.var10);
                                }
                                if (dec_var11 != -1)
                                {
                                    objMeasuredDataGlobal.var11 = dec_var11;
                                    Console.WriteLine("objMeasuredDataGlobal.var11 :" + objMeasuredDataGlobal.var11);
                                }
                                if (dec_var12 != -1)
                                {
                                    objMeasuredDataGlobal.var12 = dec_var12;
                                    Console.WriteLine("objMeasuredDataGlobal.var12 :" + objMeasuredDataGlobal.var12);
                                }
                                if (dec_var13 != -1)
                                {
                                    objMeasuredDataGlobal.var13 = dec_var13;
                                    Console.WriteLine("objMeasuredDataGlobal.var13 :" + objMeasuredDataGlobal.var13);
                                }
                                if (dec_var14 != -1)
                                {
                                    objMeasuredDataGlobal.var14 = dec_var14;
                                    Console.WriteLine("objMeasuredDataGlobal.var14 :" + objMeasuredDataGlobal.var14);
                                }
                                if (dec_var15 != -1)
                                {
                                    objMeasuredDataGlobal.var15 = dec_var15;
                                    Console.WriteLine("objMeasuredDataGlobal.var15 :" + objMeasuredDataGlobal.var15);
                                }
                                if (dec_var16 != -1)
                                {
                                    objMeasuredDataGlobal.var16 = dec_var16;
                                    Console.WriteLine("objMeasuredDataGlobal.var16 :" + objMeasuredDataGlobal.var16);
                                }
                                if (dec_var17 != -1)
                                {
                                    objMeasuredDataGlobal.var17 = dec_var17;
                                    Console.WriteLine("objMeasuredDataGlobal.var17 :" + objMeasuredDataGlobal.var17);
                                }
                                if (dec_var18 != -1)
                                {
                                    objMeasuredDataGlobal.var18 = dec_var18;
                                    Console.WriteLine("objMeasuredDataGlobal.var18 :" + objMeasuredDataGlobal.var18);
                                }
                                objMeasuredDataGlobal.MPS_status = 0;
                                objMeasuredDataGlobal.latest_update_MPS_communication = DateTime.Now;

                                updateMeasuredDataValue(objMeasuredDataGlobal);
                                //if (_GROUP == 1)
                                //{
                                //    _GROUP = 2;
                                //}
                                //else
                                //{
                                //    _GROUP = 1;
                                //}

                                ADAM4017Sign = true;
                                break;

                            }
                        }
                    }
                }
                //else
                //{
                if (text.Length >= ADAM405x_PACKET_LENGTH)
                {
                    for (int i = 0; i < text.Length - ADAM405x_PACKET_LENGTH; i++)
                    {
                        j = i;
                        int n = 0;
                        if (text[j] == 0x21 && text[j + ADAM405x_PACKET_LENGTH - 1] == 0x0D)
                        {
                            n++;
                        }
                        if (text[j] == 0x21 &&
                            text[j + ADAM405x_PACKET_LENGTH - 1] == 0x0D
                            )
                        {
                            // process data
                            string raw_data = Encoding.UTF8.GetString(SubArray(text, i, ADAM405x_PACKET_LENGTH));
                            result = String.Format("{0}-{1}", station_status_data_type_405x, string.Format("raw_data: {0}", raw_data));
                            string str_input = "0x" + raw_data.Substring(3, 2);
                            string str_output = "0x" + raw_data.Substring(1, 2);
                            int intValue = Convert.ToInt32(str_input, 16);
                            int outValue = Convert.ToInt32(str_output, 16);
                            string s = raw_data.Substring(1, 4);
                            string binarystring = String.Join(String.Empty, s.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                            string value = new string(binarystring.ToCharArray().Reverse().ToArray());
                            int intOutPutValue = Convert.ToInt32(str_output, 16);

                            result += " " + intOutPutValue + " " + intValue;
                            byte[] values = Encoding.UTF8.GetBytes(value);
                            switch (station_status_data_type_405x)
                            {
                                case ADAM_4050:
                                    byte[] ADAM4050 = SubArray(text, i, ADAM405x_PACKET_LENGTH);
                                    //Console.WriteLine("ADAM4050 : " + Encoding.UTF8.GetString(ADAM4050));
                                    result += " - " + ADAM_4050;

                                    var ADAM4050Modules = GlobalVar.moduleSettings.Where(m => m.module_id == CommonInfo.INT_ADAM_4050);
                                    foreach (module itemModule in ADAM4050Modules)
                                    {
                                        checkModuleValue(values, itemModule.channel_number, itemModule);
                                    }                                   

                                    result += "- END 4050";
                                    //ADAM405x_rx_buffer = ADAM405x_rx_buffer.Except(ADAM4050).ToArray();
                                    ADAM4050Sign = true;
                                    //Console.WriteLine(" ADAM4050Sign:TRUE");
                                    //Console.WriteLine(binarystring);
                                    break;
                                case ADAM_4051:
                                    byte[] ADAM4051 = SubArray(text, i, ADAM405x_PACKET_LENGTH);
                                    result += " - " + ADAM_4051;
                                    var ADAM4051Modules = GlobalVar.moduleSettings.Where(m => m.module_id == CommonInfo.INT_ADAM_4051);
                                    foreach (module itemModule in ADAM4051Modules)
                                    {
                                        checkModuleValue(values, itemModule.channel_number, itemModule);
                                    }
                                    result += "- 4051";
                                    //ADAM405x_rx_buffer = ADAM405x_rx_buffer.Except(ADAM4051).ToArray();
                                    ADAM4051Sign = true;
                                    //Console.WriteLine(" ADAM4051Sign:TRUE");
                                    //Console.WriteLine(binarystring);
                                    break;
                                default:
                                    break;
                            }
                            break;

                        }
                    }
                }

                //}
                //Console.WriteLine(Encoding.UTF8.GetString(ADAM405x_rx_buffer));
                //Reset buffer
                if (((ADAM4050Sign == true) || (ADAM4051Sign == true)) && (ADAM4017Sign == true))
                {
                    ADAM405x_buffer_counter = 0;
                    Array.Clear(ADAM405x_receive_buffer, 0, ADAM405x_receive_buffer.Length);

                    ADAM405x_rx_write = 0;
                    ADAM405x_rx_counter = 0;
                    ADAM405x_rx_buffer = new byte[2048];

                    ADAM405x_receive_buffer = new byte[2048];
                    ADAM405x_buffer_counter = 0;

                    ADAM4050Sign = false;
                    ADAM4051Sign = false;
                    ADAM4017Sign = false;
                }
                //if ((ADAM4050Sign == true) && (ADAM4051Sign == true) && (ADAM4017Sign == true))
                //{
                //    ADAM405x_buffer_counter = 0;
                //    Array.Clear(ADAM405x_receive_buffer, 0, ADAM405x_receive_buffer.Length);
                //    ADAM405x_rx_write = 0;
                //    ADAM405x_rx_counter = 0;
                //    ADAM405x_rx_buffer = null;
                //    ADAM405x_receive_buffer = new byte[2048];
                //    ADAM405x_buffer_counter = 0;
                //    ADAM4050Sign = false;
                //    ADAM4051Sign = false;
                //    ADAM4017Sign = false;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("ADAM : " + ex.StackTrace);
            }
            if (result == "")
                return result;
            return result;
        }
        public static string StringToByteArray(string hexstring)
        {
            return String.Join(String.Empty, hexstring
                .Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }
        //private bool checkModuleValue(int intValue, int channel_number)
        //{
        //    switch (channel_number)
        //    {
        //        case 0:
        //            break;
        //        case 1:
        //            return ((intValue % 16) & 1) > 0;
        //        case 2:
        //            return ((intValue % 16) & 2) > 0;
        //        case 3:
        //            return ((intValue % 16) & 4) > 0;
        //        case 4:
        //            return ((intValue % 16) & 8) > 0;
        //        case 5:
        //            return ((intValue / 16) & 1) > 0;
        //        case 6:
        //            return ((intValue / 16) & 2) > 0;
        //        case 7:
        //            return ((intValue / 16) & 4) > 0;
        //        case 8:
        //            return ((intValue / 16) & 8) > 0;
        //        default:
        //            break;
        //    }
        //    return false;
        //}

        private int checkModuleValue(byte[] values, int channel_number, module objModule)
        {
            int result = 0;
            bool checkWithChannelNumber = false;
            switch (channel_number)
            {
                case 0:
                    if (values[0] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 1:
                    if (values[1] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 2:
                    if (values[2] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 3:
                    if (values[3] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 4:
                    if (values[4] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 5:
                    if (values[5] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 6:
                    if (values[6] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 7:
                    if (values[7] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 8:
                    if (values[8] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 9:
                    if (values[9] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 10:
                    if (values[10] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 11:
                    if (values[11] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 12:
                    if (values[12] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 13:
                    if (values[13] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 14:
                    if (values[14] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 15:
                    if (values[15] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                case 16:
                    if (values[16] == 49) { checkWithChannelNumber = true; }
                    else { checkWithChannelNumber = false; }
                    break;
                default:
                    break;
            }

            return result;
        }

        private static void requestInforADAM(SerialPort com, string ADAM)
        {
            if (com.IsOpen)
            {
                byte[] packet;
                switch (ADAM)
                {
                    case ADAM_4050: // USED FOR DI/O (BOTH IN OUT CONTROL)
                        // Module 02
                        packet = new byte[6];
                        //Fill to packet
                        packet[0] = 0x02; // STX
                        packet[1] = 0x24; // '$'
                        packet[2] = 0x30; // '0'
                        packet[3] = 0x32; // '2'
                        packet[4] = 0x36; // '6'
                        packet[5] = 0x0D; //
                        com.Write(packet, 0, packet.Length);
                        break;
                    //02243033360D
                    case ADAM_4051:
                        // Module 03
                        packet = new byte[6];
                        //Fill to packet
                        packet[0] = 0x02; // STX
                        packet[1] = 0x24; // '$'
                        packet[2] = 0x30; // '0'
                        packet[3] = 0x33; // '3'
                        packet[4] = 0x36; // '6'
                        packet[5] = 0x0D; //
                        com.Write(packet, 0, packet.Length);
                        break;
                    case ADAM_4017_1:
                        // Module 01
                        packet = new byte[5];
                        //Fill to packet
                        packet[0] = 0x02; // STX
                        packet[1] = 0x23; // '#'
                        packet[2] = 0x30; // '0'
                        if (station_status_data_type_4017.Equals(ADAM_4017_1))
                        {
                            packet[3] = 0x31; // '1'
                        }
                        else
                        {
                            packet[3] = 0x34; // '4'
                        }
                //packet[3] = 0x31; // '1'
                packet[4] = 0x0D; //
                        com.Write(packet, 0, packet.Length);
                        break;
                    case ADAM_4017_2:
                        // Module 02
                        packet = new byte[5];
                        //Fill to packet
                        packet[0] = 0x02; // STX
                        packet[1] = 0x23; // '#'
                        packet[2] = 0x30; // '0'
                        if (station_status_data_type_4017.Equals(ADAM_4017_1))
                        {
                            packet[3] = 0x31; // '1'
                        }
                        else
                        {
                            packet[3] = 0x34; // '4'
                        }
                        packet[4] = 0x0D; //
                        com.Write(packet, 0, packet.Length);

                        break;
                    default:
                        break;
                }
            }

        }
        #endregion

        #region threading timer
        public int indexSelection = 0;
        private void tmrThreadingTimer_TimerCallback(object state)
        {
            if (is_close_form)
            {
                try
                {
                    this.Close();
                    //MessageBox.Show("123");
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(Environment.ExitCode);
                    }
                }
                catch
                {

                }
            }
            setText(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            indexSelection = (indexSelection + 1) % 5;
            switch (indexSelection)
            {
                case 0: 
                    break;
                case 1: 
                    break;
                case 2: 
                    break;
                case 3: // MPS
                    //requestInforMPS(serialPortMPS);
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }

        private void tmrThreadingTimer_HeadingTime_TimerCallback(object state)
        {
            if (is_close_form)
            {
                try
                {
                    this.Close();
                    //MessageBox.Show("123");
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(Environment.ExitCode);
                    }
                }
                catch
                {

                }
            }
            string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            setTextHeadingTimer(time);
            settingForLoginStatus();

        }

        public int indexSelectionStation = 0;
        public static string station_status_data_type_4017 = "";
        public static string station_status_data_type_405x = "";
        private void tmrThreadingTimerStationStatus_TimerCallback(object state)
        {
            if (is_close_form)
            {
                try
                {
                    this.Close();
                    //MessageBox.Show("123");
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(Environment.ExitCode);
                    }
                }
                catch
                {
                }
            }
            //setText(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            indexSelectionStation = (indexSelectionStation + 1) % 4;
            switch (indexSelectionStation)
            {
                case 0: // 4050
                    //station_status_data_type_405x = ADAM_4050;
                    //requestInforADAM(serialPortADAM, ADAM_4050);
                    //tmrThreadingTimerStationStatus.Change(Timeout.Infinite, Timeout.Infinite);
                    break;
                case 1: // 4051
                    //station_status_data_type_405x = ADAM_4051;
                    //requestInforADAM(serialPortADAM, ADAM_4051);
                    //Console.WriteLine("1");
                    break;
                case 2: // 
                    station_status_data_type_4017 = ADAM_4017_1;
                    //_GROUP = 1;
                    requestInforADAM(serialPortADAM, ADAM_4017_1);
                    //tmrThreadingTimerStationStatus.Change(Timeout.Infinite, Timeout.Infinite);
                    break;
                case 3: // 
                    //_GROUP = 2;
                    station_status_data_type_4017 = ADAM_4017_2;
                    requestInforADAM(serialPortADAM, ADAM_4017_2);
                    //tmrThreadingTimerStationStatus.Change(Timeout.Infinite, Timeout.Infinite);
                    break;
                default:
                    break;
            }
        }

        public static int countingIndex5Minute = 0;
        private void tmrThreadingTimerFor5Minute_TimerCallback(object state)
        {
            if (is_close_form)
            {
                try
                {
                    this.Close();
                    //MessageBox.Show("123");
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(Environment.ExitCode);
                    }
                }
                catch
                {

                }
            }
            // 50 seconds save current time to datavalue table
            if (countingIndex5Minute < 2)
            {
                countingIndex5Minute++;
                return;
            }
            else
            {

            }
            checkAllCommunication();

            data_value objDataValue = new data_value();
            // MPS
            if (objMeasuredDataGlobal.MPS_status < 0)
            {
                objMeasuredDataGlobal.MPS_status = CommonInfo.INT_STATUS_COMMUNICATION_ERROR;
                objMeasuredDataGlobal.var1 = -1;
                objMeasuredDataGlobal.var2 = -1;
                objMeasuredDataGlobal.var3 = -1;
                objMeasuredDataGlobal.var4 = -1;
                objMeasuredDataGlobal.var5 = -1;
                objMeasuredDataGlobal.var6 = -1;
                objMeasuredDataGlobal.var7 = -1;
                objMeasuredDataGlobal.var8 = -1;
                objMeasuredDataGlobal.var9 = -1;
                objMeasuredDataGlobal.var10 = -1;
                objMeasuredDataGlobal.var11 = -1;
                objMeasuredDataGlobal.var12 = -1;
                objMeasuredDataGlobal.var13 = -1;
                objMeasuredDataGlobal.var14 = -1;
                objMeasuredDataGlobal.var15 = -1;
                objMeasuredDataGlobal.var16 = -1;
                objMeasuredDataGlobal.var17 = -1;
                objMeasuredDataGlobal.var18 = -1;
            }
            objDataValue.var1 = System.Math.Round(objMeasuredDataGlobal.var1, 2);
            objDataValue.var1_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var2 = System.Math.Round(objMeasuredDataGlobal.var2, 2);
            objDataValue.var2_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var3 = System.Math.Round(objMeasuredDataGlobal.var3, 2);
            objDataValue.var3_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var4 = System.Math.Round(objMeasuredDataGlobal.var4, 2);
            objDataValue.var4_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var5 = System.Math.Round(objMeasuredDataGlobal.var5, 2);
            objDataValue.var5_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var6 = System.Math.Round(objMeasuredDataGlobal.var6, 2);
            objDataValue.var6_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var7 = System.Math.Round(objMeasuredDataGlobal.var7, 2);
            objDataValue.var7_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var8 = System.Math.Round(objMeasuredDataGlobal.var8, 2);
            objDataValue.var8_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var9 = System.Math.Round(objMeasuredDataGlobal.var9, 2);
            objDataValue.var9_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var10 = System.Math.Round(objMeasuredDataGlobal.var10, 2);
            objDataValue.var10_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var11 = System.Math.Round(objMeasuredDataGlobal.var11, 2);
            objDataValue.var11_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var12 = System.Math.Round(objMeasuredDataGlobal.var12, 2);
            objDataValue.var12_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var13 = System.Math.Round(objMeasuredDataGlobal.var13, 2);
            objDataValue.var13_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var14 = System.Math.Round(objMeasuredDataGlobal.var14, 2);
            objDataValue.var14_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var15 = System.Math.Round(objMeasuredDataGlobal.var15, 2);
            objDataValue.var15_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var16 = System.Math.Round(objMeasuredDataGlobal.var16, 2);
            objDataValue.var16_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var17 = System.Math.Round(objMeasuredDataGlobal.var17, 2);
            objDataValue.var17_status = objMeasuredDataGlobal.MPS_status;
            objDataValue.var18 = System.Math.Round(objMeasuredDataGlobal.var18, 2);
            objDataValue.var18_status = objMeasuredDataGlobal.MPS_status;

            objDataValue.MPS_status = objMeasuredDataGlobal.MPS_status;

            // Time
            objDataValue.stored_date = DateTime.Now;
            objDataValue.stored_hour = DateTime.Now.Hour;
            objDataValue.stored_minute = DateTime.Now.Minute;
            if (GlobalVar.isMaintenanceStatus && GlobalVar.maintenanceLog.pumping_system == 1)
            {
            }
            //// save to data value table
            if (new data_value_repository().add(ref objDataValue) > 0)
            {
                // ok --> add to 5 _minute data
                data_value objAdd5Minute = objCalCulationDataValue5Minute.addNewObjFor5Minute(objDataValue);
                if (objAdd5Minute != null)
                {
                    // add to 60 _minute data
                    objCalCulationDataValue60Minute.addNewObjFor60Minute(objAdd5Minute);
                }
                else
                {
                    // do nothing
                }
            }
            else
            {
                // fail
            }

            // cheking, calculating for saveving to datavalue 5 mintue table from current data


        }

        private void tmrThreadingTimerFor60Minute_TimerCallback(object state)
        {
            if (is_close_form)
            {
                try
                {
                    this.Close();
                    //MessageBox.Show("123");
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(Environment.ExitCode);
                    }
                }
                catch
                {

                }
            }
            // checking, calculating for save ving to data value 10 _minute table from 5 _minute data
        }
        private void tmrThreadingTimerForFTP_TimerCallback(object state)
        {
            try
            {
                push_server_repository s = new push_server_repository();
                List<push_server> listUser = s.get_all();
                //DateTime lastedPush = s.get_datetime_by_id(id);
                GlobalVar.stationSettings = new station_repository().get_info();

                /// Send File ftp	
                if (true)
                {
                    if (GlobalVar.stationSettings.ftpflag == 1)
                    {
                        if (Application.OpenForms.OfType<Form1>().Count() == 1)
                        {
                            //Application.Exit(Application.OpenForms.OfType<Form1>().First());
                            //Application.OpenForms.OfType<Form1>().First().;
                            Form1.control1.ClearTextBox(Form1.control1.getForm1fromControl, 1);
                        }                
                        //protocol = new Form1(frmConfiguration.newMain);
                        foreach (push_server push_server in listUser)
                        {
                            if (push_server.ftp_flag == 1)
                            {
                                if (ManualFTP(push_server, push_server.ftp_lasted, DateTime.Now))
                                {
                                }
                            }
                        }
                        //protocol.Show();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        private static void requestInfor(SerialPort com)
        {
            try
            {
                if (com.IsOpen)
                {
                    byte[] packet = new byte[9];
                    //Fill to packet
                    packet[0] = 0x02;//STX

                    packet[1] = 0x44;//D
                    packet[2] = 0x41;//A
                    packet[3] = 0x54;//T
                    packet[4] = 0x41;//A
                    packet[5] = 0x03;//ETX
                    packet[6] = 0x31;//CHK
                    packet[7] = 0x3F;//CHK
                    packet[8] = 0x0D;//CR

                    com.Write(packet, 0, 9);
                }
            }
            catch
            {

            }

        }

        #endregion

        #region Utility
        public string HEX_Coding(string aHex)
        {
            switch (aHex)
            {
                case "A":
                    return ":";

                case "B":
                    return ";";

                case "C":
                    return "<";

                case "D":
                    return "=";

                case "E":
                    return ">";

                case "F":
                    return "?";
            }
            return aHex;
        }
        private string Checksum(byte[] ByteArray)
        {
            int num = 0;
            int num2 = ByteArray.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                num += ByteArray[i];
            }
            num = num % 0x100;
            return (this.HEX_Coding(((int)(num / 0x10)).ToString("X")) + this.HEX_Coding(((int)(num % 0x10)).ToString("X")));
        }
        public static void SetValueTextbox(System.Windows.Forms.Control control, double text, string label)
        {
            if (control is TextBox)
            {
                TextBox tb = (TextBox)control;
                if (tb.Name.StartsWith(label))
                {
                    if (tb.InvokeRequired)
                    {
                        tb.Invoke(new MethodInvoker(delegate { tb.Text = text.ToString("##0.00"); }));
                    }
                }

            }
            else
            {
                foreach (System.Windows.Forms.Control child in control.Controls)
                {
                    SetValueTextbox(child, text, label);
                }
            }

        }
        public static void ClearLabel(System.Windows.Forms.Control control, string text, string label)
        {
            if (control is Label)
            {
                Label lbl = (Label)control;
                if (lbl.Name.StartsWith(label))
                    lbl.Text = text;

            }
            else
            {
                foreach (System.Windows.Forms.Control child in control.Controls)
                {
                    ClearLabel(child, text, label);
                }
            }

        }
        public static void ClearTextbox(System.Windows.Forms.Control control, string text, string label)
        {
            if (control is TextBox)
            {
                TextBox tb = (TextBox)control;
                if (tb.Name.StartsWith(label))
                    tb.Text = text;
            }
            else
            {
                foreach (System.Windows.Forms.Control child in control.Controls)
                {
                    ClearTextbox(child, text, label);
                }
            }
        }
        private static int getMinValueFromDatabinding(string code)
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
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            string sql_command1 = "SELECT * from " + "databinding";
                            cmd.CommandText = sql_command1;
                            NpgsqlDataReader dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr); // Load bang chua mapping cac truong
                            int min_value = -1;
                            foreach (DataRow row2 in tbcode.Rows)
                            {
                                if (Convert.ToString(row2["code"]).Equals(code))
                                {
                                    min_value = Convert.ToInt32(row2["min_value"]);
                                    break;
                                }
                            }
                            db.close_connection();
                            return min_value;
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return -1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
                return -1;
            }
        }
        private static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            try
            {
                foreach (byte b in data)
                    sb.Append(Convert.ToString(b, 16).PadLeft(2, '0') + "");
            }
            catch (Exception)
            {
                return "Error";
            }
            return sb.ToString().ToUpper();
        }

        private static Single ConvertHexToSingle(string hexVal)
        {
            try
            {
                int i = 0, j = 0;
                byte[] bArray = new byte[4];


                for (i = 0; i <= hexVal.Length - 1; i += 2)
                {
                    bArray[j] = Byte.Parse(hexVal[i].ToString() + hexVal[i + 1].ToString(), System.Globalization.NumberStyles.HexNumber);
                    j += 1;
                }
                Array.Reverse(bArray);
                Single s = BitConverter.ToSingle(bArray, 0);
                return (s);
            }
            catch (Exception ex)
            {
                throw new FormatException("The supplied hex value is either empty or in an incorrect format. Use the " +
                "following format: 00000000", ex);
            }
        }
        public static byte[] SubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        private static byte[] Combine(byte[] first, int first_length, byte[] second)
        {
            byte[] ret = new byte[first_length + second.Length];
            try
            {
                Buffer.BlockCopy(first, 0, ret, 0, first_length);
                Buffer.BlockCopy(second, 0, ret, first_length, second.Length);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("0003," + ex.Message);
            }


            return ret;
        }
        public Boolean iSAllMinValue(data_value data)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command1 = "SELECT * from " + "databinding";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command1;
                            NpgsqlDataReader dr;
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr); // Load bang chua mapping cac truong
                            int countNull = 0;
                            foreach (DataRow row2 in tbcode.Rows)
                            {
                                string code = Convert.ToString(row2["code"]);
                                int min_value = Convert.ToInt32(row2["min_value"]);
                                switch (code)
                                {
                                    case "var1":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var1)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var1)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var2":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var2)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var2)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var3":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var3)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var3)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var4":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var4)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var4)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var5":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var5)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var5)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var6":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var6)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var6)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var7":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var7)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var7)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var8":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var8)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var8)) != -1)
                                        {
                                        }
                                        else 
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var9":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var9)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var9)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var10":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var10)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var10)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var11":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var11)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var11)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var12":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var12)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var12)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var13":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var13)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var13)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var14":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var14)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var14)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var15":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var15)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var15)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var16":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var16)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var16)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var17":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var17)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var17)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                    case "var18":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var18)) >= min_value && Convert.ToDouble(String.Format("{0:0.00}", data.var18)) != -1)
                                        {
                                        }
                                        else
                                        {
                                            countNull++;
                                        }
                                        break;
                                }
                            }
                            if (countNull >= tbcode.Rows.Count)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                            db.close_connection();
                        }
                    }
                    else
                    {
                        db.close_connection();
                        return false;
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.StackTrace);
                    return false;
                }
            }
        }
        public void dataCSV(string firts, data_value data, string path, string date)
        {
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    var csv = new StringBuilder();
                    csv.Append(firts + "\t" + "");
                    csv.AppendLine();
                    //String connstring = "Server = localhost;Port = 5432; User Id = postgres;Password = 123;Database = DataLoggerDB";

                    if (db.open_connection())
                    {
                        string sql_command1 = "SELECT * from " + "databinding";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command1;
                            NpgsqlDataReader dr;
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr); // Load bang chua mapping cac truong
                            int countNull = 0;
                            foreach (DataRow row2 in tbcode.Rows)
                            {
                                string code = Convert.ToString(row2["code"]);
                                int min_value = Convert.ToInt32(row2["min_value"]);
                                switch (code)
                                {
                                    case "var1":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var1)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var1" + "\t" + String.Format("{0:0.00}", data.var1) );
                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var2":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var2)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var2" + "\t" + String.Format("{0:0.00}", data.var2) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var3":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var3)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var3" + "\t" + String.Format("{0:0.00}", data.var3) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var4":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var4)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var4" + "\t" + String.Format("{0:0.00}", data.var4) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var5":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var5)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var5" + "\t" + String.Format("{0:0.00}", data.var5) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var6":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var6)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var6" + "\t" + String.Format("{0:0.00}", data.var6) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var7":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var7)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var7" + "\t" + String.Format("{0:0.00}", data.var7) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var8":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var8)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var8" + "\t" + String.Format("{0:0.00}", data.var8) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var9":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var9)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var9" + "\t" + String.Format("{0:0.00}", data.var9) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var10":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var10)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var10" + "\t" + String.Format("{0:0.00}", data.var10) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var11":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var11)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var11" + "\t" + String.Format("{0:0.00}", data.var11) );
                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var12":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var12)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var12" + "\t" + String.Format("{0:0.00}", data.var12) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var13":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var13)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var13" + "\t" + String.Format("{0:0.00}", data.var13));

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var14":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var14)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var14" + "\t" + String.Format("{0:0.00}", data.var14));

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var15":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var15)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var15" + "\t" + String.Format("{0:0.00}", data.var15) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var16":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var6)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var16" + "\t" + String.Format("{0:0.00}", data.var16) );

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var17":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var17)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var17" + "\t" + String.Format("{0:0.00}", data.var17));

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                    case "var18":
                                        if (Convert.ToDouble(String.Format("{0:0.00}", data.var18)) >= min_value)
                                        {
                                            csv.Append(date + "\t" + "var18" + "\t" + String.Format("{0:0.00}", data.var18));

                                            csv.AppendLine();
                                        }
                                        countNull++;
                                        break;
                                }
                            }
                            if (countNull >= tbcode.Rows.Count)
                            {

                            }
                            using (StreamWriter swriter = new StreamWriter(path))
                            {
                                swriter.Write(csv.ToString());
                            }
                            db.close_connection();
                        }
                    }
                    else
                    {
                        db.close_connection();
                    }
                }
                catch (Exception e)
                {
                    db.close_connection();
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        //update lasted value
        public Boolean FTP(push_server push_server,data_value data, DateTime datetime)
        {
            try
            {
                GlobalVar.stationSettings = new station_repository().get_info();
                string stationID = GlobalVar.stationSettings.station_id;
                string stationName = GlobalVar.stationSettings.station_name;

                string server = push_server.ftp_ip;
                string username = push_server.ftp_username;
                string password = push_server.ftp_pwd;
                string folder = push_server.ftp_folder;

                String datetimeS = datetime.ToString("yyyyMMddHHmmss");
                string date = datetimeS.Substring(0, 4) + datetimeS.Substring(4, 2) + datetimeS.Substring(6, 2) + datetimeS.Substring(8, 2) + datetimeS.Substring(10, 2) + datetimeS.Substring(12, 2);
                //server = " \@" " + server + "\"" ;
                //ftp ftpClient = new ftp( @"ftp://127.0.0.1/", username, password);
                ftp ftpClient = new ftp(server, username, password);

                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                string csv = "push";

                //string tempFileName = "push.txt";
                string newFileName = stationID + "_" + stationName + "_" + date + ".txt";

                string yearFolder = datetimeS.Substring(0, 4);
                string monthFolder = datetimeS.Substring(4, 2);
                string dayFolder = datetimeS.Substring(6, 2);

                //string tempFilePath = Path.Combine(appPath, dataFolderName, tempFileName);
                string newFolderPath = Path.Combine(appPath, csv);
                string newFilePath = Path.Combine(appPath, csv, newFileName);

                /// Year Folder
                string[] simpleDirectoryYear = ftpClient.directoryListSimple(folder);
                Boolean hasFolderY = false;
                for (int i = 0; i < simpleDirectoryYear.Count(); i++)
                {
                    if (simpleDirectoryYear[i].Equals(yearFolder))
                    {
                        hasFolderY = true;
                    }
                }

                string folderPathY;
                if (hasFolderY == false)
                {
                    folderPathY = Path.Combine(folder, yearFolder);
                    ftpClient.CreateFTPDirectory(folderPathY);
                }
                else
                {
                    folderPathY = Path.Combine(folder, yearFolder);
                }
                ///
                /// Month Folder
                string[] simpleDirectoryMonth = ftpClient.directoryListSimple(folderPathY);
                Boolean hasFolderM = false;
                for (int i = 0; i < simpleDirectoryYear.Count(); i++)
                {
                    if (simpleDirectoryYear[i].Equals(monthFolder))
                    {
                        hasFolderM = true;
                    }
                }

                string folderPathM;
                if (hasFolderM == false)
                {
                    folderPathM = Path.Combine(folderPathY, monthFolder);
                    ftpClient.CreateFTPDirectory(folderPathM);
                }
                else
                {
                    folderPathM = Path.Combine(folderPathY, monthFolder);
                }
                /// 
                /// Day Folder
                string[] simpleDirectoryDay = ftpClient.directoryListSimple(folderPathM);
                Boolean hasFolderD = false;
                for (int i = 0; i < simpleDirectoryYear.Count(); i++)
                {
                    if (simpleDirectoryYear[i].Equals(dayFolder))
                    {
                        hasFolderD = true;
                    }
                }

                string folderPathD;
                if (hasFolderD == false)
                {
                    folderPathD = Path.Combine(folderPathM, dayFolder);
                    ftpClient.CreateFTPDirectory(folderPathD);
                }
                else
                {
                    folderPathD = Path.Combine(folderPathM, dayFolder);
                }
                /// 
                if (!Directory.Exists(newFolderPath))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(newFolderPath);
                }
                string header = stationID + "_" + stationName;
                if (!File.Exists(newFilePath))
                {
                    File.Create(newFilePath).Close();
                    dataCSV(header, data, newFilePath, date);
                }
                else
                {
                    System.IO.File.WriteAllText(newFilePath, string.Empty);
                    dataCSV(header, data, newFilePath, date);
                }
                /* Upload a File */
                //ftpClient.upload("/test/2017/data_report.csv", @"C:\Users\Admin\Desktop\data_report.csv");
                string filePath = Path.Combine(folderPathD, newFileName);
                ftpClient.upload(filePath, newFilePath);
                Form1.control1.AppendTextBox("Manual/Success " + newFileName + push_server.ftp_ip + Environment.NewLine, Form1.control1.getForm1fromControl, 1);
                return true;
            }
            catch (Exception e)
            {
                Form1.control1.AppendTextBox("Manual/Error " + push_server.ftp_ip + Environment.NewLine, Form1.control1.getForm1fromControl, 1);
                Form1.control1.AppendTextBox(e.StackTrace, Form1.control1.getForm1fromControl, 1);
                Form1.control1.AppendTextBox(e.Message, Form1.control1.getForm1fromControl, 1);
                return false;
            }
        }
        public Boolean FTP5Min(push_server push_server, data_value data)
        {
            string newFileName = null;
            try
            {
                GlobalVar.stationSettings = new station_repository().get_info();
                string stationID = GlobalVar.stationSettings.station_id;
                string stationName = GlobalVar.stationSettings.station_name;

                string server = push_server.ftp_ip;
                string username = push_server.ftp_username;
                string password = push_server.ftp_pwd;
                string folder = push_server.ftp_folder;

                DateTime s = DateTime.Now;
                String datetimeS = s.ToString("yyyyMMddHHmmss");
                string date = datetimeS.Substring(0, 4) + datetimeS.Substring(4, 2) + datetimeS.Substring(6, 2) + datetimeS.Substring(8, 2) + datetimeS.Substring(10, 2) + datetimeS.Substring(12, 2);
                //server = " \@" " + server + "\"" ;
                //ftp ftpClient = new ftp( @"ftp://127.0.0.1/", username, password);
                ftp ftpClient = new ftp(server, username, password);

                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                string csv = "push";

                //string tempFileName = "push.txt";
                newFileName = stationID + "_" + stationName + "_" + date + ".txt";

                string yearFolder = datetimeS.Substring(0, 4);
                string monthFolder = datetimeS.Substring(4, 2);
                string dayFolder = datetimeS.Substring(6, 2);

                //string tempFilePath = Path.Combine(appPath, dataFolderName, tempFileName);
                string newFolderPath = Path.Combine(appPath, csv);
                string newFilePath = Path.Combine(appPath, csv, newFileName);

                /// Year Folder
                string[] simpleDirectoryYear = ftpClient.directoryListSimple(folder);
                Boolean hasFolderY = false;
                for (int i = 0; i < simpleDirectoryYear.Count(); i++)
                {
                    if (simpleDirectoryYear[i].Equals(yearFolder))
                    {
                        hasFolderY = true;
                    }
                }

                string folderPathY;
                if (hasFolderY == false)
                {
                    folderPathY = Path.Combine(folder, yearFolder);
                    ftpClient.CreateFTPDirectory(folderPathY);
                }
                else
                {
                    folderPathY = Path.Combine(folder, yearFolder);
                }
                ///
                /// Month Folder
                string[] simpleDirectoryMonth = ftpClient.directoryListSimple(folderPathY);
                Boolean hasFolderM = false;
                for (int i = 0; i < simpleDirectoryYear.Count(); i++)
                {
                    if (simpleDirectoryYear[i].Equals(monthFolder))
                    {
                        hasFolderM = true;
                    }
                }

                string folderPathM;
                if (hasFolderM == false)
                {
                    folderPathM = Path.Combine(folderPathY, monthFolder);
                    ftpClient.CreateFTPDirectory(folderPathM);
                }
                else
                {
                    folderPathM = Path.Combine(folderPathY, monthFolder);
                }
                /// 
                /// Day Folder
                string[] simpleDirectoryDay = ftpClient.directoryListSimple(folderPathM);
                Boolean hasFolderD = false;
                for (int i = 0; i < simpleDirectoryYear.Count(); i++)
                {
                    if (simpleDirectoryYear[i].Equals(dayFolder))
                    {
                        hasFolderD = true;
                    }
                }

                string folderPathD;
                if (hasFolderD == false)
                {
                    folderPathD = Path.Combine(folderPathM, dayFolder);
                    ftpClient.CreateFTPDirectory(folderPathD);
                }
                else
                {
                    folderPathD = Path.Combine(folderPathM, dayFolder);
                }
                /// 
                if (!Directory.Exists(newFolderPath))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(newFolderPath);
                }
                string header = stationID + "_" + stationName;
                if (!File.Exists(newFilePath))
                {
                    File.Create(newFilePath).Close();
                    dataCSV(header, data, newFilePath, date);
                }
                else
                {
                    System.IO.File.WriteAllText(newFilePath, string.Empty);
                    dataCSV(header, data, newFilePath, date);
                }
                /* Upload a File */
                //ftpClient.upload("/test/2017/data_report.csv", @"C:\Users\Admin\Desktop\data_report.csv");
                string filePath = Path.Combine(folderPathD, newFileName);
                ftpClient.upload(filePath, newFilePath);
                Form1.control1.AppendTextBox("Auto/Success " + newFileName + push_server.ftp_ip + Environment.NewLine, Form1.control1.getForm1fromControl, 1);
                return true;
            }
            catch (Exception e)
            {
                Form1.control1.AppendTextBox("Auto/Error" + newFileName + push_server.ftp_ip + Environment.NewLine, Form1.control1.getForm1fromControl, 1);
                return false;
            }
        }
        public Boolean ManualFTP(push_server push_server,DateTime dtpDateFrom, DateTime dtpDateTo)
        {
            WinformProtocol.Control control = new WinformProtocol.Control();
            using (NpgsqlDBConnection db = new NpgsqlDBConnection())
            {
                try
                {
                    if (db.open_connection())
                    {
                        string sql_command1 = "SELECT * from " + "databinding";
                        using (NpgsqlCommand cmd = db._conn.CreateCommand())
                        {
                            cmd.CommandText = sql_command1;
                            NpgsqlDataReader dr;
                            dr = cmd.ExecuteReader();
                            DataTable tbcode = new DataTable();
                            tbcode.Load(dr); // Load bang chua mapping cac truong

                            List<string> _paramListForQuery = new List<string>();
                            List<string> _codeListForQuery = new List<string>();
                            List<string> _minListForQuery = new List<string>();

                            foreach (DataRow row2 in tbcode.Rows)
                            {
                                string code = Convert.ToString(row2["code"]);
                                _codeListForQuery.Add(code);
                                string clnnamevalue = Convert.ToString(row2["clnnamevalue"]);
                                _paramListForQuery.Add(clnnamevalue);
                                string min_value = Convert.ToString(row2["min_value"]);
                                _minListForQuery.Add(min_value);
                            }

                            _codeListForQuery.ToArray();
                            _paramListForQuery.ToArray();
                            
                            //get data from db 
                            DataTable dt_source = null;
                            dt_source = db5m.get_all_custom(dtpDateFrom, dtpDateTo, _paramListForQuery);
                            ////---------------------------------------------------------------------------------------
                            //foreach (DataRow delRow in dt_source.Rows)
                            //{
                            //    if (WinformProtocol.Control.getNullNo(delRow, tbcode) == 0)
                            //    {
                            //        delRow.Delete();
                            //    }
                            //}
                            //dt_source.AcceptChanges();
                            ////-----------------------------------------------------------------------------------------
                            foreach (DataRow row3 in dt_source.Rows)
                            {
                                frmNewMain newmain = new frmNewMain();
                                data_value data = new data_value();
                                //Type elementType = Type.GetType(_paramListForQuery[0]);
                                //Type listType = typeof(string).MakeGenericType(new Type[] { elementType });
                                //object list = Activator.CreateInstance(listType);
                                int id = Int32.Parse(Convert.ToString(row3["id"]));
                                int countNullParam = 0;
                                DateTime created = (DateTime)row3["created"];
                                data.created = created;

                                for (int i = 0; i < _paramListForQuery.Count; i++)
                                {
                                    var variable = Convert.ToDouble(String.Format("{0:0.00}", row3[_paramListForQuery[i]]));
                                    //string code = Convert.ToString(row3[_valueListForQuery[i]]);
                                    switch (_codeListForQuery[i])
                                    {
                                        case "var1":
                                            int var1 = getMinValueFromDatabinding("var1");
                                            if (variable >= var1 && variable != -1)
                                            {
                                                data.var1 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var2":
                                            int var2 = getMinValueFromDatabinding("var2");
                                            if (variable >= var2 && variable != -1)
                                            {
                                                data.var2 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var3":
                                            int var3 = getMinValueFromDatabinding("var3");
                                            if (variable >= var3 && variable != -1)
                                            {
                                                data.var3 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var4":
                                            int var4 = getMinValueFromDatabinding("var4");
                                            if (variable >= var4 && variable != -1)
                                            {
                                                data.var4 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var5":
                                            int var5 = getMinValueFromDatabinding("var5");
                                            if (variable >= var5 && variable != -1)
                                            {
                                                data.var5 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var6":
                                            int var6 = getMinValueFromDatabinding("var6");
                                            if (variable >= var6 && variable != -1)
                                            {
                                                data.var6 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var7":
                                            int var7 = getMinValueFromDatabinding("var7");
                                            if (variable >= var7 && variable != -1)
                                            {
                                                data.var7 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var8":
                                            int var8 = getMinValueFromDatabinding("var8");
                                            if (variable >= var8 && variable != -1)
                                            {
                                                data.var8 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var9":
                                            int var9 = getMinValueFromDatabinding("var9");
                                            if (variable >= var9 && variable != -1)
                                            {
                                                data.var9 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var10":
                                            int var10 = getMinValueFromDatabinding("var10");
                                            if (variable >= var10 && variable != -1)
                                            {
                                                data.var10 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var11":
                                            int var11 = getMinValueFromDatabinding("var11");
                                            if (variable >= var11 && variable != -1)
                                            {
                                                data.var11 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var12":
                                            int var12 = getMinValueFromDatabinding("var12");
                                            if (variable >= var12 && variable != -1)
                                            {
                                                data.var12 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var13":
                                            int var13 = getMinValueFromDatabinding("var13");
                                            if (variable >= var13 && variable != -1)
                                            {
                                                data.var13 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var14":
                                            int var14 = getMinValueFromDatabinding("var14");
                                            if (variable >= var14 && variable != -1)
                                            {
                                                data.var14 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var15":
                                            int var15 = getMinValueFromDatabinding("var15");
                                            if (variable >= var15 && variable != -1)
                                            {
                                                data.var15 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var16":
                                            int var16 = getMinValueFromDatabinding("var16");
                                            if (variable >= var16 && variable != -1)
                                            {
                                                data.var16 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var17":
                                            int var17 = getMinValueFromDatabinding("var17");
                                            if (variable >= var17 && variable != -1)
                                            {
                                                data.var17 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                        case "var18":
                                            int var18 = getMinValueFromDatabinding("var18");
                                            if (variable >= var18 && variable != -1)
                                            {
                                                data.var18 = variable;
                                            }
                                            else
                                            {
                                                countNullParam++;
                                            }
                                            break;
                                    }
                                }
                                if (countNullParam >= _paramListForQuery.Count)
                                {
                                    db5m.updatePush(id, 2, DateTime.Now);
                                    push_server_repository s = new push_server_repository();
                                    int idLasted = push_server.id;
                                    push_server set = new push_server();
                                    set.ftp_ip = push_server.ftp_ip;
                                    set.ftp_username = push_server.ftp_username;
                                    set.ftp_pwd = push_server.ftp_pwd;
                                    set.ftp_folder = push_server.ftp_folder;
                                    set.ftp_flag = push_server.ftp_flag;
                                    set.ftp_lasted = data.created;
                                    //int id = setre.get_id_by_key("lasted_push");
                                    s.update_with_id(ref set, idLasted);
                                }
                                else
                                {
                                    if (FTP(push_server,data, created))
                                    {
                                        db5m.updatePush(id, 1, DateTime.Now);
                                        //control1.AppendTextLog1Box();
                                        push_server_repository s = new push_server_repository();
                                        int idLasted = push_server.id;
                                        push_server set = new push_server();
                                        set.ftp_ip = push_server.ftp_ip;
                                        set.ftp_username = push_server.ftp_username;
                                        set.ftp_pwd = push_server.ftp_pwd;
                                        set.ftp_folder = push_server.ftp_folder;
                                        set.ftp_flag = push_server.ftp_flag;
                                        set.ftp_lasted = data.created;
                                        //int id = setre.get_id_by_key("lasted_push");
                                        s.update_with_id(ref set, idLasted);
                                    }
                                    else
                                    {
                                        db5m.updatePush(id, 0, DateTime.Now);
                                    }
                                }
                            }
                        }
                        Form1.control1.AppendTextBox("Lasted/Success " + "END" + Environment.NewLine, Form1.control1.getForm1fromControl, 1);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return false;
                }
            }
        }
        #endregion

        #region update data

        private double Calculator(double D, module mod)
        {
            double A;
            A = (double)((double)((double)(mod.output_min - mod.output_max) / (double)(mod.input_min - mod.input_max))
                * (double)(D - mod.input_min))
                + mod.output_min + mod.off_set;
            return A;
        }
        public void updateMeasuredDataValue(measured_data obj)
        {
            try
            {
                if (GlobalVar.isMaintenanceStatus)
                {
                    if (GlobalVar.maintenanceLog.mps == 1)
                    {
                        objMeasuredDataGlobal.MPS_status = INT_STATUS_MAINTENANCE;
                        obj.MPS_status = objMeasuredDataGlobal.MPS_status;
                    }
                }
                // check latest update communication
                if (DateTime.Compare(objMeasuredDataGlobal.latest_update_MPS_communication, DateTime.Now.AddSeconds(-PERIOD_CHECK_COMMUNICATION_ERROR)) < 0)
                {
                    objMeasuredDataGlobal.MPS_status = INT_STATUS_COMMUNICATION_ERROR;
                    obj.MPS_status = objMeasuredDataGlobal.MPS_status;
                    objMeasuredDataGlobal.var1 = -1;
                    objMeasuredDataGlobal.var2 = -1;
                    objMeasuredDataGlobal.var3 = -1;
                    objMeasuredDataGlobal.var4 = -1;
                    objMeasuredDataGlobal.var5 = -1;
                    objMeasuredDataGlobal.var6 = -1;
                    objMeasuredDataGlobal.var7 = -1;
                    objMeasuredDataGlobal.var8 = -1;
                    objMeasuredDataGlobal.var9 = -1;
                    objMeasuredDataGlobal.var10 = -1;
                    objMeasuredDataGlobal.var11 = -1;
                    objMeasuredDataGlobal.var12 = -1;
                    objMeasuredDataGlobal.var13 = -1;
                    objMeasuredDataGlobal.var14 = -1;
                    objMeasuredDataGlobal.var15 = -1;
                    objMeasuredDataGlobal.var16 = -1;
                    objMeasuredDataGlobal.var17 = -1;
                    objMeasuredDataGlobal.var18 = -1;
                }

                //txtvar6Value.Text = "---";
                //txtvar1Value.Text = "---";
                //txtvar4Value.Text = "---";
                //txtvar5Value.Text = "---";
                //txtvar2Value.Text = "---";
                //txtvar3Value.Text = "---";


                module_repository _modules = new module_repository();
                if (objMeasuredDataGlobal.MPS_status != INT_STATUS_COMMUNICATION_ERROR &&
                    objMeasuredDataGlobal.MPS_status != INT_STATUS_INSTRUMENT_ERROR &&
                    objMeasuredDataGlobal.MPS_status != INT_STATUS_EMPTY_SAMPLER_RESERVOIR)
                {
                    int var1 = getMinValueFromDatabinding("var1");
                    if (objMeasuredDataGlobal.var1 >= var1
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar1Value.Text = obj.var1.ToString("##0.00");
                        module objvar1 = _modules.get_info_by_name("var1");
                        if (objMeasuredDataGlobal.var1 > objvar1.error_max || objMeasuredDataGlobal.var1 < objvar1.error_min)
                        {
                            txtvar1Value.ForeColor = Color.Red;
                            txtvar1.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar1Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar1.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar1Value.Text = "Err";
                    }

                    int var2 = getMinValueFromDatabinding("var2");
                    if (objMeasuredDataGlobal.var2 >= var2
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar2Value.Text = obj.var2.ToString("##0.00");
                        module objvar2 = _modules.get_info_by_name("var2");
                        if (objMeasuredDataGlobal.var2 > objvar2.error_max || objMeasuredDataGlobal.var2 < objvar2.error_min)
                        {
                            txtvar2Value.ForeColor = Color.Red;
                            txtvar2.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar2Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar2.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar2Value.Text = "Err";
                    }

                    int var3 = getMinValueFromDatabinding("var3");
                    if (objMeasuredDataGlobal.var3 >= var3
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar3Value.Text = obj.var3.ToString("##0.00");
                        module objvar3 = _modules.get_info_by_name("var3");
                        if (objMeasuredDataGlobal.var3 > objvar3.error_max || objMeasuredDataGlobal.var3 < objvar3.error_min)
                        {
                            txtvar3Value.ForeColor = Color.Red;
                            txtvar3.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar3Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar3.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar3Value.Text = "Err";
                    }

                    int var4 = getMinValueFromDatabinding("var4");
                    if (objMeasuredDataGlobal.var4 >= var4
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar4Value.Text = obj.var4.ToString("##0.00");
                        module objvar4 = _modules.get_info_by_name("var4");
                        if (objMeasuredDataGlobal.var4 > objvar4.error_max || objMeasuredDataGlobal.var4 < objvar4.error_min)
                        {
                            txtvar4Value.ForeColor = Color.Red;
                            txtvar4.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar4Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar4.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar4Value.Text = "Err";
                    }

                    int var5 = getMinValueFromDatabinding("var5");
                    if (objMeasuredDataGlobal.var5 >= var5
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar5Value.Text = obj.var5.ToString("##0.00");
                        module objvar5 = _modules.get_info_by_name("var5");
                        if (objMeasuredDataGlobal.var5 > objvar5.error_max || objMeasuredDataGlobal.var5 < objvar5.error_min)
                        {
                            txtvar5Value.ForeColor = Color.Red;
                            txtvar5.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar5Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar5.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar5Value.Text = "Err";
                    }

                    int var6 = getMinValueFromDatabinding("var6");
                    if (objMeasuredDataGlobal.var6 >= var6
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar6Value.Text = obj.var6.ToString("##0.00");
                        module objvar6 = _modules.get_info_by_name("var6");
                        if (objMeasuredDataGlobal.var6 > objvar6.error_max || objMeasuredDataGlobal.var6 < objvar6.error_min)
                        {
                            txtvar6Value.ForeColor = Color.Red;
                            txtvar6.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar6Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar6.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar6Value.Text = "Err";
                    }

                    int var7 = getMinValueFromDatabinding("var7");
                    if (objMeasuredDataGlobal.var7 >= var7
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar7Value.Text = obj.var7.ToString("##0.00");
                        module objvar7 = _modules.get_info_by_name("var7");
                        if (objMeasuredDataGlobal.var7 > objvar7.error_max || objMeasuredDataGlobal.var7 < objvar7.error_min)
                        {
                            txtvar7Value.ForeColor = Color.Red;
                            txtvar7.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar7Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar7.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar7Value.Text = "Err";
                    }

                    int var8 = getMinValueFromDatabinding("var8");
                    if (objMeasuredDataGlobal.var8 >= var8
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar8Value.Text = obj.var8.ToString("##0.00");
                        module objvar8 = _modules.get_info_by_name("var8");
                        if (objMeasuredDataGlobal.var8 > objvar8.error_max || objMeasuredDataGlobal.var8 < objvar8.error_min)
                        {
                            txtvar8Value.ForeColor = Color.Red;
                            txtvar8.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar8Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar8.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar8Value.Text = "Err";
                    }

                    int var9 = getMinValueFromDatabinding("var9");
                    if (objMeasuredDataGlobal.var9 >= var9
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar9Value.Text = obj.var9.ToString("##0.00");
                        module objvar9 = _modules.get_info_by_name("var9");
                        if (objMeasuredDataGlobal.var9 > objvar9.error_max || objMeasuredDataGlobal.var9 < objvar9.error_min)
                        {
                            txtvar9Value.ForeColor = Color.Red;
                            txtvar9.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar9Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar9.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar9Value.Text = "Err";
                    }

                    int var10 = getMinValueFromDatabinding("var10");
                    if (objMeasuredDataGlobal.var10 >= var10
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar10Value.Text = obj.var10.ToString("##0.00");
                        module objvar10 = _modules.get_info_by_name("var10");
                        if (objMeasuredDataGlobal.var10 > objvar10.error_max || objMeasuredDataGlobal.var10 < objvar10.error_min)
                        {
                            txtvar10Value.ForeColor = Color.Red;
                            txtvar10.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar10Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar10.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar10Value.Text = "Err";
                    }

                    int var11 = getMinValueFromDatabinding("var11");
                    if (objMeasuredDataGlobal.var11 >= var11
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar11Value.Text = obj.var11.ToString("##0.00");
                        module objvar11 = _modules.get_info_by_name("var11");
                        if (objMeasuredDataGlobal.var11 > objvar11.error_max || objMeasuredDataGlobal.var11 < objvar11.error_min)
                        {
                            txtvar11Value.ForeColor = Color.Red;
                            txtvar11.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar11Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar11.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar11Value.Text = "Err";
                    }

                    int var12 = getMinValueFromDatabinding("var12");
                    if (objMeasuredDataGlobal.var12 >= var12
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar12Value.Text = obj.var12.ToString("##0.00");
                        module objvar12 = _modules.get_info_by_name("var12");
                        if (objMeasuredDataGlobal.var12 > objvar12.error_max || objMeasuredDataGlobal.var12 < objvar12.error_min)
                        {
                            txtvar12Value.ForeColor = Color.Red;
                            txtvar12.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar12Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar12.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar12Value.Text = "Err";
                    }

                    int var13 = getMinValueFromDatabinding("var13");
                    if (objMeasuredDataGlobal.var13 >= var13
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar13Value.Text = obj.var13.ToString("##0.00");
                        module objvar13 = _modules.get_info_by_name("var13");
                        if (objMeasuredDataGlobal.var13 > objvar13.error_max || objMeasuredDataGlobal.var13 < objvar13.error_min)
                        {
                            txtvar13Value.ForeColor = Color.Red;
                            txtvar13.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar13Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar13.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar13Value.Text = "Err";
                    }

                    int var14 = getMinValueFromDatabinding("var14");
                    if (objMeasuredDataGlobal.var14 >= var14
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar14Value.Text = obj.var14.ToString("##0.00");
                        module objvar14 = _modules.get_info_by_name("var14");
                        if (objMeasuredDataGlobal.var14 > objvar14.error_max || objMeasuredDataGlobal.var14 < objvar14.error_min)
                        {
                            txtvar14Value.ForeColor = Color.Red;
                            txtvar14.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar14Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar14.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar14Value.Text = "Err";
                    }

                    int var15 = getMinValueFromDatabinding("var15");
                    if (objMeasuredDataGlobal.var15 >= var15
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar15Value.Text = obj.var15.ToString("##0.00");
                        module objvar15 = _modules.get_info_by_name("var15");
                        if (objMeasuredDataGlobal.var15 > objvar15.error_max || objMeasuredDataGlobal.var15 < objvar15.error_min)
                        {
                            txtvar15Value.ForeColor = Color.Red;
                            txtvar15.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar15Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar15.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar15Value.Text = "Err";
                    }

                    int var16 = getMinValueFromDatabinding("var16");
                    if (objMeasuredDataGlobal.var16 >= var16
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar16Value.Text = obj.var16.ToString("##0.00");
                        module objvar16 = _modules.get_info_by_name("var16");
                        if (objMeasuredDataGlobal.var16 > objvar16.error_max || objMeasuredDataGlobal.var16 < objvar16.error_min)
                        {
                            txtvar16Value.ForeColor = Color.Red;
                            txtvar16.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar16Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar16.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar16Value.Text = "Err";
                    }

                    int var17 = getMinValueFromDatabinding("var17");
                    if (objMeasuredDataGlobal.var17 >= var17
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar17Value.Text = obj.var17.ToString("##0.00");
                        module objvar17 = _modules.get_info_by_name("var17");
                        if (objMeasuredDataGlobal.var17 > objvar17.error_max || objMeasuredDataGlobal.var17 < objvar17.error_min)
                        {
                            txtvar17Value.ForeColor = Color.Red;
                            txtvar17.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar17Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar17.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar17Value.Text = "Err";
                    }

                    int var18 = getMinValueFromDatabinding("var18");
                    if (objMeasuredDataGlobal.var18 >= var18
                            //getMinValueFromDatabinding("ec")
                            )
                    {
                        txtvar18Value.Text = obj.var18.ToString("##0.00");
                        module objvar18 = _modules.get_info_by_name("var18");
                        if (objMeasuredDataGlobal.var18 > objvar18.error_max || objMeasuredDataGlobal.var18 < objvar18.error_min)
                        {
                            txtvar18Value.ForeColor = Color.Red;
                            txtvar18.ForeColor = Color.Red;
                        }
                        else
                        {
                            txtvar18Value.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(160)))), ((int)(((byte)(186)))));
                            txtvar18.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                    else
                    {
                        txtvar18Value.Text = "Err";
                    }
                }


                switch (obj.MPS_status)
                {
                    case INT_STATUS_COMMUNICATION_ERROR:
                        this.picMPSStatus.BackgroundImage = global::DataLogger.Properties.Resources.Communication_Fault_status;
                        break;
                    case INT_STATUS_INSTRUMENT_ERROR:
                        this.picMPSStatus.BackgroundImage = global::DataLogger.Properties.Resources.Fault;
                        break;
                    case INT_STATUS_MAINTENANCE:
                        this.picMPSStatus.BackgroundImage = global::DataLogger.Properties.Resources.Maintenance_status;
                        break;
                    case INT_STATUS_NORMAL:
                        this.picMPSStatus.BackgroundImage = global::DataLogger.Properties.Resources.Normal_status;
                        break;
                    case INT_STATUS_MEASURING_STOP:
                        this.picMPSStatus.BackgroundImage = global::DataLogger.Properties.Resources.Fault;
                        break;
                    case INT_STATUS_CALIBRATING:
                        this.picMPSStatus.BackgroundImage = global::DataLogger.Properties.Resources.Calibration_status;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {

            }
            else
            {
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();
                if (!GlobalVar.isLogin)
                {
                    MessageBox.Show(lang.getText("login_before_to_do_this"));
                    return;
                }
            }
            frmConfiguration frmConfig = new frmConfiguration(lang, this);
            frmConfig.ShowDialog();
            initConfig(true);
        }

        private void frmNewMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                is_close_form = true;
                data_value obj = calculateImmediately5Minute();
                data_value obj60min = calculateImmediately60Minute();

                //MessageBox.Show("111");
                if (tmrThreadingTimer != null)
                {
                    tmrThreadingTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    tmrThreadingTimer.Dispose();
                }

                if (tmrThreadingTimerStationStatus != null)
                {
                    tmrThreadingTimerStationStatus.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    tmrThreadingTimerStationStatus.Dispose();
                }

                Process.GetCurrentProcess().Kill();

                //if (serialPortMPS != null && serialPortMPS.IsOpen)
                //{
                //    serialPortMPS.Close();
                //    serialPortMPS.Dispose();
                //}
                if (serialPortADAM != null && serialPortADAM.IsOpen)
                {
                    serialPortADAM.Close();
                    serialPortADAM.Dispose();
                }

                //MessageBox.Show("123");
                if (System.Windows.Forms.Application.MessageLoop)
                {
                    // WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Console app
                    System.Environment.Exit(Environment.ExitCode);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Process.GetCurrentProcess().Kill();
                //Environment.FailFast();
                //Application.Exit();
                //throw ex;
            }
        }
        private data_value calculateImmediately5Minute()
        {
            data_value obj = objCalCulationDataValue5Minute.addNewObjFor5Minute(null, true);

            if (obj == null)
            {
                obj = new data_5minute_value_repository().get_latest_info();
            }
            return obj;
        }
        private void btnMPS5Minute_Click(object sender, EventArgs e)
        {
            data_value obj = calculateImmediately5Minute();
            frm5MinuteMPS frm = new frm5MinuteMPS(obj, lang);
            frm.ShowDialog();
        }

        private data_value calculateImmediately60Minute()
        {
            data_value obj = objCalCulationDataValue5Minute.addNewObjFor60Minute(null, true);

            if (obj == null)
            {
                obj = new data_5minute_value_repository().get_latest_info();
            }
            return obj;
        }
        private void btnMPS1Hour_Click(object sender, EventArgs e)
        {
            data_value obj = calculateImmediately60Minute();
            frm1HourMPS frm = new frm1HourMPS(obj, lang);
            frm.ShowDialog();
        }


        private void btnMPSHistoryData_Click(object sender, EventArgs e)
        {
            frmHistoryMPS frm = new frmHistoryMPS(lang);
            frm.ShowDialog();
        }

        private void btnAllHistory_Click(object sender, EventArgs e)
        {
            frmHistoryAll frm = new frmHistoryAll(lang);
            frm.ShowDialog();
        }

        private void checkAllCommunication()
        {
            updateMeasuredDataValue(objMeasuredDataGlobal);
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {

            }
            else
            {
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();
                if (!GlobalVar.isLogin)
                {
                    MessageBox.Show(lang.getText("login_before_to_do_this"));
                    return;
                }
            }
            frmMaintenance objMaintenance = new frmMaintenance(lang);
            //this.Hide();
            objMaintenance.ShowDialog();
            //this.Show();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {

            }
            else
            {
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();
            }
            if (GlobalVar.isAdmin())
            {
                frmUserManagement frmUM = new frmUserManagement(lang);
                frmUM.ShowDialog();
            }
            else
            {
                MessageBox.Show(lang.getText("right_permission_error"));
            }


        }

        private void btnLoginLogout_Click(object sender, EventArgs e)
        {
            if (GlobalVar.isLogin)
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.logout;

                GlobalVar.isLogin = false;
                GlobalVar.loginUser = null;
            }
            else
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.login;
                frmLogin frm = new frmLogin(lang);
                frm.ShowDialog();

                if (GlobalVar.isLogin)
                {
                    this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.logout;
                }
            }
        }
        private void settingForLoginStatus()
        {
            if (GlobalVar.isLogin)
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.logout;
                setTextHeadingLogin("" + lang.getText("main_menu_welcome") + ", " + GlobalVar.loginUser.user_name + " !");
            }
            else
            {
                this.btnLoginLogout.BackgroundImage = global::DataLogger.Properties.Resources.login;
                setTextHeadingLogin("" + lang.getText("main_menu_welcome") + ", " + lang.getText("main_menu_guest") + " !");
            }
        }

        private void btnMonthlyReport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(lang.getText("monthly_report_yesno_question"), lang.getText("confirm"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                btnMonthlyReport.Enabled = false;
                vprgMonthlyReport.Value = 0;
                vprgMonthlyReport.Visible = true;

                bgwMonthlyReport.RunWorkerAsync();

                //Console.Write("1");
            }
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            switch_language();
            initConfig(false);
        }


        #region backgroundWorkerMonthlyReport
        private void backgroundWorkerMonthlyReport_DoWork(object sender, DoWorkEventArgs e)
        {
            //string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            //string dataFolderName = "data";

            //string tempFileName = "monthly_report_template.xlsx";
            //string newFileName = "MonthlyReport_" + DateTime.Now.ToString("yyyy (MMddHHmmssfff)");

            //string tempFilePath = Path.Combine(appPath, dataFolderName, tempFileName);
            //string newFilePath = Path.Combine(appPath, dataFolderName, newFileName);

            //if (File.Exists(tempFilePath))
            //{
            //    int year = DateTime.Now.Year;
            //    double dayOfYearTotal = (new DateTime(year, 12, 31)).DayOfYear;
            //    double dayOfYear = 0;
            //    int percent = 0;

            //    IEnumerable<data_value> allData = db60m.get_all_for_monthly_report(year);

            //    if (allData != null)
            //    {
            //        Excel.XLWorkbook oExcelWorkbook = new Excel.XLWorkbook(tempFilePath);
            //        // Excel.Application oExcelApp = new Excel.Application();
            //        // Excel.Workbook oExcelWorkbook = oExcelApp.Workbooks.Open(tempFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

            //        const int startRow = 5;
            //        int row;

            //        List<MonthlyReportInfo> mps_ph = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_orp = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_do = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_turbidity = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_ec = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> mps_temp = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> tn = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> tp = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> toc = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> refrigeration_temperature = new List<MonthlyReportInfo>();
            //        List<MonthlyReportInfo> bottle_position = new List<MonthlyReportInfo>();

            //        for (int month = 1; month <= 12; month++)
            //        {
            //            Excel.IXLWorksheet oExcelWorksheet = oExcelWorkbook.Worksheet(month) as Excel.IXLWorksheet;
            //            // Excel.IXLWorkSheet oExcelWorksheet = oExcelWorkbook.Worksheets[month] as Excel.Worksheet;

            //            //rename the Sheet name
            //            oExcelWorksheet.Name = (new DateTime(year, month, 1)).ToString("MMM-yy");
            //            oExcelWorksheet.Cell(2, 1).Value = "'" + (new DateTime(year, month, 1)).ToString("MM.");
            //            oExcelWorksheet.Cell(2, 17).Value = (new DateTime(year, month, 1)).ToString("MMM-yy");

            //            // calculate average value
            //            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            //            {
            //                // get maintenance by date (year, month, day)
            //                string strDate = year + "-" + month + "-" + day;
            //                IEnumerable<maintenance_log> onDateMaintenanceLogs = _maintenance_logs.get_all_by_date(strDate);
            //                // prepare data for maintenance
            //                string maintenance_operator_name = "";
            //                string maintenance_start_time = "";
            //                string maintenance_end_time = "";
            //                string maintenance_equipments = "";

            //                Color maintenance_color = StatusColorInfo.COL_STATUS_MAINTENANCE_PERIODIC;
            //                if (onDateMaintenanceLogs != null && onDateMaintenanceLogs.Count() > 0)
            //                {
            //                    foreach (maintenance_log itemMaintenanceLog in onDateMaintenanceLogs)
            //                    {
            //                        maintenance_operator_name += itemMaintenanceLog.name + ";";
            //                        maintenance_start_time += itemMaintenanceLog.start_time.ToString("HH")
            //                                                    + "h" + itemMaintenanceLog.start_time.ToString("mm") + ";";
            //                        maintenance_end_time += itemMaintenanceLog.end_time.ToString("HH")
            //                                                    + "h" + itemMaintenanceLog.end_time.ToString("mm") + ";";
            //                        if (itemMaintenanceLog.tn == 1)
            //                        {
            //                            maintenance_equipments += "TN;";
            //                        }
            //                        if (itemMaintenanceLog.tp == 1)
            //                        {
            //                            maintenance_equipments += "TP;";
            //                        }
            //                        if (itemMaintenanceLog.toc == 1)
            //                        {
            //                            maintenance_equipments += "TOC;";
            //                        }
            //                        if (itemMaintenanceLog.mps == 1)
            //                        {
            //                            maintenance_equipments += "MPS;";
            //                        }
            //                        if (itemMaintenanceLog.pumping_system == 1)
            //                        {
            //                            maintenance_equipments += "Pumping;";
            //                        }
            //                        if (itemMaintenanceLog.auto_sampler == 1)
            //                        {
            //                            maintenance_equipments += "AutoSampler;";
            //                        }
            //                        if (itemMaintenanceLog.other == 1)
            //                        {
            //                            maintenance_equipments += itemMaintenanceLog.other_para + ";";
            //                        }
            //                        if (itemMaintenanceLog.maintenance_reason == 1)
            //                        {
            //                            maintenance_color = StatusColorInfo.COL_STATUS_MAINTENANCE_INCIDENT;
            //                        }
            //                    }
            //                    maintenance_operator_name = maintenance_operator_name.Substring(0, maintenance_operator_name.Length - 1);
            //                    maintenance_start_time = maintenance_start_time.Substring(0, maintenance_start_time.Length - 1);
            //                    maintenance_end_time = maintenance_end_time.Substring(0, maintenance_end_time.Length - 1);
            //                    try
            //                    {
            //                        maintenance_equipments = maintenance_equipments.Substring(0, maintenance_equipments.Length - 1);
            //                    }
            //                    catch { }
            //                }

            //                IEnumerable<data_value> dayData = allData.Where(t => t.stored_date.Month == month && t.stored_date.Day == day);
            //                mps_ph.Clear();
            //                mps_orp.Clear();
            //                mps_do.Clear();
            //                mps_turbidity.Clear();
            //                mps_ec.Clear();
            //                mps_temp.Clear();
            //                tn.Clear();
            //                tp.Clear();
            //                toc.Clear();
            //                refrigeration_temperature.Clear();
            //                bottle_position.Clear();
            //                foreach (data_value item in dayData)
            //                {
            //                    mps_ph.AddNewDataValue(item.MPS_pH_status, item.MPS_pH);
            //                    mps_orp.AddNewDataValue(item.MPS_ORP_status, item.MPS_ORP);
            //                    mps_do.AddNewDataValue(item.MPS_DO_status, item.MPS_DO);
            //                    mps_turbidity.AddNewDataValue(item.MPS_Turbidity_status, item.MPS_Turbidity);
            //                    mps_ec.AddNewDataValue(item.MPS_EC_status, item.MPS_EC);
            //                    mps_temp.AddNewDataValue(item.MPS_Temp_status, item.MPS_Temp);
            //                    tn.AddNewDataValue(item.TN_status, item.TN);
            //                    tp.AddNewDataValue(item.TP_status, item.TP);
            //                    toc.AddNewDataValue(item.TOC_status, item.TOC);
            //                    refrigeration_temperature.AddNewDataValue(0, item.refrigeration_temperature);
            //                    bottle_position.AddNewDataValue(0, item.bottle_position);
            //                }

            //                // update to excel worksheet
            //                row = startRow + day;

            //                oExcelWorksheet.Cell(row, 2).Value = mps_ph.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 3).Value = mps_orp.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 4).Value = mps_do.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 5).Value = mps_turbidity.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 6).Value = mps_ec.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 7).Value = mps_temp.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 8).Value = tn.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 9).Value = tp.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 10).Value = toc.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 11).Value = refrigeration_temperature.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 12).Value = bottle_position.GetAverageOfMaxCountAsString();
            //                oExcelWorksheet.Cell(row, 14).Value = maintenance_operator_name;
            //                oExcelWorksheet.Cell(row, 15).Value = maintenance_start_time;
            //                oExcelWorksheet.Cell(row, 16).Value = maintenance_end_time;
            //                oExcelWorksheet.Cell(row, 17).Value = maintenance_equipments;


            //                oExcelWorksheet.Range("b" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_ph.GetStatusColor()));
            //                oExcelWorksheet.Range("c" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_orp.GetStatusColor()));
            //                oExcelWorksheet.Range("d" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_do.GetStatusColor()));
            //                oExcelWorksheet.Range("e" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_turbidity.GetStatusColor()));
            //                oExcelWorksheet.Range("f" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_ec.GetStatusColor()));
            //                oExcelWorksheet.Range("g" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(mps_temp.GetStatusColor()));
            //                oExcelWorksheet.Range("h" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(tn.GetStatusColor()));
            //                oExcelWorksheet.Range("i" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(tp.GetStatusColor()));
            //                oExcelWorksheet.Range("j" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(toc.GetStatusColor()));
            //                oExcelWorksheet.Range("k" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(refrigeration_temperature.GetStatusColor()));
            //                oExcelWorksheet.Range("l" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(bottle_position.GetStatusColor()));

            //                oExcelWorksheet.Range("n" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));
            //                oExcelWorksheet.Range("o" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));
            //                oExcelWorksheet.Range("p" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));
            //                oExcelWorksheet.Range("q" + row).Style.Fill.SetBackgroundColor(Excel.XLColor.FromColor(maintenance_color));

            //                dayOfYear = (new DateTime(year, month, day)).DayOfYear;
            //                percent = (int)(dayOfYear * 100d / dayOfYearTotal);
            //                bgwMonthlyReport.ReportProgress(percent);

            //                //Thread.Sleep(1);
            //            }
            //        }
            //        oExcelWorkbook.SaveAs(newFilePath + ".xlsx");
            //        //oExcelWorkbook.SaveAs(newFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            //    }
            //}
            //FileInfo fi = new FileInfo(newFilePath + ".xlsx");
            //if (fi.Exists)
            //{
            //    System.Diagnostics.Process.Start(newFilePath + ".xlsx");
            //}
            //else
            //{
            //    //file doesn't exist
            //}
        }

        private void backgroundWorkerMonthlyReport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vprgMonthlyReport.Value = e.ProgressPercentage;
        }

        private void backgroundWorkerMonthlyReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnMonthlyReport.Enabled = true;
            vprgMonthlyReport.Visible = false;

            if (!e.Cancelled && e.Error == null)
            {
                MessageBox.Show(lang.getText("successfully"));
            }
            else
            {

            }
        }

        #endregion backgroundWorkerMonthlyReport

        private void vprgMonthlyReport_Load(object sender, EventArgs e)
        {

        }
    }
    public class CalculationDataValue
    {
        public List<data_value> listDataValue = new List<data_value>();
        public int hour { get; set; }
        public int min_minute { get; set; } // start time
        public int max_minute { get; set; } // end time
        public DateTime latestCalculate5Minute = DateTime.Now;
        public DateTime latestCalculate60Minute = DateTime.Now;

        public CalculationDataValue()
        {
            hour = DateTime.Now.Hour;
            min_minute = DateTime.Now.Minute;
            max_minute = DateTime.Now.Minute;
        }
        public data_value addNewObjFor5Minute(data_value obj, bool isImmediatelyCalculation = false)
        {
            // checking execute transaction
            int tempHour = 0;
            if (obj != null)
            {
                tempHour = obj.created.Hour;
            }
            else
            {
                tempHour = DateTime.Now.Hour;

                if (DateTime.Compare(latestCalculate5Minute, DateTime.Now.AddSeconds(-40)) < 0)
                {
                    return null;
                }
            }

            int tempMinute = 0;
            if (obj != null)
            {
                tempMinute = obj.created.Minute;
            }
            else
            {
                tempMinute = DateTime.Now.Minute;
            }

            data_value objDataValue = null;
            data_value objLatest = null;
            int status = 0;
            if (listDataValue.Count > 0)
            {
                if ((tempHour != hour) || ((tempMinute - min_minute) > 1) || isImmediatelyCalculation)
                {
                    if (tempMinute % 5 == 0 || isImmediatelyCalculation)
                    {
                        // ok
                        // calculate and add to database
                        objDataValue = new data_value();
                        // MPS
                        objDataValue.var1 = listDataValue[0].var1;
                        objDataValue.var1_status = listDataValue[0].MPS_status;
                        objDataValue.var2 = listDataValue[0].var2;
                        objDataValue.var2_status = listDataValue[0].MPS_status;
                        objDataValue.var3 = listDataValue[0].var3;
                        objDataValue.var3_status = listDataValue[0].MPS_status;
                        objDataValue.var4 = listDataValue[0].var4;
                        objDataValue.var4_status = listDataValue[0].MPS_status;
                        objDataValue.var5 = listDataValue[0].var5;
                        objDataValue.var5_status = listDataValue[0].MPS_status;
                        objDataValue.var6 = listDataValue[0].var6;
                        objDataValue.var6_status = listDataValue[0].MPS_status;
                        objDataValue.var7 = listDataValue[0].var7;
                        objDataValue.var7_status = listDataValue[0].MPS_status;
                        objDataValue.var8 = listDataValue[0].var8;
                        objDataValue.var8_status = listDataValue[0].MPS_status;
                        objDataValue.var9 = listDataValue[0].var9;
                        objDataValue.var9_status = listDataValue[0].MPS_status;
                        objDataValue.var10 = listDataValue[0].var10;
                        objDataValue.var10_status = listDataValue[0].MPS_status;
                        objDataValue.var11 = listDataValue[0].var11;
                        objDataValue.var11_status = listDataValue[0].MPS_status;
                        objDataValue.var12 = listDataValue[0].var12;
                        objDataValue.var12_status = listDataValue[0].MPS_status;
                        objDataValue.var13 = listDataValue[0].var13;
                        objDataValue.var13_status = listDataValue[0].MPS_status;
                        objDataValue.var14 = listDataValue[0].var14;
                        objDataValue.var14_status = listDataValue[0].MPS_status;
                        objDataValue.var15 = listDataValue[0].var15;
                        objDataValue.var15_status = listDataValue[0].MPS_status;
                        objDataValue.var16 = listDataValue[0].var16;
                        objDataValue.var16_status = listDataValue[0].MPS_status;
                        objDataValue.var17 = listDataValue[0].var17;
                        objDataValue.var17_status = listDataValue[0].MPS_status;
                        objDataValue.var18 = listDataValue[0].var18;
                        objDataValue.var18_status = listDataValue[0].MPS_status;

                        objDataValue.MPS_status = listDataValue[0].MPS_status;

                        // Time
                        objDataValue.stored_date = listDataValue[0].stored_date;
                        objDataValue.stored_hour = hour;
                        objDataValue.stored_minute = (min_minute / 5) * 5;
                        int count = listDataValue.Count;

                        bool updateMPSFlag = true;
                        bool updateTNFlag = true;
                        bool updateTPFlag = true;
                        bool updateTOCFlag = true;
                        //bool updateStationStatus = true;
                        bool updateWaterSampler = true;
                        int countingMPSCal = 1;
                        int countingStationStatusCal = 1;
                        int countingTNCal = 1;
                        int countingTPCal = 1;
                        int countingTOCCal = 1;
                        int countingWaterSampler = 1;

                        for (int i = 1; i < count; i++)
                        {
                            // MPS
                            if (updateMPSFlag)
                            {
                                if (listDataValue[i].MPS_status == CommonInfo.INT_STATUS_NORMAL)
                                {
                                    objDataValue.var1 = objDataValue.var1 + listDataValue[i].var1;
                                    objDataValue.var1_status = listDataValue[i].MPS_status;
                                    objDataValue.var2 = objDataValue.var2 + listDataValue[i].var2;
                                    objDataValue.var2_status = listDataValue[i].MPS_status;
                                    objDataValue.var3 = objDataValue.var3 + listDataValue[i].var3;
                                    objDataValue.var3_status = listDataValue[i].MPS_status;
                                    objDataValue.var4 = objDataValue.var4 + listDataValue[i].var4;
                                    objDataValue.var4_status = listDataValue[i].MPS_status;
                                    objDataValue.var5 = objDataValue.var5 + listDataValue[i].var5;
                                    objDataValue.var5_status = listDataValue[i].MPS_status;
                                    objDataValue.var6 = objDataValue.var6 + listDataValue[i].var6;
                                    objDataValue.var6_status = listDataValue[i].MPS_status;
                                    objDataValue.var7 = objDataValue.var7 + listDataValue[i].var7;
                                    objDataValue.var7_status = listDataValue[i].MPS_status;
                                    objDataValue.var8 = objDataValue.var8 + listDataValue[i].var8;
                                    objDataValue.var8_status = listDataValue[i].MPS_status;
                                    objDataValue.var9 = objDataValue.var9 + listDataValue[i].var9;
                                    objDataValue.var9_status = listDataValue[i].MPS_status;
                                    objDataValue.var10 = objDataValue.var10 + listDataValue[i].var10;
                                    objDataValue.var10_status = listDataValue[i].MPS_status;
                                    objDataValue.var11 = objDataValue.var11 + listDataValue[i].var11;
                                    objDataValue.var11_status = listDataValue[i].MPS_status;
                                    objDataValue.var12 = objDataValue.var12 + listDataValue[i].var12;
                                    objDataValue.var12_status = listDataValue[i].MPS_status;
                                    objDataValue.var13 = objDataValue.var13 + listDataValue[i].var13;
                                    objDataValue.var13_status = listDataValue[i].MPS_status;
                                    objDataValue.var14 = objDataValue.var14 + listDataValue[i].var14;
                                    objDataValue.var14_status = listDataValue[i].MPS_status;
                                    objDataValue.var15 = objDataValue.var15 + listDataValue[i].var15;
                                    objDataValue.var15_status = listDataValue[i].MPS_status;
                                    objDataValue.var16 = objDataValue.var16 + listDataValue[i].var16;
                                    objDataValue.var16_status = listDataValue[i].MPS_status;
                                    objDataValue.var17 = objDataValue.var17 + listDataValue[i].var17;
                                    objDataValue.var17_status = listDataValue[i].MPS_status;
                                    objDataValue.var18 = objDataValue.var18 + listDataValue[i].var18;
                                    objDataValue.var18_status = listDataValue[i].MPS_status;

                                    objDataValue.MPS_status = listDataValue[i].MPS_status;
                                    countingMPSCal++;
                                }
                                else
                                {
                                    objDataValue.var1 = -1;
                                    objDataValue.var1_status = listDataValue[i].MPS_status;
                                    objDataValue.var2 = -1;
                                    objDataValue.var2_status = listDataValue[i].MPS_status;
                                    objDataValue.var3 = -1;
                                    objDataValue.var3_status = listDataValue[i].MPS_status;
                                    objDataValue.var4 = -1;
                                    objDataValue.var4_status = listDataValue[i].MPS_status;
                                    objDataValue.var5 = -1;
                                    objDataValue.var5_status = listDataValue[i].MPS_status;
                                    objDataValue.var6 = -1;
                                    objDataValue.var6_status = listDataValue[i].MPS_status;
                                    objDataValue.var7 = -1;
                                    objDataValue.var7_status = listDataValue[i].MPS_status;
                                    objDataValue.var8 = -1;
                                    objDataValue.var8_status = listDataValue[i].MPS_status;
                                    objDataValue.var9 = -1;
                                    objDataValue.var9_status = listDataValue[i].MPS_status;
                                    objDataValue.var10 = -1;
                                    objDataValue.var10_status = listDataValue[i].MPS_status;
                                    objDataValue.var11 = -1;
                                    objDataValue.var11_status = listDataValue[i].MPS_status;
                                    objDataValue.var12 = -1;
                                    objDataValue.var12_status = listDataValue[i].MPS_status;
                                    objDataValue.var13 = -1;
                                    objDataValue.var13_status = listDataValue[i].MPS_status;
                                    objDataValue.var14 = -1;
                                    objDataValue.var14_status = listDataValue[i].MPS_status;
                                    objDataValue.var15 = -1;
                                    objDataValue.var15_status = listDataValue[i].MPS_status;
                                    objDataValue.var16 = -1;
                                    objDataValue.var16_status = listDataValue[i].MPS_status;
                                    objDataValue.var17 = -1;
                                    objDataValue.var17_status = listDataValue[i].MPS_status;
                                    objDataValue.var18 = -1;
                                    objDataValue.var18_status = listDataValue[i].MPS_status;

                                    objDataValue.MPS_status = listDataValue[i].MPS_status;
                                    updateMPSFlag = false;
                                }
                            }

                        }
                        if (updateMPSFlag)
                        {
                            objDataValue.var1 = (double)objDataValue.var1 / (double)countingMPSCal;
                            objDataValue.var2 = (double)objDataValue.var2 / (double)countingMPSCal;
                            objDataValue.var3 = (double)objDataValue.var3 / (double)countingMPSCal;
                            objDataValue.var4 = (double)objDataValue.var4 / (double)countingMPSCal;
                            objDataValue.var5 = (double)objDataValue.var5 / (double)countingMPSCal;
                            objDataValue.var6 = (double)objDataValue.var6 / (double)countingMPSCal;
                            objDataValue.var7 = (double)objDataValue.var7 / (double)countingMPSCal;
                            objDataValue.var8 = (double)objDataValue.var8 / (double)countingMPSCal;
                            objDataValue.var9 = (double)objDataValue.var9 / (double)countingMPSCal;
                            objDataValue.var10 = (double)objDataValue.var10 / (double)countingMPSCal;
                            objDataValue.var11 = (double)objDataValue.var11 / (double)countingMPSCal;
                            objDataValue.var12 = (double)objDataValue.var12 / (double)countingMPSCal;
                            objDataValue.var13 = (double)objDataValue.var13 / (double)countingMPSCal;
                            objDataValue.var14 = (double)objDataValue.var14 / (double)countingMPSCal;
                            objDataValue.var15 = (double)objDataValue.var15 / (double)countingMPSCal;
                            objDataValue.var16 = (double)objDataValue.var16 / (double)countingMPSCal;
                            objDataValue.var17 = (double)objDataValue.var17 / (double)countingMPSCal;
                            objDataValue.var18 = (double)objDataValue.var18 / (double)countingMPSCal;
                        }

                        frmNewMain main = new frmNewMain();
                        // get latest to check before add
                        objLatest = new data_5minute_value_repository().get_latest_info();
                        GlobalVar.stationSettings = new station_repository().get_info();
                        if (objLatest != null &&
                            objLatest.created.Date == objDataValue.created.Date &&
                            objLatest.created.Month == objDataValue.created.Month &&
                            objLatest.created.Year == objDataValue.created.Year &&
                            objLatest.stored_hour == objDataValue.stored_hour &&
                            objLatest.stored_minute == objDataValue.stored_minute)
                        {
                            // update to
                            // MPS

                            if (objLatest.MPS_status == CommonInfo.INT_STATUS_NORMAL &&
                                objDataValue.MPS_status == CommonInfo.INT_STATUS_NORMAL)
                            {
                                objLatest.var1 = (objLatest.var1 + objDataValue.var1) / 2;
                                objLatest.var1_status = objDataValue.MPS_status;
                                objLatest.var2 = (objLatest.var2 + objDataValue.var2) / 2;
                                objLatest.var2_status = objDataValue.MPS_status;
                                objLatest.var3 = (objLatest.var3 + objDataValue.var3) / 2;
                                objLatest.var3_status = objDataValue.MPS_status;
                                objLatest.var4 = (objLatest.var4 + objDataValue.var4) / 2;
                                objLatest.var4_status = objDataValue.MPS_status;
                                objLatest.var5 = (objLatest.var5 + objDataValue.var5) / 2;
                                objLatest.var5_status = objDataValue.MPS_status;
                                objLatest.var6 = (objLatest.var6 + objDataValue.var6) / 2;
                                objLatest.var6_status = objDataValue.MPS_status;
                                objLatest.var7 = (objLatest.var7 + objDataValue.var7) / 2;
                                objLatest.var7_status = objDataValue.MPS_status;
                                objLatest.var8 = (objLatest.var8 + objDataValue.var8) / 2;
                                objLatest.var8_status = objDataValue.MPS_status;
                                objLatest.var9 = (objLatest.var9 + objDataValue.var9) / 2;
                                objLatest.var9_status = objDataValue.MPS_status;
                                objLatest.var10 = (objLatest.var10 + objDataValue.var10) / 2;
                                objLatest.var10_status = objDataValue.MPS_status;
                                objLatest.var11 = (objLatest.var11 + objDataValue.var11) / 2;
                                objLatest.var11_status = objDataValue.MPS_status;
                                objLatest.var12 = (objLatest.var12 + objDataValue.var12) / 2;
                                objLatest.var12_status = objDataValue.MPS_status;
                                objLatest.var13 = (objLatest.var13 + objDataValue.var13) / 2;
                                objLatest.var13_status = objDataValue.MPS_status;
                                objLatest.var14 = (objLatest.var14 + objDataValue.var14) / 2;
                                objLatest.var14_status = objDataValue.MPS_status;
                                objLatest.var15 = (objLatest.var15 + objDataValue.var15) / 2;
                                objLatest.var15_status = objDataValue.MPS_status;
                                objLatest.var16 = (objLatest.var16 + objDataValue.var16) / 2;
                                objLatest.var16_status = objDataValue.MPS_status;
                                objLatest.var17 = (objLatest.var17 + objDataValue.var17) / 2;
                                objLatest.var17_status = objDataValue.MPS_status;
                                objLatest.var18 = (objLatest.var18 + objDataValue.var18) / 2;
                                objLatest.var18_status = objDataValue.MPS_status;

                                objLatest.MPS_status = objDataValue.MPS_status;
                            }
                            else
                            {
                                objLatest.var1 = -1;
                                objLatest.var1_status = objLatest.MPS_status;
                                objLatest.var2 = -1;
                                objLatest.var2_status = objLatest.MPS_status;
                                objLatest.var3 = -1;
                                objLatest.var3_status = objLatest.MPS_status;
                                objLatest.var4 = -1;
                                objLatest.var4_status = objLatest.MPS_status;
                                objLatest.var5 = -1;
                                objLatest.var5_status = objLatest.MPS_status;
                                objLatest.var6 = -1;
                                objLatest.var6_status = objLatest.MPS_status;

                                objLatest.var7 = -1;
                                objLatest.var7_status = objLatest.MPS_status;
                                objLatest.var8 = -1;
                                objLatest.var8_status = objLatest.MPS_status;
                                objLatest.var9 = -1;
                                objLatest.var9_status = objLatest.MPS_status;
                                objLatest.var10 = -1;
                                objLatest.var10_status = objLatest.MPS_status;
                                objLatest.var11 = -1;
                                objLatest.var11_status = objLatest.MPS_status;
                                objLatest.var12 = -1;
                                objLatest.var12_status = objLatest.MPS_status;

                                objLatest.var13 = -1;
                                objLatest.var13_status = objLatest.MPS_status;
                                objLatest.var14 = -1;
                                objLatest.var14_status = objLatest.MPS_status;
                                objLatest.var15 = -1;
                                objLatest.var15_status = objLatest.MPS_status;
                                objLatest.var16 = -1;
                                objLatest.var16_status = objLatest.MPS_status;
                                objLatest.var17 = -1;
                                objLatest.var17_status = objLatest.MPS_status;
                                objLatest.var18 = -1;
                                objLatest.var18_status = objLatest.MPS_status;

                                objLatest.MPS_status = objLatest.MPS_status;
                                if (objDataValue.MPS_status != CommonInfo.INT_STATUS_NORMAL)
                                {
                                    objLatest.var1 = objDataValue.MPS_status;
                                    objLatest.var2 = objDataValue.MPS_status;
                                    objLatest.var3 = objDataValue.MPS_status;
                                    objLatest.var4 = objDataValue.MPS_status;
                                    objLatest.var5 = objDataValue.MPS_status;
                                    objLatest.var6 = objDataValue.MPS_status;

                                    objLatest.var7 = objDataValue.MPS_status;
                                    objLatest.var8 = objDataValue.MPS_status;
                                    objLatest.var9 = objDataValue.MPS_status;
                                    objLatest.var10 = objDataValue.MPS_status;
                                    objLatest.var11 = objDataValue.MPS_status;
                                    objLatest.var12 = objDataValue.MPS_status;

                                    objLatest.var13 = objDataValue.MPS_status;
                                    objLatest.var14 = objDataValue.MPS_status;
                                    objLatest.var15 = objDataValue.MPS_status;
                                    objLatest.var16 = objDataValue.MPS_status;
                                    objLatest.var17 = objDataValue.MPS_status;
                                    objLatest.var18 = objDataValue.MPS_status;

                                    objLatest.MPS_status = objDataValue.MPS_status;
                                }

                            }

                            ///
                            push_server_repository s = new push_server_repository();
                            List<push_server> listUser = s.get_all();
                            /// Send File ftp			
                            /// 
                            //iSAllMinValue(objLatest);
                            /// 
                            /// 
                            foreach (push_server push_server in listUser)
                            {
                                if (push_server.ftp_flag == 1)
                                {
                                    if (main.iSAllMinValue(objLatest))
                                    {
                                        if (
                                            //main.ManualFTP(lastedPush, DateTime.Now) && 
                                            main.FTP5Min(push_server,objLatest))
                                        {
                                            objLatest.push = 1;
                                            objLatest.push_time = DateTime.Now;
                                            ////setting_repository setre = new setting_repository();
                                            //setting set = new setting();
                                            //set.setting_key = "lasted_push";
                                            //set.setting_type = "";
                                            //set.setting_value = "";
                                            //set.note = "";
                                            //set.setting_datetime = objLatest.created;
                                            ////int id = setre.get_id_by_key("lasted_push");
                                            //s.update_with_id(ref set, id);
                                        }
                                        else
                                        {
                                            objLatest.push = 0;
                                            objLatest.push_time = DateTime.Now;
                                        }
                                    }
                                    else
                                    {
                                        objLatest.push = 2;
                                        objLatest.push_time = DateTime.Now;
                                        Form1.control1.AppendTextBox("Auto/Success : Error value " + Environment.NewLine, Form1.control1.getForm1fromControl, 1);
                                    }
                                }
                                else if (push_server.ftp_flag == 0)
                                {
                                    objLatest.push = 0;
                                    objLatest.push_time = new DateTime();
                                }
                            }
                            ///
                            //// save to data value table
                            if (new data_5minute_value_repository().update(ref objLatest) > 0)
                            {
                                // ok
                            }
                            else
                            {
                                // fail
                            }
                            status = 1; // update
                        }
                        else
                        {

                            if (GlobalVar.isMaintenanceStatus && GlobalVar.maintenanceLog.pumping_system == 1)
                            {
                                //objDataValue.pumping_system_status = CommonInfo.INT_STATUS_MAINTENANCE;
                                //objDataValue.station_status = CommonInfo.INT_STATUS_MAINTENANCE;
                            }
                            ///
                            push_server_repository s = new push_server_repository();
                            List<push_server> listUser = s.get_all();
                            /// Send File ftp	
                            /// 
                            foreach (push_server push_server in listUser)
                            {
                                if (push_server.ftp_flag == 1)
                                {
                                    if (main.iSAllMinValue(objDataValue))
                                    {
                                        if (
                                        //main.ManualFTP(lastedPush, DateTime.Now) && 
                                        main.FTP5Min(push_server,objDataValue))
                                        {
                                            objDataValue.push = 1;
                                            objDataValue.push_time = DateTime.Now;
                                            ////setting_repository setre = new setting_repository();
                                            //setting set = new setting();
                                            //set.setting_key = "lasted_push";
                                            //set.setting_type = "";
                                            //set.setting_value = "";
                                            //set.note = "";
                                            //set.setting_datetime = objDataValue.created;
                                            ////int id = setre.get_id_by_key("lasted_push");
                                            //s.update_with_id(ref set, id);
                                        }
                                        else
                                        {
                                            objDataValue.push = 0;
                                            objDataValue.push_time = DateTime.Now;
                                        }
                                    }
                                    else
                                    {
                                        objDataValue.push = 2;
                                        objDataValue.push_time = DateTime.Now;
                                        Form1.control1.AppendTextBox("Auto/Success : Error value " + Environment.NewLine, Form1.control1.getForm1fromControl, 1);
                                    }
                                }
                                else if (push_server.ftp_flag == 0)
                                {
                                    objDataValue.push = 0;
                                    objDataValue.push_time = new DateTime();
                                }
                            }
                            ///
                            //// save to data value table
                            if (new data_5minute_value_repository().add(ref objDataValue) > 0)
                            {
                                // ok
                            }
                            else
                            {
                                // fail
                            }
                            status = 2; // add
                        }
                        ////// save to data value table
                        //if (new data_5minute_value_repository().add(ref objDataValue) > 0)
                        //{
                        //    // ok
                        //}
                        //else
                        //{
                        //    // fail
                        //}
                        min_minute = tempMinute;
                        listDataValue.Clear();
                    }
                    else
                    {
                        // add to list
                    }
                }
                else
                {
                    // add to list
                }
            }
            latestCalculate5Minute = DateTime.Now;
            max_minute = tempMinute;
            hour = tempHour;
            if (obj != null)
            {
                listDataValue.Add(obj);
            }

            if (status == 0)
            {
                return null;
            }
            else if (status == 1)
            {
                return objLatest;
            }
            else
            {
                return objDataValue;
            }

        }
        public data_value addNewObjFor60Minute(data_value obj, bool isImmediatelyCalculation = false)
        {

            // checking execute transaction
            int tempHour = 0;
            if (obj != null)
            {
                tempHour = obj.created.Hour;
            }
            else
            {
                tempHour = DateTime.Now.Hour;
            }

            int tempMinute = 0;
            if (obj != null)
            {
                tempMinute = obj.created.Minute;
            }
            else
            {
                tempMinute = DateTime.Now.Minute;

                if (DateTime.Compare(latestCalculate60Minute, DateTime.Now.AddMinutes(-1)) < 0)
                {
                    return null;
                }
            }
            data_value objDataValue = null;
            data_value objLatest = null;
            int status = 0;

            if (listDataValue.Count > 0)
            {
                if ((tempHour != hour) || isImmediatelyCalculation)
                {
                    // ok
                    // calculate and add to database
                    objDataValue = new data_value();
                    objDataValue.MPS_status = listDataValue[0].MPS_status;
                    // MPS
                    objDataValue.var1 = listDataValue[0].var1;
                    objDataValue.var1_status = listDataValue[0].MPS_status;
                    objDataValue.var2 = listDataValue[0].var2;
                    objDataValue.var2_status = listDataValue[0].MPS_status;
                    objDataValue.var3 = listDataValue[0].var3;
                    objDataValue.var3_status = listDataValue[0].MPS_status;
                    objDataValue.var4 = listDataValue[0].var4;
                    objDataValue.var4_status = listDataValue[0].MPS_status;
                    objDataValue.var5 = listDataValue[0].var5;
                    objDataValue.var5_status = listDataValue[0].MPS_status;
                    objDataValue.var6 = listDataValue[0].var6;
                    objDataValue.var6_status = listDataValue[0].MPS_status;
                    objDataValue.var7 = listDataValue[0].var7;
                    objDataValue.var7_status = listDataValue[0].MPS_status;
                    objDataValue.var8 = listDataValue[0].var8;
                    objDataValue.var8_status = listDataValue[0].MPS_status;
                    objDataValue.var9 = listDataValue[0].var9;
                    objDataValue.var9_status = listDataValue[0].MPS_status;
                    objDataValue.var10 = listDataValue[0].var10;
                    objDataValue.var10_status = listDataValue[0].MPS_status;
                    objDataValue.var11 = listDataValue[0].var11;
                    objDataValue.var11_status = listDataValue[0].MPS_status;
                    objDataValue.var12 = listDataValue[0].var12;
                    objDataValue.var12_status = listDataValue[0].MPS_status;
                    objDataValue.var13 = listDataValue[0].var13;
                    objDataValue.var13_status = listDataValue[0].MPS_status;
                    objDataValue.var14 = listDataValue[0].var14;
                    objDataValue.var14_status = listDataValue[0].MPS_status;
                    objDataValue.var15 = listDataValue[0].var15;
                    objDataValue.var15_status = listDataValue[0].MPS_status;
                    objDataValue.var16 = listDataValue[0].var16;
                    objDataValue.var16_status = listDataValue[0].MPS_status;
                    objDataValue.var17 = listDataValue[0].var17;
                    objDataValue.var17_status = listDataValue[0].MPS_status;
                    objDataValue.var18 = listDataValue[0].var18;
                    objDataValue.var18_status = listDataValue[0].MPS_status;
                    // Time
                    objDataValue.stored_date = listDataValue[0].stored_date;
                    objDataValue.stored_hour = hour;
                    objDataValue.stored_minute = 0;
                    int count = listDataValue.Count;

                    bool updateMPSFlag = true;
                    bool updateTNFlag = true;
                    bool updateTPFlag = true;
                    bool updateTOCFlag = true;
                    //bool updateStationStatus = true;
                    bool updateWaterSampler = true;
                    int countingMPSCal = 1;
                    int countingStationStatusCal = 1;
                    int countingTNCal = 1;
                    int countingTPCal = 1;
                    int countingTOCCal = 1;
                    int countingWaterSampler = 1;

                    for (int i = 1; i < count; i++)
                    {
                        // MPS
                        if (updateMPSFlag)
                        {
                            if (listDataValue[i].MPS_status == CommonInfo.INT_STATUS_NORMAL)
                            {
                                objDataValue.var1 = objDataValue.var1 + listDataValue[i].var1;
                                objDataValue.var1_status = listDataValue[i].MPS_status;
                                objDataValue.var2 = objDataValue.var2 + listDataValue[i].var2;
                                objDataValue.var2_status = listDataValue[i].MPS_status;
                                objDataValue.var3 = objDataValue.var3 + listDataValue[i].var3;
                                objDataValue.var3_status = listDataValue[i].MPS_status;
                                objDataValue.var4 = objDataValue.var4 + listDataValue[i].var4;
                                objDataValue.var4_status = listDataValue[i].MPS_status;
                                objDataValue.var5 = objDataValue.var5 + listDataValue[i].var5;
                                objDataValue.var5_status = listDataValue[i].MPS_status;
                                objDataValue.var6 = objDataValue.var6 + listDataValue[i].var6;
                                objDataValue.var6_status = listDataValue[i].MPS_status;
                                objDataValue.var7 = objDataValue.var7 + listDataValue[i].var7;
                                objDataValue.var7_status = listDataValue[i].MPS_status;
                                objDataValue.var8 = objDataValue.var8 + listDataValue[i].var8;
                                objDataValue.var8_status = listDataValue[i].MPS_status;
                                objDataValue.var9 = objDataValue.var9 + listDataValue[i].var9;
                                objDataValue.var9_status = listDataValue[i].MPS_status;
                                objDataValue.var10 = objDataValue.var10 + listDataValue[i].var10;
                                objDataValue.var10_status = listDataValue[i].MPS_status;
                                objDataValue.var11 = objDataValue.var11 + listDataValue[i].var11;
                                objDataValue.var11_status = listDataValue[i].MPS_status;
                                objDataValue.var12 = objDataValue.var12 + listDataValue[i].var12;
                                objDataValue.var12_status = listDataValue[i].MPS_status;
                                objDataValue.var13 = objDataValue.var13 + listDataValue[i].var13;
                                objDataValue.var13_status = listDataValue[i].MPS_status;
                                objDataValue.var14 = objDataValue.var14 + listDataValue[i].var14;
                                objDataValue.var14_status = listDataValue[i].MPS_status;
                                objDataValue.var15 = objDataValue.var15 + listDataValue[i].var15;
                                objDataValue.var15_status = listDataValue[i].MPS_status;
                                objDataValue.var16 = objDataValue.var16 + listDataValue[i].var16;
                                objDataValue.var16_status = listDataValue[i].MPS_status;
                                objDataValue.var17 = objDataValue.var17 + listDataValue[i].var17;
                                objDataValue.var17_status = listDataValue[i].MPS_status;
                                objDataValue.var18 = objDataValue.var18 + listDataValue[i].var18;
                                objDataValue.var18_status = listDataValue[i].MPS_status;

                                objDataValue.MPS_status = listDataValue[i].MPS_status;
                                countingMPSCal++;
                            }
                            else
                            {
                                objDataValue.var1 = -1;
                                objDataValue.var1_status = listDataValue[i].MPS_status;
                                objDataValue.var2 = -1;
                                objDataValue.var2_status = listDataValue[i].MPS_status;
                                objDataValue.var3 = -1;
                                objDataValue.var3_status = listDataValue[i].MPS_status;
                                objDataValue.var4 = -1;
                                objDataValue.var4_status = listDataValue[i].MPS_status;
                                objDataValue.var5 = -1;
                                objDataValue.var5_status = listDataValue[i].MPS_status;
                                objDataValue.var6 = -1;
                                objDataValue.var6_status = listDataValue[i].MPS_status;
                                objDataValue.var7 = -1;
                                objDataValue.var7_status = listDataValue[i].MPS_status;
                                objDataValue.var8 = -1;
                                objDataValue.var8_status = listDataValue[i].MPS_status;
                                objDataValue.var9 = -1;
                                objDataValue.var9_status = listDataValue[i].MPS_status;
                                objDataValue.var10 = -1;
                                objDataValue.var10_status = listDataValue[i].MPS_status;
                                objDataValue.var11 = -1;
                                objDataValue.var11_status = listDataValue[i].MPS_status;
                                objDataValue.var12 = -1;
                                objDataValue.var12_status = listDataValue[i].MPS_status;
                                objDataValue.var13 = -1;
                                objDataValue.var13_status = listDataValue[i].MPS_status;
                                objDataValue.var14 = -1;
                                objDataValue.var14_status = listDataValue[i].MPS_status;
                                objDataValue.var15 = -1;
                                objDataValue.var15_status = listDataValue[i].MPS_status;
                                objDataValue.var16 = -1;
                                objDataValue.var16_status = listDataValue[i].MPS_status;
                                objDataValue.var17 = -1;
                                objDataValue.var17_status = listDataValue[i].MPS_status;
                                objDataValue.var18 = -1;
                                objDataValue.var18_status = listDataValue[i].MPS_status;

                                objDataValue.MPS_status = listDataValue[i].MPS_status;
                                updateMPSFlag = false;
                            }
                        }
                    }
                    if (updateMPSFlag)
                    {
                        objDataValue.var1 = (double)objDataValue.var1 / (double)countingMPSCal;
                        objDataValue.var2 = (double)objDataValue.var2 / (double)countingMPSCal;
                        objDataValue.var3 = (double)objDataValue.var3 / (double)countingMPSCal;
                        objDataValue.var4 = (double)objDataValue.var4 / (double)countingMPSCal;
                        objDataValue.var5 = (double)objDataValue.var5 / (double)countingMPSCal;
                        objDataValue.var6 = (double)objDataValue.var6 / (double)countingMPSCal;
                        objDataValue.var7 = (double)objDataValue.var7 / (double)countingMPSCal;
                        objDataValue.var8 = (double)objDataValue.var8 / (double)countingMPSCal;
                        objDataValue.var9 = (double)objDataValue.var9 / (double)countingMPSCal;
                        objDataValue.var10 = (double)objDataValue.var10 / (double)countingMPSCal;
                        objDataValue.var11 = (double)objDataValue.var11 / (double)countingMPSCal;
                        objDataValue.var12 = (double)objDataValue.var12 / (double)countingMPSCal;
                        objDataValue.var13 = (double)objDataValue.var13 / (double)countingMPSCal;
                        objDataValue.var14 = (double)objDataValue.var14 / (double)countingMPSCal;
                        objDataValue.var15 = (double)objDataValue.var15 / (double)countingMPSCal;
                        objDataValue.var16 = (double)objDataValue.var16 / (double)countingMPSCal;
                        objDataValue.var17 = (double)objDataValue.var17 / (double)countingMPSCal;
                        objDataValue.var18 = (double)objDataValue.var18 / (double)countingMPSCal;
                    }

                    // get latest to check before add
                    objLatest = new data_60minute_value_repository().get_latest_info();
                    if (objLatest != null &&
                        objLatest.created.Date == objDataValue.created.Date &&
                        objLatest.created.Month == objDataValue.created.Month &&
                        objLatest.created.Year == objDataValue.created.Year &&
                        objLatest.stored_hour == objDataValue.stored_hour &&
                        objLatest.stored_minute == objDataValue.stored_minute)
                    {
                        // update to
                        // MPS

                        if (objLatest.MPS_status == CommonInfo.INT_STATUS_NORMAL &&
                            objDataValue.MPS_status == CommonInfo.INT_STATUS_NORMAL)
                        {
                            objLatest.var1 = (objLatest.var1 + objDataValue.var1) / 2;
                            objLatest.var1_status = objDataValue.MPS_status;
                            objLatest.var2 = (objLatest.var2 + objDataValue.var2) / 2;
                            objLatest.var2_status = objDataValue.MPS_status;
                            objLatest.var3 = (objLatest.var3 + objDataValue.var3) / 2;
                            objLatest.var3_status = objDataValue.MPS_status;
                            objLatest.var4 = (objLatest.var4 + objDataValue.var4) / 2;
                            objLatest.var4_status = objDataValue.MPS_status;
                            objLatest.var5 = (objLatest.var5 + objDataValue.var5) / 2;
                            objLatest.var5_status = objDataValue.MPS_status;
                            objLatest.var6 = (objLatest.var6 + objDataValue.var6) / 2;
                            objLatest.var6_status = objDataValue.MPS_status;
                            objLatest.var7 = (objLatest.var7 + objDataValue.var7) / 2;
                            objLatest.var7_status = objDataValue.MPS_status;
                            objLatest.var8 = (objLatest.var8 + objDataValue.var8) / 2;
                            objLatest.var8_status = objDataValue.MPS_status;
                            objLatest.var9 = (objLatest.var9 + objDataValue.var9) / 2;
                            objLatest.var9_status = objDataValue.MPS_status;
                            objLatest.var10 = (objLatest.var10 + objDataValue.var10) / 2;
                            objLatest.var10_status = objDataValue.MPS_status;
                            objLatest.var11 = (objLatest.var11 + objDataValue.var11) / 2;
                            objLatest.var11_status = objDataValue.MPS_status;
                            objLatest.var12 = (objLatest.var12 + objDataValue.var12) / 2;
                            objLatest.var12_status = objDataValue.MPS_status;
                            objLatest.var13 = (objLatest.var13 + objDataValue.var13) / 2;
                            objLatest.var13_status = objDataValue.MPS_status;
                            objLatest.var14 = (objLatest.var14 + objDataValue.var14) / 2;
                            objLatest.var14_status = objDataValue.MPS_status;
                            objLatest.var15 = (objLatest.var15 + objDataValue.var15) / 2;
                            objLatest.var15_status = objDataValue.MPS_status;
                            objLatest.var16 = (objLatest.var16 + objDataValue.var16) / 2;
                            objLatest.var16_status = objDataValue.MPS_status;
                            objLatest.var17 = (objLatest.var17 + objDataValue.var17) / 2;
                            objLatest.var17_status = objDataValue.MPS_status;
                            objLatest.var18 = (objLatest.var18 + objDataValue.var18) / 2;
                            objLatest.var18_status = objDataValue.MPS_status;

                            objLatest.MPS_status = objDataValue.MPS_status;
                        }
                        else
                        {
                            objLatest.var1 = -1;
                            objLatest.var1_status = objLatest.MPS_status;
                            objLatest.var2 = -1;
                            objLatest.var2_status = objLatest.MPS_status;
                            objLatest.var3 = -1;
                            objLatest.var3_status = objLatest.MPS_status;
                            objLatest.var4 = -1;
                            objLatest.var4_status = objLatest.MPS_status;
                            objLatest.var5 = -1;
                            objLatest.var5_status = objLatest.MPS_status;
                            objLatest.var6 = -1;
                            objLatest.var6_status = objLatest.MPS_status;

                            objLatest.var7 = -1;
                            objLatest.var7_status = objLatest.MPS_status;
                            objLatest.var8 = -1;
                            objLatest.var8_status = objLatest.MPS_status;
                            objLatest.var9 = -1;
                            objLatest.var9_status = objLatest.MPS_status;
                            objLatest.var10 = -1;
                            objLatest.var10_status = objLatest.MPS_status;
                            objLatest.var11 = -1;
                            objLatest.var11_status = objLatest.MPS_status;
                            objLatest.var12 = -1;
                            objLatest.var12_status = objLatest.MPS_status;

                            objLatest.var13 = -1;
                            objLatest.var13_status = objLatest.MPS_status;
                            objLatest.var14 = -1;
                            objLatest.var14_status = objLatest.MPS_status;
                            objLatest.var15 = -1;
                            objLatest.var15_status = objLatest.MPS_status;
                            objLatest.var16 = -1;
                            objLatest.var16_status = objLatest.MPS_status;
                            objLatest.var17 = -1;
                            objLatest.var17_status = objLatest.MPS_status;
                            objLatest.var18 = -1;
                            objLatest.var18_status = objLatest.MPS_status;

                            objLatest.MPS_status = objLatest.MPS_status;
                            if (objDataValue.MPS_status != CommonInfo.INT_STATUS_NORMAL)
                            {
                                objLatest.var1 = objDataValue.MPS_status;
                                objLatest.var2 = objDataValue.MPS_status;
                                objLatest.var3 = objDataValue.MPS_status;
                                objLatest.var4 = objDataValue.MPS_status;
                                objLatest.var5 = objDataValue.MPS_status;
                                objLatest.var6 = objDataValue.MPS_status;

                                objLatest.var7 = objDataValue.MPS_status;
                                objLatest.var8 = objDataValue.MPS_status;
                                objLatest.var9 = objDataValue.MPS_status;
                                objLatest.var10 = objDataValue.MPS_status;
                                objLatest.var11 = objDataValue.MPS_status;
                                objLatest.var12 = objDataValue.MPS_status;

                                objLatest.var13 = objDataValue.MPS_status;
                                objLatest.var14 = objDataValue.MPS_status;
                                objLatest.var15 = objDataValue.MPS_status;
                                objLatest.var16 = objDataValue.MPS_status;
                                objLatest.var17 = objDataValue.MPS_status;
                                objLatest.var18 = objDataValue.MPS_status;

                                objLatest.MPS_status = objDataValue.MPS_status;
                            }

                        }
                        //// save to data value table
                        if (new data_60minute_value_repository().update(ref objLatest) > 0)
                        {
                            // ok
                        }
                        else
                        {
                            // fail
                        }
                        status = 1;
                    }
                    else
                    {
                        if (GlobalVar.isMaintenanceStatus && GlobalVar.maintenanceLog.pumping_system == 1)
                        {
                        }
                        //// save to data value table
                        if (new data_60minute_value_repository().add(ref objDataValue) > 0)
                        {
                            // ok
                        }
                        else
                        {
                            // fail
                        }
                        status = 2;
                    }

                    min_minute = tempMinute;
                    listDataValue.Clear();
                }
                else
                {
                    // add to list
                }

            }
            latestCalculate60Minute = DateTime.Now;
            max_minute = tempMinute;
            hour = tempHour;
            if (obj != null)
            {
                listDataValue.Add(obj);
            }
            if (status == 0)
            {
                return null;
            }
            else if (status == 1)
            {
                return objLatest;
            }
            else
            {
                return objDataValue;
            }
        }
    }
    public class ReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; set; }
    }
}
