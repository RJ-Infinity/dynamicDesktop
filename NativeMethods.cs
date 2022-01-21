using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dynamicDesktop
{
    class NativeMethods
    {
        public const int SPI_SETDESKWALLPAPER = 0x0014;
        public const int SPIF_UPDATEINIFILE = 1;
        public const int SPIF_SENDWININICHANGE = 2;


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SystemParametersInfo(
            int uiAction,
            int uiParam,
            string pvParam,
            int fWinIn
        );
        [DllImport("Shell32.dll")]
        public static extern int ExtractIconEx(
            string lpszFile,
            int nIconIndex,
            out IntPtr phiconLarge,
            out IntPtr phiconSmall,
            int nIcons
        );

    }
}
