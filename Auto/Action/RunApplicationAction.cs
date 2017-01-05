using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auto
{
    class RunApplicationAction : Action
    {
        public string ApplicationPath { get; set; }

        public string ApplicationArguments { get; set; }

        public RunApplicationAction(string actionText) : base(actionText)
        {
            this.Command = "Run";
            this.ActionText = actionText;
            string[] s = this.ActionText.Split(new string[] { "|" }, StringSplitOptions.None);
            if (s != null && s.Length > 0)
            {
                // Application Path
                this.ApplicationPath = s[2];

                // Application Arguments
                this.ApplicationArguments = "";
                if (s.Length >= 4)
                {
                    for (int i = 3; i < s.Length; i++)
                    {
                        this.ApplicationArguments += s[i];
                    }
                }
            }
        }

        public override void Run()
        {
            Thread.Sleep(this.Delay);
            Process.Start(ApplicationPath, ApplicationArguments);
        }
    }
}
