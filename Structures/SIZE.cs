using System.Runtime.InteropServices;

namespace Loxifi.Interfaces
{

	[StructLayout(LayoutKind.Sequential)]
	internal struct SIZE
	{
		public int cx;

		public int cy;

		public SIZE()
		{

		}
		public SIZE(int x, int y) : this()
		{
			cx = x;
			cy = y;
		}
	}
}

