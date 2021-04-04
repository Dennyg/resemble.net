namespace ResembleNet.Settings
{
    /// <summary>
    /// A comparison type.
    /// </summary>
    public enum DifferenceType
    {
        /// <summary>
        /// A flat comparison.
        /// </summary>
        Flat,
        
        /// <summary>
        /// A movement comparison.
        /// </summary>
        Movement,

        /// <summary>
        /// A flat comparison with difference intensity.
        /// </summary>
        FlatDifferenceIntensity,

        /// <summary>
        /// A movement comparison with difference intensity.
        /// </summary>
        MovementDifferenceIntensity,

        /// <summary>
        /// A comparison to return only difference.
        /// </summary>
        DiffOnly
    }
}
