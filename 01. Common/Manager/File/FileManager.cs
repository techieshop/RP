using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.HtmlConverter;
using System.IO;
using System.Threading;

namespace RP.Common.Manager
{
	public static class FileManager
	{
		public static byte[] ConvertHtmlToPdf(string html)
		{
			MemoryStream msOutput = new MemoryStream();
			PdfDocument doc = new PdfDocument();
			Thread thread = new Thread(() =>
			{
				doc.LoadFromHTML(
					html,
					true,
					new PdfPageSettings
					{
						Margins = new PdfMargins(5),
						Orientation = PdfPageOrientation.Portrait,
					},
					new PdfHtmlLayoutFormat
					{
						Layout = PdfLayoutType.Paginate
					}
				);
				doc.ColorSpace=PdfColorSpace.RGB;
				doc.CompressionLevel = PdfCompressionLevel.None;
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			doc.SaveToStream(msOutput);
			doc.Close();

			return msOutput.ToArray();
		}
	}
}