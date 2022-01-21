using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RJJSON;
using System.IO;
using System.Timers;

namespace dynamicDesktop
{
    static class Program
    {
        public static string SettingsFile;
        public static string SettingsString;
        public static bool ShouldExit = false;
        public static Settings settings;
        public static Icon icon = ExtractIcon("imageres.dll", 145, true);
        public static System.Timers.Timer MainTimer;

        static NotifyIcon trayIcon;
        public/*keep only for debug*/ static ContextMenu trayMenu;
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                //while (Args[Args.Length-1] == "-h"){
                //    Args = (string[])Args.Take(Args.Length - 1);
                //}
                SettingsFile = args[args.Length - 1];
            }
            if (!File.Exists(SettingsFile))
            {
                SettingsFile = Directory.GetCurrentDirectory() + SettingsFile;
            }
            if (!File.Exists(SettingsFile))
            {
                SettingsFile = Directory.GetCurrentDirectory() + "Settings.json";
            }
            if (!File.Exists(SettingsFile))
            {
                SettingsFile = AppDomain.CurrentDomain.BaseDirectory + "Settings.json";
            }
            if (!File.Exists(SettingsFile))
            {
                File.WriteAllText(SettingsFile, "{}");
            }
            SettingsString = File.ReadAllText(SettingsFile);
            settings = new Settings(SettingsString);

            MainTimer = new System.Timers.Timer(settings.Time.TotalMiliSecs);
            MainTimer.Elapsed += TimmerTick;
            MainTimer.AutoReset = true;
            MainTimer.Enabled = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", trayMenu_Exit);
            trayMenu.MenuItems.Add("Change", trayMenu_Change);
            trayMenu.MenuItems.Add("test", trayMenu_test);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Dynamic Wallpaper";
            trayIcon.Icon = new Icon(icon, 40, 40);
            trayIcon.MouseClick += new MouseEventHandler(TrayIcon_MouseClick);

            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            Application.Run(new Form1(args.Contains("-h")));
        }
        private static void TimmerTick(Object source, ElapsedEventArgs e)
        {
            BackgroundChanger.ChangeBackground(Program.settings);
            ((Form1)Application.OpenForms[0]).UpdateImage();
        }
        static void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Application.OpenForms[0].Show();
            }
            ((Form1)Application.OpenForms[0]).UpdateImage();
        }

        static void trayMenu_Exit(object sender, EventArgs e)
        {
            ShouldExit = true;
            Application.Exit();
        }
        static void trayMenu_Change(object sender, EventArgs e)
        {
            BackgroundChanger.ChangeBackground(Program.settings);
            ((Form1)Application.OpenForms[0]).UpdateImage();
        }
        public/*keep only for debug*/ static void trayMenu_test(object sender, EventArgs e) { }
        public static Icon ExtractIcon(string file, int number, bool largeIcon)
        {//https://stackoverflow.com/questions/6872957/how-can-i-use-the-images-within-shell32-dll-in-my-c-sharp-project
            IntPtr large;
            IntPtr small;
            NativeMethods.ExtractIconEx(file, number, out large, out small, 1);
            try
            {
                return Icon.FromHandle(largeIcon ? large : small);
            }
            catch
            {
                return null;
            }
        }
        //Style = style;
        //Tile = tile;
        //SizeRestriction = sizeRestriction;
        //Orientation = orientation;
        //MaxSize = maxSize;
        //MinSize = minSize;
        //Time = time;
    }
}
