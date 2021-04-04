namespace ResembleNet.Settings
{
    /// <summary>
    /// A tolerance type for color.
    /// </summary>
    public enum Tolerance
    {
        /// <summary>
        /// Tolerance of red channel. 
        /// </summary>
        Red,

        /// <summary>
        /// Tolerance of green channel. 
        /// </summary>
        Green,

        /// <summary>
        /// Tolerance of blue channel.
        /// </summary>
        Blue,

        /// <summary>
        /// Tolerance of alpha channel.
        /// </summary>
        Alpha,

        /// <summary>
        /// Tolerance of min brightness. 
        /// </summary>
        MinBrightness,

        /// <summary>
        /// Tolerance of max brightness. 
        /// </summary>
        MaxBrightness
    }
}
