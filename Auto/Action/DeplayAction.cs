using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Auto
{
    class DelayAction : Action
    {
        public override string ActionCommand { get => "Delay"; }

        public int Time { get; set; }

        public DelayAction(string actionText) : base(actionText)
        {
            string[] s = this.ActionText.Split(new string[] { Action.ActionParameterSeparator }, StringSplitOptions.None);
            if (this.IsValid && s != null && s.Length > 0)
            {
                // X
                int t = 0;
                if (int.TryParse(s[1].Trim(), out t))
                {
                    this.Time = t;
                }
            }
        }

        public override void Run()
        {
            Thread.Sleep(this.Time);
        }

        public override string ToString()
        {
            return $"{this.ActionCommand} ({this.Time}) {this.ActionComment}".Trim();
        }
    }
}
