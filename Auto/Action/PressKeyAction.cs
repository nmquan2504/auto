using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto
{
    class SendKeysAction : Action
    {
        public string Keys { get; set; }

        public SendKeysAction(string actionText) : base(actionText)
        {
            this.ActionText = actionText;
            this.Command = "SendKeys";
            string[] s = this.ActionText.Split(new string[] { "|" }, StringSplitOptions.None);
            if (s != null && s.Length > 0)
            {
                // Keys
                this.Keys = s[2];
            }
        }

        public override void Run()
        {
            SendKeys.Send(this.Keys);
        }
    }
}
