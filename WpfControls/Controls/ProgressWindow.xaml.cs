using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace WpfControls.Controls
{
	/// <summary>
	/// Progress windows, which shows the progress of long running tasks.
	/// </summary>
	public partial class ProgressWindow
	{
		public bool CanBeCanceled
		{
			get { return _cancelButtonControl.Visibility == Visibility.Visible; }
			set { _cancelButtonControl.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
		}

		public string FormattedMessage { get; set; }

		public bool IsCancelled { get; set; }

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

		public static void Run(
			Action action,
			string formattedMessage = null,
			bool showDescription = false,
			string title = "Wait...")
		{
			CreateStaThread(_ =>
			{
				var progress = new ProgressWindow
				{
					ShowDescription = showDescription,
					CanBeCanceled = false,
					FormattedMessage = formattedMessage,
					WindowTitle = title
				};

				progress.RunInternal(action);
			});
		}

		public static void Run(
			Action<CommonExtensions.IProgress<double>> action,
			string formattedMessage = null,
			double? maximum = null,
			bool showDescription = false,
			bool canBeCancelled = true,
			string title = "Wait...")
		{
			CreateStaThread(_ =>
			{
				var progress = new ProgressWindow
				{
					ShowDescription = showDescription,
					CanBeCanceled = canBeCancelled,
					FormattedMessage = formattedMessage,
					WindowTitle = title
				};

				progress.RunInternal(action, maximum);
			});
		}

		public static void RunModal(
			Action action,
			string formattedMessage = null,
			bool showDescription = false,
			string title = "Wait...")
		{
			var progress = new ProgressWindow
			{
				ShowDescription = showDescription,
				CanBeCanceled = false,
				FormattedMessage = formattedMessage,
				WindowTitle = title
			};

			progress.RunInternal(action);
		}

		public static void RunModal(
			Action<CommonExtensions.IProgress<double>> action,
			string formattedMessage = null,
			double? maximum = null,
			bool showDescription = false,
			bool canBeCancelled = true,
			string title = "Wait...")
		{
			var progress = new ProgressWindow
			{
				ShowDescription = showDescription,
				CanBeCanceled = canBeCancelled,
				FormattedMessage = formattedMessage,
				WindowTitle = title
			};

			progress.RunInternal(action, maximum);
		}

		private static void CreateStaThread(ParameterizedThreadStart parameterizedThreadStart)
		{
			var thread = new Thread(parameterizedThreadStart);
			thread.SetApartmentState(ApartmentState.STA);
			thread.IsBackground = true;
			thread.Start();
		}

		private void Close(object state)
		{
			_currenTask.Wait();
			Thread.Sleep(1000);
			Hide();
			Close();
			_closeReadyEvent.Set();
		}

		private void CreateBackgroundAction(Action action)
		{
			_currentProgress = new CommonExtensions.Progress<double>(Close);

			_currenTask = Task.Factory
				.StartNew(action)
				.ContinueWith(_ => _currentProgress.OnCompleted(null));
		}

		private void CreateBackgroundAction(Action<CommonExtensions.IProgress<double>> action)
		{
			_currentProgress = new CommonExtensions.Progress<double>(ReportProgress, Close);

			_currenTask = Task.Factory
				.StartNew(() => action(_currentProgress))
				.ContinueWith(_ => _currentProgress.OnCompleted(null));
		}

		private void OnCancelClick(object sender, RoutedEventArgs e)
		{
			Action action = () => UpdateMessage("Cancel operation...");
			Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);

			IsCancelled = true;

			if (_currentProgress != null)
			{
				_currentProgress.IsCancellationPending = true;
			}
		}

		private void OnCloseWindow(object sender, CancelEventArgs e)
		{
			if (!IsCancelled)
			{
				e.Cancel = true;
				OnCancelClick(sender, null);
			}
		}

		private void ReportProgress(double value)
		{
			_progressControl.Value = value;

			if (IsCancelled)
			{
				UpdateMessage(string.Format(FormattedMessage, value) + "... Cancel operation...");
			}
			else
			{
				UpdateMessage(string.Format(FormattedMessage, value));
			}
		}

		private void RunInternal(Action action)
		{
			SetupProgressBar();
			_action = () => CreateBackgroundAction(action);
			ShowDialog();
		}

		private void RunInternal(Action<CommonExtensions.IProgress<double>> action, double? maximum = null)
		{
			SetupProgressBar(maximum);
			_action = () => CreateBackgroundAction(action);
			ShowDialog();
			_closeReadyEvent.WaitOne();
		}

		private void SetupProgressBar(double? maximum = null)
		{
			_progressControl.IsIndeterminate = !maximum.HasValue;
			_progressControl.Maximum = maximum.HasValue ? maximum.Value : 0;
			_progressControl.Minimum = 0;
			_progressControl.Value = 0;
		}

		private void UpdateMessage(string message)
		{
			_textControl.Text = message;
			_progressControl.Tag = message;
		}

		private ProgressWindow()
		{
			InitializeComponent();

			Topmost = true;
			ShowInTaskbar = false;
			FormattedMessage = "Current value: {0}";
			WindowTitle = "Подождите...";
			CanBeCanceled = true;
			ShowDescription = true;
			IsCancelled = false;

			Loaded += (o, args) =>
			{
				_closeReadyEvent = new AutoResetEvent(false);
				UpdateMessage("Prepare...");

				if (_action != null)
				{
					_action();
				}
			};
		}

		private Action _action;
		private AutoResetEvent _closeReadyEvent;
		private Task _currenTask;
		private CommonExtensions.IProgress<double> _currentProgress;
	}
}