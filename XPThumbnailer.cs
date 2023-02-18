using Loxifi.Structures;
using Loxifi.Utils;
using Microsoft.Diagnostics.Runtime.Utilities;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Loxifi.Interfaces
{
	/// <summary>Extracts thumbnail images using the IExtractImage interface. Windows XP.</summary>
	/// <remarks><para>
	/// In Windows Vista and later, the <b>IShellItemImageFactory</b> interface should be used instead,
	/// as used by my <see cref="Thumbnailer"/> type. This is the old way of extracting shell
	/// images. It is slower and the implementation is non-trivial. In fact, most implementations of it
	/// that can be found online are copies of the same poorly written code, and perform needless extra
	/// enumeration. (While fetching each individual thumbnail in a directory, for each file they
	/// enumerate the whole directory over again. I have seen this error in all examples online.)</para>
	/// <para>
	/// Also, this extracts thumbnails using the older Windows XP logic. i.e. directories only get
	/// thumbnails if they contain files that are thumbnailed, such as images and videos.</para></remarks>
	internal static partial class XPThumbnailer
	{
		#region Private Fields

		private static Guid _iID_IExtractImage = new("{BB2E617C-0920-11d1-9A0B-00C04FC2D6C1}");

		private static Guid _iID_ShellFolder = new("000214E6-0000-0000-C000-000000000046");

		#endregion Private Fields

		#region Public Methods

		public static Bitmap? ExtractThumbnail(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException(nameof(path));
			}

			if (File.Exists(path) || Directory.Exists(path))
			{
				IntPtr pidlMain = IntPtr.Zero;
				IntPtr pidlChild = IntPtr.Zero;
				if (NativeMethods.SHGetDesktopFolder(out IShellFolder desktopFolder) == HResult.S_OK)
				{
					IShellFolder? parentFolder = null;
					try
					{
						uint pdwAttrib = 0;
						string directroryPath = Path.GetDirectoryName(path);
						string basePath = Path.GetFileName(path);

						HResult hRes = desktopFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, directroryPath, out uint cParsed, out pidlMain, ref pdwAttrib);

						if (hRes == HResult.S_OK)
						{
							IntPtr ppv = IntPtr.Zero;
							hRes = desktopFolder.BindToObject(pidlMain, IntPtr.Zero, ref _iID_ShellFolder, out ppv);
							if (hRes == HResult.S_OK)
							{
								parentFolder = (IShellFolder)Marshal.GetTypedObjectForIUnknown(ppv, typeof(IShellFolder));

								if (parentFolder != null)
								{
									uint childAttrib = 0;

									hRes = parentFolder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, basePath, out uint parsedChild, out pidlChild, ref childAttrib);

									if (hRes == HResult.S_OK)
									{
										int prgf = 0;
										IntPtr ppvOut = IntPtr.Zero;

										hRes = parentFolder.GetUIObjectOf(IntPtr.Zero, 1, ref pidlChild, ref _iID_IExtractImage, ref prgf, ref ppvOut);

										if (hRes == HResult.S_OK)
										{
											IExtractImage extractImage = (IExtractImage)Marshal.GetTypedObjectForIUnknown(ppvOut, typeof(IExtractImage));

											if (extractImage != null)
											{
												IntPtr hbitmap = IntPtr.Zero;
												try
												{
													SIZE sz = new() { cx = 256, cy = 256 };

													StringBuilder location = new(260);
													int priority = 0;
													EIEIFLAG flags = EIEIFLAG.IEIFLAG_ORIGSIZE | EIEIFLAG.IEIFLAG_QUALITY;
													int uFlags = (int)flags;

													hRes = extractImage.GetLocation(location, location.Capacity, ref priority, ref sz, 32, ref uFlags);

													if (hRes == HResult.S_OK)
													{
														hRes = extractImage.Extract(ref hbitmap);

														if (hRes == HResult.S_OK && hbitmap != IntPtr.Zero)
														{
															return ImageUtils.FixShellBitmap(path, Image.FromHbitmap(hbitmap));
														}
													}
												}
												finally
												{
													_ = Marshal.ReleaseComObject(extractImage);

													if (hbitmap != IntPtr.Zero)
													{
														_ = NativeMethods.DeleteObject(hbitmap);
													}
												}
											}
										}
									}
								}
							}
						}
					}
					finally
					{
						if (pidlChild != IntPtr.Zero)
						{
							Marshal.FreeCoTaskMem(pidlChild);
						}

						if (pidlMain != IntPtr.Zero)
						{
							Marshal.FreeCoTaskMem(pidlMain);
						}

						if (parentFolder != null)
						{
							_ = Marshal.ReleaseComObject(parentFolder);
						}

						_ = Marshal.ReleaseComObject(desktopFolder);
					}
				}
			}

			return null;
		}

		#endregion Private Classes
	}
}

