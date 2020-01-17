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
        public override string Command { get => "Run"; }

        public string ApplicationPath { get; set; }

        public string ApplicationArguments { get; set; }

        public RunApplicationAction(string actionText) : base(actionText)
        {
            string[] s = this.ActionText.Split(new string[] { Action.Separator }, StringSplitOptions.None);
            if (this.IsValid && s != null && s.Length > 0)
            {
                // Application Path
                this.ApplicationPath = s[1].Trim();

                // Application Arguments
                this.ApplicationArguments = "";
                if (s.Length >= 3)
                {
                    for (int i = 2; i < s.Length; i++)
                    {
                        this.ApplicationArguments += s[i].Trim() + " ";
                    }
                }
            }
        }

        public override void Run()
        {
            Console.WriteLine($"{this.Command} ({this.ApplicationPath}, {this.ApplicationArguments})");
            Process.Start(this.ApplicationPath, this.ApplicationArguments);
        }
    }
}
