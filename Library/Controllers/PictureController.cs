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
    }
}
