using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Resources;
using System.Reflection;
using System.Globalization;
using DataLogger.Utils;
using System.IO.Ports;

using DataLogger.Entities;
using DataLogger.Data;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using WinformProtocol;

namespace DataLogger
{
    public partial class frmConfiguration : Form
    {
        LanguageService lang;
        // 1: KECO_STD; 2: ANALYZER; 3: MODBUS
        public string[] PROTOCOL_LIST = { "KECO_STD", "ANALYZER", "MODBUS" };

        SerialPort SAMPPort;
        frmNewMain newMain;
        // module configuration list
        public string[] MODULE_CONFIG_LIST = {"var1", "var2", "var3", "var4", "var5", "var6",
                                              "var7", "var8", "var9", "var10", "var11", "var12",
                                              "var13", "var14", "var15", "var16", "var17", "var18"
                                                };
        public string[] MODULE_ID_LIST = { "40171", "4050", "4051", "40172" };

        DataTable dt = new DataTable();

        module_repository _modules = new module_repository();

        public static Form1 protocol;
        public frmConfiguration()
        {
            InitializeComponent();
        }

        public frmConfiguration(LanguageService _lang,frmNewMain newmain)
        {
            InitializeComponent();
            lang = _lang;
            switch_language();
            //SAMPPort = SAMP;
            newMain = newmain;
        }
        private void switch_language()
        {
            this.Text = lang.getText("configuration");
            //this.grbStationConfig.Text = lang.getText("station_configuration");
            //this.grbComportConfiguration.Text = lang.getText("comport_configuration");
            //this.grbModuleConfiguration.Text = lang.getText("module_configuration");
            // statoin configuration            
            lang.setText(lblStationName, "station_name");
            lang.setText(lblStationID, "station_id");
            lang.setText(lblSocketPort, "socket_port");
            // preview                                    
            lang.setText(btnSave, "button_save");

        }
        private void frmConfiguration_Load(object sender, EventArgs e)
        {
            refreshDataForControl();
        }
        private bool isOpen(int port)
        {

            //int port = 456; //<--- This is your value
            int isAvailable = 0;
            // Evaluate current system tcp connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();
            foreach (IPEndPoint tcpi in tcpConnInfoArray)
            {
                if (tcpi.Port == port)
                {
                    isAvailable = 1;
                    break;
                }
            }
            if (isAvailable == 1)
            {
                return true;
            }
            else return false;
        }

        private void refreshDataForControl()
        {
            // get all comport name from computer.
            string[] availableComportList;
            availableComportList = SerialPort.GetPortNames();
            module_repository modules = new module_repository();
            IEnumerable<module> moduleConfigList = modules.get_all();
            MODULE_CONFIG_LIST = moduleConfigList.Select(x => x.item_name).ToArray();

            if (availableComportList.Length <= 0)
            {
                MessageBox.Show(lang.getText("available_port_null"));
                //this.Close();
                //return;
            }
            // check station info setting
            station existedStationsSetting = new station_repository().get_info();
            if (existedStationsSetting == null)
            {
                existedStationsSetting = new station();
                if (new station_repository().add(ref existedStationsSetting) > 0)
                {
                    // insert ok to database
                }
                else
                {
                    MessageBox.Show(lang.getText("system_error"));
                    return;
                }
            }
            else
            {
                // set data to control from existed station setting
                txtStationName.Text = existedStationsSetting.station_name;
                txtStationID.Text = existedStationsSetting.station_id;
                txtSocketPort.Text = existedStationsSetting.socket_port.ToString();
                if (isOpen(existedStationsSetting.socket_port))
                {
                    this.btnSOCKET.Image = global::DataLogger.Properties.Resources.ON_switch_96x25;
                    btnShow.Enabled = true;
                }
                else
                {
                    this.btnSOCKET.Image = global::DataLogger.Properties.Resources.OFF_switch_96x25;
                    btnShow.Enabled = false;
                }
                var sortComportList = availableComportList.OrderBy(port => Convert.ToInt32(port.Replace("COM", string.Empty)));
                int selectedIndex = 0;
                cbModule.Items.Clear();

                var1Module.Items.Clear();
                var2Module.Items.Clear();
                var3Module.Items.Clear();
                var4Module.Items.Clear();
                var5Module.Items.Clear();
                var6Module.Items.Clear();
                foreach (string itemAvailableComportName in sortComportList)
                {
                    cbModule.Items.Add(itemAvailableComportName);


                    if (existedStationsSetting.module_comport == itemAvailableComportName)
                    {
                        cbModule.SelectedIndex = selectedIndex;
                    }
 
                    selectedIndex = selectedIndex + 1;
                }

            }

            foreach (string itemModuleID in MODULE_ID_LIST)
            {
                var1Module.Items.Add(itemModuleID);
                var2Module.Items.Add(itemModuleID);
                var3Module.Items.Add(itemModuleID);
                var4Module.Items.Add(itemModuleID);
                var5Module.Items.Add(itemModuleID);
                var6Module.Items.Add(itemModuleID);
                var7Module.Items.Add(itemModuleID);
                var8Module.Items.Add(itemModuleID);
                var9Module.Items.Add(itemModuleID);
                var10Module.Items.Add(itemModuleID);
                var11Module.Items.Add(itemModuleID);
                var12Module.Items.Add(itemModuleID);
                var13Module.Items.Add(itemModuleID);
                var14Module.Items.Add(itemModuleID);
                var15Module.Items.Add(itemModuleID);
                var16Module.Items.Add(itemModuleID);
                var17Module.Items.Add(itemModuleID);
                var18Module.Items.Add(itemModuleID);
            }

            IEnumerable<module> moduleConfigurationList = checkAndInsertModuleConfiguration();
            foreach (module itemModuleSetting in moduleConfigurationList)
            {
                displayModuleSetting(itemModuleSetting);
            }

         
        }

