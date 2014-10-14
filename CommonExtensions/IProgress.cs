namespace CommonExtensions
{
	public interface IProgress<in T>
	{
		bool IsCancellationPending { get; set; }

		void OnCompleted(object state);

		/// <summary>
		/// Reports a progress update.
		/// </summary>
		/// <param name="value">The value of the updated progress.</param>
		void Report(T value);
	}
}