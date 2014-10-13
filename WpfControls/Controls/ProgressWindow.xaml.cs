using System;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControls.Controls
{
	/// <summary>
	/// Progress windows, which shows the progress of long running tasks.
	/// </summary>
	public partial class ProgressWindow
	{
		public ProgressWindow()
		{
			InitializeComponent();

			Topmost = true;
			ShowInTaskbar = false;
			FormattedMessage = "Current value: {0}";
		}

		public string FormattedMessage { get; set; }

		public bool ShowLabel
		{
			get { return _showLabel; }
			set
			{
				_showLabel = value;
				_textControl.Visibility = _showLabel ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		public void Run(Action<CommonExtensions.IProgress<double>> action, double maximum)
		{
			SetupProgressBar(maximum);

			CommonExtensions.IProgress<double> progress = new CommonExtensions.Progress<double>(ReportProgress, Close);

			Task.Run(() => action(progress)).GetAwaiter().OnCompleted(() => progress.OnCompleted(null));

			ShowDialog();
		}

		public void Run(Action<CommonExtensions.IProgress<double>> action)
		{
			SetupProgressBar();

			var progress = new CommonExtensions.Progress<double>(ReportProgress, Close);

			Task.Run(() => action(progress)).GetAwaiter().OnCompleted(() => progress.OnCompleted(null));

			ShowDialog();
		}

		public void Run(Action action)
		{
			ShowLabel = false;

			SetupProgressBar();

			var progress = new CommonExtensions.Progress<double>(Close);

			Task.Run(action).GetAwaiter().OnCompleted(() => progress.OnCompleted(null));

			ShowDialog();
		}

		private void Close(object state)
		{
			Hide();
			Close();
		}

		private void ReportProgress(double value)
		{
			_textControl.Text = string.Format(FormattedMessage, value);
			_progressControl.Tag = string.Format(FormattedMessage, value);
			_progressControl.Value = value;
		}

		private void SetupProgressBar()
		{
			_progressControl.IsIndeterminate = true;
		}

		private void SetupProgressBar(double maximum)
		{
			_progressControl.IsIndeterminate = false;
			_progressControl.Maximum = maximum;
			_progressControl.Minimum = 0;
			_progressControl.Value = 0;
		}

		private bool _showLabel;
	}
}