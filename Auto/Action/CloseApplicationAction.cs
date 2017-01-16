using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auto
{
    class CloseApplicationAction : Action
    {
        public string ApplicationName { get; set; }

        public string ApplicationTitle { get; set; }

        public CloseApplicationAction(string actionText) : base(actionText)
        {
            this.ActionText = actionText;
            this.Command = "Close";
            string[] s = this.ActionText.Split(new string[] { "|" }, StringSplitOptions.None);
            if (s != null && s.Length > 0)
            {
                // Application Path
                this.ApplicationName = s[2];

                // Application Title
                if (s.Length >= 4)
                {
                    this.ApplicationTitle = s[3];
                }
            }
        }

        public override void Run()
        {
            Thread.Sleep(this.Delay);
            IntPtr hWnd = SearchForWindow(this.ApplicationName, "", this.ApplicationTitle);
            if (hWnd != IntPtr.Zero)
            {
                while (hWnd != IntPtr.Zero)
                {
                    SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    hWnd = SearchForWindow(this.ApplicationName, "", this.ApplicationTitle);
                }
            }
            else
            {
                Process[] processes = Process.GetProcesses();
                if (processes != null && processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        if (process.ProcessName.ToLower().Equals(this.ApplicationName.ToString()) && process.MainWindowTitle.Contains(this.ApplicationTitle))
                        {
                            process.CloseMainWindow();
                            break;
                        }
                    }
                }
            }
        }

        private static IntPtr SearchForWindow(string processName, string wndclass, string title)
        {
            SearchData sd = new SearchData { ProcessName = processName, Wndclass = wndclass, Title = title };
            EnumWindows(new EnumWindowsProc(EnumProc), ref sd);
            return sd.hWnd;
        }

        private static bool EnumProc(IntPtr hWnd, ref SearchData data)
        {
            bool bMatched = true;

            if (bMatched && !string.IsNullOrEmpty(data.Wndclass))
            {
                StringBuilder sb = new StringBuilder(1024);
                GetClassName(hWnd, sb, sb.Capacity);
                if (!sb.ToString().Contains(data.Wndclass))
                {
                    bMatched = false;
                }
            }

            if (bMatched && !string.IsNullOrEmpty(data.Title))
            {
                StringBuilder sb = new StringBuilder(1024);
                GetWindowText(hWnd, sb, sb.Capacity);
                if (!sb.ToString().Contains(data.Title))
                {
                    bMatched = false;
                }
            }

            if (bMatched && !string.IsNullOrEmpty(data.ProcessName))
            {
                bool bMatchedProcessName = false;
                uint nProcessId = 0;
                GetWindowThreadProcessId(hWnd, out nProcessId);
                Process[] processes = Process.GetProcesses();
                if (processes != null && processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        if (process.ProcessName.ToLower().Equals(data.ProcessName.ToString()) && process.Id == nProcessId)
                        {
                            bMatchedProcessName = true;
                        }
                    }
                }

                if (!bMatchedProcessName)
                {
                    bMatched = false;
                }
            }

            if (bMatched)
            {
                data.hWnd = hWnd;
                return false;   // Return false to stop EnumWindows
            }
                
            return true;
        }

        private class SearchData
        {
            public string ProcessName;
            public string Wndclass;
            public string Title;
            public IntPtr hWnd;
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        private const UInt32 WM_CLOSE = 0x0010;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
