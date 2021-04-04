using ResembleNet.Exceptions;
using ResembleNet.Settings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ResembleNet.Utils;

namespace ResembleNet
{
    /// <summary>
    /// Provides functionality to compare 2 images. Use fluent interface to configure comparison.
    /// </summary>
    public class Resemble
    {
        private Bitmap _actualImage;
        private Bitmap _baselineImage;
        private ComparisonSettings _settings;
        private Dictionary<Tolerance, byte> _tolerance;
        private float _returnEarlyThreshold;
        private bool _compareOnly = false;
        private IImageResizer _resizer;
        private bool _scaleToSameSize = false;
        private bool _ignoreAntialiasing = false;
        private bool _ignoreColors = false;
        private bool _isAnyIgnoreOptionApplied = false;

        /// <summary>
        /// Creates an instance of <see cref="Resemble"/> with default settings.
        /// Transparency - 1, DifferenceType - Flat, DifferenceColor - Color.Magenta.
        /// </summary>
        /// <param name="actualImage">Image to compare.</param>
        public Resemble(Image actualImage)
        {
            _actualImage = new Bitmap(actualImage);
            _tolerance = ToleranceSettings.Default;
            _settings = new ComparisonSettings
            {
                Transparency = 1,
                DifferenceType = DifferenceType.Flat,
                DifferenceColor = Color.Magenta
            };
        }

        /// <summary>
        /// Sets image as baseline for verification. Image may contains mask with ignored color.
        /// </summary>
        /// <param name="baselineImage">Baseline image.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble CompareTo(Image baselineImage)
        {
            _baselineImage = new Bitmap(baselineImage);
            return this;
        }

        /// <summary>
        /// Sets threshold to stop comparison in compare-only mode.
        /// </summary>
        /// <param name="threshold">Threshold is float number and represents percents, i.e. 2.2.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble SetReturnEarlyThreshold(float threshold)
        {
            _compareOnly = true;
            _returnEarlyThreshold = threshold;
            return this;
        }

        /// <summary>
        /// Sets transparency of background image. Does not have impact on difference representation.
        /// </summary>
        /// <param name="transparency">Float number from 0.01 to 1.0. If set 1.0 image colors as in original.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble WithResultTransparency(float transparency)
        {
            if(transparency > 1 || transparency < 0.01)
            {
                throw new ArgumentOutOfRangeException("Transparency can not be less than 0 and more than 1.");
            }

            _settings.Transparency = transparency;
            return this;
        }

        /// <summary>
        /// Sets color to display difference. Default value for comparison is Color.Magenta
        /// </summary>
        /// <param name="color">Color to show difference.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble WithDifferenceColor(Color color)
        {
            _settings.DifferenceColor = color;
            return this;
        }

        /// <summary>
        /// Sets variant how to display difference on result image.
        /// Difference type by default is DifferenceType.Flat.
        /// </summary>
        /// <param name="type">A value of <see cref="DifferenceType"/>.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble WithDifferenceType(DifferenceType type)
        {
            _settings.DifferenceType = type;
            return this;
        }

        /// <summary>
        /// Determines whether we need to scale actual image to baseline image or not.
        /// It's not recommended to compare images of different size. Resize action may lead to change of some pixel that is not will for human eye.
        /// Recommended to use with IgnoreAntialising or other proper thresholds.
        /// </summary>
        /// <param name="resizer">An implementation of <see cref="IImageResizer"/>.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble ScaleToBaselineSize(IImageResizer resizer)
        {
            _resizer = resizer;
            _scaleToSameSize = true;
            return this;
        }

        /// <summary>
        /// Limits comparison to predefined rectangle based areas.
        /// </summary>
        /// <param name="areasToCompare">An array of <see cref="Rectangle"/>.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble LimitComparisonAreaTo(params Rectangle[] areasToCompare)
        {
            _settings.BoundingBoxes = areasToCompare;
            return this;
        }

        /// <summary>
        /// Sets rectangle based areas that should not be included in comparison.
        /// </summary>
        /// <param name="ignoredAreas">An array of <see cref="Rectangle"/>.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble IgnoreComparisonIn(params Rectangle[] ignoredAreas)
        {
            _settings.IgnoredBoxes = ignoredAreas;
            return this;
        }

        /// <summary>
        /// Sets color of areas that should not be included in comparison. Colored mask should be added to baseline image.
        /// </summary>
        /// <param name="color">An instance of <see cref="Color"/>.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble IgnoreAreasWithColor(Color color)
        {
            _settings.IgnoreAreasColoredWith = color;
            return this;
        }

