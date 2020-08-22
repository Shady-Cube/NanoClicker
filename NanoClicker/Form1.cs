// Good luck reading this code lmao
// I can't even read this shit
// Might tidy up the code if more people download this

// If you want to contact me, here is my website with all my contacts: https://shady-cube.github.io/Official-Shady-Cube-Website/

// NanoClicker made by Shady-Cube

using NanoClicker.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Drawing.Text;
using System.Diagnostics;
using System.Net;

namespace NanoClicker
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int RMUP = 0x0010;
        private const int RMDOWN = 0x0008;
        private const int LMUP = 0x0004;
        private const int LMDOWN = 0x0002;
        

        Random random = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        int AVGcps = 13;
        string Hkey = "F";
        bool Left_click = true;
        bool Right_click = true;
        bool Mc_only = true;

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        Point lastPoint;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForUpdates();
            
            NanoClicker.InterceptKeys.OnKeyDown += new KeyEventHandler(myKeyDown);
            NanoClicker.InterceptKeys.Start();

            AverageCPS.Value = AVGcps;
            button3.Text = Hkey;
            checkBox1.Checked = Left_click;
            checkBox2.Checked = Mc_only;
            label3.Text = Convert.ToString(AVGcps) + " CPS";

            try
            {
                string[] testline = { "test" };
                System.IO.File.WriteAllLines(@"C:\Windows\Prefetch\test.txt", testline);
                File.Delete(@"C:\Windows\Prefetch\test.txt");
            }
            catch (Exception)
            {
                MessageBox.Show("Self Destruct is only functional if you launch NanoClicker as an administrator.", "Self Destruct disabled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4.Enabled = false;
            }
        }

        private void CheckForUpdates()
        {
            try
            {
                WebRequest wr = WebRequest.Create(new Uri("https://raw.githubusercontent.com/Shady-Cube/Version-Checker/master/NanoClicker"));
                WebResponse ws = wr.GetResponse();
                StreamReader sr = new StreamReader(ws.GetResponseStream());

                double version = 15;
                double currentversion = Convert.ToDouble(sr.ReadToEnd());

                if (version != currentversion)
                {
                    DialogResult UserWantsNewUpdate = MessageBox.Show("A new version of NanoClicker is available! Would you like to update?", "New version available", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (UserWantsNewUpdate == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("https://shady-cube.github.io/NanoClicker/");
                        this.Close();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to search for new updates. If this keeps ocurring please check your version of NanoClicker.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AverageCPS_Scroll(object sender, EventArgs e)
        {
            AVGcps = AverageCPS.Value;
            label3.Text = Convert.ToString(AverageCPS.Value) + " CPS";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Text = "...";
        }

        private void button3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (button3.Text == "...")
            {
                string Hotkey;
                Hotkey = e.KeyChar.ToString();
                Hotkey = Hotkey.ToUpper();
                button3.Text = Hotkey;
                Hkey = Hotkey;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Mc_only = true;
            }
            else
            {
                Mc_only = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Left_click = true;
            }
            else
            {
                Left_click = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string appname = AppDomain.CurrentDomain.FriendlyName;
            DirectoryInfo dinfo = new DirectoryInfo(@"C:\Windows\Prefetch");
            FileInfo[] Files = dinfo.GetFiles(appname + "*");
            foreach (FileInfo file in Files)
            {
                File.Delete(file.FullName);
            }

            Environment.Exit(0);
        }

        private void Clicker_Tick(object sender, EventArgs e)
        {
            int MinCps, MaxCps;
            MinCps = 1000 / AverageCPS.Value - random.Next(0, 4);
            MaxCps = 1000 / AverageCPS.Value + random.Next(0, 4);
            int Srandom = random.Next(-40, 40);
            int Srandom2 = random.Next(-10, 10);
            int Srandom3 = random.Next(-5, 5);

            try
            {
                Clicker.Interval = random.Next(MinCps, MaxCps) + Srandom + Srandom2 + Srandom3;
            }
            catch (Exception) {}
            if (Mc_only)
            {
                string ActiveWindowTitle = GetActiveWindowTitle();

                try
                {
                    if (ActiveWindowTitle.Contains("inecraft") || ActiveWindowTitle.Contains("1.") || ActiveWindowTitle.Contains("avaw") || ActiveWindowTitle.Contains("unar") || ActiveWindowTitle.Contains("ounge") || ActiveWindowTitle.Contains("eatBreaker")) { }
                    else
                    {
                        return;
                    }
                }
                catch (Exception)
                {}
                              
            }

            if (Right_click = true && MouseButtons == MouseButtons.Right)
            {
                mouse_event(RMUP, 0, 0, 0, 0);
                Thread.Sleep(random.Next(4, 10));
                mouse_event(RMDOWN, 0, 0, 0, 0);
            }

            if (Left_click = true && MouseButtons == MouseButtons.Left)
            {
                mouse_event(LMUP, 0, 0, 0, 0);
                Thread.Sleep(random.Next(4, 10));
                mouse_event(LMDOWN, 0, 0, 0, 0);
            }          
        }

        void myKeyDown(object sender, KeyEventArgs e)
        {
            string Hotkey = Hkey;
            Hotkey = Hotkey.ToUpper();

            if (Convert.ToString(e.KeyCode) == Hotkey && button3.Text != "...")
            {
                if (label6.Text == "Enabled")
                {
                    Clicker.Stop();
                    label6.Text = "Disabled";
                    label6.ForeColor = Color.Red;
                }
                else
                {
                    Clicker.Start();
                    label6.Text = "Enabled";
                    label6.ForeColor = Color.Lime;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UC9EDLhh6ePIDCxXG0HKR0zw");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                Right_click = true;
            }
            else
            {
                Right_click = false;
            }
        }
    }
}
