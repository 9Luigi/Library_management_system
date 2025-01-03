﻿using System.Drawing.Imaging;

namespace Library.Presentation.Controllers.PictureController
{
    //TODO add logs?
    /// <summary>
    /// A controller class for handling image-related operations such as conversion, compression, and file selection.
    /// </summary>
    static internal class PictureController
    {
        /// <summary>
        /// Converts an image to a byte array.
        /// </summary>
        /// <param name="img">The image to convert.</param>
        /// <returns>A byte array representing the image in PNG format.</returns>
        internal static byte[] ImageToByteConvert(Image img)
        {
            try
            {
                using MemoryStream ms = new();
                using Bitmap bitmap = new(img);
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                MessageBoxController.ShowError(ex.Message);
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Converts a byte array to an image.
        /// </summary>
        /// <param name="imageByte">The byte array representing the image.</param>
        /// <returns>An <see cref="Image"/> object, or a default "No Image" if the byte array is null or empty.</returns>
        internal static Image ConvertByteToImage(byte[]? imageByte)
        {
            if (imageByte == null || imageByte.Length == 0)
            {
                return Properties.Resources.NoImage;
            }

            try
            {
                using MemoryStream ms = new(imageByte);
                return Image.FromStream(ms);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error during image conversion: {ex.Message}");
                return Properties.Resources.NoImage;
            }
        }

        /// <summary>
        /// Opens a file dialog to allow the user to select an image file and validates its aspect ratio.
        /// If the image is larger than 10 MB, it is compressed.
        /// </summary>
        /// <returns>The selected image if valid, or <c>null</c> if invalid or canceled.</returns>
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

                // Get image size in bytes before any processing
                byte[] imageBytes = ImageToByteConvert(photo);
                long imageSize = imageBytes.Length;
                // Check if image is larger than 1 MB (1 MB = 1024 * 1024 bytes)
                if (imageSize > 1024 * 1024)
                {
                    MessageBoxController.ShowInfo($"Image is {imageSize / 1024} MB, compressing image...");
                    photo = ConvertByteToImage(CompressImage(photo)); // Compress the image if it's larger than 10 MB
                }
                imageBytes = ImageToByteConvert(photo);
                imageSize = imageBytes.Length;
                double aspectRatio = (double)photo.Width / photo.Height;
                photo = CorrectImageOrientation(photo); // Restrict photo rotation

				// Checking aspect ratio using values ​​from a static class
				bool isValidAspectRatio = AspectRatioRequirement.ValidAspectRatios
					.Any(ratio => Math.Abs(aspectRatio - ratio) <= 0.01);

				if (!isValidAspectRatio)
				{
					MessageBoxController.ShowWarning("Photo must be be 3:4 / 6:8 / 4:5");
					photo.Dispose();
					return null;
				}
				else
				{
					return photo;
				}

			}
			catch (OutOfMemoryException ex)
            {
                ErrorController.HandleException(ex, "Corrupted image or wrong format");
                return null;
            }
            catch (Exception ex)
            {
				ErrorController.HandleException(ex, "An error occurred: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Corrects the image orientation based on EXIF data.
        /// </summary>
        /// <param name="image">The image to correct.</param>
        /// <returns>The corrected image.</returns>
        private static Image CorrectImageOrientation(Image image)
        {
            const int orientationTag = 0x0112; // EXIF tag for orientation
            if (image.PropertyIdList.Contains(orientationTag))
            {
                // Read EXIF data
                var propItem = image.GetPropertyItem(orientationTag);
                short orientation = BitConverter.ToInt16(propItem!.Value!, 0);

                // Fix image rotation based on EXIF data
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

        /// <summary>
        /// Compresses the given image to a specified quality level.
        /// </summary>
        /// <param name="image">The image to compress.</param>
        /// <param name="quality">The quality level for the compression (0 to 100).</param>
        /// <returns>A byte array representing the compressed image.</returns>
        internal static byte[] CompressImage(Image image, long quality = 40)
        {
            try
            {
                // Create an EncoderParameters object to specify the quality level
                var qualityEncoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(qualityEncoder, quality);

                // Create a memory stream to store the compressed image
                using MemoryStream ms = new();
                var jpegCodec = GetEncoder(ImageFormat.Jpeg);

                // Save the image to the memory stream with the specified quality
                image.Save(ms, jpegCodec, encoderParameters);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                MessageBoxController.ShowError("Error compressing image: " + ex.Message);
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// Gets the appropriate image encoder for the given format.
        /// </summary>
        /// <param name="format">The image format.</param>
        /// <returns>The encoder for the specified image format.</returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var encoders = ImageCodecInfo.GetImageEncoders();
            return encoders.FirstOrDefault(e => e.FormatID == format.Guid) ?? throw new Exception("Encoder not found.");
        }
    }
}
