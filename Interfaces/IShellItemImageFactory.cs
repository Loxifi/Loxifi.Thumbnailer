using Loxifi.Structures;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Runtime.InteropServices;
using System.Security;

namespace Loxifi.Interfaces
{
	[ComImport, Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	internal interface IShellItemImageFactory
	{
		[PreserveSig]
		HResult GetImage([In, MarshalAs(UnmanagedType.Struct)] SIZE size, [In] SIIGBF flags, [Out] out IntPtr phbm);
	}
}
