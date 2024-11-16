using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Controllers
{
    static internal class PictureController
    {
		internal static byte[] ImageToByteConvert(Image img)
        {//without this method if image don't changes via filedialog while editing exceptions trows
			try
			{
				byte[] byteArray = new byte[0];
				using (MemoryStream stream = new MemoryStream())
				{
					Bitmap bitmap = new Bitmap(img);
					bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
					stream.Close();

					byteArray = stream.ToArray();
				}
				return byteArray;
			}
			catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); return null; }
        }
		internal static Image GetImageFromFileDialog()
		{
			using OpenFileDialog fd = new();
			fd.ShowDialog();
			var photo = Image.FromFile(fd.FileName); 
			//TODO out of memory exception
			//TODO format check
			double aspectRatio = (double)photo.Width / photo.Height;
			double requiredAspectRatio = 3.0 / 4.0; //TODO add variants
			if (Math.Abs(aspectRatio - requiredAspectRatio) > 0.01)
			{
				MessageBox.Show("Photo might be 3:4");
				return null;
			}
			else
			{
				return photo;
			}
		}
    }
}
