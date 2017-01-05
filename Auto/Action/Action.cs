using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Auto
{
    abstract class Action
    {
        public Action(string action)
        {
            this.ActionText = action;
            string[] s = this.ActionText.Split(new string[] { "|" }, StringSplitOptions.None);
            if (s != null && s.Length > 0)
            {
                // Delay (default = 100)
                int delay = 100;
                if (int.TryParse(s[1], out delay))
                {
                    this.Delay = delay;
                }
            }
        }

        public static Action CreateAction(string action)
        {
            Action a = null;

            if ((a = new RunApplicationAction(action)).IsValid())
            {
                return a;
            }

            if ((a = new CloseApplicationAction(action)).IsValid())
            {
                return a;
            }

            if ((a = new LeftClickAction(action)).IsValid())
            {
                return a;
            }

            return null;
        }

        protected string ActionText { get; set; }

        public string Command { get; set; }

        public int Delay { get; set; }

        public bool IsValid()
        {
            return this.ActionText.StartsWith(this.Command);
        }

        public abstract void Run();
    }
}
