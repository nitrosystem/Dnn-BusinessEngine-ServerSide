using NitroSystem.Dnn.BusinessEngine.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Utilities
{
    public static class ImageUtil
    {
        public static Byte[] GetImageThumbnailBytes(string path, int width, int height)
        {
            var image = Image.FromFile(path);
            using (Image thumbnail = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    thumbnail.Save(memoryStream, ImageFormat.Png);
                    Byte[] bytes = new Byte[memoryStream.Length];
                    memoryStream.Position = 0;
                    memoryStream.Read(bytes, 0, (int)bytes.Length);

                    return bytes;
                }
            }
        }

        public static Image ResizeImage(Stream stream, string newImagePath, int width, int height)
        {
            Image result = null;

            try
            {
                Bitmap srcBmp = new Bitmap(stream);
                float ratio = srcBmp.Width / srcBmp.Height;
                SizeF newSize = new SizeF(width, height != 0 ? height : (srcBmp.Height * width / srcBmp.Width));
                Bitmap target = new Bitmap((int)newSize.Width, (int)newSize.Height);
                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(srcBmp, 0, 0, newSize.Width, newSize.Height);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        target.Save(memoryStream, GetImageFormat(newImagePath));
                        result = Image.FromStream(memoryStream);
                        result.Save(newImagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public static bool IsImageContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType) && contentType.ToLower().StartsWith("image/");
        }

        public static byte[] GetResizedImage(string path, string path2, int width, int height)
        {
            Bitmap imgIn = new Bitmap(path);
            double y = imgIn.Height;
            double x = imgIn.Width;
            double factor = 1;
            if (width > 0)
            {
                factor = width / x;
            }
            else if (height > 0)
            {
                factor = height / y;
            }
            System.IO.MemoryStream outStream = new System.IO.MemoryStream();
            Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));
            Graphics g = Graphics.FromImage(imgOut);
            g.Clear(Color.White);
            g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)), new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);
            imgOut.Save(path);

            Bitmap imgOut2 = new Bitmap(width, height);
            Graphics g2 = Graphics.FromImage(imgOut2);
            g2.Clear(Color.White);
            g2.DrawImage(imgOut, new RectangleF((width - imgOut.Width) / 2, (height - imgOut.Height) / 2, (int)(factor * x), (int)(factor * y)), new RectangleF(0, 0, (int)(factor * x), (int)(factor * y)), GraphicsUnit.Pixel);
            imgOut2.Save(path2);

            return outStream.ToArray();
        }

        public static ImageFormat GetImageFormat(string filePath)
        {
            switch (Path.GetExtension(filePath))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: return ImageFormat.Jpeg;
            }
        }

        public static string GetContentType(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }

        private static byte[] ResizeFromByteArray(int MaxSideSize, Byte[] byteArrayIn, string fileName)
        {
            byte[] byteArray = null;  // really make this an error gif
            MemoryStream ms = new MemoryStream(byteArrayIn);
            byteArray = ResizeFromStream(MaxSideSize, ms, fileName);

            return byteArray;
        }

        private static byte[] ResizeFromStream(int MaxSideSize, Stream Buffer, string fileName)
        {
            byte[] byteArray = null;  // really make this an error gif

            try
            {

                Bitmap bitMap = new Bitmap(Buffer);
                int intOldWidth = bitMap.Width;
                int intOldHeight = bitMap.Height;

                int intNewWidth;
                int intNewHeight;

                int intMaxSide;

                if (intOldWidth >= intOldHeight)
                {
                    intMaxSide = intOldWidth;
                }
                else
                {
                    intMaxSide = intOldHeight;
                }

                if (intMaxSide > MaxSideSize)
                {
                    //set new width and height
                    double dblCoef = MaxSideSize / (double)intMaxSide;
                    intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
                    intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
                }
                else
                {
                    intNewWidth = intOldWidth;
                    intNewHeight = intOldHeight;
                }

                Size ThumbNailSize = new Size(intNewWidth, intNewHeight);
                System.Drawing.Image oImg = System.Drawing.Image.FromStream(Buffer);
                System.Drawing.Image oThumbNail = new Bitmap(ThumbNailSize.Width, ThumbNailSize.Height);

                Graphics oGraphic = Graphics.FromImage(oThumbNail);
                oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle oRectangle = new Rectangle
                    (0, 0, ThumbNailSize.Width, ThumbNailSize.Height);

                oGraphic.DrawImage(oImg, oRectangle);

                MemoryStream ms = new MemoryStream();
                oThumbNail.Save(ms, ImageFormat.Jpeg);
                byteArray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));

                oGraphic.Dispose();
                oImg.Dispose();
                ms.Close();
                ms.Dispose();
            }
            catch (Exception)
            {
                int newSize = MaxSideSize - 20;
                Bitmap bitMap = new Bitmap(newSize, newSize);
                Graphics g = Graphics.FromImage(bitMap);
                g.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(0, 0, newSize, newSize));

                Font font = new Font("Courier", 8);
                SolidBrush solidBrush = new SolidBrush(Color.Red);
                g.DrawString("Failed File", font, solidBrush, 10, 5);
                g.DrawString(fileName, font, solidBrush, 10, 50);

                MemoryStream ms = new MemoryStream();
                bitMap.Save(ms, ImageFormat.Jpeg);
                byteArray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(byteArray, 0, Convert.ToInt32(ms.Length));

                ms.Close();
                ms.Dispose();
                bitMap.Dispose();
                solidBrush.Dispose();
                g.Dispose();
                font.Dispose();

            }
            return byteArray;
        }

        private static byte[] AddWaterMark(Byte[] byteArrayIn, string watermarkText, Brush brushcolor)
        {
            byte[] byteArray = null;
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image img = System.Drawing.Image.FromStream(ms);

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            myEncoderParameters.Param[0] = myEncoderParameter;

            Graphics gr = Graphics.FromImage(img);
            gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            gr.DrawString(watermarkText, new Font("Tahoma", 10), brushcolor, new Point(0, 0));

            MemoryStream output = new MemoryStream();
            img.Save(output, jgpEncoder, myEncoderParameters);
            byteArray = new byte[output.Length];
            output.Position = 0;
            output.Read(byteArray, 0, Convert.ToInt32(output.Length));
            return byteArray;
        }

        private static byte[] AddWaterMarkWithQualitySetting(Byte[] byteArrayIn, string watermarkText, Brush brushcolor)
        {
            byte[] byteArray = null;
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image img = System.Drawing.Image.FromStream(ms);

            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 75L); //%75
            myEncoderParameters.Param[0] = myEncoderParameter;

            Graphics gr = Graphics.FromImage(img);
            gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            gr.DrawString(watermarkText, new Font("Tahoma", 10), brushcolor, new Point(0, 0));

            MemoryStream output = new MemoryStream();
            img.Save(output, jgpEncoder, myEncoderParameters);
            byteArray = new byte[output.Length];
            output.Position = 0;
            output.Read(byteArray, 0, Convert.ToInt32(output.Length));
            return byteArray;
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static Stream AddWaterToImage(Image source, Image watermark)
        {
            return null;
            //return AddWaterToImage(new Bitmap(source), new Bitmap(watermark), WatermarkPosition.BottomRight);
        }

        public static Stream AddWaterToImage(Bitmap source, Bitmap watermark, WatermarkPosition position, byte opacity, int marginLeft = 0, int marginRight = 0, int marginTop = 0, int marginBottom = 0, int x = 0, int y = 0)
        {
            //const byte ALPHA = opacity;
            // Set the watermark's pixels' Alpha components.
            Color clr;
            for (int py = 0; py < watermark.Height; py++)
            {
                for (int px = 0; px < watermark.Width; px++)
                {
                    clr = watermark.GetPixel(px, py);
                    watermark.SetPixel(px, py, Color.FromArgb(opacity, clr.R, clr.G, clr.B));
                }
            }

            // Set the watermark's transparent color.
            watermark.MakeTransparent(watermark.GetPixel(0, 0));

            var point = GetWatermarkPosition(source, watermark, position, x, y);

            if (marginLeft != 0)
                point.X += marginLeft;

            if (marginRight != 0)
                point.X -= marginRight;

            if (marginTop != 0)
                point.Y += marginTop;

            if (marginBottom != 0)
                point.Y -= marginBottom;

            // Copy onto the result image.
            using (Graphics gr = Graphics.FromImage(source))
            {
                gr.DrawImage(watermark, point);
            }

            return source.ToStream();
        }

        private static Point GetWatermarkPosition(Bitmap image, Bitmap watermark, WatermarkPosition position, int x = 0, int y = 0)
        {
            switch (position)
            {
                case WatermarkPosition.Absolute:
                    break;
                case WatermarkPosition.TopLeft:
                    x = 0; y = 0;
                    break;
                case WatermarkPosition.TopRight:
                    x = image.Width - watermark.Width; y = 0;
                    break;
                case WatermarkPosition.TopMiddle:
                    x = (image.Width - watermark.Width) / 2; y = 0;
                    break;
                case WatermarkPosition.BottomLeft:
                    x = 0; y = image.Height - watermark.Height;
                    break;
                case WatermarkPosition.BottomRight:
                    x = image.Width - watermark.Width;
                    y = image.Height - watermark.Height;
                    break;
                case WatermarkPosition.BottomMiddle:
                    x = (image.Width - watermark.Width) / 2;
                    y = image.Height - watermark.Height;
                    break;
                case WatermarkPosition.MiddleLeft:
                    x = 0; y = (image.Height - watermark.Height) / 2;
                    break;
                case WatermarkPosition.MiddleRight:
                    x = image.Width - watermark.Width;
                    y = (image.Height - watermark.Height) / 2;
                    break;
                case WatermarkPosition.Center:
                    x = (image.Width - watermark.Width) / 2;
                    y = (image.Height - watermark.Height) / 2;
                    break;
                default:
                    break;
            }

            return new Point(x, y);
        }

        public static Stream ToStream(this Image image)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
        }
    }
}


