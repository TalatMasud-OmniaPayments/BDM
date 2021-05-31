namespace Omnia.Pie.Vtm.Framework.ControlExtenders
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;

	public class BitmapExtender
	{
		public static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
		{
			// BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

			using (MemoryStream outStream = new MemoryStream())
			{
				var enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				var bitmap = new Bitmap(outStream);

				return new Bitmap(bitmap);
			}
		}

		public static BitmapImage LoadImageAsJpeg(string imageFilePath)
		{
			if (!File.Exists(imageFilePath))
				return null;

			var originalImage = new BitmapImage();
			originalImage.BeginInit();
			originalImage.CacheOption = BitmapCacheOption.OnLoad;
			originalImage.UriSource = new Uri(imageFilePath);
			originalImage.EndInit();

			var jpegImage = ConvertBitmapToJpegImage(originalImage);
			return jpegImage;
		}

		public static BitmapImage LoadImageFromExecutablePath(string fileName)
		{
			string codeBase = Assembly.GetExecutingAssembly().CodeBase;
			var uri = new UriBuilder(codeBase);
			string path = Uri.UnescapeDataString(uri.Path);
			path = Path.GetDirectoryName(path);
			var imageFilePath = Path.Combine(path, fileName);

			if (!File.Exists(imageFilePath))
				return null;

			var originalImage = new BitmapImage();
			originalImage.BeginInit();
			originalImage.CacheOption = BitmapCacheOption.OnLoad;
			originalImage.UriSource = new Uri(imageFilePath);
			originalImage.EndInit();

			var jpegImage = ConvertBitmapToJpegImage(originalImage);
			return jpegImage;
		}

		public static BitmapImage ConvertBitmapToJpegImage(BitmapSource bitmap)
		{
			byte[] jpegImageBytes = ConvertBitmapToJpegBytes(bitmap);

			var jpegImage = new BitmapImage();
			using (var memoryStream = new MemoryStream(jpegImageBytes))
			{
				memoryStream.Position = 0;
				jpegImage.BeginInit();
				jpegImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				jpegImage.CacheOption = BitmapCacheOption.OnLoad;
				jpegImage.StreamSource = memoryStream;
				jpegImage.EndInit();
			}

			jpegImage.Freeze();

			return jpegImage;
		}

		public static byte[] ConvertBitmapToJpegBytes(BitmapSource bitmap)
		{
			using (var memoryStream = new MemoryStream())
			{
				WriteBitmapToStream(bitmap, new JpegBitmapEncoder(), memoryStream);
				return memoryStream.ToArray();
			}
		}

		private static void WriteBitmapToStream(BitmapSource bitmap, BitmapEncoder encoder, Stream stream)
		{
			if (bitmap == null)
			{
				throw new ArgumentNullException("bitmap");
			}

			encoder.Frames.Add(BitmapFrame.Create(bitmap));
			encoder.Save(stream);
		}

		public static BitmapImage Resize(BitmapSource bitmap, int newWidth, int newHeight)
		{
			byte[] jpegImageBytes = ConvertBitmapToJpegBytes(bitmap);

			var jpegImage = new BitmapImage();
			using (var jpegImageStream = new MemoryStream(jpegImageBytes))
			{
				jpegImage.BeginInit();
				jpegImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				jpegImage.CacheOption = BitmapCacheOption.OnLoad;
				jpegImage.StreamSource = jpegImageStream;
				jpegImage.DecodePixelWidth = newWidth;
				jpegImage.DecodePixelHeight = newHeight;
				jpegImage.EndInit();
			}
			jpegImage.Freeze();

			return jpegImage;
		}

		public static BitmapImage CombineBitmaps(Bitmap topImage, Bitmap bottomImage)
		{
			//read all images into memory
			System.Collections.Generic.List<Bitmap> images = new System.Collections.Generic.List<Bitmap>();
			Bitmap finalImage = null;

			try
			{
				int width = topImage.Width;
				int height = topImage.Height + bottomImage.Height;

				images.Add(topImage);
				images.Add(bottomImage);

				//create a bitmap to hold the combined image
				finalImage = new System.Drawing.Bitmap(width, height);

				//get a graphics object from the image so we can draw on it
				using (Graphics g = Graphics.FromImage(finalImage))
				{
					//set background color
					g.Clear(System.Drawing.Color.Black);

					//go through each image and draw it on the final image
					int offset = 0;
					foreach (Bitmap image in images)
					{
						g.DrawImage(image,
						  new Rectangle(0, offset, image.Width, image.Height));
						offset += image.Height;
					}
				}

				var bitmapimage = new BitmapImage();
				using (MemoryStream memory = new MemoryStream())
				{
					finalImage.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
					memory.Position = 0;
					bitmapimage.BeginInit();
					bitmapimage.StreamSource = memory;
					bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapimage.EndInit();

				}

				return bitmapimage;
			}
			catch (Exception ex)
			{
				if (finalImage != null)
					finalImage.Dispose();

				throw ex;
			}
			finally
			{
				//clean up memory
				foreach (Bitmap image in images)
				{
					image.Dispose();
				}
			}
		}

		public static BitmapImage BitmapImageFromStream(Stream x)
		{
			var bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.StreamSource = x;
			bitmapImage.EndInit();
			return bitmapImage;
		}

		public static BitmapImage BitmapImageFromResource(string path)
		{
			var dll = Assembly.GetCallingAssembly();
			return BitmapImageFromStream(dll.GetManifestResourceStream($"{Path.GetFileNameWithoutExtension(dll.Location)}.{path}"));
		}

		public static BitmapImage BitmapImageFromMemory(byte[] x) => BitmapImageFromStream(new MemoryStream(x));

		public static BitmapImage BitmapImageFromMemory(object[] x) => BitmapImageFromMemory(((object[])x)?.Select(i => (byte)i).ToArray());

		static BitmapPixel[,] ConvertBitmapToPixels(BitmapSource bitmap)
		{
			if (bitmap.Format != PixelFormats.Bgra32)
			{
				bitmap = new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0);
			}

			// Get byte representation of the bitmap pixels
			int stride = bitmap.PixelWidth * bitmap.Format.BitsPerPixel / 8;
			byte[] pixelBytes = new byte[bitmap.PixelHeight * stride];
			bitmap.CopyPixels(pixelBytes, stride, 0);

			// Convert bytes to BitmapPixel structure
			var pixels = new BitmapPixel[bitmap.PixelWidth, bitmap.PixelHeight];
			int pixelIndex = 0;
			for (int i = 0; i < bitmap.PixelWidth; ++i)
			{
				for (int j = 0; j < bitmap.PixelHeight; ++j)
				{
					pixels[i, j].Blue = pixelBytes[pixelIndex++];
					pixels[i, j].Green = pixelBytes[pixelIndex++];
					pixels[i, j].Red = pixelBytes[pixelIndex++];
					pixels[i, j].Alpha = pixelBytes[pixelIndex++];
				}
			}

			return pixels;
		}

		static WriteableBitmap ConvertPixelsToBitmap(BitmapPixel[,] pixels, double dpiX, double dpiY)
		{
			// Build byte representation of the bitmap pixels
			int width = pixels.GetLength(0);
			int height = pixels.GetLength(1);
			byte[] pixelBytes = new byte[width * height * 4];

			int pixelIndex = 0;
			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					pixelBytes[pixelIndex++] = pixels[i, j].Blue;
					pixelBytes[pixelIndex++] = pixels[i, j].Green;
					pixelBytes[pixelIndex++] = pixels[i, j].Red;
					pixelBytes[pixelIndex++] = pixels[i, j].Alpha;
				}
			}

			// Convert bytes to bitmap
			var bitmap = new WriteableBitmap(width, height, dpiX, dpiY, PixelFormats.Bgra32, null);
			bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0, 0);

			return bitmap;
		}

		public static BitmapSource IncreaseContrast(BitmapSource bitmap)
		{
			BitmapPixel[,] pixels = ConvertBitmapToPixels(bitmap);
			IncreaseContrast(pixels);
			BitmapSource contrastBitmap = ConvertPixelsToBitmap(pixels, bitmap.DpiX, bitmap.DpiY);
			return contrastBitmap;
		}

		static void IncreaseContrast(BitmapPixel[,] pixels)
		{
			byte maxColor = 255;
			byte minColor = 0;

			int width = pixels.GetLength(0);
			int height = pixels.GetLength(1);
			for (int i = 0; i < width; ++i)
			{
				for (int j = 0; j < height; ++j)
				{
					bool isWhite =
						pixels[i, j].Blue == maxColor &&
						pixels[i, j].Green == maxColor &&
						pixels[i, j].Red == maxColor;

					if (!isWhite)
					{
						pixels[i, j].Blue = minColor;
						pixels[i, j].Green = minColor;
						pixels[i, j].Red = minColor;
					}
				}
			}
		}
	}

	public static class BitmapExtensions
	{
		public static string ToBase64String(this BitmapSource bitmap)
		{
			return Convert.ToBase64String(bitmap.ToJpegBytes());
		}

		private static byte[] ToJpegBytes(this BitmapSource bitmap)
		{
			using (var memoryStream = new MemoryStream())
			{
				bitmap.WriteToStream(new JpegBitmapEncoder(), memoryStream);
				return memoryStream.ToArray();
			}
		}

		private static void WriteToStream(this BitmapSource bitmap, BitmapEncoder encoder, Stream stream)
		{
			encoder.Frames.Add(BitmapFrame.Create(bitmap));
			encoder.Save(stream);
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BitmapPixel
	{
		public byte Blue;
		public byte Green;
		public byte Red;
		public byte Alpha;
	}
}