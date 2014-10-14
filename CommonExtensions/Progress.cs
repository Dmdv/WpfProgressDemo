using System;
using System.ComponentModel;
using System.Threading;

namespace CommonExtensions
{
	/// <summary>
	/// Provides an IProgress{T} that invokes callbacks for each reported progress value.
	/// </summary>
	/// <typeparam name="T">Specifies the type of the progress report value.</typeparam>
	/// <remarks>
	/// Any action provided to the constructor or event handlers registered with
	/// the <see cref="ProgressChanged"/> event are invoked through a 
	/// <see cref="System.Threading.SynchronizationContext"/> instance captured
	/// when the instance is constructed.  If there is no current SynchronizationContext
	/// at the time of construction, the callbacks will be invoked on the ThreadPool.
	/// </remarks>
	public sealed class Progress<T> : IProgress<T>
	{
		/// <summary>Raised for each reported progress value.</summary>
		/// <remarks>
		/// Handlers registered with this event will be invoked on the 
		/// <see cref="System.Threading.SynchronizationContext"/> captured when the instance was constructed.
		/// </remarks>
		public event EventHandler<T> ProgressChanged;

		/// <summary>Initializes the <see cref="System.Progress{T}"/> with the specified callback.</summary>
		/// <param name="action">
		/// A action to invoke for each reported progress value.  This action will be invoked
		/// in addition to any delegates registered with the <see cref="ProgressChanged"/> event.
		/// Depending on the <see cref="System.Threading.SynchronizationContext"/> instance captured by 
		/// the <see cref="Progress{T}"/> at construction, it's possible that this action instance
		/// could be invoked concurrently with itself.
		/// </param>
		/// <param name="onCompleted">Action is invoked after operation completes.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="action"/> is null (Nothing in Visual Basic).</exception>
		public Progress(Action<T> action, Action<object> onCompleted)
			: this()
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			_action = action;
			_onCompletedCallback = new SendOrPostCallback(onCompleted);
		}

		public Progress(Action<object> onCompleted)
			: this()
		{
			_onCompletedCallback = new SendOrPostCallback(onCompleted);
		}

		/// <summary>Reports a progress change.</summary>
		/// <param name="value">The value of the updated progress.</param>
		void IProgress<T>.Report(T value)
		{
			OnReport(value);
		}

		public bool IsCancellationPending { get; set; }

		public void OnCompleted(object state)
		{
			_asyncOperation.PostOperationCompleted(_onCompletedCallback, state);
		}

		/// <summary>Invokes the action and event callbacks.</summary>
		/// <param name="state">The progress value.</param>
		private void InvokeHandlers(object state)
		{
			var value = (T) state;

			var handler = _action;
			var changedEvent = ProgressChanged;

			if (handler != null)
			{
				handler(value);
			}

			if (changedEvent != null)
			{
				changedEvent(this, value);
			}
		}

		/// <summary>Reports a progress change.</summary>
		/// <param name="value">The value of the updated progress.</param>
		private void OnReport(T value)
		{
			// If there's no action, don't bother going through the [....] context.
			// Inside the callback, we'll need to check again, in case 
			// an event action is removed between now and then.
			var handler = _action;
			var changedEvent = ProgressChanged;

			if (handler != null || changedEvent != null)
			{
				// Post the processing to the [....] context.
				// (If T is a value type, it will get boxed here.)
				_asyncOperation.Post(InvokeHandlers, value);
			}
		}

		/// <summary>Initializes the <see cref="System.Progress{T}"/>.</summary>
		private Progress()
		{
			// Capture the current synchronization context.  "current" is determined by CurrentNoFlow,
			// which doesn't consider the [....] ctx flown with an ExecutionContext, avoiding
			// [....] ctx reference identity issues where the [....] ctx for one thread could be Current on another.
			// If there is no current context, we use a default instance targeting the ThreadPool.

			_asyncOperation = AsyncOperationManager.CreateOperation(null);
		}

		private readonly Action<T> _action;
		private readonly AsyncOperation _asyncOperation;
		private readonly SendOrPostCallback _onCompletedCallback;
	}
}