using System;

namespace ResembleNet
{
    /// <summary>
    /// A class to describe comparison result.
    /// </summary>
    public class ComparisonResult
    {
        /// <summary>
        /// Bounds of area where difference was found.
        /// </summary>
        public DifferenceBounds DiffBounds { get; set; }
       
        /// <summary>
        /// Size difference between 2 images.
        /// </summary>
        public DimensionDifference DimensionDifference { get; set; }

        /// <summary>
        /// Returns whether 2 images had same dimensions or not.
        /// </summary>
        public bool IsSameDimensions { get; set; }
        
        /// <summary>
        /// Represent mismatch between to images from 0 to 1.
        /// </summary>
        public float Mismatch { get; set; }
        
        /// <summary>
        /// Returns an image with comparison results as Base64 string.
        /// </summary>
        public string ImageDifference { get; set; }
        
        /// <summary>
        /// Returns a time spent for analysis.
        /// </summary>
        public TimeSpan AnalysisTime { get; set; }
    }

    /// <summary>
    /// A class to describe difference bounds.
    /// </summary>
    public class DifferenceBounds
    {
        /// <summary>
        /// Top coordinate of area.
        /// </summary>
        public int Top { get; set; }
        
        /// <summary>
        /// Bottom coordinate of area.
        /// </summary>
        public int Bottom { get; set; }

        /// <summary>
        /// Right coordinate of area.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// Left coordinate of area.
        /// </summary>
        public int Left { get; set; }
    }

    
    /// <summary>
    /// A class to describe difference between to images.
    /// </summary>
    public class DimensionDifference
    {
        /// <summary>
        /// A height difference.
        /// </summary>
        public int Height { get; set; }
        
        /// <summary>
        /// A width difference.
        /// </summary>
        public int Width { get; set; }
    }
}
