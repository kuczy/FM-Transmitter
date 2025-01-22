using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;

namespace FM_Transmitter
{
    public partial class Transmitter : Form
    {
        Boolean dataReceive = false;        // UART Data Receive flag. true when receive '\n' character
        string DataReceiveBuffer = "";      // UART Data Receive buffer
        string DataTransmittBuffer = "";    // UART Data Receive buffer
        string Env_StationName = "";        // String: Station name
        string Env_Frequency = "";          // String: frequency
        string Env_AudioSource = "";        // String: audio source
        string Env_MonoStereo = "";         // String: stereo / mono
        string Env_PreEmphase = "";         // String: pre emphase
        string Env_RdsStatus = "";          // String: RDS on/off
        string Env_RdsPiCodeMSB = "";       // String RDS Pi Code MSB as hex
        string Env_RdsPiCodeLSB = "";       // String RDS Pi Code LSB as hex
        string Env_RdsPty = "";             // String PTY 0...31
        string Env_RdsTp = "";              // String Traffic programme
        string Env_RdsMS = "";              // String Music / Speech
        string Env_RadioText = "";          // String Radiotext
        string Env_RadioTextStatic = "";    // String Radiotext store in transmitter broadcasted when USB is disconected
        string Env_AfNumber = "";
        string Env_Af1 = "";
        string Env_Af2 = "";
        string Env_Af3 = "";
        string Env_Af4 = "";
        string Env_Af5 = "";
        string DynamicTemporaryPs = "";     // String: Station name
        string DynamicPs = "";     // String: Station name
        int DynamicPsIndex = 0;
        int DynamicPsIndexRest = 0;
        int DynamicPsCurrentIndex = 0;
        string RadioTextAuto = "";          
        string initialText = "";
        string fileName = "";
        string saveFilePath = "";
        string applicationPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory); // the directory that your program is installed in  
        int PastMinute;

        string[] EuropePTY =
        {
            "Undefined",
            "News",
            "Current affairs",
            "Information",
            "Sport",
            "Education",
            "Drama",
            "Culture",
            "Science",
            "Varied",
            "Pop music",
            "Rock music",
            "Easy listening",
            "Light classical",
            "Serious classical",
            "Other music",
            "Weather",
            "Finance",
            "Children's programmes",
            "Social affairs",
            "Religion",
            "Phone-in",
            "Travel",
            "Leisure",
            "Jazz music",
            "Country music",
            "National music",
            "Oldies music",
            "Folk music",
            "Documentary",
            "Alarm test",
            "Alarm"
        };            // List of European PTY name code
        string[] UsaPTY =
        {
            "Undefined",
            "News",
            "Information",
            "Sports",
            "Talk",
            "Rock",
            "Classic rock",
            "Adult hits",
            "Soft rock",
            "Top 40",
            "Country",
            "Oldies",
            "Soft music",
            "Nostalgia",
            "Jazz",
            "Classical",
            "Rhythm and blues",
            "Soft rhythm and blues",
            "Language",
            "Religious music",
            "Religious talk",
            "Personality",
            "Public",
            "College",
            "Spanish Talk",
            "Spanish Music",
            "Hip hop",
            "Unassigned",
            "Unassigned",
            "Weather",
            "Emergency test",
            "Emergency"
        };               // List of USA PTY name code

        public Transmitter()
        {
            InitializeComponent();
        }

