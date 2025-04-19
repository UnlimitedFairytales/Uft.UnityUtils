using NUnit.Framework;
using Uft.UnityUtils.Common;

namespace Uft.UnityUtils.Tests.Editor.Common
{
    public class NumberExtension_Test
    {
        [Test]
        public void SafeToFloat_18Patterns_BasedOn_floatParse()
        {
            // Arrange
            var input01 = "+13230";
            var input02 = "-0";
            var input03 = "$1,234";
            var input04 = "0xFA1B";
            var input05 = "-10";
            var input06 = "007";
            var input07 = "2147483647";
            var input08 = "2147483648"; //  int.Parse is overflow.
            var input09 = "16e02";
            var input10 = "134985.5"; //    float and double are correct format. Although int is bad format.
            var input11 = "-12034";
            var input12 = "-2147483648";
            var input13 = "-2147483649"; // int.Parse is overflow.
            var input14 = "false";
            var input15 = "true";
            var input16 = (string)null;
            var input17 = "134985.0";
            var input18 = "134,985";

            // Act
            var output01 = NumberExtension.SafeToFloat(input01);
            var output02 = NumberExtension.SafeToFloat(input02);
            var output03 = NumberExtension.SafeToFloat(input03);
            var output04 = NumberExtension.SafeToFloat(input04);
            var output05 = NumberExtension.SafeToFloat(input05);
            var output06 = NumberExtension.SafeToFloat(input06);
            var output07 = NumberExtension.SafeToFloat(input07);
            var output08 = NumberExtension.SafeToFloat(input08);
            var output09 = NumberExtension.SafeToFloat(input09);
            var output10 = NumberExtension.SafeToFloat(input10);
            var output11 = NumberExtension.SafeToFloat(input11);
            var output12 = NumberExtension.SafeToFloat(input12);
            var output13 = NumberExtension.SafeToFloat(input13);
            var output14 = NumberExtension.SafeToFloat(input14);
            var output15 = NumberExtension.SafeToFloat(input15);
            var output16 = NumberExtension.SafeToFloat(input16);
            var output17 = NumberExtension.SafeToFloat(input17);
            var output18 = NumberExtension.SafeToFloat(input18);

            // Assert
            Assert.AreEqual(13230, output01, 0.001);
            Assert.AreEqual(0, output02, 0.001);
            Assert.AreEqual(0, output03, 0.001);
            Assert.AreEqual(0, output04, 0.001);
            Assert.AreEqual(-10, output05, 0.001);
            Assert.AreEqual(7, output06, 0.001);
            Assert.AreEqual(2147483647, output07, 1); // float 16,777,216
            Assert.AreEqual(2147483648, output08, 1); // float 16,777,216
            Assert.AreEqual(0, output09, 0.001);
            Assert.AreEqual(134985.5, output10, 0.001);
            Assert.AreEqual(-12034, output11, 0.001);
            Assert.AreEqual(-2147483648, output12, 1); // float -16,777,216
            Assert.AreEqual(-2147483649, output13, 1); // float -16,777,216
            Assert.AreEqual(0, output14, 0.001);
            Assert.AreEqual(0, output15, 0.001);
            Assert.AreEqual(0, output16, 0.001);
            Assert.AreEqual(134985, output17, 0.001);
            Assert.AreEqual(134985, output18, 0.001);
        }

