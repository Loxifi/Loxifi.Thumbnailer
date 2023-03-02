using Loxifi.Structures;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Runtime.InteropServices;
using System.Text;

namespace Loxifi.Interfaces
{

	[ComImport(), Guid("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IExtractImage
	{
		[PreserveSig()]
		HResult GetLocation([Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref int pdwFlags);

		[PreserveSig()]
		HResult Extract(ref IntPtr phBmpThumbnail);
	}
}
