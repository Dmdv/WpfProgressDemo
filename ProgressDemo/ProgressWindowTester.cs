using System;
using System.Threading;
using System.Windows.Forms;
using WpfControls.Controls;

namespace ProgressDemo
{
	public partial class ProgressWindowTester : Form
	{
		public ProgressWindowTester()
		{
			InitializeComponent();
		}

		private void FiniteActionWithReport(CommonExtensions.IProgress<double> progress)
		{
			for (var idx = 0; idx < 10; idx++)
			{
				Thread.Sleep(500);
				progress.Report(idx);
			}
		}

		private void IndefiniteAction()
		{
			for (var idx = 0; idx < 10; idx++)
			{
				Thread.Sleep(500);
			}
		}

		private void IndefiniteActionWithReport(CommonExtensions.IProgress<double> progress)
		{
			for (var idx = 0; idx < 10; idx++)
			{
				Thread.Sleep(500);
				progress.Report(idx);
			}
		}

		private void bnIndefWithReport_Click(object sender, EventArgs e)
		{
			var progress = new ProgressWindow();
			progress.Run(IndefiniteActionWithReport);
		}

		private void bnStartDef_Click(object sender, EventArgs e)
		{
			var progress = new ProgressWindow();
			progress.Run(FiniteActionWithReport, 10);
		}

		private void bnStartIndef_Click(object sender, EventArgs e)
		{
			var progress = new ProgressWindow();
			progress.Run(IndefiniteAction);
		}
	}
}