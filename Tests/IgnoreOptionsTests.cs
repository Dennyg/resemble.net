using ResembleNet;
using ResembleNet.Exceptions;
using System.Drawing;
using Xunit;

namespace Tests
{
    public class IgnoreOptionsTests
    {
        [Fact]
        public void IgnoreAntialiasing_ShouldBeApplied()
        {
            var actualImage = Image.FromFile(@".\Examples\text.png");
            var baseline = Image.FromFile(@".\Examples\textAa.png");

            var result = new Resemble(actualImage)
                .CompareTo(baseline)
                .IgnoreAntialiasing()
                .Compare();

            Assert.Equal("0.00", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void IgnoreAntialiasing_IsOff()
        {
            var actualImage = Image.FromFile(@".\Examples\text.png");
            var baseline = Image.FromFile(@".\Examples\textAa.png");

            var result = new Resemble(actualImage)
                .CompareTo(baseline)
                .Compare();

            Assert.Equal("5.19", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void ShouldThrowExceptionIfAnyIgnoreOptionApplied_Antialiasing()
        {
            var result = new Resemble(new Bitmap(1, 1))
                .CompareTo(new Bitmap(1, 1))
                .IgnoreNothing();

            Assert.Throws<IgnoreOptionIsAlreadyAppliedException>(() => result.IgnoreAntialiasing());
        }

        [Fact]
        public void IgnoreNothing_ShouldBeApplied()
        {
            var actualImage = Image.FromFile(@".\Examples\People.jpg");
            var baseline = Image.FromFile(@".\Examples\People2.jpg");

            var result = new Resemble(actualImage)
                .CompareTo(baseline)
                .IgnoreNothing()
                .Compare();

            Assert.Equal(499, result.DiffBounds.Bottom);
            Assert.Equal(0, result.DiffBounds.Left);
            Assert.Equal(499, result.DiffBounds.Right);
            Assert.Equal(0, result.DiffBounds.Top);

            Assert.Equal(0, result.DimensionDifference.Height);
            Assert.Equal(0, result.DimensionDifference.Width);
            Assert.True(result.IsSameDimensions);

            Assert.Equal("97.27", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void ShouldThrowExceptionIfAnyIgnoreOptionApplied_Nothing()
        {
            var result = new Resemble(new Bitmap(1, 1))
                .CompareTo(new Bitmap(1, 1))
                .IgnoreAntialiasing();

            Assert.Throws<IgnoreOptionIsAlreadyAppliedException>(() => result.IgnoreNothing());
        }

        [Fact]
        public void IgnoreLess_ShouldBeApplied()
        {
            var actualImage = Image.FromFile(@".\Examples\People.jpg");
            var baseline = Image.FromFile(@".\Examples\People2.jpg");

            var result = new Resemble(actualImage)
                .CompareTo(baseline)
                .IgnoreLess()
                .Compare();

            Assert.Equal(431, result.DiffBounds.Bottom);
            Assert.Equal(22, result.DiffBounds.Left);
            Assert.Equal(450, result.DiffBounds.Right);
            Assert.Equal(58, result.DiffBounds.Top);

            Assert.Equal(0, result.DimensionDifference.Height);
            Assert.Equal(0, result.DimensionDifference.Width);
            Assert.True(result.IsSameDimensions);

            Assert.Equal("8.66", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void ShouldThrowExceptionIfAnyIgnoreOptionApplied_Less()
        {
            var result = new Resemble(new Bitmap(1, 1))
                .CompareTo(new Bitmap(1, 1))
                .IgnoreAntialiasing();

            Assert.Throws<IgnoreOptionIsAlreadyAppliedException>(() => result.IgnoreLess());
        }

        [Fact]
        public void IgnoreColors_ShouldBeApplied()
        {
            var actualImage = Image.FromFile(@".\Examples\People.jpg");
            var baseline = Image.FromFile(@".\Examples\People2.jpg");

            var result = new Resemble(actualImage)
                .CompareTo(baseline)
                .IgnoreColors()
                .Compare();

            Assert.Equal(431, result.DiffBounds.Bottom);
            Assert.Equal(22, result.DiffBounds.Left);
            Assert.Equal(449, result.DiffBounds.Right);
            Assert.Equal(58, result.DiffBounds.Top);

            Assert.Equal(0, result.DimensionDifference.Height);
            Assert.Equal(0, result.DimensionDifference.Width);
            Assert.True(result.IsSameDimensions);

            Assert.Equal("1.04", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void ShouldThrowExceptionIfAnyIgnoreOptionApplied_Colors()
        {
            var result = new Resemble(new Bitmap(1, 1))
                .CompareTo(new Bitmap(1, 1))
                .IgnoreNothing();

            Assert.Throws<IgnoreOptionIsAlreadyAppliedException>(() => result.IgnoreColors());
        }

        [Fact]
        public void IgnoreAlpha_ShouldBeApplied()
        {
            var actualImage = Image.FromFile(@".\Examples\People.jpg");
            var baseline = Image.FromFile(@".\Examples\People2.jpg");

            var result = new Resemble(actualImage)
                .CompareTo(baseline)
                .IgnoreAlpha()
                .Compare();

            Assert.Equal(431, result.DiffBounds.Bottom);
            Assert.Equal(22, result.DiffBounds.Left);
            Assert.Equal(450, result.DiffBounds.Right);
            Assert.Equal(58, result.DiffBounds.Top);

            Assert.Equal(0, result.DimensionDifference.Height);
            Assert.Equal(0, result.DimensionDifference.Width);
            Assert.True(result.IsSameDimensions);

            Assert.Equal("8.66", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void ShouldThrowExceptionIfAnyIgnoreOptionApplied_Alpha()
        {
            var result = new Resemble(new Bitmap(1, 1))
                .CompareTo(new Bitmap(1, 1))
                .IgnoreAntialiasing();

            Assert.Throws<IgnoreOptionIsAlreadyAppliedException>(() => result.IgnoreAlpha());
        }

        [Fact]
        public void PartialDiffWithSingleBoundingBox()
        {
            var people = Image.FromFile(@".\Examples\ghost1.png");
            var people2 = Image.FromFile(@".\Examples\ghost2.png");

            var result = new Resemble(people)
                .CompareTo(people2)
                .LimitComparisonAreaTo(new Rectangle(80, 80, 50, 50))
                .Compare();

            Assert.Equal("0.04", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void PartialDiffWithBoundingBoxes()
        {
            var people = Image.FromFile(@".\Examples\text.png");
            var people2 = Image.FromFile(@".\Examples\textAa.png");

            var result = new Resemble(people)
                .CompareTo(people2)
                .LimitComparisonAreaTo(new[]
                {
                    new Rectangle(20, 20, 330, 60),
                    new Rectangle(20, 200, 330, 50),
                })
                .Compare();

            Assert.Equal("3.39", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void PartialDiffWithIgnoredBoxes()
        {
            var people = Image.FromFile(@".\Examples\text.png");
            var people2 = Image.FromFile(@".\Examples\textAa.png");

            var result = new Resemble(people)
                .CompareTo(people2)
                .IgnoreComparisonIn(new[]
                {
                    new Rectangle(20, 20, 330, 60),
                    new Rectangle(20, 200, 330, 50)
                })
                .Compare();

            Assert.Equal("1.80", result.Mismatch.ToString("F2"));
        }

        [Fact]
        public void PartialDiffWithSingleIgnoredBox()
        {
            var people = Image.FromFile(@".\Examples\text.png");
            var people2 = Image.FromFile(@".\Examples\textAa.png");

            var result = new Resemble(people)
                .CompareTo(people2)
                .IgnoreComparisonIn(new Rectangle(20, 20, 330, 60))
                .Compare();

            Assert.Equal("3.52", result.Mismatch.ToString("F2"));
        }


        [Fact]
        public void PartialDiffWithIgnoredColor()
        {
            var actual = Image.FromFile(@".\Examples\People2.jpg");
            var baseline = Image.FromFile(@".\Examples\PeopleWithIgnoreMask.png");

            var result = new Resemble(actual)
                .CompareTo(baseline)
                .IgnoreAreasWithColor(Color.FromArgb(255, 255, 0, 0))
                .Compare();

            Assert.Equal("5.49", result.Mismatch.ToString("F2"));
        }
    }
}