        [Test]
        public void SafeToInt_18Patterns_BasedOn_intParse()
        {
            // Arrange
            var input01 = "+13230";
            var input02 = "-0";
            var input03 = "$1,234";
            var input04 = "0xFA1B";
            var input05 = "-10";
            var input06 = "007";
            var input07 = "2147483647";
            var input08 = "2147483648"; //  int.Parse is overflow.
            var input09 = "16e02";
            var input10 = "134985.5"; //    float and double are correct format. Although int is bad format.
            var input11 = "-12034";
            var input12 = "-2147483648";
            var input13 = "-2147483649"; // int.Parse is overflow.
            var input14 = "false";
            var input15 = "true";
            var input16 = (string)null;
            var input17 = "134985.0";
            var input18 = "134,985";

            // Act
            var output01 = NumberExtension.SafeToInt(input01);
            var output02 = NumberExtension.SafeToInt(input02);
            var output03 = NumberExtension.SafeToInt(input03);
            var output04 = NumberExtension.SafeToInt(input04);
            var output05 = NumberExtension.SafeToInt(input05);
            var output06 = NumberExtension.SafeToInt(input06);
            var output07 = NumberExtension.SafeToInt(input07);
            var output08 = NumberExtension.SafeToInt(input08);
            var output09 = NumberExtension.SafeToInt(input09);
            var output10 = NumberExtension.SafeToInt(input10);
            var output11 = NumberExtension.SafeToInt(input11);
            var output12 = NumberExtension.SafeToInt(input12);
            var output13 = NumberExtension.SafeToInt(input13);
            var output14 = NumberExtension.SafeToInt(input14);
            var output15 = NumberExtension.SafeToInt(input15);
            var output16 = NumberExtension.SafeToInt(input16);
            var output17 = NumberExtension.SafeToFloat(input17);
            var output18 = NumberExtension.SafeToFloat(input18);

            // Assert
            Assert.AreEqual(13230, output01);
            Assert.AreEqual(0, output02);
            Assert.AreEqual(0, output03);
            Assert.AreEqual(0, output04);
            Assert.AreEqual(-10, output05);
            Assert.AreEqual(7, output06);
            Assert.AreEqual(2147483647, output07);
            Assert.AreEqual(0, output08);
            Assert.AreEqual(0, output09);
            Assert.AreEqual(0, output10);
            Assert.AreEqual(-12034, output11);
            Assert.AreEqual(-2147483648, output12, 0.001);
            Assert.AreEqual(0, output13, 0.001);
            Assert.AreEqual(0, output14, 0.001);
            Assert.AreEqual(0, output15, 0.001);
            Assert.AreEqual(0, output16, 0.001);
            Assert.AreEqual(134985, output17, 0.001);
            Assert.AreEqual(134985, output18, 0.001);
        }

        [Test]
        public static void Clamp_float_Test()
        {
            // Arrange
            var val1 = -1.0f;
            var val2 = -0.1f;
            var val3 = +0.0f;
            var val4 = +0.1f;
            var val5 = +9.9f;
            var val6 = +10f;
            var val7 = +10.1f;
            var val8 = +11.0f;

            // Act
            var res1 = val1.Clamp(0, 10);
            var res2 = val2.Clamp(0, 10);
            var res3 = val3.Clamp(0, 10);
            var res4 = val4.Clamp(0, 10);
            var res5 = val5.Clamp(0, 10);
            var res6 = val6.Clamp(0, 10);
            var res7 = val7.Clamp(0, 10);
            var res8 = val8.Clamp(0, 10);

            // Assert
            Assert.AreEqual(0, res1);
            Assert.AreEqual(0, res2);
            Assert.AreEqual(0, res3);
            Assert.AreEqual(0.1f, res4);
            Assert.AreEqual(9.9f, res5);
            Assert.AreEqual(10f, res6);
            Assert.AreEqual(10f, res7);
            Assert.AreEqual(10f, res8);
        }

        [Test]
        public static void Clamp_int_Test()
        {
            // Arrange
            var val1 = -1;
            var val2 = +0;
            var val3 = +9;
            var val4 = +10;
            var val5 = +11;

            // Act
            var res1 = val1.Clamp(0, 10);
            var res2 = val2.Clamp(0, 10);
            var res3 = val3.Clamp(0, 10);
            var res4 = val4.Clamp(0, 10);
            var res5 = val5.Clamp(0, 10);

            // Assert
            Assert.AreEqual(0, res1);
            Assert.AreEqual(0, res2);
            Assert.AreEqual(9, res3);
            Assert.AreEqual(10, res4);
            Assert.AreEqual(10, res5);
        }

        [Test]
        public static void Is0fEpsilon_float_Test()
        {
            // Arrange
            var val1 = -0.00000012f;
            var val2 = +0.00000012f;
            var val3 = -0.00000011f;
            var val4 = +0.00000011f;

            // Act
            var res1 = val1.Is0fEpsilon();
            var res2 = val2.Is0fEpsilon();
            var res3 = val3.Is0fEpsilon();
            var res4 = val4.Is0fEpsilon();

            // Assert
            Assert.AreEqual(false, res1); // 1.1920929e-7
            Assert.AreEqual(false, res2); // 1.1920929e-7
            Assert.AreEqual(true, res3); // 1.1920929e-7
            Assert.AreEqual(true, res4); // 1.1920929e-7
        }

        [Test]
        public static void IsEven__Test()
        {
            // Arrange
            var val1 = -2;
            var val2 = -1;
            var val3 = -0;
            var val4 = +1;
            var val5 = +2;

            // Act
            var res1 = val1.IsEven();
            var res2 = val2.IsEven();
            var res3 = val3.IsEven();
            var res4 = val4.IsEven();
            var res5 = val5.IsEven();

            // Assert
            Assert.AreEqual(true, res1);
            Assert.AreEqual(false, res2);
            Assert.AreEqual(true, res3);
            Assert.AreEqual(false, res4);
            Assert.AreEqual(true, res5);
        }
    }
}