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
        public const string ActionParameterSeparator = "|";

        public const string ActionCommentSeparator = "#";

        protected Action(string action)
        {
            var s = action.Trim().Split(new string[] { ActionCommentSeparator }, StringSplitOptions.None);
            this.ActionText = s[0].Trim();
            this.ActionComment = (s.Length > 1 && !string.IsNullOrWhiteSpace(s[1]) && s[1].Trim().Length > 1) ? $"{ActionCommentSeparator} {s[1].Trim()}" : "";
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

        protected string ActionComment { get; }

        public abstract string ActionCommand { get; }

        public bool IsValid { get => this.ActionText.StartsWith(this.ActionCommand); }

        public abstract void Run();
    }
}
