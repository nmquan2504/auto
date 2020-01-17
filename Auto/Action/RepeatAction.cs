using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Auto
{
    class RepeatAction : Action
    {
        public override string ActionCommand { get => "Repeat"; }

        public uint RepeatCount { get; set; }

        public RepeatAction(string actionText) : base(actionText)
        {
            string[] s = this.ActionText.Split(new string[] { Action.ActionParameterSeparator }, StringSplitOptions.None);
            if (this.IsValid && s != null && s.Length > 0)
            {
                // LoopCount
                uint c = 0;
                if (uint.TryParse(s[1].Trim(), out c))
                {
                    this.RepeatCount = c;
                }
            }
        }

        public override void Run()
        {
            if (this.RepeatCount > 0)
            {
                this.RepeatCount--;
            }
            else
            {
                Process p = Process.GetCurrentProcess();
                if (p != null)
                {
                    p.Kill();
                }
            }
        }

        public override string ToString()
        {
            if (this.RepeatCount > 0)
            {
                return $"{this.ActionCommand} ({this.RepeatCount}) {this.ActionComment}".Trim();
            }
            else
            {
                return $"Stop {this.ActionComment}".Trim();
            }
        }
    }
}
