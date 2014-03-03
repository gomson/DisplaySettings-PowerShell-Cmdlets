/********************************
	written by Alexander Jung
	modified by Adam Byrd
********************************/

using System.Collections.Generic;
using System.Management.Automation;
using DisplaySettings.Core;

namespace DisplaySettings
{
	[Cmdlet(VerbsCommon.Get, "DisplaySettings")]
	public class GetDisplaySettingsCommand : PSCmdlet
	{
		bool _all;

		[Parameter(HelpMessage =
		           "provide this parameter to show a complete list of available screen settings; " +
		           "otherwise only the current screen setting is reported.")]
		public SwitchParameter All
		{
			get { return _all; }
			set { _all = (bool) value; }
		}

		protected override void ProcessRecord ()
		{
			if ( _all )
			{
				IList<DisplayData> list = Display.GetDisplaySettings();
				foreach ( DisplayData data in list )
					WriteObject(data);
			}
			else
			{
				DisplayData current = Display.GetCurrentDisplaySettings();
				WriteObject(current);
			}
		}
	}
}