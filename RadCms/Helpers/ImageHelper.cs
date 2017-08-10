using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RadCms.Helpers
{
    public class ImageHelper
    {
        public static Bitmap CropBitmap(Bitmap bitmap, int cropX, int cropY, int cropWidth, int cropHeight, bool scaleFirst)
        {
            if (scaleFirst)
            {
                int orgWidth = bitmap.Width;
                int orgHeight = bitmap.Height;
                if(orgWidth/orgHeight > cropWidth/cropHeight)
                {
                    //cut width
                    Image img = bitmap.GetThumbnailImage(cropHeight * orgWidth / orgHeight, cropHeight, null, IntPtr.Zero);
                    bitmap = new Bitmap(img);
                }
                else
                {
                    //cut width
                    Image img = bitmap.GetThumbnailImage(cropWidth, cropWidth * orgHeight / orgWidth, null, IntPtr.Zero);
                    bitmap = new Bitmap(img);
                }
            }
            //Image img = bm.GetThumbnailImage(bm.Width, bm.Height, null, IntPtr.Zero);
            Rectangle rect = new Rectangle(cropX, cropY, cropWidth, cropHeight);
            Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);
            return cropped;
        }
    }
}
