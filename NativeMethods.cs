using Loxifi.Interfaces;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Runtime.InteropServices;
using System.Security;

namespace Loxifi
{

	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = true)]
		public static extern HResult SHCreateItemFromParsingName([In][MarshalAs(UnmanagedType.LPWStr)] string pszPath, [In] IntPtr pbc, [In][MarshalAs(UnmanagedType.LPStruct)] Guid riid, [Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItem ppv);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern HResult SHGetDesktopFolder([MarshalAs(UnmanagedType.Interface)] out IShellFolder ppshf);
	}
}

