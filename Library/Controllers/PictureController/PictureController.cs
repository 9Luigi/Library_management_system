﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Controllers.PictureController
{
	static internal class PictureController
	{
		internal static byte[] ImageToByteConvert(Image img)
		{
			try
			{
				byte[] byteArray = new byte[0];
				using (MemoryStream ms = new MemoryStream())
				{
					using (Bitmap bitmap = new Bitmap(img)) { bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png); }
					return ms.ToArray();
				}
			}
			catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); return new byte[0]; }
		}
		internal static Image ConvertByteToImage(byte[]? imageByte)
		{
			if (imageByte == null || imageByte.Length == 0)
			{
				return Properties.Resources.NoImage;
			}

			try
			{
				using (MemoryStream ms = new MemoryStream(imageByte))
				{
					return Image.FromStream(ms);
				}
			}
			catch (ArgumentException ex)
			{
				Console.WriteLine($"Ошибка при конвертации изображения: {ex.Message}");
				return Properties.Resources.NoImage; 
			}
		}
		internal static Image? GetImageFromFile()
		{
			try
			{
				using OpenFileDialog fd = new();
				fd.Filter = "Image Files|*.jpg;*.jpeg;";
				var result = fd.ShowDialog();
				var filePath = fd.FileName;
				if (string.IsNullOrEmpty(fd.FileName) || result != DialogResult.OK) return null;
				var photo = Image.FromFile(fd.FileName);

				double aspectRatio = (double)photo.Width / photo.Height;
				photo = CorrectImageOrientation(photo); //restrict photo rotation

				if (Math.Abs(aspectRatio - AspectRatioRequirement.ThreeToFour) > 0.01 //check photo aspect ratio
					&& Math.Abs(aspectRatio - AspectRatioRequirement.FourToFive) > 0.01
					&& Math.Abs(aspectRatio - AspectRatioRequirement.TenToTwelve) > 0.01)
				{
					MessageBox.Show("Photo might be 3:4 / 6:8 / 4:5"); //3:4 == 6:8
					return null;
				}
				else
				{
					return photo;
				}
			}
			catch (OutOfMemoryException ex)
			{
				MessageBox.Show("Error, corrupted image or wrong format: " + ex.Message);
				return null;
			}
			catch (Exception ex)
			{
				MessageBox.Show("An error occurred: " + ex.Message);
				return null;
			}
		}
		private static Image CorrectImageOrientation(Image image) //if photo has EXIF orientation tag it could rotate, this method restrict that behavior
		{
			const int orientationTag = 0x0112; // EXIF tag for orientation
			if (image.PropertyIdList.Contains(orientationTag))
			{
				// read EXIF data
				var propItem = image.GetPropertyItem(orientationTag);
				short orientation = BitConverter.ToInt16(propItem!.Value!, 0);

				// fix image rotation base of EXIF data
				switch (orientation)
				{
					case 1: // Normal
						break;
					case 3: // 180 degrees rotation
						image.RotateFlip(RotateFlipType.Rotate180FlipNone);
						break;
					case 6: // 90 degrees clockwise
						image.RotateFlip(RotateFlipType.Rotate90FlipNone);
						break;
					case 8: // 90 degrees counter-clockwise
						image.RotateFlip(RotateFlipType.Rotate270FlipNone);
						break;
				}
			}

			return image;
		}
	}
}
