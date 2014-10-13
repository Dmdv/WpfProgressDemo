using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ProgressDemo.Properties;

namespace ProgressDemo
{
	public partial class Gui : Form
	{
		public Gui()
		{
			InitializeComponent();
			try
			{
				Process.Start(Path.Combine(Application.StartupPath, "MITLicense.exe"));
			}
			catch (Exception)
			{
				MessageBox.Show(
					@"Error showing license, please select Help > About in the menu to read it.",
					@"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}

		// This event handler is where the time-consuming work is done.
		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			var worker = sender as BackgroundWorker;

			for (var i = 1; i <= 10; i++)
			{
				if (worker != null && worker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}

				// Perform a time consuming operation and report progress.
				if (worker != null)
				{
					worker.ReportProgress(i*10);
				}

				Thread.Sleep(500);
			}
		}

		// This event handler updates the progress.
		private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			// Show the progress in main form (GUI)
			labelResult.Text = (e.ProgressPercentage + @"%");
			// Pass the progress to AlertForm label and progressbar
			_alert.Message = "In progress, please wait... " + e.ProgressPercentage + "%";
			_alert.ProgressValue = e.ProgressPercentage;
		}

		// This event handler deals with the results of the background operation.
		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				labelResult.Text = @"Canceled!";
			}
			else if (e.Error != null)
			{
				labelResult.Text = Resources.Gui_backgroundWorker1_RunWorkerCompleted_Error__ + e.Error.Message;
			}
			else
			{
				labelResult.Text = Resources.Gui_backgroundWorker1_RunWorkerCompleted_Done_;
			}
			// Close the AlertForm
			_alert.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			if (backgroundWorker1.WorkerSupportsCancellation)
			{
				// Cancel the asynchronous operation.
				backgroundWorker1.CancelAsync();
				// Close the AlertForm
				_alert.Close();
			}
		}

		private void buttonStart_Click(object sender, EventArgs e)
		{
			if (backgroundWorker1.IsBusy != true)
			{
				// create a new instance of the alert form
				_alert = new AlertForm();
				// event handler for the Cancel button in AlertForm
				_alert.Canceled += buttonCancel_Click;
				_alert.Show();
				// Start the asynchronous operation.
				backgroundWorker1.RunWorkerAsync();
			}
		}

		private AlertForm _alert;
	}
}