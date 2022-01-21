using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dynamicDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool StartHidden = false;
        public Form1(bool hide)
        {
            InitializeComponent();
            StartHidden = hide;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), new Rectangle(new Point(0, 0), new Size(Width, 22)));
            base.OnPaint(e);
        }
        private void Form1_Resize(object sender, EventArgs e){
            splitContainer1.Height = Height - toolStrip1.Height - /*tabControl1.Height -*/ 40;
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void addFolderBtn_Click(object sender, EventArgs e)
        {
            Program.settings.Folders.Add(textBox1.Text);
            listBox1.Items.Add(textBox1.Text);
            textBox1.Text = "";
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void removeFolderBtn_Click(object sender, EventArgs e)
        {
            int se = listBox1.SelectedIndex;
            Program.settings.Folders.RemoveAt(listBox1.SelectedIndex);
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            try{
                listBox1.SelectedIndex = se;
            }catch (ArgumentOutOfRangeException)
            {
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            if (listBox1.SelectedIndex == -1) {
                listBox1.SelectedIndex = listBox1.Items.Count -1;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeFolderBtn.Enabled = ((ListBox)sender).Items.Count != 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            addFolderBtn.Enabled = ((TextBox)sender).Text.Length != 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon = Program.icon;
            foreach (string folder in Program.settings.Folders){
                listBox1.Items.Add(folder);
            }
            splitContainer1.Height = Height - toolStrip1.Height - /*tabControl1.Height - */40;
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            removeFolderBtn.Enabled = listBox1.Items.Count != 0;
            minutesUpDownToolStrip.NumericUpDownControl.Maximum = 999;
            minutesUpDownToolStrip.NumericUpDownControl.Minimum = -999;
            minutesUpDownToolStrip.NumericUpDownControl.ValueChanged += minutesUpDownToolStrip_ValueChanged;
            minutesUpDownToolStrip.NumericUpDownControl.Value = Program.settings.Time.Mins;

            hoursUpDownToolStrip.NumericUpDownControl.Maximum = 999;
            hoursUpDownToolStrip.NumericUpDownControl.Minimum = -999;
            hoursUpDownToolStrip.NumericUpDownControl.ValueChanged += hoursUpDownToolStrip_ValueChanged;
            hoursUpDownToolStrip.NumericUpDownControl.Value = Program.settings.Time.Hours;

            daysUpDownToolStrip.NumericUpDownControl.Maximum = 999;
            daysUpDownToolStrip.NumericUpDownControl.Minimum = 0;
            daysUpDownToolStrip.NumericUpDownControl.ValueChanged += daysUpDownToolStrip_ValueChanged;
            daysUpDownToolStrip.NumericUpDownControl.Value = Program.settings.Time.Days;
            UpdateTimeMaxMin();

            addFolderBtn.Enabled = false;

            UpdateImage();

            timmer();
        }

        private void minutesUpDownToolStrip_ValueChanged(object sender, EventArgs e)
        {
            if(minutesUpDownToolStrip.NumericUpDownControl.Value >= 60){
                hoursUpDownToolStrip.NumericUpDownControl.Value += 1;
                minutesUpDownToolStrip.NumericUpDownControl.Value -= 60;
            }
            if (minutesUpDownToolStrip.NumericUpDownControl.Value < 0)
            {
                minutesUpDownToolStrip.NumericUpDownControl.Value += 60;
                hoursUpDownToolStrip.NumericUpDownControl.Value -= 1;
            }
            UpdateTimeMaxMin();
        }

        private void hoursUpDownToolStrip_ValueChanged(object sender, EventArgs e)
        {
            if(hoursUpDownToolStrip.NumericUpDownControl.Value >= 24)
            {
                daysUpDownToolStrip.NumericUpDownControl.Value += 1;
                hoursUpDownToolStrip.NumericUpDownControl.Value -= 24;
            }
            else if(hoursUpDownToolStrip.NumericUpDownControl.Value < 0)
            {
                hoursUpDownToolStrip.NumericUpDownControl.Value += 24;
                daysUpDownToolStrip.NumericUpDownControl.Value -= 1;
            }
            UpdateTimeMaxMin();
        }

        private void daysUpDownToolStrip_ValueChanged(object sender, EventArgs e)
        {
            UpdateTimeMaxMin();
        }
        private void UpdateTimeMaxMin()
        {
            if (daysUpDownToolStrip.NumericUpDownControl.Value == 0)
            {
                hoursUpDownToolStrip.NumericUpDownControl.Minimum = 0;
                if (hoursUpDownToolStrip.NumericUpDownControl.Value == 0) {
                    minutesUpDownToolStrip.NumericUpDownControl.Minimum = 1;
                }
                else
                {
                    minutesUpDownToolStrip.NumericUpDownControl.Minimum = -999;
                }
            }
            else
            {
                hoursUpDownToolStrip.NumericUpDownControl.Minimum = -999;
                minutesUpDownToolStrip.NumericUpDownControl.Minimum = -999;
            }
            Program.settings.Time = new CustomTimeSpan(
                (int)daysUpDownToolStrip.NumericUpDownControl.Value,
                (int)hoursUpDownToolStrip.NumericUpDownControl.Value,
                (int)minutesUpDownToolStrip.NumericUpDownControl.Value,
                0,
                0
            );
            Program.MainTimer.Interval = Program.settings.Time.TotalMiliSecs;
        }


        private void toolStripButton1_Paint(object sender, PaintEventArgs e)
        {
            ToolStripButton btn = (ToolStripButton)sender;
            ControlPaint.DrawBorder(
                   e.Graphics,
                   new Rectangle(0, 0, btn.Width, btn.Height),
                   // or as @LarsTech commented, this works fine too!
                   //  btn.ContentRectangle,
                   Color.FromArgb(173,173,173),
                   ButtonBorderStyle.Solid);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            BackgroundChanger.ChangeBackground(Program.settings);
            UpdateImage();

        }
        public void UpdateImage()
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            string file = BackgroundChanger.GetWallpaper().FileLocation;
            try
            {
                pictureBox1.Image = Image.FromFile(file);
            }
            catch (OutOfMemoryException)
            {
                pictureBox1.Image = Misc.Text("Error File Not Recognised As Image", pictureBox1.Width, pictureBox1.Height, new SolidBrush(Color.Red));
            }
            catch (FileNotFoundException)
            {
                pictureBox1.Image = Misc.Text("Error File Not Found", pictureBox1.Width, pictureBox1.Height, new SolidBrush(Color.Red));
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.settings.WriteSettings(Program.SettingsFile);
            if (!Program.ShouldExit)
            {
                Hide();
                e.Cancel = true;
            }
        }
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        public void timmer(){
            myTimer.Tick += new EventHandler(TimerEventProcessor);
            myTimer.Interval = 10000;
            myTimer.Start();
        }
        public void TimerEventProcessor(Object myObject, EventArgs myEventArgs){
            toolStripLabel6.Text = Program.MainTimer.Interval.ToString();
            UpdateImage();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            UpdateImage();
            if (StartHidden) { Hide(); }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