        /// <summary>
        /// Apply settings to compare images 'as is' without any tolerance and anti-aliasing.
        /// Only one type of ignore is allowed.
        /// </summary>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble IgnoreNothing()
        {
            AssertIgnoreOptionAvailability();

            _tolerance = ToleranceSettings.IgnoreNothing;
            _ignoreAntialiasing = false;
            _ignoreColors = false;
            _isAnyIgnoreOptionApplied = true;
            
            return this;
        }

        /// <summary>
        /// Apply settings to compare images with default tolerance values.
        /// Only one type of ignore is allowed.
        /// </summary>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble IgnoreLess()
        {
            AssertIgnoreOptionAvailability();
            
            _tolerance = ToleranceSettings.IgnoreLess;
            _ignoreAntialiasing = false;
            _ignoreColors = false;
            _isAnyIgnoreOptionApplied = true;

            return this;
        }


        /// <summary>
        /// Apply settings to compare images with included anti-aliasing check and tolerance level.
        /// Only one type of ignore is allowed.
        /// </summary>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble IgnoreAntialiasing()
        {
            AssertIgnoreOptionAvailability();
            
            _tolerance = ToleranceSettings.IgnoreAntialiasing;
            _ignoreAntialiasing = true;
            _ignoreColors = false;
            _isAnyIgnoreOptionApplied = true;

            return this;
        }

        /// <summary>
        /// Apply settings to compare images with ignored colors.
        /// Only one type of ignore is allowed.
        /// </summary>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble IgnoreColors()
        {
            AssertIgnoreOptionAvailability();
            
            _tolerance = ToleranceSettings.IgnoreColors;
            _ignoreAntialiasing = false;
            _ignoreColors = true;
            _isAnyIgnoreOptionApplied = true;

            return this;
        }

        /// <summary>
        /// Apply settings to compare images with ignored alpha channel of pixel.
        /// Only one type of ignore is allowed.
        /// </summary>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble IgnoreAlpha()
        {
            AssertIgnoreOptionAvailability();
            
            _tolerance = ToleranceSettings.IgnoreAlpha;
            _ignoreAntialiasing = false;
            _ignoreColors = false;
            _isAnyIgnoreOptionApplied = true;

            return this;
        }

        /// <summary>
        /// Allows to set up own tolerance levels. Supported levels: <see cref="Tolerance"/>
        /// </summary>
        /// <param name="newTolerance">A dictionary that contains tolerance with corresponding values 0..255.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble WithTolerance(Dictionary<Tolerance, byte> newTolerance)
        {
            foreach (var kvp in newTolerance)
            {
                _tolerance[kvp.Key] = kvp.Value;
            }

            return this;
        }

        /// <summary>
        /// Allows to pass all external settings at once./>
        /// </summary>
        /// <param name="newSettings">An instance of <see cref="ComparisonSettings"/> settings.</param>
        /// <returns>An instance of <see cref="Resemble"/>.</returns>
        public Resemble WithSettings(ComparisonSettings newSettings)
        {
            _settings = newSettings;

            if (newSettings.IgnoreAreasColoredWith.HasValue)
            {
                _settings.IgnoreAreasColoredWith = newSettings.IgnoreAreasColoredWith;
            }

            if (newSettings.DifferenceType.HasValue)
            {
                _settings.DifferenceType = newSettings.DifferenceType;
            }

            if (newSettings.Transparency.HasValue)
            {
                _settings.Transparency = newSettings.Transparency;
            }

            if (newSettings.DifferenceColor.HasValue)
            {
                _settings.DifferenceColor = newSettings.DifferenceColor;
            }

            if (newSettings.BoundingBoxes != null)
            {
                _settings.BoundingBoxes = newSettings.BoundingBoxes;
            }

            if (newSettings.IgnoredBoxes != null)
            {
                _settings.IgnoredBoxes = newSettings.IgnoredBoxes;
            }

            return this;
        }

