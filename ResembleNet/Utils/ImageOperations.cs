using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ResembleNet.Utils
{
    /// <summary>
    /// A class to describe common operations with image.
    /// </summary>
    public class ImageOperations
    {
        /// <summary>
        /// Converts an image to base64 string.
        /// </summary>
        /// <param name="image">An image to convert.</param>
        /// <returns>A base64 string.</returns>
        public static string ConvertToBase64(Image image)
        {
            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            var byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage);
        }
    }
}
