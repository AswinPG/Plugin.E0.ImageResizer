using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID
using Android.Graphics;
#elif IOS
using CoreGraphics;
using UIKit;
#endif

namespace Plugin.E0.ImageResizer
{
    public class ImageResizer : IImageResizer
    {
        public byte[] Resize(float toHeight, float toWidth, byte[] Image)
        {
#if ANDROID
            Bitmap originalImage = BitmapFactory.DecodeByteArray(Image, 0, Image.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)toWidth, (int)toHeight, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
#elif IOS
            UIImage originalImage = ImageFromByteArray(Image);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)toWidth, (int)toHeight, 8,
                                                 4 * (int)toWidth, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, toWidth, toHeight);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
#else
            throw new NotImplementedException();
#endif
        }

        public byte[] ResizeHeight(float toHeight, byte[] Image)
        {
#if ANDROID
            Bitmap originalImage = BitmapFactory.DecodeByteArray(Image, 0, Image.Length);
            float toWidth = (Convert.ToSingle(originalImage.Width) / Convert.ToSingle(originalImage.Height)) * toHeight;
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)toWidth, (int)toHeight, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
#elif IOS
            UIImage originalImage = ImageFromByteArray(Image);
            float width = (float)((originalImage.Size.Width / originalImage.Size.Height) * toHeight);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)width, (int)toHeight, 8,
                                                 4 * (int)width, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, width, toHeight);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
#else
            throw new NotImplementedException();
#endif
        }

        public byte[] ResizeWidth(float toWidth, byte[] Image)
        {
#if ANDROID
            Bitmap originalImage = BitmapFactory.DecodeByteArray(Image, 0, Image.Length);
            float toHeight = (Convert.ToSingle(originalImage.Height) / Convert.ToSingle(originalImage.Width)) * toWidth;
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)toWidth, (int)toHeight, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
#elif IOS
            UIImage originalImage = ImageFromByteArray(Image);
            float height = (float)((originalImage.Size.Height / originalImage.Size.Width) * toWidth);
            UIImageOrientation orientation = originalImage.Orientation;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                                                 (int)toWidth, (int)height, 8,
                                                 4 * (int)toWidth, CGColorSpace.CreateDeviceRGB(),
                                                 CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, toWidth, height);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
#else
            throw new NotImplementedException();
#endif
        }


#if IOS
        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
#endif
    }
}
