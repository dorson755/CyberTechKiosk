using System;
using System.Windows.Forms;

namespace CyberTechKiosk
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DatabaseManager.InitializeDatabase(); // Initialize DB before running app
            Application.Run(new MainForm());
        }
    }
}