        private void displayModuleSetting(module itemModuleSetting)
        {
            string channel_no = itemModuleSetting.channel_number.ToString("0#");
            int module_id = itemModuleSetting.module_id - 1;
            string on = itemModuleSetting.on_value;
            string off = itemModuleSetting.off_value;
            int input_min = itemModuleSetting.input_min;
            int input_max = itemModuleSetting.input_max;
            int output_min = itemModuleSetting.output_min;
            int output_max = itemModuleSetting.output_max;
            int error_min = itemModuleSetting.error_min;
            int error_max = itemModuleSetting.error_max;
            double offset = itemModuleSetting.off_set;
            var unit = itemModuleSetting.unit;
            var value_column = itemModuleSetting.value_column;
            var status_column = itemModuleSetting.status_column;
            var type_value = itemModuleSetting.type_value;
            var display_name = itemModuleSetting.display_name;
            switch (itemModuleSetting.item_name.ToLower())
            {
                case "var1":
                    var1Channel.Text = channel_no;
                    var1Module.SelectedIndex = module_id;
                    var1InputMin.Text = input_min.ToString();
                    var1InputMax.Text = input_max.ToString();
                    var1OutputMin.Text = output_min.ToString();
                    var1OutputMax.Text = output_max.ToString();

                    var1DisplayName.Text = display_name.ToString();
                    var1Unit.Text = unit.ToString();
                    var1StatusColumn.Text = status_column.ToString();
                    var1Type.Text = type_value.ToString();
                    var1ValueColumn.Text = value_column.ToString();

                    txtpHErrorMin.Text = error_min.ToString();
                    txtpHErrorMax.Text = error_max.ToString();

                    var1Offset.Text = offset.ToString();
                    break;
                case "var2":
                    var2Channel.Text = channel_no;
                    var2Module.SelectedIndex = module_id;
                    var2InputMin.Text = input_min.ToString();
                    var2InputMax.Text = input_max.ToString();
                    var2OutputMin.Text = output_min.ToString();
                    var2OutputMax.Text = output_max.ToString();

                    var2DisplayName.Text = display_name.ToString();
                    var2Unit.Text = unit.ToString();
                    var2StatusColumn.Text = status_column.ToString();
                    var2Type.Text = type_value.ToString();
                    var2ValueColumn.Text = value_column.ToString();

                    txtOrpErrorMin.Text = error_min.ToString();
                    txtOrpErrorMax.Text = error_max.ToString();

                    var2Offset.Text = offset.ToString();
                    break;
                case "var3":
                    var3Channel.Text = channel_no;
                    var3Module.SelectedIndex = module_id;
                    var3InputMin.Text = input_min.ToString();
                    var3InputMax.Text = input_max.ToString();
                    var3OutputMin.Text = output_min.ToString();
                    var3OutputMax.Text = output_max.ToString();

                    var3DisplayName.Text = display_name.ToString();
                    var3Unit.Text = unit.ToString();
                    var3StatusColumn.Text = status_column.ToString();
                    var3Type.Text = type_value.ToString();
                    var3ValueColumn.Text = value_column.ToString();

                    txtTempErrorMin.Text = error_min.ToString();
                    txtTempErrorMax.Text = error_max.ToString();

                    var3Offset.Text = offset.ToString();
                    break;
                case "var4":
                    var4Channel.Text = channel_no;
                    var4Module.SelectedIndex = module_id;
                    var4InputMin.Text = input_min.ToString();
                    var4InputMax.Text = input_max.ToString();
                    var4OutputMin.Text = output_min.ToString();
                    var4OutputMax.Text = output_max.ToString();

                    var4DisplayName.Text = display_name.ToString();
                    var4Unit.Text = unit.ToString();
                    var4StatusColumn.Text = status_column.ToString();
                    var4Type.Text = type_value.ToString();
                    var4ValueColumn.Text = value_column.ToString();

                    txtDoErrorMin.Text = error_min.ToString();
                    txtDoErrorMax.Text = error_max.ToString();

                    var4Offset.Text = offset.ToString();
                    break;
                case "var5":
                    var5Channel.Text = channel_no;
                    var5Module.SelectedIndex = module_id;
                    var5InputMin.Text = input_min.ToString();
                    var5InputMax.Text = input_max.ToString();
                    var5OutputMin.Text = output_min.ToString();
                    var5OutputMax.Text = output_max.ToString();

                    var5DisplayName.Text = display_name.ToString();
                    var5Unit.Text = unit.ToString();
                    var5StatusColumn.Text = status_column.ToString();
                    var5Type.Text = type_value.ToString();
                    var5ValueColumn.Text = value_column.ToString();

                    txtTssErrorMin.Text = error_min.ToString();
                    txtTssErrorMax.Text = error_max.ToString();

                    var5Offset.Text = offset.ToString();
                    break;
                case "var6":
                    var6Channel.Text = channel_no;
                    var6Module.SelectedIndex = module_id;
                    var6InputMin.Text = input_min.ToString();
                    var6InputMax.Text = input_max.ToString();
                    var6OutputMin.Text = output_min.ToString();
                    var6OutputMax.Text = output_max.ToString();

                    var6DisplayName.Text = display_name.ToString();
                    var6Unit.Text = unit.ToString();
                    var6StatusColumn.Text = status_column.ToString();
                    var6Type.Text = type_value.ToString();
                    var6ValueColumn.Text = value_column.ToString();

                    txtEcErrorMin.Text = error_min.ToString();
                    txtEcErrorMax.Text = error_max.ToString();

                    var6Offset.Text = offset.ToString();
                    break;
                case "var7":
                    var7Channel.Text = channel_no;
                    var7Module.SelectedIndex = module_id;
                    var7InputMin.Text = input_min.ToString();
                    var7InputMax.Text = input_max.ToString();
                    var7OutputMin.Text = output_min.ToString();
                    var7OutputMax.Text = output_max.ToString();

                    var7DisplayName.Text = display_name.ToString();
                    var7Unit.Text = unit.ToString();
                    var7StatusColumn.Text = status_column.ToString();
                    var7Type.Text = type_value.ToString();
                    var7ValueColumn.Text = value_column.ToString();

                    txtpHErrorMin.Text = error_min.ToString();
                    txtpHErrorMax.Text = error_max.ToString();

                    var7Offset.Text = offset.ToString();
                    break;
                case "var8":
                    var8Channel.Text = channel_no;
                    var8Module.SelectedIndex = module_id;
                    var8InputMin.Text = input_min.ToString();
                    var8InputMax.Text = input_max.ToString();
                    var8OutputMin.Text = output_min.ToString();
                    var8OutputMax.Text = output_max.ToString();

                    var8DisplayName.Text = display_name.ToString();
                    var8Unit.Text = unit.ToString();
                    var8StatusColumn.Text = status_column.ToString();
                    var8Type.Text = type_value.ToString();
                    var8ValueColumn.Text = value_column.ToString();

                    txtOrpErrorMin.Text = error_min.ToString();
                    txtOrpErrorMax.Text = error_max.ToString();

                    var8Offset.Text = offset.ToString();
                    break;
                case "var9":
                    var9Channel.Text = channel_no;
                    var9Module.SelectedIndex = module_id;
                    var9InputMin.Text = input_min.ToString();
                    var9InputMax.Text = input_max.ToString();
                    var9OutputMin.Text = output_min.ToString();
                    var9OutputMax.Text = output_max.ToString();

                    var9DisplayName.Text = display_name.ToString();
                    var9Unit.Text = unit.ToString();
                    var9StatusColumn.Text = status_column.ToString();
                    var9Type.Text = type_value.ToString();
                    var9ValueColumn.Text = value_column.ToString();

                    txtTempErrorMin.Text = error_min.ToString();
                    txtTempErrorMax.Text = error_max.ToString();

                    var9Offset.Text = offset.ToString();
                    break;
                case "var10":
                    var10Channel.Text = channel_no;
                    var10Module.SelectedIndex = module_id;
                    var10InputMin.Text = input_min.ToString();
                    var10InputMax.Text = input_max.ToString();
                    var10OutputMin.Text = output_min.ToString();
                    var10OutputMax.Text = output_max.ToString();

                    var10DisplayName.Text = display_name.ToString();
                    var10Unit.Text = unit.ToString();
                    var10StatusColumn.Text = status_column.ToString();
                    var10Type.Text = type_value.ToString();
                    var10ValueColumn.Text = value_column.ToString();

                    txtDoErrorMin.Text = error_min.ToString();
                    txtDoErrorMax.Text = error_max.ToString();

                    var10Offset.Text = offset.ToString();
                    break;
                case "var11":
                    var11Channel.Text = channel_no;
                    var11Module.SelectedIndex = module_id;
                    var11InputMin.Text = input_min.ToString();
                    var11InputMax.Text = input_max.ToString();
                    var11OutputMin.Text = output_min.ToString();
                    var11OutputMax.Text = output_max.ToString();

                    var11DisplayName.Text = display_name.ToString();
                    var11Unit.Text = unit.ToString();
                    var11StatusColumn.Text = status_column.ToString();
                    var11Type.Text = type_value.ToString();
                    var11ValueColumn.Text = value_column.ToString();

                    txtTssErrorMin.Text = error_min.ToString();
                    txtTssErrorMax.Text = error_max.ToString();

                    var11Offset.Text = offset.ToString();
                    break;
                case "var12":
                    var12Channel.Text = channel_no;
                    var12Module.SelectedIndex = module_id;
                    var12InputMin.Text = input_min.ToString();
                    var12InputMax.Text = input_max.ToString();
                    var12OutputMin.Text = output_min.ToString();
                    var12OutputMax.Text = output_max.ToString();

                    var12DisplayName.Text = display_name.ToString();
                    var12Unit.Text = unit.ToString();
                    var12StatusColumn.Text = status_column.ToString();
                    var12Type.Text = type_value.ToString();
                    var12ValueColumn.Text = value_column.ToString();

                    txtEcErrorMin.Text = error_min.ToString();
                    txtEcErrorMax.Text = error_max.ToString();

                    var12Offset.Text = offset.ToString();
                    break;
                case "var13":
                    var13Channel.Text = channel_no;
                    var13Module.SelectedIndex = module_id;
                    var13InputMin.Text = input_min.ToString();
                    var13InputMax.Text = input_max.ToString();
                    var13OutputMin.Text = output_min.ToString();
                    var13OutputMax.Text = output_max.ToString();

                    var13DisplayName.Text = display_name.ToString();
                    var13Unit.Text = unit.ToString();
                    var13StatusColumn.Text = status_column.ToString();
                    var13Type.Text = type_value.ToString();
                    var13ValueColumn.Text = value_column.ToString();

                    txtpHErrorMin.Text = error_min.ToString();
                    txtpHErrorMax.Text = error_max.ToString();

                    var13Offset.Text = offset.ToString();
                    break;
                case "var14":
                    var14Channel.Text = channel_no;
                    var14Module.SelectedIndex = module_id;
                    var14InputMin.Text = input_min.ToString();
                    var14InputMax.Text = input_max.ToString();
                    var14OutputMin.Text = output_min.ToString();
                    var14OutputMax.Text = output_max.ToString();

                    var14DisplayName.Text = display_name.ToString();
                    var14Unit.Text = unit.ToString();
                    var14StatusColumn.Text = status_column.ToString();
                    var14Type.Text = type_value.ToString();
                    var14ValueColumn.Text = value_column.ToString();

                    txtOrpErrorMin.Text = error_min.ToString();
                    txtOrpErrorMax.Text = error_max.ToString();

                    var14Offset.Text = offset.ToString();
                    break;
                case "var15":
                    var15Channel.Text = channel_no;
                    var15Module.SelectedIndex = module_id;
                    var15InputMin.Text = input_min.ToString();
                    var15InputMax.Text = input_max.ToString();
                    var15OutputMin.Text = output_min.ToString();
                    var15OutputMax.Text = output_max.ToString();

                    var15DisplayName.Text = display_name.ToString();
                    var15Unit.Text = unit.ToString();
                    var15StatusColumn.Text = status_column.ToString();
                    var15Type.Text = type_value.ToString();
                    var15ValueColumn.Text = value_column.ToString();

                    txtTempErrorMin.Text = error_min.ToString();
                    txtTempErrorMax.Text = error_max.ToString();

                    var15Offset.Text = offset.ToString();
                    break;
                case "var16":
                    var16Channel.Text = channel_no;
                    var16Module.SelectedIndex = module_id;
                    var16InputMin.Text = input_min.ToString();
                    var16InputMax.Text = input_max.ToString();
                    var16OutputMin.Text = output_min.ToString();
                    var16OutputMax.Text = output_max.ToString();

                    var16DisplayName.Text = display_name.ToString();
                    var16Unit.Text = unit.ToString();
                    var16StatusColumn.Text = status_column.ToString();
                    var16Type.Text = type_value.ToString();
                    var16ValueColumn.Text = value_column.ToString();

                    txtDoErrorMin.Text = error_min.ToString();
                    txtDoErrorMax.Text = error_max.ToString();

                    var16Offset.Text = offset.ToString();
                    break;
                case "var17":
                    var17Channel.Text = channel_no;
                    var17Module.SelectedIndex = module_id;
                    var17InputMin.Text = input_min.ToString();
                    var17InputMax.Text = input_max.ToString();
                    var17OutputMin.Text = output_min.ToString();
                    var17OutputMax.Text = output_max.ToString();

                    var17DisplayName.Text = display_name.ToString();
                    var17Unit.Text = unit.ToString();
                    var17StatusColumn.Text = status_column.ToString();
                    var17Type.Text = type_value.ToString();
                    var17ValueColumn.Text = value_column.ToString();

                    txtTssErrorMin.Text = error_min.ToString();
                    txtTssErrorMax.Text = error_max.ToString();

                    var17Offset.Text = offset.ToString();
                    break;
                case "var18":
                    var18Channel.Text = channel_no;
                    var18Module.SelectedIndex = module_id;
                    var18InputMin.Text = input_min.ToString();
                    var18InputMax.Text = input_max.ToString();
                    var18OutputMin.Text = output_min.ToString();
                    var18OutputMax.Text = output_max.ToString();

                    var18DisplayName.Text = display_name.ToString();
                    var18Unit.Text = unit.ToString();
                    var18StatusColumn.Text = status_column.ToString();
                    var18Type.Text = type_value.ToString();
                    var18ValueColumn.Text = value_column.ToString();

                    txtEcErrorMin.Text = error_min.ToString();
                    txtEcErrorMax.Text = error_max.ToString();

                    var18Offset.Text = offset.ToString();
                    break;
                default:
                    break;
            }
        }

