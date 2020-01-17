using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Auto
{
    class ClickMouseAction : Action
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

        public override string ActionCommand { get => "Click"; }

        public string Button { get; set; }

        public uint X { get; set; }

        public uint Y { get; set; }

        public ClickMouseAction(string actionText) : base(actionText)
        {
            string[] s = this.ActionText.Split(new string[] { Action.ActionParameterSeparator }, StringSplitOptions.None);
            if (this.IsValid && s != null && s.Length > 0)
            {
                // Button
                this.Button = s[1].Trim().ToUpper();

                // X
                uint x = 0;
                if (uint.TryParse(s[2].Trim(), out x))
                {
                    this.X = x;
                }

                // Y
                uint y = 0;
                if (uint.TryParse(s[3].Trim(), out y))
                {
                    this.Y = y;
                }
            }
        }

        public override void Run()
        {
            SetCursorPos(this.X, this.Y);
            Thread.Sleep(50);
            uint button = MOUSEEVENTF_LEFTDOWN;
            if (this.Button == "L")
            {
                button = MOUSEEVENTF_LEFTDOWN;
            }
            else if (this.Button == "R")
            {
                button = MOUSEEVENTF_RIGHTDOWN;
            }
            else if (this.Button == "M")
            {
                button = MOUSEEVENTF_MIDDLEDOWN;
            }
            mouse_event(button, 0, 0, 0, 0);
            Thread.Sleep(50);
            if (this.Button == "L")
            {
                button = MOUSEEVENTF_LEFTUP;
            }
            else if (this.Button == "R")
            {
                button = MOUSEEVENTF_RIGHTUP;
            }
            else if (this.Button == "M")
            {
                button = MOUSEEVENTF_MIDDLEUP;
            }
            mouse_event(button, 0, 0, 0, 0);
        }

        public override string ToString()
        {
            return $"{this.ActionCommand} ({this.Button}, {this.X}, {this.Y}) {this.ActionComment}".Trim();
        }
    }
}
