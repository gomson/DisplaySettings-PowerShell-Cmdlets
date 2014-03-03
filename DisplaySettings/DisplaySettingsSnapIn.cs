/********************************
	written by Alexander Jung
	modified by Adam Byrd
********************************/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;

namespace DisplaySettings
{
	[RunInstaller(true)]
	public class DisplaySettingsSnapIn : PSSnapIn
	{
		private string[] _formats = { "DisplaySettings.Format.ps1xml" };

		public DisplaySettingsSnapIn () : base()
		{
		}

		public override string Name
		{
			get { return "DisplaySettingsCmdlets"; }
		}

		public override string Vendor
		{
			get { return "(c) by Alexander Jung - mailto:info@Alexander-Jung.NET"; }
		}

		public override string Description
		{
			get { return "Display Settings Cmdlets"; }
		}

		public override string[] Formats
		{
			get { return _formats; }
		}
	}
}