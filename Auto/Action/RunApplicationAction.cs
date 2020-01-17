using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auto
{
    class RunApplicationAction : Action
    {
        public override string ActionCommand { get => "Run"; }

        public string ApplicationPath { get; set; }

        public string ApplicationArguments { get; set; }

        public RunApplicationAction(string actionText) : base(actionText)
        {
            string[] s = this.ActionText.Split(new string[] { Action.ActionParameterSeparator }, StringSplitOptions.None);
            if (this.IsValid && s != null && s.Length > 0)
            {
                // Application Path
                this.ApplicationPath = s[1].Trim();

                // Application Arguments
                this.ApplicationArguments = "";
                if (s.Length >= 3)
                {
                    this.ApplicationArguments = s[2].Trim();
                }
            }
        }

        public override void Run()
        {
            if (File.Exists(this.ApplicationPath))
            {
                Process.Start(this.ApplicationPath, this.ApplicationArguments);
            }
        }

        public override string ToString()
        {
            if (File.Exists(this.ApplicationPath))
            {
                FileInfo fi = new FileInfo(this.ApplicationPath);
                if (!string.IsNullOrWhiteSpace(this.ApplicationArguments))
                {
                    return $"{this.ActionCommand} ({fi.Name}, {this.ApplicationArguments}) {this.ActionComment}".Trim();
                }
                else
                {
                    return $"{this.ActionCommand} ({fi.Name}) {this.ActionComment}".Trim();
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(this.ApplicationArguments))
                {
                    return $"{this.ActionCommand} ({this.ApplicationPath}, {this.ApplicationArguments})";
                }
                else
                {
                    return $"{this.ActionCommand} ({this.ApplicationPath})";
                }
            }
        }
    }
}
