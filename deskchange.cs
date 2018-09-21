using System;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

//reference https://github.com/ebkalderon/experimental-repo/blob/master/VirtualDesktop.cs
//          https://blogs.msdn.microsoft.com/thottams/2006/08/11/accessing-createprocess-from-c-and-vb-net/
namespace DeskChange
{
    class DeskChangeApp
    {
        enum DESKTOP_ACCESS_MASK : uint
        {
            DESKTOP_NONE = 0,
            DESKTOP_READOBJECTS = 0x0001,
            DESKTOP_CREATEWINDOW = 0x0002,
            DESKTOP_CREATEMENU = 0x0004,
            DESKTOP_HOOKCONTROL = 0x0008,
            DESKTOP_JOURNALRECORD = 0x0010,
            DESKTOP_JOURNALPLAYBACK = 0x0020,
            DESKTOP_ENUMERATE = 0x0040,
            DESKTOP_WRITEOBJECTS = 0x0080,
            DESKTOP_SWITCHDESKTOP = 0x0100,
            EVERYTHING = (DESKTOP_READOBJECTS | DESKTOP_CREATEWINDOW | DESKTOP_CREATEMENU |
                          DESKTOP_HOOKCONTROL | DESKTOP_JOURNALRECORD | DESKTOP_JOURNALPLAYBACK |
                          DESKTOP_ENUMERATE | DESKTOP_WRITEOBJECTS | DESKTOP_SWITCHDESKTOP),
        }
        enum WINSTA_ACCESS_MASK : uint
        {
            WINSTA_NONE = 0,
            WINSTA_ACCESSCLIPBOARD = 0x0004,
            WINSTA_ACCESSGLOBALATOMS = 0x0020,
            WINSTA_CREATEDESKTOP = 0x0008,
            WINSTA_ENUMDESKTOPS = 0x0001,
            WINSTA_ENUMERATE = 0x0100,
            WINSTA_EXITWINDOWS = 0x0040,
            WINSTA_READATTRIBUTES = 0x0002,
            WINSTA_READSCREEN = 0x0200,
            WINSTA_WRITEATTRIBUTES = 0x0010,
            EVERYTHING = (WINSTA_ACCESSCLIPBOARD | WINSTA_ACCESSGLOBALATOMS | WINSTA_CREATEDESKTOP |
                          WINSTA_ENUMDESKTOPS | WINSTA_ENUMERATE | WINSTA_EXITWINDOWS | 
                          WINSTA_READATTRIBUTES | WINSTA_READSCREEN | WINSTA_WRITEATTRIBUTES),
        }
        
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }
        
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        
        [DllImport("kernel32.dll")]
        private static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes,
                                                 IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags,
                                                 IntPtr lpEnvironment, string lpCurrentDirectory, ref STARTUPINFO lpStartupInfo,
                                                 out PROCESS_INFORMATION lpProcessInformation);
        
        [DllImport("user32.dll")]
        private static extern IntPtr CreateWindowStation(string lpwinsta, long dwDesiredAccess, IntPtr lpsa);
        
        [DllImport("user32.dll", SetLastError=true)]
        private static extern int SetProcessWindowStation(IntPtr hWinSta);
        
        [DllImport("user32.dll")]
        private static extern IntPtr CreateDesktop(string lpszDesktop, IntPtr lpszDevice, 
        IntPtr pDevmode, int dwFlags, long dwDesiredAccess, IntPtr lpsa);
        
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetThreadDesktop(int dwThreadId);
        
        [DllImport("user32.dll")]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);
        
        [DllImport("user32.dll")]
        private static extern bool SwitchDesktop(IntPtr hDesktop);
        
        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
        
        
        static void Main()
        {
            IntPtr OrigDesktop = GetThreadDesktop(GetCurrentThreadId());
            IntPtr Desktop = CreateDesktop("desk0", IntPtr.Zero, IntPtr.Zero,
                             0, (long)DESKTOP_ACCESS_MASK.EVERYTHING, IntPtr.Zero);

            SetThreadDesktop(Desktop);
            SwitchDesktop(Desktop);
            
            STARTUPINFO si = new STARTUPINFO();
            si.lpDesktop = "desk0";
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            
            CreateProcess("C:\\WINDOWS\\SYSTEM32\\cmd.exe", null, IntPtr.Zero, IntPtr.Zero, false, 0,
                          IntPtr.Zero, null, ref si, out pi);
        }
    }
}