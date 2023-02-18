using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Loxifi.Tests
{
	[TestClass]
	public class ThumbnailerTests
	{
		[TestMethod]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
		public void TestExtraction()
		{
			throw new NotImplementedException("Needs a path");

			string path = "";

			string output = "icons";

			if (!Directory.Exists(output))
			{
				_ = Directory.CreateDirectory(output);
			}

			foreach (string file in Directory.EnumerateFiles(path))
			{
				Image thumbnail = Thumbnailer.GetImage(file);

				thumbnail.Save(Path.Combine(output, Path.GetFileNameWithoutExtension(file) + ".png"));
			}
		}
	}
}
