using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dynamicDesktop
{
    class BackgroundChanger
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
                SetWallpaper(new Wallpaper(file, 10, false));
                //}
                //catch (Exception)
                //{

                //    throw;
                //}

            }
        }
        public static void SetWallpaper(string fileLocation, int style, bool tile)
        {
            SetWallpaper(new Wallpaper(fileLocation, style, tile));
        }
        public static void SetWallpaper(Wallpaper wp)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
            key.SetValue(@"WallpaperStyle", wp.Style.ToString());
            key.SetValue(@"TileWallpaper", wp.Tile ? "1" : "0");
            if (!NativeMethods.SystemParametersInfo(
                NativeMethods.SPI_SETDESKWALLPAPER,
                0,
                wp.FileLocation,
                NativeMethods.SPIF_UPDATEINIFILE | NativeMethods.SPIF_SENDWININICHANGE
            ))
            {
                int lastError = Marshal.GetLastWin32Error();
                throw new Exception(lastError.ToString());
            }
            #if DEBUG
            //debug
            Program.trayMenu.MenuItems.RemoveAt(2);
            Program.trayMenu.MenuItems.Add(wp.FileLocation, Program.trayMenu_test);
            #endif
        }
        public static Wallpaper GetWallpaper()
        {
            Wallpaper wp = new Wallpaper();
            wp.FileLocation = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "Wallpaper", "");
            string wallpaperStyle = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", @"WallpaperStyle", null);
            int style;
            if (!int.TryParse(wallpaperStyle, out style))
            {
                throw new Exception("test");
            }
            wp.Style = style;
            string tile = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", @"TileWallpaper", null);
            if (tile == "0")
            {
                wp.Tile = false;
            }
            else if (tile == "1")
            {
                wp.Tile = true;
            }
            else
            {
                throw new Exception("test");
            }
            return wp;
        }
    }
    class Wallpaper
    {
        public Wallpaper(){ }
        public Wallpaper(string fileLocation, int style, bool tile)
        {
            FileLocation = fileLocation;
            Style = style;
            Tile = tile;
        }
        public string FileLocation;
        public int Style;
        public bool Tile;
    }
}
