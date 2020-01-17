using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Auto
{
    class MoveMouseAction : Action
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetCursorPos(uint x, uint y);

        public override string ActionCommand { get => "Move"; }
        public uint X { get; set; }

        public uint Y { get; set; }

        public MoveMouseAction(string actionText) : base(actionText)
        {
            string[] s = this.ActionText.Split(new string[] { Action.ActionParameterSeparator }, StringSplitOptions.None);
            if (this.IsValid && s != null && s.Length > 0)
            {
                // X
                uint x = 0;
                if (uint.TryParse(s[1].Trim(), out x))
                {
                    this.X = x;
                }

                // Y
                uint y = 0;
                if (uint.TryParse(s[2].Trim(), out y))
                {
                    this.Y = y;
                }
            }
        }

        public override void Run()
        {
            SetCursorPos(this.X, this.Y);
        }

        public override string ToString()
        {
            return $"{this.ActionCommand} ({this.X}, {this.Y}) {this.ActionComment}".Trim();
        }
    }
}