        private IEnumerable<module> checkAndInsertModuleConfiguration()
        {
            using (module_repository _modules = new module_repository())
            {
                IEnumerable<module> moduleConfigList = _modules.get_all();
                MODULE_CONFIG_LIST = moduleConfigList.Select(x => x.item_name).ToArray();

                if (moduleConfigList.Count() == MODULE_CONFIG_LIST.Count())
                {
                    return moduleConfigList;
                }
                else
                {
                    foreach (string itemModuleName in MODULE_CONFIG_LIST)
                    {
                        module objExistedModuleByName = _modules.get_info_by_name(itemModuleName);
                        if (objExistedModuleByName != null)
                        {
                            continue;
                        }
                        else
                        {
                            objExistedModuleByName = new module();
                            objExistedModuleByName.item_name = itemModuleName;
                            if (_modules.add(ref objExistedModuleByName) > 0)
                            {
                                // ok
                            }
                            else
                            {
                                // fail
                                MessageBox.Show(lang.getText("system_error"));
                                this.Close();
                                return null;
                            }
                        }
                    }
                    moduleConfigList = _modules.get_all();
                    return moduleConfigList;
                }
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            module_repository modules = new module_repository();
            IEnumerable<module> moduleConfigList = modules.get_all();
            MODULE_CONFIG_LIST = moduleConfigList.Select(x => x.item_name).ToArray();

            // saving to db
            station objStationSetting = new station_repository().get_info();

            if (isOpen(objStationSetting.socket_port) && (!txtSocketPort.Text.Equals(objStationSetting.socket_port.ToString())))
            {
                //this.btnSOCKET.Image = global::DataLogger.Properties.Resources.OFF_switch_96x25;
                //close socket
                try
                {
                    //frmNewMain.tcpListener.Stop();
                    if (Application.OpenForms.OfType<Form1>().Count() == 1)
                    {
                        Application.OpenForms.OfType<Form1>().First().Close();
                        btnShow.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error ! Cant close this socket.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            try
            {
                objStationSetting.station_name = txtStationName.Text;
                objStationSetting.station_id = txtStationID.Text;
                objStationSetting.socket_port = Convert.ToInt32(txtSocketPort.Text.Trim());
                //MessageBox.Show("1");
                objStationSetting.module_comport = cbModule.Text;
                //MessageBox.Show("2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cant SAVE !");
                this.Close();
                return;
            }
            try
            {
                //MessageBox.Show("3");
                if (new station_repository().update(ref objStationSetting) > 0)
                {
                    // ok
                }
                else
                {
                    // fail
                }

                foreach (string itemModule in MODULE_CONFIG_LIST)
                {
                    module obj = _modules.get_info_by_name(itemModule);
                    if (obj.type_value == 1)
                    {
                        string currentvar = obj.item_name;
                        //TextBox value = null;
                        string currentmodule = currentvar + "Module";
                        string currentmoduleText = null;
                        GetTextBoxStrings(this, currentmodule, ref currentmoduleText);

                        string currentchannel = currentvar + "Channel";
                        string currentchannelText = null ;
                        GetTextBoxStrings(this, currentchannel,ref currentchannelText);

                        string currentinputmin = currentvar + "InputMin";
                        string currentinputminText = null;
                        GetTextBoxStrings(this, currentinputmin, ref currentinputminText);

                        string currentinputmax = currentvar + "InputMax";
                        string currentinputmaxText = null;
                        GetTextBoxStrings(this, currentinputmax, ref currentinputmaxText);

                        string currentoutputmin = currentvar + "OutputMin";
                        string currentoutputminText = null;
                        GetTextBoxStrings(this, currentoutputmin, ref currentoutputminText);

                        string currentoutputmax = currentvar + "OutputMax";
                        string currentoutputmaxText = null;
                        GetTextBoxStrings(this, currentoutputmax, ref currentoutputmaxText);

                        string currentoffset = currentvar + "Offset";
                        string currentoffsetText = null;
                        GetTextBoxStrings(this, currentoffset, ref currentoffsetText);

                        string currentunit = currentvar + "Unit";
                        string currentunitText = null;
                        GetTextBoxStrings(this, currentunit, ref currentunitText);

                        string currentvaluecolumn = currentvar + "ValueColumn";
                        string currentvaluecolumnText = null;
                        GetTextBoxStrings(this, currentvaluecolumn, ref currentvaluecolumnText);

                        string currentstatuscolumn = currentvar + "StatusColumn";
                        string currentstatuscolumnText = null;
                        GetTextBoxStrings(this, currentstatuscolumn, ref currentstatuscolumnText);

                        string currenttype = currentvar + "Type";
                        string currenttypeText = null;
                        GetTextBoxStrings(this, currenttype, ref currenttypeText);

                        string currentdisplayname = currentvar + "DisplayName";
                        string currentdisplaynameText = null;
                        GetTextBoxStrings(this, currentdisplayname, ref currentdisplaynameText);




                        obj.module_id = Convert.ToInt32(currentmoduleText);
                        obj.channel_number = Convert.ToInt32(currentchannelText);
                        obj.input_min = Convert.ToInt32(currentinputminText);
                        obj.input_max = Convert.ToInt32(currentinputmaxText);
                        obj.output_min = Convert.ToInt32(currentoutputminText);
                        obj.output_max = Convert.ToInt32(currentoutputmaxText);
                        obj.off_set = Convert.ToDouble(currentoffsetText);
                        obj.unit = currentunitText;
                        obj.value_column = currentvaluecolumnText;
                        obj.status_column = currentstatuscolumnText;
                        obj.type_value = Convert.ToInt32(currenttypeText);
                        obj.display_name = currentdisplaynameText;

                        if (_modules.update(ref obj) > 0)
                        {
                            // ok
                        }
                        else
                        {
                            // fail
                        }

                    }
                    else if (obj.type_value == 2)
                    {

                    }
                }
                MessageBox.Show("Success", "", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refreshDataForControl();
        }

        private void grbStationConfig_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void txtTemperatureOffset_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtHumidityOffset_TextChanged(object sender, EventArgs e)
        {

        }
        public static string StringToByteArray(string hexstring)
        {
            return String.Join(String.Empty,hexstring
                .Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
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
            throw new Exception("127.0.0.1");
        }
        private void GetTextBoxStrings(System.Windows.Forms.Control control,string txtName,ref string text)
        {
            //List<String> list = new List<String>();
            //foreach (System.Windows.Forms.Control c in this.Controls)
            //{
            //    if (c is TextBox)
            //        list.Add(((TextBox)c).Text);
            //}
            //return list.ToArray();
            if (control is TextBox)
            {
                TextBox txt = (TextBox)control;
                if (txt.Name.StartsWith(txtName))
                    text = txt.Text;

            } else if (control is ComboBox)
            {
                ComboBox txt = (ComboBox)control;
                if (txt.Name.StartsWith(txtName))
                    text = (txt.SelectedIndex + 1).ToString();
            }
            else
            {
                foreach (System.Windows.Forms.Control child in control.Controls)
                {
                    GetTextBoxStrings(child, txtName, ref text);
                }
                //return "";
            }
        }
        private void btnSOCKET_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Save change ?", "Important Question", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.Yes)
            {
                station existedStationsSetting = new station_repository().get_info();
                if (existedStationsSetting == null)
                {

                }
                else
                {
                    if (isOpen(existedStationsSetting.socket_port))
                    {
                        this.btnSOCKET.Image = global::DataLogger.Properties.Resources.OFF_switch_96x25;
                        //close socket
                        try
                        {
                            if (Application.OpenForms.OfType<Form1>().Count() == 1)
                            {
                                Application.OpenForms.OfType<Form1>().First().Close();
                                btnShow.Enabled = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error ! Cant close this socket.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        this.btnSOCKET.Image = global::DataLogger.Properties.Resources.ON_switch_96x25;
                        //open socket
                        Int32 port = existedStationsSetting.socket_port;
                        IPAddress localAddr = IPAddress.Parse(frmConfiguration.GetLocalIPAddress());
                        try
                        {
                            //frmNewMain.tcpListener = new TcpListener(localAddr, port);
                            //frmNewMain.tcpListener.Start();
                            if (Application.OpenForms.OfType<Form1>().Count() == 1)
                            {
                                Application.OpenForms.OfType<Form1>().First().Close();
                            }

                            protocol = new Form1(newMain);
                            //protocol = new Form1(this.SAMPPort);
                            protocol.Show();
                            btnShow.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error ! Cant open this socket.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Form1>().Count() == 1)
            {
                if (btnShow.Text.Equals("Show"))
                {
                    protocol.Show();
                    btnShow.Text = "Hide";
                }
                if (btnShow.Text.Equals("Hide"))
                {
                    protocol.Hide();
                    btnShow.Text = "Show";
                }
            }
        }
        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void textBox35_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
