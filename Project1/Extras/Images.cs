using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Extras
{
    public class Images
    {
        public byte[] ImagetoByteArray(Image img)
        {
            using (var ms = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(img);
                bmp.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
        }

        public Image ByteArraytoImage(byte[] arr)
        {
            using (var ms = new MemoryStream(arr))
            {
                return Image.FromStream(ms);
            }
        }
    }
}
