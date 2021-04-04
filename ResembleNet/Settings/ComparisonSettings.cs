using System.Drawing;

namespace ResembleNet.Settings
{
    /// <summary>
    /// A class to represent external settings for comparison.
    /// </summary>
    public class ComparisonSettings
    {
        /// <summary>
        /// Represents areas that should be verified only.
        /// </summary>
        public Rectangle[] BoundingBoxes { get; set; }

        /// <summary>
        /// Represents areas that should be ignored during verification.
        /// </summary>
        public Rectangle[] IgnoredBoxes { get; set; }

        /// <summary>
        /// Represents a color that should be ignored during verification.
        /// Color is used as mask on baseline image.
        /// </summary>
        public Color? IgnoreAreasColoredWith { get; set; }

        /// <summary>
        /// Identifies a type how difference should be presented.
        /// </summary>
        public DifferenceType? DifferenceType { get; set; }

        /// <summary>
        /// Identifies transparency of background image.
        /// </summary>
        public float? Transparency { get; set; }

        /// <summary>
        /// Identifies a base color to show difference.
        /// </summary>
        public Color? DifferenceColor { get; set; }
    }
}
