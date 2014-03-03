/********************************
	written by Alexander Jung
	modified by Adam Byrd
********************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DisplaySettings
{
	internal class Win32
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct DEVMODE
		{
			public const int CCHDEVICENAME = 32;
			public const int CCHFORMNAME = 32;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
			public string dmDeviceName;
			public short dmSpecVersion;
			public short dmDriverVersion;
			public short dmSize;
			public short dmDriverExtra;
			public int dmFields;

			public short dmOrientation;
			public short dmPaperSize;
			public short dmPaperLength;
			public short dmPaperWidth;

			public short dmScale;
			public short dmCopies;
			public short dmDefaultSource;
			public short dmPrintQuality;
			public short dmColor;
			public short dmDuplex;
			public short dmYResolution;
			public short dmTTOption;
			public short dmCollate;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
			public string dmFormName;
			public short dmLogPixels;
			public short dmBitsPerPel;
			public int dmPelsWidth;
			public int dmPelsHeight;
			public int dmDisplayFlags;
			public int dmDisplayFrequency;

			public int dmICMMethod;
			public int dmICMIntent;
			public int dmMediaType;
			public int dmDitherType;
			public int dmReserved1;
			public int dmReserved2;
			public int dmPanningWidth;
			public int dmPanningHeight;

			public int dmPositionX; // Using a PointL Struct does not work 
			public int dmPositionY;
		}

		public enum ReturnValues : long
		{
			DISP_CHANGE_SUCCESSFUL = 0,
			DISP_CHANGE_RESTART = 1,
			DISP_CHANGE_FAILED = -1,
			DISP_CHANGE_BADMODE = -2,
			DISP_CHANGE_NOTUPDATED = -3,
			DISP_CHANGE_BADFLAGS = -4,
			DISP_CHANGE_BADPARAM = -5,
			DISP_CHANGE_BADDUALVIEW = -6
		}

		[Flags]
		public enum ChangeDisplayFlags
		{
			CDS_DYNAMICALLY=0,
			CDS_UPDATEREGISTRY = 0x00000001,
			CDS_TEST = 0x00000002,
			CDS_FULLSCREEN = 0x00000004,
			CDS_GLOBAL = 0x00000008,
			CDS_SET_PRIMARY = 0x00000010,
			CDS_VIDEOPARAMETERS = 0x00000020,
			CDS_RESET = 0x40000000,
			CDS_NORESET = 0x10000000
		}

		public struct ReturnValueText
		{
			public const string DISP_CHANGE_SUCCESSFUL = null;
			public const string DISP_CHANGE_RESTART = "Please restart your system";
			public const string DISP_CHANGE_FAILED = "ChangeDisplaySettigns API failed";
			public const string DISP_CHANGE_BADDUALVIEW = "The settings change was unsuccessful because system is DualView capable.";
			public const string DISP_CHANGE_BADFLAGS = "An invalid set of flags was passed in.";
			public const string DISP_CHANGE_BADPARAM = "An invalid parameter was passed in. This can include an invalid flag or combination of flags.";
			public const string DISP_CHANGE_NOTUPDATED = "Unable to write settings to the registry.";
			public const string DISP_CHANGE_OTHER = "Unknown return value from ChangeDisplaySettings API";
		}

		public enum DmFields : int
		{
			DM_ORIENTATION = 0x00000001,
			DM_PAPERSIZE = 0x00000002,
			DM_PAPERLENGTH = 0x00000004,
			DM_PAPERWIDTH = 0x00000008,
			DM_SCALE = 0x00000010,
			DM_POSITION = 0x00000020,
			DM_NUP = 0x00000040,
			DM_DISPLAYORIENTATION = 0x00000080,
			DM_COPIES = 0x00000100,
			DM_DEFAULTSOURCE = 0x00000200,
			DM_PRINTQUALITY = 0x00000400,
			DM_COLOR = 0x00000800,
			DM_DUPLEX = 0x00001000,
			DM_YRESOLUTION = 0x00002000,
			DM_TTOPTION = 0x00004000,
			DM_COLLATE = 0x00008000,
			DM_FORMNAME = 0x00010000,
			DM_LOGPIXELS = 0x00020000,
			DM_BITSPERPEL = 0x00040000,
			DM_PELSWIDTH = 0x00080000,
			DM_PELSHEIGHT = 0x00100000,
			DM_DISPLAYFLAGS = 0x00200000,
			DM_DISPLAYFREQUENCY = 0x00400000,
			DM_ICMMETHOD = 0x00800000,
			DM_ICMINTENT = 0x01000000,
			DM_MEDIATYPE = 0x02000000,
			DM_DITHERTYPE = 0x04000000,
			DM_PANNINGWIDTH = 0x08000000,
			DM_PANNINGHEIGHT = 0x10000000,
			DM_DISPLAYFIXEDOUTPUT = 0x20000000
		}

		[DllImport("user32.dll")]
		public static extern int EnumDisplaySettings ( string deviceName, int modeNum, ref DEVMODE devMode );

		public const int ENUM_CURRENT_SETTINGS = -1;

		[DllImport("user32.dll")]
		public static extern int ChangeDisplaySettings ( ref DEVMODE devMode, int flags );

		[DllImport("user32.dll")]
		public static extern int ChangeDisplaySettingsEx ( string lpszDeviceName, IntPtr lpDevMode,
			IntPtr hwnd, uint dwflags, IntPtr lParam );

		public static List<DEVMODE> GetDisplaySettings ()
		{
			List<DEVMODE> modes = new List<DEVMODE>();
			DEVMODE dm = new DEVMODE();
			int i = 0;
			while ( EnumDisplaySettings(null, i, ref dm) != 0 )
			{
				modes.Add(dm);
				i++;
			}

			return modes;
		}

		public DEVMODE GetCurrentDisplaySettings ()
		{
			DEVMODE dm = new DEVMODE();
			int ret = EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm);
			return dm;
		}

		public ReturnValues ChangeSettings ( DEVMODE devmode, out string err )
		{
			err = null;

			ReturnValues iRet = (ReturnValues) ChangeDisplaySettings(ref devmode, 0);

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
			return iRet;
		}

		public bool IsEqual ( DEVMODE x, DEVMODE y )
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

	}
}