using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dynamicDesktop
{
    class LockScreenChanger
    {
        public static void ChangeBackground(Settings settings)
        {
            if (settings.Folders.Count != 0)
            {
                Random rnd = new Random();
                var settingsfolder = settings.Folders[rnd.Next(0, settings.Folders.Count)];
                string[] files = Directory.GetFiles(settingsfolder);
                string file = files[rnd.Next(0, files.Length)];
                //try
                //{
                //BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open));
                //byte[] header = reader.ReadBytes(10);
                //if (
                //    header.Take(4) == new byte[] { 0x89, Convert.ToByte('P'), Convert.ToByte('N'), Convert.ToByte('G') }

                //)
                //{

                //}
                //SetLockScreen(file, 10, false);
                //}
                //catch (Exception)
                //{

                //    throw;
                //}

            }
        }
        public static void SetLockScreen(string file)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\PersonalizationCSP", true);
            key.SetValue(@"DesktopImagePath", file);
            key.SetValue(@"DesktopImageUrl", file);
            key.SetValue(@"LockScreenImagePath", file);
            key.SetValue(@"LockScreenImageUrl", file);
        }
    }
}
