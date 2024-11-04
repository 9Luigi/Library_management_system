using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Controllers
{
    static internal class pictureBoxController
    {
        static internal void pictureBoxImageSetDefault(PictureBox pictureBox)
        {
            pictureBox.Image = Properties.Resources.NoImage;
        }
    }
}
