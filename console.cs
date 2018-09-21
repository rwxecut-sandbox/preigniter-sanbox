using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;

namespace ConsApp
{
    class Cons
    {
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);
        
        public static int GWL_STYLE = -16;
        const int GWL_EXSTYLE = -20;
        
        public static int WS_CHILD = 0x40000000; //child window
        public static int WS_BORDER = 0x00800000; //window with border
        public static int WS_DLGFRAME = 0x00400000; //window with double border but no title
        public const int WS_SIZEBOX = 0x00040000;
        
        const int WS_EX_DLGMODALFRAME = 0x0001;
        const int WS_EX_CLIENTEDGE = 0x200;
        const int WS_EX_STATICTEDGE = 0x20000;
        
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;
        
        static void Main()
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.Start();
            
            System.Threading.Thread.Sleep(2000);
            IntPtr window = process.MainWindowHandle;
            
            int style = GetWindowLong(window, GWL_STYLE);
            int exstyle = GetWindowLong(window, GWL_EXSTYLE);
            
            SetWindowLong(window, GWL_STYLE, (style & ~(WS_BORDER | WS_DLGFRAME | WS_SIZEBOX)));
            SetWindowLong(window, GWL_EXSTYLE, (exstyle & ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICTEDGE)));
            
            SetWindowPos(window, IntPtr.Zero, 10, 10, 0, 0, SWP_NOZORDER | SWP_NOSIZE | SWP_SHOWWINDOW);
        }
    }
}