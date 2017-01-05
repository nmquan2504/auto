using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Auto
{
    class LeftClickAction : Action
    {
        const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        const uint MOUSEEVENTF_LEFTUP = 0x0004;
        const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        const uint MOUSEEVENTF_MOVE = 0x0001;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        const uint MOUSEEVENTF_XDOWN = 0x0080;
        const uint MOUSEEVENTF_XUP = 0x0100;
        const uint MOUSEEVENTF_WHEEL = 0x0800;
        const uint MOUSEEVENTF_HWHEEL = 0x01000;

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(uint x, uint y);

        public uint X { get; set; }

        public uint Y { get; set; }

        public LeftClickAction(string actionText) : base(actionText)
        {
            this.ActionText = actionText;
            this.Command = "LeftClick";
            string[] s = this.ActionText.Split(new string[] { "|" }, StringSplitOptions.None);
            if (s != null && s.Length > 0)
            {
                // X
                uint x = 0;
                if (uint.TryParse(s[2], out x))
                {
                    this.X = x;
                    //this.X = (uint)(this.X * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width);
                }

                // Y
                uint y = 0;
                if (uint.TryParse(s[3], out y))
                {
                    this.Y = y;
                    //this.Y = (uint)(this.Y * 65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
                }
            }
        }

        public override void Run()
        {
            Thread.Sleep(this.Delay);
            SetCursorPos(this.X, this.Y);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(100);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