        /// <summary>
        /// Compares two images based on defined set up.
        /// </summary>
        /// <returns>An instance of <see cref="ComparisonResult"/> that results of comparison.</returns>
        public ComparisonResult Compare()
        {
            if (_scaleToSameSize)
            {
                _actualImage = _resizer.Resize(_actualImage, _baselineImage.Width, _baselineImage.Height);
            }

            var comparisonAreaWidth = _actualImage.Width > _baselineImage.Width
                ? _actualImage.Width
                : _baselineImage.Width;

            var comparisonAreaHeight = _actualImage.Height > _baselineImage.Height
                ? _actualImage.Height
                : _baselineImage.Height;

            var comparisonResultImage = new Bitmap(comparisonAreaWidth, comparisonAreaHeight);

            var pixelMismatchCount = 0;

            var differenceBounds = new DifferenceBounds
            {
                Top = comparisonAreaHeight,
                Left = comparisonAreaWidth,
                Bottom = 0,
                Right = 0
            };

            void updateBounds(int x, int y)
            {
                differenceBounds.Left = Math.Min(x, differenceBounds.Left);
                differenceBounds.Right = Math.Max(x, differenceBounds.Right);
                differenceBounds.Top = Math.Min(y, differenceBounds.Top);
                differenceBounds.Bottom = Math.Max(y, differenceBounds.Bottom);
            }

            var startTime = DateTime.Now;

            var skipTheRest = false;

            for (var horizontalPosition = 0; horizontalPosition < comparisonAreaWidth; horizontalPosition++)
            {
                for (var verticalPosition = 0; verticalPosition < comparisonAreaHeight; verticalPosition++)
                {
                    if (skipTheRest
                        || !IsPixelInBounds(_actualImage, horizontalPosition, verticalPosition)
                        || !IsPixelInBounds(_baselineImage, horizontalPosition, verticalPosition))
                    {
                        break;
                    }

                    var actualPixel = _actualImage.GetPixel(horizontalPosition, verticalPosition);
                    var expectedPixel = _baselineImage.GetPixel(horizontalPosition, verticalPosition);

                    var errorPixel = GetColorOfDifference(_settings.DifferenceType.Value)(actualPixel, expectedPixel);

                    var isPixelComparable = IsPixelComparable(expectedPixel, horizontalPosition, verticalPosition);

                    if (_ignoreColors)
                    {
                        if (IsPixelBrightnessSimilar(actualPixel, expectedPixel) || !isPixelComparable)
                        {
                            if (!_compareOnly && _settings.DifferenceType != DifferenceType.DiffOnly)
                            {
                                SetGrayScaledPixel(comparisonResultImage, expectedPixel, horizontalPosition, verticalPosition);
                            }
                        }
                        else
                        {
                            if (!_compareOnly)
                            {
                                comparisonResultImage.SetPixel(horizontalPosition, verticalPosition, errorPixel);
                            }

                            pixelMismatchCount++;
                            updateBounds(horizontalPosition, verticalPosition);
                        }
                        continue;
                    }

                    if (IsRGBSimilar(actualPixel, expectedPixel) || !isPixelComparable)
                    {
                        if (!_compareOnly && _settings.DifferenceType != DifferenceType.DiffOnly)
                        {
                            SetTransparentPixel(comparisonResultImage, actualPixel, horizontalPosition, verticalPosition);
                        }
                    }
                    else if (
                      _ignoreAntialiasing &&
                      (IsAntialiased(_actualImage, horizontalPosition, verticalPosition) 
                        || IsAntialiased(_baselineImage, horizontalPosition, verticalPosition))
                    )
                    {
                        if (IsPixelBrightnessSimilar(actualPixel, expectedPixel) || !isPixelComparable)
                        {
                            if (!_compareOnly && _settings.DifferenceType != DifferenceType.DiffOnly)
                            {
                                SetGrayScaledPixel(comparisonResultImage, expectedPixel, horizontalPosition, verticalPosition);
                            }
                        }
                        else
                        {
                            if (!_compareOnly)
                            {
                                comparisonResultImage.SetPixel(horizontalPosition, verticalPosition, errorPixel);
                            }

                            pixelMismatchCount++;
                            updateBounds(horizontalPosition, verticalPosition);
                        }
                    }
                    else
                    {
                        if (!_compareOnly)
                        {
                            comparisonResultImage.SetPixel(horizontalPosition, verticalPosition, errorPixel);
                        }

                        pixelMismatchCount++;
                        updateBounds(horizontalPosition, verticalPosition);
                    }

                    if (_compareOnly)
                    {
                        var currentMisMatchPercent = (double)pixelMismatchCount / (comparisonAreaHeight * comparisonAreaWidth) * 100;

                        if (currentMisMatchPercent > _returnEarlyThreshold)
                        {
                            skipTheRest = true;
                        }
                    }
                }
            }

            var rawMisMatchPercentage = (float)pixelMismatchCount / (comparisonAreaHeight * comparisonAreaWidth) * 100;

            return new ComparisonResult
            {
                DiffBounds = differenceBounds,
                Mismatch = rawMisMatchPercentage,
                AnalysisTime = DateTime.Now - startTime,

                ImageDifference = !_compareOnly 
                    ? ImageOperations.ConvertToBase64(comparisonResultImage)
                    : null,

                IsSameDimensions = 
                    _actualImage.Width == _baselineImage.Width 
                        && _actualImage.Height == _baselineImage.Height,

                DimensionDifference = new DimensionDifference
                {
                    Width = _actualImage.Width - _baselineImage.Width,
                    Height = _actualImage.Height - _baselineImage.Height
                }
            };
        }

