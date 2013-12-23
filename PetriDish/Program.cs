using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ScreenSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ScreenSaverForm());

            // From http://www.harding.edu/fmccown/screensaver/screensaver.html
            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                // Handle cases where arguments are separated by colon.
                // Examples: /c:1234567 or /P:1234567
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                    secondArgument = args[1];

                if (firstArgument == "/c")           // Configuration mode
                {
                    MessageBox.Show("We'll figure out to configure it later.", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else if (firstArgument == "/p")      // Preview mode
                {
                    if (secondArgument == null)
                    {
                        MessageBox.Show("Something went wrong trying to show the preview. Um, sorry?", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    // Make a window pointer (if parse fails, we want to error out)
                    IntPtr hWnd = new IntPtr(long.Parse(secondArgument));
                    Application.Run(new PetriDishForm(hWnd));
                }
                else if (firstArgument == "/s")      // Full-screen mode
                {
                    ShowScreenSaver();
                    Application.Run();
                }
                else    // Undefined argument
                {
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                        "\" is not valid.", "ScreenSaver",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else    // No arguments - treat like /c
            {
                MessageBox.Show("We'll figure out to configure it later. (And you called it without params, btw)", "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            } 
        } // end main

        static void ShowScreenSaver()
        {
            foreach (Screen aScreen in Screen.AllScreens)
            {
                PetriDishForm aScreenSaver = new PetriDishForm(aScreen.Bounds);
                aScreenSaver.Show();
            }
        } // end showscreensaver
    }
}
