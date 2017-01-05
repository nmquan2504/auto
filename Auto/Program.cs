using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auto
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args == null || args.Length <= 0)
            {
                return;
            }

            // Read action list
            List<Action> actions = new List<Action>();
            try
            {
                using (StreamReader sr = new StreamReader(args[0]))
                {
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            string s = sr.ReadLine();
                            if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                            {
                                actions.Add(Action.CreateAction(s));
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }

            // Run forever
            try
            {
                int nCurrentActionIndex = 0;
                while (true)
                {
                    try
                    {
                        // Current action
                        Action action = actions[nCurrentActionIndex];
                        if (action != null)
                        {
                            action.Run();
                        }
                    }
                    catch (Exception)
                    {

                    }

                    // Next action
                    nCurrentActionIndex++;
                    if (nCurrentActionIndex >= actions.Count)
                    {
                        nCurrentActionIndex = 0;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
