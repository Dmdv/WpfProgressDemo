using System;
using System.Windows.Forms;

namespace ProgressDemo
{
	public partial class AlertForm : Form
	{
		public event EventHandler<EventArgs> Canceled;

		public AlertForm()
		{
			InitializeComponent();
		}

		public string Message
		{
			set { labelMessage.Text = value; }
		}

		public int ProgressValue
		{
			set { progressBar1.Value = value; }
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			// Create a copy of the event to work with
			var ea = Canceled;
			/* If there are no subscribers, eh will be null so we need to check
			 * to avoid a NullReferenceException. */
			if (ea != null)
			{
				ea(this, e);
			}
		}
	}
}