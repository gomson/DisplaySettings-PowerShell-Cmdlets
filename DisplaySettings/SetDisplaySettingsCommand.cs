/********************************
	written by Alexander Jung
	modified by Adam Byrd
********************************/

using System;
using System.Collections.Generic;
using System.Management.Automation;
using DisplaySettings.Core;

namespace DisplaySettings
{
	/// <summary>
	/// - shouldprocess/noPrompt
	/// </summary>
	[Cmdlet(VerbsCommon.Set, "DisplaySettings", SupportsShouldProcess = true)]
	public class SetDisplaySettingsCommand : PSCmdlet
	{
		int _width;

		[Parameter(Position = 0, Mandatory = true, ValueFromPipeline = false,
			HelpMessage = "The horizontal screen resolution")]
		[ValidateRange(0, (int) short.MaxValue)]
		public int Width
		{
			get { return _width; }
			set { _width = value; }
		}

		int _height;

		[Parameter(Position = 1, Mandatory = true, HelpMessage = "The vertical screen resolution")]
		[ValidateRange(0, (int) short.MaxValue)]
		public int Height
		{
			get { return _height; }
			set { _height = value; }
		}

		int _colorDepth = -1;

		[Parameter(Position = 2, HelpMessage = "The color depth in bits (8, 16, 32)")]
		[ValidateRange(0, (int) byte.MaxValue)]
		[ValidateSet("8", "16", "32", "64")]
		public int ColorDepth
		{
			get { return _colorDepth; }
			set { _colorDepth = value; }
		}

		int _frequency = -1;

		[Parameter(Position = 3, HelpMessage = "The screen refresh rate")]
		[ValidateRange(0, (int) byte.MaxValue)]
		public int Frequency
		{
			get { return _frequency; }
			set { _frequency = value; }
		}

		private bool _noPrompt;

		[Parameter]
		public SwitchParameter NoPrompt
		{
			get { return _noPrompt; }
			set { _noPrompt = value; }
		}

		private bool _force;

		[Parameter]
		public SwitchParameter Force
		{
			get { return _force; }
			set { _force = value; }
		}


		protected override void ProcessRecord ()
		{
			DisplayData data = Display.GetCurrentDisplaySettings();
			data.Width = this.Width;
			data.Height = this.Height;
			data.ColorDepth = this.ColorDepth < 0 ? data.ColorDepth : this.ColorDepth;
			data.ScreenRefreshRate = this.Frequency < 0 ? data.ScreenRefreshRate : this.Frequency;

			// ensure the new settings exist, unless forcing the display
			if ( !_force )
			{
				IList<DisplayData> list = Display.GetDisplaySettings();
				data = Display.FindData(list, data);
				if ( data == null )
					throw new NotSupportedException("No matching display setting found.");
			}

			// check the -whatif parameter
			string msg = "Display settings " + data.GetDisplayText();
			if ( !ShouldProcess(msg) )
				return;

			// check the users intention
			if ( !_noPrompt )
			{
				msg = "Set display settings to " + data.GetDisplayText() + "?";
				if ( !ShouldContinue(msg, "Warning!") )
					return;
			}

			// and finally go
			string err;
			if ( Display.ChangeSettings(data, out err) != 0 )
				throw new NotSupportedException("Error setting values: " + err);
			if ( err != null )
			{
				WriteWarning("Error setting values: " + err);
			}
			else
			{
				msg = "Display settings set to " + data.GetDisplayText();
				WriteVerbose(msg);
			}
		}

	}
}