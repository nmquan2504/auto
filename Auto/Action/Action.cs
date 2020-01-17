using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Auto
{
    abstract class Action
    {
        public const string Separator = "|";

        protected Action(string action)
        {
            this.ActionText = action.Trim();
        }

        public static Action CreateAction(string action)
        {
            var asm = Assembly.GetExecutingAssembly();
            var types = asm.DefinedTypes;
            foreach (var type in types)
            {
                if (type.BaseType == typeof(Action))
                {
                    var constructor = type.GetConstructor(new Type[] { typeof(string) });
                    if (constructor != null)
                    {
                        var a = (Action)constructor.Invoke(new object[] { action });
                        if (a != null && a.IsValid)
                        {
                            return a;
                        }
                    }
                }
            }
            return null;
        }

        protected string ActionText { get; }

        public abstract string Command { get; }

        public bool IsValid { get => this.ActionText.StartsWith(this.Command); }

        public abstract void Run();
    }
}