        /// <summary>
        /// Sets pixel color as gray scale with predefined transparency. 
        /// </summary>
        /// <param name="image">An image where need to update pixel.</param>
        /// <param name="color">Color info to gray scale.</param>
        /// <param name="x">Horizontal position of pixel to update in image.</param>
        /// <param name="y">Vertical position of pixel to update in image.</param>
        private void SetGrayScaledPixel(Bitmap image, Color color, int x, int y)
        {
            var grayScale = (int)Math.Round(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
            var newColor = Color.FromArgb((int)(color.A * _settings.Transparency.Value), grayScale, grayScale, grayScale);
            image.SetPixel(x, y, newColor);
        }

        /// <summary>
        /// Sets pixel transparency. 
        /// </summary>
        /// <param name="image">An image where need to update pixel.</param>
        /// <param name="color">Color info to make transparent.</param>
        /// <param name="x">A horizontal position of pixel to update in image.</param>
        /// <param name="y">A vertical position of pixel to update in image.</param>
        private void SetTransparentPixel(Bitmap image, Color color, int x, int y)
        {
            var newColor = Color.FromArgb((int)Math.Round(color.A * _settings.Transparency.Value), color.R, color.G, color.B);
            image.SetPixel(x, y, newColor);
        }

        /// <summary>
        /// Verifies whether pixel was anti-aliased or not.
        /// </summary>
        /// <param name="image">An image to verify.</param>
        /// <param name="horizontalPos">A horizontal position of pixel to update in image.</param>
        /// <param name="verticalPos">A vertical position of pixel to update in image.</param>
        /// <returns>Returns <see cref="bool"/> whether pixel was anti-aliased or not.</returns>
        private bool IsAntialiased(Bitmap image, int horizontalPos, int verticalPos)
        {
            var sourcePix = image.GetPixel(horizontalPos, verticalPos);
            var distance = 1;
            var hasHighContrastSibling = 0;
            var hasSiblingWithDifferentHue = 0;
            var hasEquivalentSibling = 0;

            for (var i = -distance; i <= distance; i++)
            {
                for (var j = -distance; j <= distance; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    var x = horizontalPos + i;
                    var y = verticalPos + j;

                    if (!IsPixelInBounds(image, x, y))
                    {
                        continue;
                    }

                    var targetPix = image.GetPixel(x, y);

                    if (IsContrasting(sourcePix, targetPix))
                    {
                        hasHighContrastSibling++;
                    }

                    if (IsRGBSame(sourcePix, targetPix))
                    {
                        hasEquivalentSibling++;
                    }

                    if (Math.Abs(targetPix.GetHue() - sourcePix.GetHue()) > 0.3)
                    {
                        hasSiblingWithDifferentHue++;
                    }

                    if (hasSiblingWithDifferentHue > 1 || hasHighContrastSibling > 1)
                    {
                        return true;
                    }
                }
            }

            return hasEquivalentSibling < 2;
        }

        /// <summary>
        /// Verifies whether pixel can be found im image or not.
        /// </summary>
        /// <param name="image">An image to verify.</param>
        /// <param name="x">A horizontal position of pixel to update in image.</param>
        /// <param name="y">A vertical position of pixel to update in image.</param>
        /// <returns>Returns <see cref="bool"/> whether pixel can be found im image or not.</returns>
        private bool IsPixelInBounds(Bitmap image, int x, int y)
        {
            return x >= 0 && x < image.Width && y >= 0 && y < image.Height;
        }

        /// <summary>
        /// Verifies whether pixel is in bounds of box.
        /// </summary>
        /// <param name="x">A horizontal position of pixel to update in image.</param>
        /// <param name="y">A vertical position of pixel to update in image.</param>
        /// <param name="box">A rectangle to identify boundaries.</param>
        /// <returns>Returns <see cref="bool"/> whether pixel is in bounds of box.</returns>
        private bool IsPixelInBox(int x, int y, Rectangle box)
        {
            return x > box.Left && x < box.Right && y > box.Top && y < box.Bottom;
        }


        /// <summary>
        /// Verifies whether pixel should be compared or ignored based on ignore color, ignored and bounding box values.
        /// </summary>
        /// <param name="pixel">A pixel to verify.</param>
        /// <param name="x">A horizontal position of pixel to update in image.</param>
        /// <param name="y">A vertical position of pixel to update in image.</param>
        /// <returns>Returns <see cref="bool"/> whether pixel is in bounds of box.</returns>
        private bool IsPixelComparable(Color pixel, int x, int y)
        {
            if (_settings.IgnoreAreasColoredWith.HasValue)
            {
                return ColorsDistance(pixel, _settings.IgnoreAreasColoredWith.Value) != 0;
            }

            var selected = true;
            var ignored = false;

            if (_settings.BoundingBoxes != null)
            {
                selected = _settings.BoundingBoxes.Any(b => IsPixelInBox(x, y, b));
            }

            if (_settings.IgnoredBoxes != null)
            {
                ignored = _settings.IgnoredBoxes.Any(b => IsPixelInBox(x, y, b));
            }

            return selected && !ignored;
        }

        /// <summary>
        /// Calculates distance between 2 colors.
        /// </summary>
        /// <param name="pixel1">A color 1.</param>
        /// <param name="pixel2">A color 2.</param>
        /// <returns>A distance value.</returns>
        private double ColorsDistance(Color pixel1, Color pixel2)
        {
            return (Math.Abs(pixel1.R - pixel2.R) + Math.Abs(pixel1.G - pixel2.G) + Math.Abs(pixel1.B - pixel2.B)) / 3;
        }

        /// <summary>
        /// Verifies whether pixels has similar brightness or not.
        /// </summary>
        /// <param name="pixel1">A color 1.</param>
        /// <param name="pixel2">A color 2.</param>
        /// <returns>Returns <see cref="bool"/> whether pixels has similar brightness or not.</returns>
        private bool IsPixelBrightnessSimilar(Color pixel1, Color pixel2)
        {
            var alpha = IsColorSimilar(pixel1.A, pixel2.A, Tolerance.Alpha);
            var brightness = IsColorSimilar((byte)Math.Round(pixel1.GetBrightness() * 255), (byte)Math.Round(pixel2.GetBrightness() * 255), Tolerance.MinBrightness);
            return brightness && alpha;
        }

        /// <summary>
        /// Verifies whether pixels have same RGB values or not.
        /// </summary>
        /// <param name="pixel1">A color 1.</param>
        /// <param name="pixel2">A color 2.</param>
        /// <returns>Returns <see cref="bool"/> whether pixels have same RGB values or not.</returns>
        private bool IsRGBSame(Color pixel1, Color pixel2)
        {
            var red = pixel1.R == pixel2.R;
            var green = pixel1.G == pixel2.G;
            var blue = pixel1.B == pixel2.B;
            return red && green && blue;
        }

        /// <summary>
        /// Verifies whether pixels have similar RGB values or not based on tolerance levels.
        /// </summary>
        /// <param name="pixel1">A color 1.</param>
        /// <param name="pixel2">A color 2.</param>
        /// <returns>Returns <see cref="bool"/> whether pixels have similar RGB values or not based on tolerance levels.</returns>
        private bool IsRGBSimilar(Color pixel1, Color pixel2)
        {
            var red = IsColorSimilar(pixel1.R, pixel2.R, Tolerance.Red);
            var green = IsColorSimilar(pixel1.G, pixel2.G, Tolerance.Green);
            var blue = IsColorSimilar(pixel1.B, pixel2.B, Tolerance.Blue);
            var alpha = IsColorSimilar(pixel1.A, pixel2.A, Tolerance.Alpha);

            return red && green && blue && alpha;
        }

        /// <summary>
        /// Verifies color similarity based on tolerance levels.
        /// </summary>
        /// <param name="colorValue1">A color 1.</param>
        /// <param name="colorValue2">A color 2.</param>
        /// <param name="toleranceType">A tolerance</param>
        /// <returns>Returns <see cref="bool"/> whether color is similar or not based on tolerance levels.</returns>
        private bool IsColorSimilar(byte colorValue1, byte colorValue2, Tolerance toleranceType)
        {
            return colorValue1 == colorValue2 || Math.Abs(colorValue1 - colorValue2) < GetTolerance(toleranceType);
        }

        /// <summary>
        /// Verifies whether colors are contrasted to each other.
        /// </summary>
        /// <param name="pixel1">A color 1.</param>
        /// <param name="pixel2">A color 2.</param>
        /// <returns>Returns <see cref="bool"/> whether colors are contrasted to each other.</returns>
        private bool IsContrasting(Color pixel1, Color pixel2)
        {
            return Math.Abs(pixel1.GetBrightness() - pixel2.GetBrightness()) > (float)GetTolerance(Tolerance.MaxBrightness) / 255;
        }

        /// <summary>
        /// Returns func to set color of difference based on chosen type, difference color and pixel colors.
        /// </summary>
        /// <param name="type">A type of difference</param>
        /// <returns>A <see cref="Func{TResult}"/> func that returns a color after execution.</returns>
        private Func<Color, Color, Color> GetColorOfDifference(DifferenceType type)
        {
            var errorPixelTransform = new Dictionary<DifferenceType, Func<Color, Color, Color>>
            {
                [DifferenceType.Flat] = (_, _) =>
                {
                    return _settings.DifferenceColor.Value;
                },

                [DifferenceType.Movement] = (_, d2) =>
                {
                    var r = Math.Round((d2.R * _settings.DifferenceColor.Value.R / 255 + _settings.DifferenceColor.Value.R) / 2.0);
                    var g = Math.Round((d2.G * _settings.DifferenceColor.Value.G / 255 + _settings.DifferenceColor.Value.G) / 2.0);
                    var b = Math.Round((d2.B * _settings.DifferenceColor.Value.B / 255 + _settings.DifferenceColor.Value.B) / 2.0);
                    var a = d2.A;

                    return Color.FromArgb(a, (int)r, (int)g, (int)b);
                },

                [DifferenceType.FlatDifferenceIntensity] = (d1, d2) =>
                {
                    return Color.FromArgb((int)Math.Round(ColorsDistance(d1, d2)), _settings.DifferenceColor.Value.R, _settings.DifferenceColor.Value.G, _settings.DifferenceColor.Value.B);
                },

                [DifferenceType.MovementDifferenceIntensity] = (d1, d2) =>
                {
                    var ratio = (ColorsDistance(d1, d2) / 255) * 0.8;

                    var r = Math.Round((1 - ratio) * (d2.R * (_settings.DifferenceColor.Value.R / 255)) + ratio * _settings.DifferenceColor.Value.R);
                    var g = Math.Round((1 - ratio) * (d2.G * (_settings.DifferenceColor.Value.G / 255)) + ratio * _settings.DifferenceColor.Value.G);
                    var b = Math.Round((1 - ratio) * (d2.B * (_settings.DifferenceColor.Value.B / 255)) + ratio * _settings.DifferenceColor.Value.B);
                    var a = d2.A;

                    return Color.FromArgb(a, (int)r, (int)g, (int)b);
                },

                [DifferenceType.DiffOnly] = (_, d2) =>
                {
                    return Color.FromArgb(d2.R, d2.G, d2.B);
                }
            };

            return errorPixelTransform[type];
        }

        /// <summary>
        /// Gets a tolerance level.
        /// </summary>
        /// <param name="tolerance">A tolerance type.</param>
        /// <returns>A tolerance value for a type</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws an exception if tolerance was not set up by some reasons.</exception>
        private int GetTolerance(Tolerance tolerance)
        {
            if (_tolerance.TryGetValue(tolerance, out var value))
            {
                return value;
            }

            throw new ArgumentOutOfRangeException($"{tolerance} was not set up. Check your settings.");
        }

        /// <summary>
        /// Assert if only 1 ignore option was chosen during set up.
        /// </summary>
        /// <exception cref="IgnoreOptionIsAlreadyAppliedException">Throws an exception when multiple ignore options are applied.</exception>
        private void AssertIgnoreOptionAvailability()
        {
            if (_isAnyIgnoreOptionApplied)
            {
                throw new IgnoreOptionIsAlreadyAppliedException("Ignore option has been already applied. You can select only one per verification.");
            }
        }
    }
}
