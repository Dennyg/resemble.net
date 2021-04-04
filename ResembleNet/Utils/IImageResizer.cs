using System.Drawing;

namespace ResembleNet.Utils
{
    /// <summary>
    /// An interface to represent method required for image resizer.
    /// </summary>
    public interface IImageResizer
    {
        /// <summary>
        /// Resizes image to specified dimensions.
        /// </summary>
        /// <param name="image">An image to resize.</param>
        /// <param name="width">A target width.</param>
        /// <param name="height">A target height.</param>
        /// <returns></returns>
        Bitmap Resize(Image image, int width, int height);
    }
}
