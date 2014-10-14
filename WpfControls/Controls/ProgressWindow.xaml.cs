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
			WindowTitle = "Подождите...";
			CanBeCanceled = true;
			ShowDescription = true;
		}

		public bool CanBeCanceled
		{
			get { return _cancelButtonControl.Visibility == Visibility.Visible; }
			set { _cancelButtonControl.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
		}

		public string FormattedMessage { get; set; }

		public bool ShowDescription
		{
			get { return _textControl.Visibility == Visibility.Visible; }
			set { _textControl.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
		}

		public string WindowTitle
		{
			get { return Title; }
			set { Title = value; }
		}

		public void Run(Action<CommonExtensions.IProgress<double>> action, double maximum)
		{
			SetupProgressBar(maximum);

			_currentTask = new CommonExtensions.Progress<double>(ReportProgress, Close);

			Task.Run(() => action(_currentTask)).GetAwaiter().OnCompleted(() => _currentTask.OnCompleted(null));

			ShowDialog();
		}

		public void Run(Action<CommonExtensions.IProgress<double>> action)
		{
			SetupProgressBar();

			_currentTask = new CommonExtensions.Progress<double>(ReportProgress, Close);

			Task.Run(() => action(_currentTask)).GetAwaiter().OnCompleted(() => _currentTask.OnCompleted(null));

			ShowDialog();
		}

		public void Run(Action action)
		{
			ShowDescription = false;
			CanBeCanceled = false;

			SetupProgressBar();

			_currentTask = new CommonExtensions.Progress<double>(Close);

			Task.Run(action).GetAwaiter().OnCompleted(() => _currentTask.OnCompleted(null));

			ShowDialog();
		}

		private void Close(object state)
		{
			Hide();
			Close();
		}

		private void OnCancelClick(object sender, RoutedEventArgs e)
		{
			if (_currentTask != null)
			{
				_currentTask.IsCancellationPending = true;
			}
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

		private CommonExtensions.IProgress<double> _currentTask;
	}
}