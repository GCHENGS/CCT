using CCT.Resource.Enums;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace CCT.Resource.Helpers
{
    /// <summary>
    /// 图片生成器
    /// </summary>
    public class ImageGenerateHelper
    {
        //转为图片并保存
        public static void GenerateImage(BitmapSource bitmap, ImageFormat format, Stream destStream)
        {
            BitmapEncoder encoder = null;

            switch (format)
            {
                case ImageFormat.JPG:
                    encoder = new JpegBitmapEncoder();
                    break;
                case ImageFormat.PNG:
                    encoder = new PngBitmapEncoder();
                    break;
                case ImageFormat.BMP:
                    encoder = new BmpBitmapEncoder();
                    break;
                case ImageFormat.GIF:
                    encoder = new GifBitmapEncoder();
                    break;
                case ImageFormat.TIF:
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    throw new InvalidOperationException();
            }

            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(destStream);
        }
    }
}
