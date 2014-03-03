/********************************
	written by Alexander Jung
	modified by Adam Byrd
********************************/

using System.Collections.Generic;

namespace DisplaySettings.Core
{
	public class Display
	{
		public static DisplayData GetCurrentDisplaySettings ()
		{
			Win32.DEVMODE dmCurrent = new Win32.DEVMODE();
			int ret = Win32.EnumDisplaySettings(null, Win32.ENUM_CURRENT_SETTINGS, ref dmCurrent);
			return new DisplayData(dmCurrent, true);
		}

		public static IList<DisplayData> GetDisplaySettings ()
		{
			Win32.DEVMODE dmCurrent = new Win32.DEVMODE();
			int ret = Win32.EnumDisplaySettings(null, Win32.ENUM_CURRENT_SETTINGS, ref dmCurrent);

			List<DisplayData> list = new List<DisplayData>();
			Win32.DEVMODE dm = new Win32.DEVMODE();
			int i = 0;
			bool isCurrent;
			while ( Win32.EnumDisplaySettings(null, i, ref dm) != 0 )
			{
				isCurrent = IsEqual(dm, dmCurrent);
				list.Add(new DisplayData(dm, isCurrent));
				i++;
			}
			return list;
		}

		public static DisplayData FindData ( IList<DisplayData> list, DisplayData data )
		{
			foreach ( DisplayData test in list )
			{
				if ( IsEqual(test.Data, data.Data) )
					return test;
			}
			return null;
		}

		public static bool IsEqual ( DisplayData x, DisplayData y )
		{
			return IsEqual(x.Data, y.Data);
		}

		static bool IsEqual ( Win32.DEVMODE x, Win32.DEVMODE y )
		{
			if ( x.dmPelsHeight == y.dmPelsHeight
				 && x.dmPelsWidth == y.dmPelsWidth
				 && x.dmBitsPerPel == y.dmBitsPerPel
				 && x.dmDisplayFrequency == y.dmDisplayFrequency
				 && x.dmOrientation == y.dmOrientation
				 && x.dmDeviceName == y.dmDeviceName )
				return true;
			return false;
			;
		}

		public static int ChangeSettings ( DisplayData data, out string err )
		{
			err = "";

			Win32.DEVMODE devmode = data.Data;
			Win32.ChangeDisplayFlags flags = Win32.ChangeDisplayFlags.CDS_UPDATEREGISTRY;
			Win32.ReturnValues iRet = (Win32.ReturnValues) Win32.ChangeDisplaySettings(ref devmode, (int) flags);

			switch ( iRet )
			{
			case Win32.ReturnValues.DISP_CHANGE_SUCCESSFUL:
				err = Win32.ReturnValueText.DISP_CHANGE_SUCCESSFUL;
				break;
			case Win32.ReturnValues.DISP_CHANGE_RESTART:
				err = Win32.ReturnValueText.DISP_CHANGE_RESTART;
				break;
			case Win32.ReturnValues.DISP_CHANGE_FAILED:
				err = Win32.ReturnValueText.DISP_CHANGE_FAILED;
				break;
			case Win32.ReturnValues.DISP_CHANGE_BADDUALVIEW:
				err = Win32.ReturnValueText.DISP_CHANGE_BADDUALVIEW;
				break;
			case Win32.ReturnValues.DISP_CHANGE_BADFLAGS:
				err = Win32.ReturnValueText.DISP_CHANGE_BADFLAGS;
				break;
			case Win32.ReturnValues.DISP_CHANGE_BADPARAM:
				err = Win32.ReturnValueText.DISP_CHANGE_BADPARAM;
				break;
			case Win32.ReturnValues.DISP_CHANGE_NOTUPDATED:
				err = Win32.ReturnValueText.DISP_CHANGE_NOTUPDATED;
				break;
			default:
				err = Win32.ReturnValueText.DISP_CHANGE_OTHER;
				break;
			}
			return (int) iRet;
		}

	}
}