        // Form functions
        private void Form1_Load(object sender, EventArgs e)
        {

            // if not exist - create, or read existing file - Broadcasting music Ratiotext
            initialText = "0";
            fileName = "configPTY.data";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    using (FileStream fs = File.Create(fileName))
                    {
                        // Add some text to file
                        Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                        fs.Write(title, 0, title.Length);
                    }
                    comboPtyStandard.SelectedIndex = 0;
                    comboPty.DataSource = EuropePTY;
                }
                else
                {
                    string text = File.ReadAllLines(saveFilePath).LastOrDefault();
                    if (text == "0")
                    {
                        comboPtyStandard.SelectedIndex = 0;
                        comboPty.DataSource = EuropePTY;
                    }
                    else
                    {
                        comboPtyStandard.SelectedIndex = 1;
                        comboPty.DataSource = UsaPTY;
                    }
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                }
                comboPtyStandard.SelectedIndex = 0;
                comboPty.DataSource = EuropePTY;
            }

            // if not exist - create, or read existing file - Broadcasting dynamic PS combo
            comboDynamicPSTime.SelectedIndex = 1;
            initialText = "1";
            fileName = "configDynamicPTY.data";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    using (FileStream fs = File.Create(fileName))
                    {
                        // Add some text to file
                        Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                        fs.Write(title, 1, title.Length);
                    }
                }
                else
                {
                    string text = File.ReadAllLines(saveFilePath).LastOrDefault();
                    comboDynamicPSTime.SelectedIndex = Int32.Parse(text);
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                }
                comboPtyStandard.SelectedIndex = 0;
                comboPty.DataSource = EuropePTY;
            }

            comboRdsDataTime.SelectedIndex = 0;
            // if not exist - create, or read existing file - RDS Date / Time broadcasting
            initialText = "0";
            fileName = "configTime.data";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    using (FileStream fs = File.Create(fileName))
                    {
                        // Add some text to file
                        Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                        fs.Write(title, 0, title.Length);
                    }
                    comboRdsDataTime.SelectedIndex = 1;
                }
                else
                {
                    string text = File.ReadAllLines(saveFilePath).LastOrDefault();
                    if (text == "0")
                    {
                        comboRdsDataTime.SelectedIndex = 0;
                    }
                    else
                    {
                        comboRdsDataTime.SelectedIndex = 1;
                    }
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                }
                comboPtyStandard.SelectedIndex = 1;
            }

            // if not exist - create, or read existing file - Broadcasting dynamic text
            initialText = "RDS Text of dynamic Station Name";
            fileName = "RDS_DynamicPs.txt";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    txtDynamicPS.Text = initialText;
                }
                else
                {
                    txtDynamicPS.Text = readLine;
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                    txtDynamicPS.Text = initialText;
                }
            }

            // if not exist - create, or read existing file - Broadcasting music Ratiotext
            initialText = "RDS Text of music broadcast";
            fileName = "RDS_Music.txt";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    txtMusic.Text = initialText;
                }
                else
                {
                    txtMusic.Text = readLine;
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                    txtMusic.Text = initialText;
                }
            }

            // if not exist - create, or read existing file - Broadcasting speech Ratiotext
            initialText = "RDS Text of speech broadcast";
            fileName = "RDS_Speech.txt";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    txtSpeech.Text = initialText;
                }
                else
                {
                    txtSpeech.Text = readLine;
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                    txtSpeech.Text = initialText;
                }
            }

            // if not exist - create, or read existing file - Broadcasting news Ratiotext
            initialText = "RDS Text of news broadcast";
            fileName = "RDS_News.txt";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    txtNews.Text = initialText;
                }
                else
                {
                    txtNews.Text = readLine;
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                    txtNews.Text = initialText;
                }
            }

            // if not exist - create, or read existing file - Broadcasting traffic Ratiotext
            initialText = "RDS Text of traffic broadcast";
            fileName = "RDS_Traffic.txt";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    txtTraffic.Text = initialText;
                }
                else
                {
                    txtTraffic.Text = readLine;
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                    txtTraffic.Text = initialText;
                }
            }

            // if not exist - create, or read existing file - Automation Ratiotext
            initialText = "Automatic RDS music broadcast text";
            fileName = "RDS_Automation.txt";
            saveFilePath = Path.Combine(applicationPath, fileName);
            if (File.Exists(saveFilePath))
            {
                string readLine = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (readLine == null)
                {
                    using (FileStream fs = File.Create(fileName))
                    {
                        // Add some text to file
                        Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                        fs.Write(title, 0, title.Length);
                    }
                }
            }
            else
            {
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(initialText);
                    fs.Write(title, 0, title.Length);
                }
            }

            Connect();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            lblStationName.Text = Env_StationName;
            sendDynamicPS();
            string applicationPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory); // the directory that your program is installed in  
            string saveFilePath = Path.Combine(applicationPath, "configPTY.data");
            File.Delete(saveFilePath);
            StreamWriter w = new StreamWriter(saveFilePath, true);
            w.WriteLine(comboPtyStandard.SelectedIndex.ToString());
            w.Close();

            saveFilePath = Path.Combine(applicationPath, "configTime.data");
            File.Delete(saveFilePath);
            w = new StreamWriter(saveFilePath, true);
            w.WriteLine(comboRdsDataTime.SelectedIndex.ToString());
            w.Close();

            saveFilePath = Path.Combine(applicationPath, "configDynamicPTY.data");
            File.Delete(saveFilePath);
            w = new StreamWriter(saveFilePath, true);
            w.WriteLine(comboDynamicPSTime.SelectedIndex.ToString());
            w.Close();

            saveFilePath = Path.Combine(applicationPath, "RDS_DynamicPs.txt");
            File.Delete(saveFilePath);
            w = new StreamWriter(saveFilePath, true);
            w.WriteLine(txtDynamicPS.Text);
            w.Close();

            saveFilePath = Path.Combine(applicationPath, "RDS_Music.txt");
            File.Delete(saveFilePath);
            w = new StreamWriter(saveFilePath, true);
            w.WriteLine(txtMusic.Text);
            w.Close();

            saveFilePath = Path.Combine(applicationPath, "RDS_Speech.txt");
            File.Delete(saveFilePath);
            w = new StreamWriter(saveFilePath, true);
            w.WriteLine(txtSpeech.Text);
            w.Close();

            saveFilePath = Path.Combine(applicationPath, "RDS_News.txt");
            File.Delete(saveFilePath);
            w = new StreamWriter(saveFilePath, true);
            w.WriteLine(txtNews.Text);
            w.Close();

            saveFilePath = Path.Combine(applicationPath, "RDS_Traffic.txt");
            File.Delete(saveFilePath);
            w = new StreamWriter(saveFilePath, true);
            w.WriteLine(txtTraffic.Text);
            w.Close();

            Disconnect();
        }

        // Connect / Disconnect functions
        private void Connect()                                          // Search COM port (device SN: VHFFM01)
        {

            dataReceive = false;        // UART Data Receive flag. true when receive '\n' character
            DataReceiveBuffer = "";      // UART Data Receive buffer
            DataTransmittBuffer = "";    // UART Data Receive buffer
            Env_StationName = "";        // String: Station name
            Env_Frequency = "";          // String: frequency
            Env_AudioSource = "";        // String: audio source
            Env_MonoStereo = "";         // String: stereo / mono
            Env_PreEmphase = "";         // String: pre emphase
            Env_RdsStatus = "";          // String: RDS on/off
            Env_RdsPiCodeMSB = "";       // String RDS Pi Code MSB as hex
            Env_RdsPiCodeLSB = "";       // String RDS Pi Code LSB as hex
            Env_RdsPty = "";             // String PTY 0...31
            Env_RdsTp = "";              // String Traffic programme
            Env_RdsMS = "";              // String Music / Speech
            Env_RadioText = "";          // String Radiotext
            Env_RadioTextStatic = "";    // String Radiotext store in transmitter broadcasted when USB is disconected



            lblStationName.Text = "";
            lblRadiotext.Text = " Is transmitter ON? - Click \"Rerty\"";
            lblFrequency.Text = "";

            timerScroll.Enabled = true;

            //--------------------------------------------------------------------------------------
            string query = "SELECT * FROM Win32_PnPEntity WHERE DeviceID LIKE '%VHFFM01%'";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            string description = "";

            foreach (ManagementObject obj in searcher.Get())
            {
                description = obj["Caption"].ToString();
            }

            string[] destination;
            destination = description.Split('(');
            if (destination[0].Length > 0)
            {
                description = destination[1].ToString();
                destination = description.Split(')');
            }
            string ProperComPort = destination[0].ToString();
            //--------------------------------------------------------------------------------------

            try
            {
                serialPort1.PortName = ProperComPort;
                serialPort1.Open();
                lblDisconnect.Visible = false;
                pictureBox1.BackColor = Color.Orange;
                lblStationName.Visible = true;
                scrollPanel.Visible = true;
                lblAudioSource.Visible = true;
                lblFrequency.Visible = true;
                serialPort1.WriteLine("ENV=?");
            }
            catch (Exception)
            {
                    scrollPanel.Visible = false;
                    MessageBox.Show("No connected transmitter found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnRetry.Visible = true;
        }
        private void Disconnect()                                       // Disconnect function
        {
            pictureBox1.BackColor = Color.Orange;
            lblDisconnect.Visible = true;
            Env_StationName = "";
            Env_RadioText = "";
            Env_Frequency = "";
            lblStationName.Text = "";
            lblRadiotext.Text = "";
            lblFrequency.Text = "";
            lblAudioSource.Text = "";
                try
                {
                    serialPort1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        private void BtnRetry_Click(object sender, EventArgs e)         // Retry function
        {
            Disconnect();
            Connect();
        }

        // COM Port data Received & Parse
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Action act;             //utworzenie delegata
            act = ReadComPort;      //przypisanie naszej metody do delegata
            Invoke(act);            //wywołanie delegata
        }
        private void ReadComPort()
        {
            while (dataReceive != true)
            {
                DataReceiveBuffer += serialPort1.ReadExisting();

                if (DataReceiveBuffer.IndexOf('\n') != -1)
                {
                    dataReceive = true;
                    break;
                }
            }
            Parse();
            dataReceive = false;
        }
        private void Parse()
        {
            string[] destination;
            string[] parameters;
            destination = DataReceiveBuffer.Split('=');

            if ((destination[0].Length > 0) && (destination[1].Length > 0))
            {
                switch (destination[0])
                {
                    case "ENV":
                        parameters = destination[1].Split('^');

                        Env_StationName = parameters[0];
                        Env_Frequency = parameters[1];//.Substring(0, 5);
                        Env_AudioSource = parameters[2];
                        Env_MonoStereo = parameters[3];
                        Env_PreEmphase = parameters[4];
                        Env_RdsStatus = parameters[5];
                        Env_RdsPiCodeMSB = parameters[6];
                        Env_RdsPiCodeLSB = parameters[7];
                        Env_RdsPty = parameters[8];
                        Env_RdsTp = parameters[9];
                        Env_RdsMS = parameters[10];
                        Env_RadioText = parameters[11];
                        Env_RadioTextStatic = parameters[12];
                        Env_AfNumber = parameters[13];
                        Env_Af1 = parameters[14];
                        Env_Af2 = parameters[15];
                        Env_Af3 = parameters[16];
                        Env_Af4 = parameters[17];
                        Env_Af5 = parameters[18];

                        btnRetry.Visible = false;
                        ShowFrontPanel();
                        ShowSettings();
                        ShowAF();

                        break;
                    case "AS":
                        parameters = destination[1].Split('^');
                        Env_AudioSource = parameters[0];

                        if (Env_AudioSource == "0")
                        {
                            lblAudioSource.Text = "~A~";
                        }
                        else
                        {
                            lblAudioSource.Text = "010";
                        }
                        break;

                    case "TEXT":
                        parameters = destination[1].Split('^');
                        Env_RadioText = parameters[0];
                        ShowFrontPanel();
                        break;
                }
            }
            DataReceiveBuffer = "";
        }


        // Show Panels (copy data from variables to screens
        private void ShowFrontPanel()
        {
            lblStationName.Text = Env_StationName;
            lblFrequency.Text = Env_Frequency;
            lblFrequency.Text += " MHz";
            if (Env_AudioSource == "0")
            {
               lblAudioSource.Text = "~A~";
            }
            else
            {
                lblAudioSource.Text = "010";
            }
            if (Env_MonoStereo != "0")
            {
                icoStereo.Visible = false;
                icoMono.Visible = true;
            }
            else
            {
                icoMono.Visible = false;
                icoStereo.Visible = true;

            }
            icoUSB.Visible = true;
            if (Env_RdsStatus != "0")
            {
                icoRDS.Visible = true;
                scrollPanel.Visible = true;
                grpRdsCntrl.Enabled = true;
            }
            else
            {
                icoRDS.Visible = false;
                scrollPanel.Visible = false;
                grpRdsCntrl.Enabled = false;
            }
            lblRadiotext.Text = Env_RadioText;
            RadiotextLabelStartPosition();
            grpRadioTexts.Enabled = true;
            grpDynamicPS.Enabled = true;
            grpAF.Enabled = true;
            RdsDataTime.Enabled = true;
        }
        private void ShowSettings()
        {
            txtStationName.Text = Env_StationName;
            string searchFq = (Env_Frequency + " MHz");
            int index = listFrequency.FindString(searchFq);
            listFrequency.SetSelected(index, true);

            btnApplySettings.Enabled = true;
            btnApplySettings_2.Enabled = true;

            if (Env_MonoStereo == "0")
            {
                comboAudioMode.SelectedIndex = 0;
            }
            else
            {
                comboAudioMode.SelectedIndex = 1;
            }

            if (Env_PreEmphase == "0")
            {
                comboPreEmphase.SelectedIndex = 0;
            }
            else
            {
                comboPreEmphase.SelectedIndex = 1;
            }

            if (Env_RdsStatus == "0")
            {
                comboRdsMode.SelectedIndex = 0;
            }
            else
            {
                comboRdsMode.SelectedIndex = 1;
            }

            txtPi.Text = (Env_RdsPiCodeMSB + Env_RdsPiCodeLSB);
            SelectPTY();

            if (Env_RdsMS == "0")
            {
                comboMS.SelectedIndex = 0;
                radioSpeech.Select();
            }
            else
            {
                comboMS.SelectedIndex = 1;
                radioMusic.Select();
            }

            if (Env_RdsTp == "1")
            {
                comboTp.SelectedIndex = 1;
                chBoxTraffic.Enabled = true;
            }
            else
            {
                comboTp.SelectedIndex = 0;
                chBoxTraffic.Enabled = false;
            }
            txtRadioTextStatic.Text = Env_RadioTextStatic;

        }

        private void ShowAF()
        {
            comboAfNumber.SelectedIndex = Int32.Parse(Env_AfNumber);
            comboAf1.SelectedIndex = Int32.Parse(Env_Af1);
            comboAf2.SelectedIndex = Int32.Parse(Env_Af2);
            comboAf3.SelectedIndex = Int32.Parse(Env_Af3);
            comboAf4.SelectedIndex = Int32.Parse(Env_Af4);
            comboAf5.SelectedIndex = Int32.Parse(Env_Af5);

            ShowAfCombos();
        }
        private void ShowAfCombos()
        {
            if (comboAfNumber.SelectedIndex == 0)
            {
                comboAf1.Enabled = false;
                comboAf2.Enabled = false;
                comboAf3.Enabled = false;
                comboAf4.Enabled = false;
                comboAf5.Enabled = false;
            }
            if (comboAfNumber.SelectedIndex == 1)
            {
                comboAf1.Enabled = true;
                comboAf2.Enabled = false;
                comboAf3.Enabled = false;
                comboAf4.Enabled = false;
                comboAf5.Enabled = false;
            }
            if (comboAfNumber.SelectedIndex == 2)
            {
                comboAf1.Enabled = true;
                comboAf2.Enabled = true;
                comboAf3.Enabled = false;
                comboAf4.Enabled = false;
                comboAf5.Enabled = false;
            }
            if (comboAfNumber.SelectedIndex == 3)
            {
                comboAf1.Enabled = true;
                comboAf2.Enabled = true;
                comboAf3.Enabled = true;
                comboAf4.Enabled = false;
                comboAf5.Enabled = false;
            }
            if (comboAfNumber.SelectedIndex == 4)
            {
                comboAf1.Enabled = true;
                comboAf2.Enabled = true;
                comboAf3.Enabled = true;
                comboAf4.Enabled = true;
                comboAf5.Enabled = false;
            }
            if (comboAfNumber.SelectedIndex == 5)
            {
                comboAf1.Enabled = true;
                comboAf2.Enabled = true;
                comboAf3.Enabled = true;
                comboAf4.Enabled = true;
                comboAf5.Enabled = true;
            }
        }

        // GUI functions (clicks, index changed etc...)
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Env_Frequency != "")
            {
                grpBasicSettings.Enabled = true;
                grpRdsSettings.Enabled = true;
                if (tabControl1.SelectedIndex == 1)
                {
                    ShowSettings();
                }
            }
        }
        private void ComboPtyStandard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPtyStandard.SelectedIndex == 0)
            {
                comboPty.DataSource = EuropePTY;
            }
            else
            {
                comboPty.DataSource = UsaPTY;
            };
            SelectPTY();
        }
        private void ChBoxTraffic_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxTraffic.Checked == true)
            {
                chBoxNews.Checked = false;
                if (txtTraffic.TextLength > 0)
                {
                    string data = "TEXT=" + txtTraffic.Text;
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    lblRdsSource.Text = "Traffic";
                    data = "TA=1";
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    RadiotextLabelStartPosition();
                }
                else
                {
                    chBoxTraffic.Checked = false;
                    MessageBox.Show("Fill in the text box: Traffic information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                string data = "TA=0";
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(data);
                }

                if (radioMusic.Checked)
                {
                    if (txtMusic.Text.Length > 0)
                    {
                        lblRdsSource.Text = "Music";
                        data = "TEXT=" + txtMusic.Text;
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.WriteLine(data);
                        }
                        RadiotextLabelStartPosition();
                    }
                    else
                    {
                        radioMusic.Checked = false;
                        MessageBox.Show("Fill in the text box: Broadcasting music", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (radioSpeech.Checked)
                {
                    if (txtSpeech.Text.Length > 0)
                    {
                        lblRdsSource.Text = "Speech";
                        data = "TEXT=" + txtSpeech.Text;
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.WriteLine(data);
                        }
                        RadiotextLabelStartPosition();
                    }
                    else
                    {
                        radioSpeech.Checked = false;
                        MessageBox.Show("Fill in the text box: Broadcasting speech", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                
            }
            RadiotextLabelStartPosition();
        }
        private void RadioMusic_Click(object sender, EventArgs e)
        {
            if (chBoxTraffic.Checked == false)
            {
                if(txtMusic.Text.Length > 0)
                {
                    string data = "TEXT=" + txtMusic.Text;
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    lblRdsSource.Text = "Music";
                    data = "MS=1";
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    RadiotextLabelStartPosition();
                }
                else
                {
                    if (txtSpeech.Text.Length > 0)
                    {
                        radioSpeech.Checked = true;
                    }
                    else
                    {
                        radioMusic.Checked = false;
                    }
                    MessageBox.Show("Fill in the text box: Broadcasting music", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void RadioSpeech_Click(object sender, EventArgs e)
        {
            if (chBoxTraffic.Checked == false)
            {
                if (txtSpeech.Text.Length > 0)
                {
                    string data = "TEXT=" + txtSpeech.Text;
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    lblRdsSource.Text = "Speech";
                    data = "MS=0";
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    RadiotextLabelStartPosition();
                }
                else
                {
                    if(txtMusic.Text.Length > 0)
                    {
                        radioMusic.Checked = true;
                    }
                    else
                    {
                        radioSpeech.Checked = false;
                    }
                    MessageBox.Show("Fill in the text box: Broadcasting speech", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SelectPTY()
        {
            int index=0;
            // PTY code
            try
            {
                index = Int32.Parse(Env_RdsPty);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{Env_RdsPty}'");
            }
            comboPty.SelectedIndex = index;
        }

        // RadioText Scrolling functions
        private void TimerScroll_Tick(object sender, EventArgs e)
        {
            int LblX = lblRadiotext.Location.X;
            int LblY = lblRadiotext.Location.Y;

            if(lblRadiotext.Width > scrollPanel.Width)
            {
                if (LblX > 23 - lblRadiotext.Width)
                {
                    lblRadiotext.Location = new Point(LblX - 1, LblY);
                }
                else
                {
                    lblRadiotext.Location = new Point(210 - 1, LblY);
                }
            }
            else
            {
                lblRadiotext.Location = new Point(0, LblY);
            }
        }
        private void RadiotextLabelStartPosition()
        {
            int LblX = lblRadiotext.Location.X;
            int LblY = lblRadiotext.Location.Y;
            lblRadiotext.Location = new Point(210, LblY);
        }

        private void BtnApplySettings_Click(object sender, EventArgs e)
        {
            if ((txtStationName.TextLength == 0) || (txtPi.TextLength < 4) || (txtRadioTextStatic.TextLength == 0))
            {
                MessageBox.Show("Complete all text fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string[] items = { };

                DataTransmittBuffer = "ENV=";

                DataTransmittBuffer += txtStationName.Text;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += listFrequency.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboAudioMode.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboPreEmphase.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboRdsMode.SelectedIndex;
                DataTransmittBuffer += "^";
                int tmp = 0;
                tmp = Convert.ToInt32((txtPi.Text.Substring(0, 2)), 16);
                DataTransmittBuffer += tmp;
                DataTransmittBuffer += "^";
                tmp = Convert.ToInt32((txtPi.Text.Substring(2, 2)), 16);
                DataTransmittBuffer += tmp;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboPty.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboTp.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboMS.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += txtRadioTextStatic.Text;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboAfNumber.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboAf1.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboAf2.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboAf3.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboAf4.SelectedIndex;
                DataTransmittBuffer += "^";
                DataTransmittBuffer += comboAf5.SelectedIndex;
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(DataTransmittBuffer);
                }
                DataTransmittBuffer = "";
            }
            
        }

        private void TimerRadioTextAuto_Tick(object sender, EventArgs e)
        {
            string fileName = "RDS_Automation.txt";
            string applicationPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory); // the directory that your program is installed in  
            string saveFilePath = Path.Combine(applicationPath, fileName);

                if (File.Exists(saveFilePath))
                {
                string text = File.ReadAllLines(saveFilePath).LastOrDefault();
                if (text != null)
                {
                    if (text != RadioTextAuto)
                    {
                        if ((Env_RdsStatus == "1") & (radioMusic.Checked == true) & (chBoxTraffic.Checked == false) & (radioTxtSourceAutomatic.Checked == true))
                        {
                            RadioTextAuto = text;
                            //txtMusic.Text = text;
                            string data = "TEXT=" + text;
                            if (serialPort1.IsOpen)
                            {
                                serialPort1.WriteLine(data);
                            }
                        }
                    }
                }
            }
        }

        private void RadioTxtSourceAutomatic_Click(object sender, EventArgs e)
        {
            RadioTextAuto = "";
        }

        private void RadioTxtSourceManual_Click(object sender, EventArgs e)
        {
            RadioTextAuto = "";
            RadioMusic_Click(sender, e);
        }

        private void TxtMusic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                chBoxNews.Checked = false;
                chBoxTraffic.Checked = false;
                radioTxtSourceManual.PerformClick();
                radioMusic.PerformClick();
                e.Handled = true; //Disable "ding" sound
            }
        }

        private void TxtSpeech_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                chBoxNews.Checked = false;
                chBoxTraffic.Checked = false;
                radioSpeech.PerformClick();
                e.Handled = true; //Disable "ding" sound
            }
        }

        private void TxtTraffic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                chBoxNews.Checked = false;
                chBoxTraffic.Checked = true;
                e.Handled = true; //Disable "ding" sound
            }
        }

        private void ChBoxNews_CheckedChanged(object sender, EventArgs e)
        {
            if(chBoxNews.Checked == true)
            {
                chBoxTraffic.Checked = false;
                if (txtNews.TextLength > 0)
                {
                    string data = "TEXT=" + txtNews.Text;
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    lblRdsSource.Text = "News";
                    data = "PTY=1";
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                    RadiotextLabelStartPosition();
                }
                else
                {
                    chBoxTraffic.Checked = false;
                    MessageBox.Show("Fill in the text box: News information", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                string data = "PTY="+ Env_RdsPty;
                if (serialPort1.IsOpen)
                {
                    serialPort1.WriteLine(data);
                }

                if (radioMusic.Checked)
                {
                    if (txtMusic.Text.Length > 0)
                    {
                        lblRdsSource.Text = "Music";
                        data = "TEXT=" + txtMusic.Text;
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.WriteLine(data);
                        }
                        RadiotextLabelStartPosition();
                    }
                    else
                    {
                        radioMusic.Checked = false;
                        MessageBox.Show("Fill in the text box: Broadcasting music", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (radioSpeech.Checked)
                {
                    if (txtSpeech.Text.Length > 0)
                    {
                        lblRdsSource.Text = "Speech";
                        data = "TEXT=" + txtSpeech.Text;
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.WriteLine(data);
                        }
                        RadiotextLabelStartPosition();
                    }
                    else
                    {
                        radioSpeech.Checked = false;
                        MessageBox.Show("Fill in the text box: Broadcasting speech", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
            RadiotextLabelStartPosition();
        }

        private void TxtNews_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                chBoxNews.Checked = true;
                chBoxTraffic.Checked = false;
                e.Handled = true; //Disable "ding" sound
            }
        }

        private void RdsDataTime_Tick(object sender, EventArgs e)
        {

            // Addons - check is COM Port is open - if not - make app windows inactive
            if (serialPort1.IsOpen)
            {
                // do nothing
            }
            else
            {
                icoStereo.Visible = false;
                icoMono.Visible = false;
                icoRDS.Visible = false;
                icoUSB.Visible = false;
                RdsDataTime.Enabled = false;
                grpRdsCntrl.Enabled = false;
                grpRadioTexts.Enabled = false;
                grpDynamicPS.Enabled = false;
                grpBasicSettings.Enabled = false;
                grpRdsSettings.Enabled = false;
                grpAF.Enabled = false;
                btnApplySettings.Enabled = false;
                Disconnect();
                Connect();
            }

            int Year = DateTime.UtcNow.Year;
            int Month = DateTime.UtcNow.Month;
            int Day = DateTime.UtcNow.Day;
            int Hour = DateTime.UtcNow.Hour;
            int Minute = DateTime.UtcNow.Minute;
            int TimeOffset;
            int TimeOffsetAbs;
            int TimeOffsetSign;
            int ModifiedJulianDate;

            TimeSpan span = DateTime.Now.Subtract(DateTime.UtcNow);
            span = DateTime.Now.Subtract(DateTime.UtcNow);              // Powtórzone 2x ponieważ przy pierwszym odczycie źle obliczał offset

            TimeOffset = ((span.Hours * 2) + (span.Minutes / 30));
            if (TimeOffset < 0){
                TimeOffsetSign = 1;
            }
            else{
                TimeOffsetSign = 0;
            }
            TimeOffsetAbs = Math.Abs(TimeOffset);

            toolStripStatusLabel.Text = "DATE / TIME    UTC: ";
            toolStripStatusLabel.Text += DateTime.UtcNow.ToString();
            toolStripStatusLabel.Text += "    Local: ";
            toolStripStatusLabel.Text += DateTime.Now.ToString();

            if (Minute != PastMinute){
                //DateTime dateTime = DateTime.UtcNow; //https://www.juliasos.com/misc/convert-datetime-to-julian-date-in-c-sharp-tooadate-safe/
                int a = (14 - Month) / 12;
                int y = Year + 4800 - a;
                int m = Month + 12 * a - 3;
                int julianDate = Day + (153 * m + 2) / 5 + 365 * y + y / 4 - y / 100 + y / 400 - 32045;
                ModifiedJulianDate = julianDate - 2400001;

                string data = "DATETIME=" + ModifiedJulianDate + "^" + Hour + "^" + Minute + "^" + TimeOffsetSign + "^" + TimeOffsetAbs;
                //debug.Text = data;
                if ((Env_RdsStatus == "1") && (comboRdsDataTime.SelectedIndex == 0)) {
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.WriteLine(data);
                    }
                }
                PastMinute = Minute;
            }
        }

        private void LockAfCombos(object sender, EventArgs e)
        {
            ShowAfCombos();
        }

        private void comboAf1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env_Af1 = comboAf1.SelectedIndex.ToString();
        }

        private void comboAf2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env_Af2 = comboAf2.SelectedIndex.ToString();
        }

        private void comboAf3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env_Af3 = comboAf3.SelectedIndex.ToString();
        }

        private void comboAf4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env_Af4 = comboAf4.SelectedIndex.ToString();
        }

        private void comboAf5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env_Af5 = comboAf5.SelectedIndex.ToString();
        }

        
        private void scrollDynamicPs()
        {
            if (radioPsDynamic.Checked == true)
            {
                if(DynamicTemporaryPs.Length == 0)
                {
                    radioPsStatic.Checked = true;
                    MessageBox.Show("Fill in the text box: Dynamic text", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if(DynamicTemporaryPs.Length <= 8)
                    {
                        lblStationName.Text = DynamicPs = DynamicTemporaryPs;
                        sendDynamicPS();
                    }
                    else
                    {
                        if (DynamicPsCurrentIndex < DynamicPsIndex)
                        {
                            DynamicPs = DynamicTemporaryPs.Substring((DynamicPsCurrentIndex * 8), 8);
                            lblStationName.Text = DynamicPs;
                            sendDynamicPS();
                            DynamicPsCurrentIndex++;
                        }else if (DynamicPsCurrentIndex == DynamicPsIndex)
                        {
                            if(DynamicPsIndexRest == 0)
                            {
                                DynamicPsCurrentIndex = 0;
                            }
                            else
                            {
                                DynamicPs = DynamicTemporaryPs.Substring((DynamicPsCurrentIndex * 8), DynamicPsIndexRest);
                                lblStationName.Text = DynamicPs;
                                sendDynamicPS();
                                DynamicPsCurrentIndex = 0;
                            }
                            
                        }
                    }
                }
            }
        }

        private void PsDynamicStart()
        {
            DynamicTemporaryPs = txtDynamicPS.Text;
            if (DynamicTemporaryPs.Length > 8)
            {
                DynamicPsCurrentIndex = 0;
                DynamicPsIndex = DynamicTemporaryPs.Length / 8;
                DynamicPsIndexRest = DynamicTemporaryPs.Length - (DynamicPsIndex * 8);
                timerDynamicPs.Start();
            }
            scrollDynamicPs();
        }
        private void radioPsDynamic_Click(object sender, EventArgs e)
        {
            PsDynamicStart();
        }
        private void radioPsStatic_Click(object sender, EventArgs e)
        {
            timerDynamicPs.Stop();
            lblStationName.Text = Env_StationName;
            sendDynamicPS();
        }
        private void sendDynamicPS()
        {
            string data = "DYNAMICPS=" + lblStationName.Text;
            if (serialPort1.IsOpen)
            {
                serialPort1.WriteLine(data);
            }
        }

        private void timerDynamicPs_Tick(object sender, EventArgs e)
        {
            scrollDynamicPs();
        }

        private void comboDynamicPSTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerDynamicPs.Stop();
            timerDynamicPs.Interval = 1000 * Int32.Parse(comboDynamicPSTime.Text);
            timerDynamicPs.Start();
        }

        private void txtDynamicPS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                radioPsStatic.Checked = false;
                radioPsDynamic.Checked = true;
                e.Handled = true; //Disable "ding" sound
                PsDynamicStart();
            }
        }
    }
}
