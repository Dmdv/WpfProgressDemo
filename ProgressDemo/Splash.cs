using System;
using System.Windows.Forms;
using ProgressDemo.Properties;

namespace ProgressDemo
{
	public partial class Splash : Form
	{
		public Splash()
		{
			InitializeComponent();

			ExtraFormSettings();
			SetAndStartTimer();
		}

		private void ExtraFormSettings()
		{
			FormBorderStyle = FormBorderStyle.None;
			Opacity = 0.5;
			BackgroundImage = Resources.splash;
		}

		private void SetAndStartTimer()
		{
			_timer.Interval = 100;
			_timer.Tick += t_Tick;
			_timer.Start();
		}

		private void t_Tick(object sender, EventArgs e)
		{
			if (_fadeIn)
			{
				if (Opacity < 1.0)
				{
					Opacity += 0.02;
				}
				else
				{
					_fadeIn = false;
					_fadeOut = true;
				}
			}
			else if (_fadeOut)
			{
				if (Opacity > 0)
				{
					Opacity -= 0.02;
				}
				else
				{
					_fadeOut = false;
				}
			}

			if (!(_fadeIn || _fadeOut))
			{
				_timer.Stop();
				Close();
			}
		}

		private readonly Timer _timer = new Timer();
		private bool _fadeIn = true;
		private bool _fadeOut = true;
	}
}