using System.Collections.Generic;

namespace ResembleNet.Settings
{
    /// <summary>
    /// A class with default tolerance settings for ignore options.
    /// </summary>
    public class ToleranceSettings
    {
        /// <summary>
        /// A default tolerance values:
        /// red - 16
        /// green 16
        /// blue - 16
        /// alpha - 16
        /// min brightness 16
        /// max brightness 244
        /// </summary>
        public static Dictionary<Tolerance, byte> Default
            => new()
            {
                [Tolerance.Red] = 16,
                [Tolerance.Green] = 16,
                [Tolerance.Blue] = 16,
                [Tolerance.Alpha] = 16,
                [Tolerance.MinBrightness] = 16,
                [Tolerance.MaxBrightness] = 244
            };

        /// <summary>
        /// Tolerance values without ignore:
        /// red - 0
        /// green 0
        /// blue - 0
        /// alpha - 0
        /// min brightness 0
        /// max brightness 255
        /// </summary>
        public static Dictionary<Tolerance, byte> IgnoreNothing
        => new()
        {
            [Tolerance.Red] = 0,
            [Tolerance.Green] = 0,
            [Tolerance.Blue] = 0,
            [Tolerance.Alpha] = 0,
            [Tolerance.MinBrightness] = 0,
            [Tolerance.MaxBrightness] = 255
        };

        /// <summary>
        /// Tolerance values to ignore less:
        /// red - 16
        /// green 16
        /// blue - 16
        /// alpha - 16
        /// min brightness 16
        /// max brightness 240
        /// </summary>
        public static Dictionary<Tolerance, byte> IgnoreLess
            => new()
            {
                [Tolerance.Red] = 16,
                [Tolerance.Green] = 16,
                [Tolerance.Blue] = 16,
                [Tolerance.Alpha] = 16,
                [Tolerance.MinBrightness] = 16,
                [Tolerance.MaxBrightness] = 240
            };

        /// <summary>
        /// Tolerance values to ignore anti-aliasing:
        /// red - 32
        /// green 32
        /// blue - 32
        /// alpha - 32
        /// min brightness 64
        /// max brightness 96
        /// </summary>
        public static Dictionary<Tolerance, byte> IgnoreAntialiasing
            => new()
            {
                [Tolerance.Red] = 32,
                [Tolerance.Green] = 32,
                [Tolerance.Blue] = 32,
                [Tolerance.Alpha] = 32,
                [Tolerance.MinBrightness] = 64,
                [Tolerance.MaxBrightness] = 96
            };

        /// <summary>
        /// Tolerance values to ignore colors:
        /// red - 255
        /// green 255
        /// blue - 255
        /// alpha - 255
        /// min brightness 16
        /// max brightness 240
        /// </summary>
        public static Dictionary<Tolerance, byte> IgnoreColors
            => new()
            {
                [Tolerance.Red] = 255,
                [Tolerance.Green] = 255,
                [Tolerance.Blue] = 255,
                [Tolerance.Alpha] = 255,
                [Tolerance.MinBrightness] = 16,
                [Tolerance.MaxBrightness] = 240
            };

        /// <summary>
        /// Tolerance values to ignore alpha channel:
        /// red - 16
        /// green 16
        /// blue - 16
        /// alpha - 255
        /// min brightness 16
        /// max brightness 240
        /// </summary>
        public static Dictionary<Tolerance, byte> IgnoreAlpha
            => new()
            {
                [Tolerance.Red] = 16,
                [Tolerance.Green] = 16,
                [Tolerance.Blue] = 16,
                [Tolerance.Alpha] = 255,
                [Tolerance.MinBrightness] = 16,
                [Tolerance.MaxBrightness] = 240
            };
    }
}
