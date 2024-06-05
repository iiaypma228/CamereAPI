using System.Drawing;
using System.Drawing.Imaging;
using Avalonia.Controls;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Image = System.Drawing.Image;

namespace Camera.UI.Extensions;

public static class ImageExtensions
{
    public static Bitmap ConvertToAvaloniaBitmap(this Image bitmap)
    {
        if (bitmap == null)
            return null;
        System.Drawing.Bitmap bitmapTmp = new System.Drawing.Bitmap(bitmap);
        var bitmapdata = bitmapTmp.LockBits(new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        Bitmap bitmap1 = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
            bitmapdata.Scan0,
            new Avalonia.PixelSize(bitmapdata.Width, bitmapdata.Height),
            new Avalonia.Vector(96, 96),
            bitmapdata.Stride);
        bitmapTmp.UnlockBits(bitmapdata);
        bitmapTmp.Dispose();
        return bitmap1;
    }
    public static byte[] ImageToByte(this Image img)
    {
        ImageConverter converter = new ImageConverter();
        return (byte[])converter.ConvertTo(img, typeof(byte[]));
    }

}