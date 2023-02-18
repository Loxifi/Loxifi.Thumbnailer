using Loxifi.Interfaces;
using Loxifi.Structures;
using Loxifi.Utils;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Loxifi
{

	/// <summary>Extracts thumbnail images using Windows Shell interface methods.</summary>
	/// <remarks>
	/// In Windows Vista/7 up, the IShellItemImageFactory interface is used to extract the image,
	/// while in older Windows operating systems such as Windows XP, the IExtractImage interface
	/// is used instead.</remarks>
	public static class Thumbnailer
	{
		private static Guid _iID_ShellItem = new("43826d1e-e718-42ee-bc55-a1e261c37bfe");

		internal static bool SupportsShellItemImageFactory => Environment.OSVersion.Version.Major >= 6;

		public static Bitmap? GetImage(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException(nameof(filename));
			}

			if (!SupportsShellItemImageFactory)
			{
				// Use my IExtractImage wrapper instead
				return XPThumbnailer.ExtractThumbnail(filename);
			}

			if (NativeMethods.SHCreateItemFromParsingName(filename, IntPtr.Zero, _iID_ShellItem, out IShellItem ppsi) == HResult.S_OK)
			{
				HResult hRes = ((IShellItemImageFactory)ppsi).GetImage(new SIZE((int)ShellImageSize.ExtraLarge, (int)ShellImageSize.ExtraLarge), SIIGBF.SIIGBF_BIGGERSIZEOK, out IntPtr hbitmap);

				if (hRes == HResult.S_OK)
				{
					try
					{
						return ImageUtils.FixShellBitmap(filename, Image.FromHbitmap(hbitmap));
						//return Bitmap.FromHbitmap(hbitmap);
					}
					finally
					{
						_ = NativeMethods.DeleteObject(hbitmap);
					}
				}
				else
				{
					Marshal.ThrowExceptionForHR((int)hRes);
				}
			}

			return null;
		}
	}
}
