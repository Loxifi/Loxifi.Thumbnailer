using Loxifi.Structures;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Runtime.InteropServices;

namespace Loxifi.Interfaces
{

	[ComImport()]
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellFolder
	{
		/// <summary>
		/// Translates a file object's or folder's display name into an item identifier items.
		/// </summary>
		/// <returns>error code, if any</returns>
		[PreserveSig()]
		HResult ParseDisplayName(
			// Optional window handle
			IntPtr hwnd,
			// Optional bind context that controls the
			// parsing operation. This parameter is
			// normally set to NULL.
			IntPtr pbc,
			// Null-terminated UNICODE string with the display name.
			[In(), MarshalAs(UnmanagedType.LPWStr)]
				string pszDisplayName,
			// Pointer to a ULONG value that receives the
			// number of characters of the
			// display name that was parsed.
			out uint pchEaten,
			// Pointer to an ITEMIDLIST pointer that receives
			// the item identifier items for
			// the object.
			out IntPtr ppidl,
			// Optional parameter that can be used to
			// query for file attributes.
			// this can be values from the SFGAO enum
			ref uint pdwAttributes);

		[PreserveSig()]
		HResult EnumObjects(IntPtr hwndOwner, [MarshalAs(UnmanagedType.U4)] ESHCONTF grfFlags, ref IEnumIDList ppenumIDList);

		/// <summary>
		/// Retrieves an IShellFolder object for a subfolder.
		/// </summary>
		/// <returns>error code, if any</returns>
		[PreserveSig()]
		HResult BindToObject(
			// Address of an ITEMIDLIST structure (PIDL)
			// that identifies the subfolder.
			IntPtr pidl,
			// Optional address of an IBindCtx interface on
			// a bind context object to be
			// used during this operation.
			IntPtr pbc,
			// Identifier of the interface to return.
			[In()] ref Guid riid,
			// Address that receives the interface pointer.
			//[MarshalAs(UnmanagedType.Interface)]
			out IntPtr ppv);

		void BindToStorage(IntPtr pidl, IntPtr pbcReserved, ref Guid riid, IntPtr ppvObj);

		[PreserveSig()]
		HResult CompareIDs(IntPtr lParam, IntPtr pidl1, IntPtr pidl2);

		[PreserveSig()]
		HResult CreateViewObject(IntPtr hwndOwner, ref Guid riid, IntPtr ppvOut);

		[PreserveSig()]
		HResult GetAttributesOf(int cidl, IntPtr apidl, [MarshalAs(UnmanagedType.U4)] ref ESFGAO rgfInOut);

		[PreserveSig()]
		HResult GetUIObjectOf(IntPtr hwndOwner, int cidl, ref IntPtr apidl, ref Guid riid, ref int prgfInOut, ref IntPtr ppvOut);

		[PreserveSig()]
		HResult GetDisplayNameOf(IntPtr pidl, [MarshalAs(UnmanagedType.U4)] ESHGDN uFlags, out IntPtr lpName);

		[PreserveSig()]
		HResult SetNameOf(IntPtr hwndOwner, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string lpszName, [MarshalAs(UnmanagedType.U4)] ESHCONTF uFlags, ref IntPtr ppidlOut);
	}
}
