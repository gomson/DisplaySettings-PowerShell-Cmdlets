/********************************
	written by Alexander Jung
	modified by Adam Byrd
********************************/

using System.Diagnostics;

namespace DisplaySettings.Core
{
	[DebuggerDisplay("{Width} x {Height}, {ColorDepth} bpp, {ScreenRefreshRate} Hz, {DeviceName}")]
	public class DisplayData
	{
		Win32.DEVMODE _data;

		internal Win32.DEVMODE Data
		{
			get { return _data; }
		}

		internal DisplayData ( Win32.DEVMODE data, bool isCurrent )
		{
			_data = data;
			_isCurrent = isCurrent;
		}

		public int Width
		{
			get { return _data.dmPelsWidth; }
			set { _data.dmPelsWidth = value; }
		}
		public int Height
		{
			get { return _data.dmPelsHeight; }
			set { _data.dmPelsHeight = value; }
		}

		public int ColorDepth
		{
			get { return _data.dmBitsPerPel; }
			set { _data.dmBitsPerPel = (short) value; }
		}

		public int ScreenRefreshRate
		{
			get { return _data.dmDisplayFrequency; }
			set { _data.dmDisplayFrequency = value; }
		}

		public string DeviceName
		{
			get { return _data.dmDeviceName; }
		}

		bool _isCurrent;

		public bool IsCurrent
		{
			get { return _isCurrent; }
			set { _isCurrent = value; }
		}

		public string GetDisplayText ()
		{
			return this.Width + " x " + this.Height + ", " + this.ColorDepth + " bpp, " + this.ScreenRefreshRate + " Hertz";
		}
	}
}