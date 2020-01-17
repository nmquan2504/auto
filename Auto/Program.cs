using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
            Console.WriteLine("Reading action list...");
            List<Action> actions = new List<Action>();
            try
            {
                using (StreamReader sr = new StreamReader(args[0]))
                {
                    while (!sr.EndOfStream)
                    {
                        string s = sr.ReadLine();
                        try
                        {
                            if (!string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                            {
                                var action = Action.CreateAction(s);
                                if (action != null)
                                {
                                    Console.WriteLine($"Action '{action.ToString()}'");
                                    actions.Add(action);
                                }
                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"ERROR! Unknown action '{s}'!");
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
            Console.WriteLine("Done reading action list.");

            // Run
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
                            Console.WriteLine(action.ToString());
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
