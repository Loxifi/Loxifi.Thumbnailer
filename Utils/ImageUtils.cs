using Loxifi.Extensions;
using System.Drawing;

namespace Loxifi.Utils
{
	internal static class ImageUtils
	{
		public static Bitmap FixShellBitmap(string filename, Bitmap? result)
		{

			if (string.IsNullOrEmpty(filename))
			{
				throw new ArgumentException($"'{nameof(filename)}' cannot be null or empty.", nameof(filename));
			}

			if (result is null)
			{
				throw new ArgumentNullException(nameof(result));
			}

			result.MakeTransparent();


			// Some special handling for certain thumbnail types.
			try
			{
				if (Directory.Exists(filename))
				{
					
					//Flood fill code removed from here
					result.MakeTransparent(Color.Transparent);
				}

				if (result != null && (result.Size.Width > 256 || result.Size.Height > 256))
				{
					result = result.Resize(256, 256);
				}
			}
			catch (Exception)
			{
				if (result != null)
				{
					result.Dispose();
					result = null;
				}
			}

			return result;
		}
	}
}
