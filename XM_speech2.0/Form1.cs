using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Media;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics.Eventing;
using System.Speech;
using System.Speech.AudioFormat;
using System.Speech.Recognition.SrgsGrammar;

namespace XM_speech2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //初始化界面
            this.MIN_CONFIDENCE = confidenceTrackBar.Value / 100.0;
            this.confidenceTextBox.Text = MIN_CONFIDENCE.ToString("F2");
            this.srIpTextBox.Text = ip;
            this.srPortTextBox.Text = port.ToString();
        }
        // 语音发音模块相关成员定义
        SpeechSynthesizer ttsSp = new SpeechSynthesizer();
        string ip = "10.130.7.162";
        int port = 10000;
        String GPSRmsg = "";
        string spout = "";
        bool cansend = false;
        // 语音识别模块相关成员定义
        double MIN_CONFIDENCE;
        string remoteIp;
        int remotePort;
        string cmdFile;
        int time = 0;
        bool gpsrsend = true;
        int gpsrorders = 0;
        byte[] xm_out = null;
        bool water = false;
        //***********************************************************
        //ttsload
        int ttsPort = 10000;
        TcpListener ttsListner;
        Thread ttsThread;
        String msg;
        bool speakReady = false;
        private byte[] readLen(Stream stream, int len)
        {
            byte[] buf = new byte[len];
            int offset = 0;
            while (len > 0)
            {
                int count = stream.Read(buf, offset, len);
                len -= count;
                offset += count;
            }
            return buf;
        }
        private void sendgpsr(byte[] msg)
        {
            List<byte> data = new List<byte>();
            for(int i=gpsrorders;i<msg.Length;i++)
            {
                if(msg[i]!=0x00) data.Add(msg[i]);
                else
                {
                    if (gpsrsend) sendSrResult(data.ToArray());
                    gpsrorders = i + 1;
                    gpsrsend = false;
                }
            }
        }
        private void ttsReceiveThread()
        {
            ttsListner.Start();
            while (true)
            {
                TcpClient ttsClient = ttsListner.AcceptTcpClient();
                NetworkStream stream = ttsClient.GetStream();
                byte[] data = this.readLen(stream, 4);
                int dataLen = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, 0));
                Debug.WriteLine(dataLen);
                byte[] msgData = this.readLen(stream, 1);
                if(msgData[0]==0x01)
                {
                    byte[] msgchoice = this.readLen(stream, 1);
                    //int channel = msgchoice[0];
                    switch(msgchoice[0])
                    {
                        case 0x00:
                            ttsSp.Speak("hello, I have found you");
                            break;
                        case 0x01:
                            ttsSp.Speak("already setup");
                            break;
                        case 0x02:
                            ttsSp.Speak("record compelete");
                            break;
                        case 0x03:
                            ttsSp.Speak("please stand aside");
                            break;
                        case 0x05:
                            ttsSp.Speak("I know what you said");
                            break;
                        case 0x11:
                            ttsSp.Speak("what's the matter with you, my master");
                            break;
                        case 0x12:
                            ttsSp.Speak("please wait, let me bring you some medicine");
                            break;
                        case 0x21:
                            ttsSp.Speak("I know your order");
                            break;
                        case 0x22:
                            ttsSp.Speak("please come and get your order");
                            break;
                    }
                }
                else
                {
                    sendgpsr(xm_out);
                }
                stream.Close();
                
                ttsClient.Close();
            }
        }
        //***********************************************************************************
        private SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
        private void strbtn_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.Cursor = Cursors.Default;
        }

        private void strbtn_MouseEnter(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            p.Cursor = Cursors.Hand;
        }
        private void srBtn_Click(object sender, EventArgs e)
        {
            ttsListner = new TcpListener(ttsPort);
            //启动tts接收线程
            //ttsSp.Speak("yes I received");
            ttsThread = new Thread(new ThreadStart(ttsReceiveThread));
            ttsThread.Start();
            srRichTextBox.AppendText("tts接收已激活\n");
            if (this.cmdfilecomboBox.Text != "")
            {
                if (this.cmdfilecomboBox.Text == "GPSR.ini")//GPSR输入语法
                {
                    srPicBox.Image = global::XM_speech2._0.Properties.Resources.msg_ok;
                    remoteIp = srIpTextBox.Text;
                    remotePort = Convert.ToInt32(srPortTextBox.Text);
                    sre.SetInputToDefaultAudioDevice();//默认的语音输入设备
                    SrgsDocument srg = new SrgsDocument("GPSR.xml");
                    this.loadCmdFile("GPSR.xml");
                    sre.LoadGrammar(new Grammar(srg));
                }
                else
                {
                    srPicBox.Image = global::XM_speech2._0.Properties.Resources.msg_ok;
                    remoteIp = srIpTextBox.Text;
                    remotePort = Convert.ToInt32(srPortTextBox.Text);
                    cmdFile = cmdfilecomboBox.Text;
                    sre.SetInputToDefaultAudioDevice();//默认的语音输入设备
                    GrammarBuilder gb = new GrammarBuilder();
                    string[] cmds = this.loadCmdFile(cmdFile);
                    gb.Append(new Choices(cmds));
                    Grammar grammar = new Grammar(gb);
                    sre.LoadGrammar(grammar);
                }

                if (this.cmdfilecomboBox.Text == "GPSR.ini")
                {

                    sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_GPSRrecognized);
                    sre.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(sre_GPSRDetected);
                    Console.WriteLine("ing...");
                }
                else
                {
                    Console.WriteLine("ing...");
                    sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
                    sre.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(sre_SpeechDetected);
                }
                sre.RecognizeAsync(RecognizeMode.Multiple);//异步调用识别引擎，允许多次识别（否则程序只响应你的一句话）
            }
            if(this.cmdfilecomboBox.Text=="emmergency.ini")
            {
                Thread.Sleep(5000);
                ttsSp.Speak("I can't find you, please come here");
                
            }
        }
        void sre_GPSRrecognized(object sender, SpeechRecognizedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {

                float confidence = e.Result.Confidence;
                if (confidence < MIN_CONFIDENCE)
                {
                    srRichTextBox.AppendText("too little confidence!!!!!,confidence:" + confidence.ToString("F2") + "\n");
                    ttsSp.Speak("I can't understand your words, please speak more clear");
                    return;
                }
                string cmd = e.Result.Text;
                srRichTextBox.AppendText("result:" + cmd + "\t" + "confidence:" + confidence.ToString("F2") + "\n");
                GPSRmsg = GPSRmsg + cmd + " ";
                ttsSp.Speak("you said " + cmd);
                ttsSp.Speak("is it right");
                if (GPSRmsg != "") xm_out = cmddetect(GPSRmsg);
                Console.WriteLine("recognize msgresult: " + GPSRmsg);
            });
        }
        void sre_GPSRDetected(object sender, SpeechDetectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                srRichTextBox.AppendText("speech detect\n");
            });
            
            //time = 0;
            //GPSRtimer.Enabled = true;
            //this.GPSRtimer.Start();
        }

        private void GPSRtimer_Tick(object sender, EventArgs e)
        {
            time++;
            Console.WriteLine(time);
            if (time >= 7)
            {
                GPSRtimer.Stop();
                
                spout = "我理解了：";
                if (GPSRmsg != "") xm_out = cmddetect(GPSRmsg);
                if (GPSRmsg == "") srRichTextBox.AppendText("Sorry I haven't understand you well\n");
                srRichTextBox.AppendText(GPSRmsg);
                srRichTextBox.AppendText(spout);
                srRichTextBox.AppendText("\n");
                GPSRmsg = "";
                Console.WriteLine("speech clear up\n");
            }
        }
        void sre_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                srRichTextBox.AppendText("speech detect\n");
                
            });
            //Thread.Sleep(6000);
            //if (this.cmdfilecomboBox.Text == "emmergency.ini"&&(!water))
            //{
              //  ttsSp.Speak("I thing you need some manual water");
                //water = true;
            //}
        }
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)//对照winsever改写（处理是重中之重）
        {
            this.Invoke((MethodInvoker)delegate
            {

                float confidence = e.Result.Confidence;
                if (confidence < MIN_CONFIDENCE)
                {
                    srRichTextBox.AppendText("too little confidence!!!!!,confidence:" + confidence.ToString("F2") + "\n");
                    ttsSp.Speak("I didn't hear clearly");
                    return;
                }
                string cmd = e.Result.Text;
                srRichTextBox.AppendText("result:" + cmd + "\t" + "confidence:" + confidence.ToString("F2") + "\n");
                byte[] xm_send;
                xm_send = cmddetect(cmd);
                ttsSp.Speak("you said " + cmd + " is it");
                //sendSrResult(xm_send);
                Console.WriteLine("recognize result: " + cmd);
            });
        }
        private string[] loadCmdFile(string path)//读文件，略
        {
            List<string> list = new List<string>();
            if (this.cmdfilecomboBox.Text != "")
            {
                StreamReader sr = new StreamReader(path, Encoding.Default);
                srRichTextBox.AppendText("all commands:\n");
                string line;
                if (this.cmdfilecomboBox.Text != "GPSR.ini")
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.StartsWith("#"))
                            continue;
                        list.Add(line);
                        srRichTextBox.AppendText(line + "\n");
                    }
                }
                if (this.cmdfilecomboBox.Text == "GPSR.ini")
                {
                    srRichTextBox.AppendText("VERBS:\n");
                    loadGPSRnameFile("A.ini");
                    srRichTextBox.AppendText("OBJECTS:\n");
                    loadGPSRnameFile("B.ini");
                    srRichTextBox.AppendText("LOCS:\n");
                    loadGPSRnameFile("L.ini");
                }
            }
            return list.ToArray();            
        }
        private string[] loadGPSRnameFile(string path)//读文件，略
        {
            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.StartsWith("#"))
                    continue;
                list.Add(line);
                srRichTextBox.AppendText(line + "  ");
            }
            srRichTextBox.AppendText("\n");
            return list.ToArray();
        }
        object socketobj = new object();
        
        private void sendSrResult(byte[] msg)
        {
            List<byte> data = new List<byte>();
            //id
            lock (socketobj)
            {
                if (this.cmdfilecomboBox.Text == "follow.ini") data.Add(0x01);
                if (this.cmdfilecomboBox.Text == "GPSR.ini") data.Add(0x06);
                if (this.cmdfilecomboBox.Text == "whoiswho.ini") data.Add(0x02);
                if (this.cmdfilecomboBox.Text == "shopping.ini") data.Add(0x05);
                data.AddRange(msg);
                List<byte> senData = new List<byte>();
                byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Count));
                senData.AddRange(len);
                senData.AddRange(data);
                //send
                if (cansend)
                {
                    try
                    {
                        TcpClient client = new TcpClient();
                        try
                        {
                            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
                            srRichTextBox.AppendText("connected");
                            client.Connect(ipep);
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine("connect error" + e.Message);
                        }
                        NetworkStream stream = client.GetStream();
                        srRichTextBox.AppendText(senData.ToArray().ToString());
                        stream.Write(senData.ToArray(), 0, senData.Count);
                        cansend = false;
                        stream.Close();
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        srRichTextBox.AppendText("连接失败，请检查ip和port:" + ex.Message + "\n");
                    }
                }
            }
        }

        private void confidenceTrackBar_Scroll(object sender, EventArgs e)
        {
            this.MIN_CONFIDENCE = confidenceTrackBar.Value / 100.0;
            this.confidenceTextBox.Text = MIN_CONFIDENCE.ToString("F2");
        }

        private void srRichTextBox_TextChanged(object sender, EventArgs e)
        {
            srRichTextBox.Focus();
            srRichTextBox.Select(srRichTextBox.Text.Length, 0);
            srRichTextBox.ScrollToCaret();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
            this.Dispose();
            this.Close();
        }
        //natual language understanding
        //0x01:follow
        //0x02:whoiswho
        //0x03:GPRS
        //0x04:music
        //0x05:shopping
        //0x06:nothing
        //follow:记住0x01
        //follow:跟随0x02
        //follow:等候回来0x03
        //follow:等10秒0x04        
        private byte[] cmddetect(string a)//指令集合区，目前先这样吧
        {
            List<byte> xm_state = new List<byte>();
            List<byte> xm_state_lin = new List<byte>();
            if (this.cmdfilecomboBox.Text == "GPSR.ini")//GPSR
            {
                int flag = 0;//flag=1导航  =2抓取  =3放下
                string result = rar(a);
                Console.WriteLine(result);
                foreach (string substr in result.Split(' '))
                {
                    Console.WriteLine(substr);
                    if (flag != 0)
                    {
                        if (substr != "person")
                        {
                            if (flag == 1)
                            {
                                string loc = substr;
                                //float x;
                                //float y;
                                //float z;
                                xm_state.Add(0x01);
                                //location now become NO. instead
                                /*switch (loc)
                                {
                                    case "table":
                                        x = (float)1.0;
                                        y = (float)2.0;
                                        z = (float)3.0;
                                        xm_state.AddRange(BitConverter.GetBytes(x));
                                        xm_state.AddRange(BitConverter.GetBytes(y));
                                        xm_state.AddRange(BitConverter.GetBytes(z));
                                        break;
                                    case "kitchen":
                                        x = (float)2.0;
                                        y = (float)1.6;
                                        z = (float)-20;
                                        xm_state.AddRange(BitConverter.GetBytes(x));
                                        xm_state.AddRange(BitConverter.GetBytes(y));
                                        xm_state.AddRange(BitConverter.GetBytes(z));
                                        break;
                                }*/
                                byte[] c = System.Text.Encoding.Default.GetBytes(substr);
                                byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                                xm_state.AddRange(len);
                                xm_state.AddRange(c);
                                spout = spout + "go to" + substr;
                                //this.sendSrResult(xm_state.ToArray());
                                xm_state.Add(0x00);

                            }
                            else
                            {
                                if (flag == 2)
                                {
                                    byte[] c = System.Text.Encoding.Default.GetBytes(substr);
                                    byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                                    xm_state.AddRange(len);
                                    xm_state.AddRange(c);
                                    spout = spout + "catch";
                                    spout = spout + substr;
                                }
                                if (flag == 3)
                                {
                                    //xm_state.Add(0x05);
                                    spout = spout + "put";
                                }
                                //this.sendSrResult(xm_state.ToArray());
                                xm_state.Add(0x00);

                            }
                        }
                        else
                        {
                            /*
                            float x = (float)0.0;
                            float y = (float)0.0;
                            float z = (float)0.0;
                            xm_state.Add(0x01);
                            xm_state.AddRange(BitConverter.GetBytes(x));
                            xm_state.AddRange(BitConverter.GetBytes(y));
                            xm_state.AddRange(BitConverter.GetBytes(z));*/
                            spout = spout + "go to the master";
                            xm_state.Add(0x01);
                            byte[] c = System.Text.Encoding.Default.GetBytes("start");
                            byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                            xm_state.AddRange(len);
                            xm_state.AddRange(c);
                            //this.sendSrResult(xm_state.ToArray());
                            xm_state.Add(0x00);
                        }
                        flag = 0;
                    }
                    if (flag == 0)
                    {
                        if (substr == "0x02")
                        {
                            xm_state.Add(0x01);
                            flag = 1;
                        }
                        if (substr == "0x04")
                        {
                            xm_state.Add(0x04);
                            flag = 2;
                        }
                        if (substr == "0x05")
                        {
                            xm_state.Add(0x05);
                            flag = 3;
                        }
                    }
                }
                xm_state.Add(0xff);
                //this.sendSrResult(xm_state.ToArray());
                //xm_state.Clear();
                return xm_state.ToArray();
            }
            if (this.cmdfilecomboBox.Text == "follow.ini")//follow
            {
                if (a == "please remember me")//follow
                {
                    xm_state.Add(0x01);
                    string name = "player";
                    byte[] c = System.Text.Encoding.Default.GetBytes(name);
                    byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                    xm_state.AddRange(len);
                    xm_state.AddRange(c);
                }
                if (a == "follow me")//follow
                {
                    xm_state.Add(0x02);
                }
                if (a == "please leave the elevateor")//follow
                {
                    xm_state.Add(0x03);
                }
            }
            if (this.cmdfilecomboBox.Text == "emmergency.ini")//em
            {
                if (a.Substring(0, 12) == "I want some ")//whoiswho
                {
                    a = a.Remove(0, 12);
                    ttsSp.Speak("you need some " + a + "is it right");
                }
                else
                {
                    ttsSp.Speak("I thing you need some manual water");
                }
            }
            if (this.cmdfilecomboBox.Text == "whoiswho.ini")//follow
            {
                if (a.Substring(0, 5) == "I am ")//whoiswho
                {
                    xm_state.Add(0x01);
                    a = a.Remove(0, 5);
                    //a = a.Replace(", please remember me", "");
                    string username = a;
                    byte[] c = System.Text.Encoding.Default.GetBytes(a);
                    byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                    xm_state.AddRange(len);
                    xm_state.AddRange(c);
                }
            }
            //***********************************************************************************
            //whatdiyoysay
            //***********************************************************************************
            if (this.cmdfilecomboBox.Text == "whatdidyousay.ini")//whatdidyousay
            {
                if (a == "") ttsSp.Speak("");
            }
            //*************************************************************************************
            if (this.cmdfilecomboBox.Text == "shopping.ini")//shopping
            {
                bool end = false;
                if (a.Substring(0, 6) == "bring ")//whoiswho
                {
                    xm_state.Add(0x02);
                    ttsSp.Speak("yes, I see");
                    a = a.Remove(0, 8);
                    string goods = a;
                    byte[] c = System.Text.Encoding.Default.GetBytes(a);
                    byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                    xm_state.AddRange(len);
                    xm_state.AddRange(c);
                    end = true;              
                }
                if (!end)
                {
                    if (a.Substring(0, 8) == "this is ")//whoiswho
                    {
                        xm_state.Add(0x01);
                        a = a.Remove(0, 8);
                        ttsSp.Speak("yes, I see");
                        string goods = a;
                        byte[] c = System.Text.Encoding.Default.GetBytes(a);
                        byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                        xm_state.AddRange(len);
                        xm_state.AddRange(c);
                        end = true;
                    }
                }
                if (!end)
                {
                    if (a.Substring(0, 8) == "here is ")//whoiswho
                    {
                        xm_state.Add(0x01);
                        a = a.Remove(0, 8);
                        string loc = a;
                        ttsSp.Speak("I know here is " + loc);
                        byte[] c = System.Text.Encoding.Default.GetBytes(a);
                        byte[] len = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(c.Length));
                        xm_state.AddRange(len);
                        xm_state.AddRange(c);
                        end = true;
                    }
                }
            }
            if (this.cmdfilecomboBox.Text != "GPSR.ini")
            {
                cansend = true;
                if(xm_state.Count>0) sendSrResult(xm_state.ToArray());
                return xm_state.ToArray();
            }
            else return null;
        }

        private string rar(string a)//调用nltk
        {
            writefile(a);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";//要执行的程序名称
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;//可能接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序
            //向CMD窗口发送输入信息：
            p.StandardInput.WriteLine("E:\\eclipse\\新建文件夹\\finish1.0\\finish1.0.py");
            p.StandardInput.WriteLine("exit");
            //获取CMD窗口的输出信息：
            string sOutput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            int flag = 0;
            int count = 0;
            int n = 0;
            int i;
            string msgreturn = " ";
            string cmdreturn = "";
            string[] ContentLines = sOutput.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            n = ContentLines.Length;
            for (i = 0; i < n; i++)
            {
                if (ContentLines[i] == "end")
                {
                    flag = 0;
                    Console.WriteLine(msgreturn);
                    cmdreturn = cmdreturn + " " + msgreturn;
                    msgreturn = "";
                    Console.WriteLine("all mission end");
                    Console.WriteLine();
                }
                if (ContentLines[i] == "mission")
                {
                    count++;
                    if (count != 1)
                    {
                        srRichTextBox.AppendText(msgreturn + "\n");
                    }
                    cmdreturn = cmdreturn + " " + msgreturn;
                    msgreturn = "";
                }
                if (flag == 1) msgreturn = msgreturn + " " + ContentLines[i];
                if (ContentLines[i] == "begin") flag = 1;
            }
            srRichTextBox.AppendText(msgreturn + "\n");
            cmdreturn = change(cmdreturn);
            return cmdreturn;
        }
        private void writefile(string a)//将英文字符串写入中转文件
        {
            string lines = a;
            System.IO.File.WriteAllText(@"E:\\txt.txt", lines);
        }

        private string change(string msg)//change the order
        {
            string ret = "";
            string lin = "";
            string back = "";
            string ll = "";
            int flag = 0;
            int lf = 0;
            foreach (string substr in msg.Split(' '))
            {
                if (substr == "mission")
                {
                    ret = ret + " " + lin + " " + ll;
                    back = back + " " + ret;
                    //i++;
                    ret = "mission";
                    lin = "";
                    ll = "";
                }
                if (flag == 1)
                {
                    if (substr != "person")
                    {
                        ret = ret + " " + "0x02";
                        ret = ret + " " + substr;
                        flag = 0;
                        lf = 1;
                    }
                    else
                    {
                        ll = "0x02 person";
                        flag = 0;
                        lf = 1;
                    }
                }

                if (substr == "0x02")
                {
                    flag = 1;
                }
                else
                {
                    if (flag != 1 && substr != "mission" && lf == 0) lin = lin + " " + substr;
                    if (lf == 1) lf = 0;
                }
            }
            ret = ret + " " + lin + ll;
            back = back + " " + ret;
            return back;
        }

        private void cmdfilecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
