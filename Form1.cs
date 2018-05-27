using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Media;
using System.Diagnostics;

namespace TwitchBot
{
    public partial class Form1 : Form
    {
        #region Variables
        private static string userName = "";
        private static string password = "";

        IrcClient irc = new IrcClient("irc.twitch.tv", 6667, userName, password);
        NetworkStream serverStream = default(NetworkStream);
        string readData = "";
        Thread chatthread;
        #endregion
        public Form1()
        {
            InitializeComponent();

            irc.joinRoom("dqxygamer");
            chatthread = new Thread(getMessage);
            chatthread.Start();
        }
        







            private void getMessage()
            {
            serverStream = irc.tcpClient.GetStream();
            int buffsize = 0;
            byte[] inStream = new byte[10025];
            buffsize = irc.tcpClient.ReceiveBufferSize;
            while (true)
            {
                try
                {
                    readData = irc.readMessage();
                    msg();
                }
                catch (Exception)
                {

                }
            }
        }

        private void msg()
        {
            if (this.InvokeRequired) this.Invoke(new MethodInvoker(msg));
            else
            {
                ChatBox.Text = ChatBox.Text + readData.ToString() + Environment.NewLine;
            }
        }

       


    class IrcClient
        {
            private string userName;
            private string channel;

            public TcpClient tcpClient;
            private StreamReader inputStream;
            private StreamWriter outputStream;
            public IrcClient(string ip, int port, string userName, string password)
            {
                tcpClient = new TcpClient(ip, port);
                inputStream = new StreamReader(tcpClient.GetStream());
                outputStream = new StreamWriter(tcpClient.GetStream());

                outputStream.WriteLine("PASS" + password);
                outputStream.WriteLine("NICK" + userName);
                outputStream.WriteLine("USER" + userName + " 8 * :" + userName);
                outputStream.WriteLine("CAP REQ :twitch.tv/membership");
                outputStream.WriteLine("CAP REQ :twitch.tv/commands");
                outputStream.Flush();
            }

            public void joinRoom(string channel)
            {
                this.channel = channel;
                outputStream.WriteLine("JOIN #" + channel);
                outputStream.Flush();
            }

            public void leaveRoom()
            {
                outputStream.Close();
                inputStream.Close();
            }

            public void sendIrcMessage(string messsage)
            {
                outputStream.WriteLine(messsage);
                outputStream.Flush();
            }
            public void sendChatMessage(string message)
            {
                sendIrcMessage(":" + userName + "!" + userName + "@" + userName + ".irc.twitch.tv PRIVMSG #" + channel + ":" + message);

            }

            public void PingResponse()
            {
                sendIrcMessage("PONG irc.twitch.tv/r/n");
            }

            public string readMessage()
            {
                string message = "";
                message = inputStream.ReadLine();
                return message;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
         
        {

        }
    }
}
















 