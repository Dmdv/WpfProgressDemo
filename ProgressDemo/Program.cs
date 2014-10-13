using System;
using System.Windows.Forms;

namespace ProgressDemo
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			// Application.Run(new Splash());
			// After splash form closed, start the main form.
			// Application.Run(new Gui());
			Application.Run(new ProgressWindowTester());
		}
	}
}