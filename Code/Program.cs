using static Mint.Code.Mods;

namespace Mint.Code
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Retrieve the user saved settings by using .upgrade
            Properties.Settings.Default.Upgrade();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            //TODO : find a better way to load mods from library
 
            Application.Run(new MainForm());
        }
    }
}