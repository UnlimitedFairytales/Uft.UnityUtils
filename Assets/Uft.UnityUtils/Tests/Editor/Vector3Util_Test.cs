using NUnit.Framework;
using System;
using UnityEngine;

namespace Uft.UnityUtils.Tests.Editor
{
    public class Vector3Util_Test
    {
        static (Vector3, Vector3) Arrange()
        {
            return (new Vector3(1, 2, 3), new Vector3(4, 6, 9));
        }

        // GetDirection

        [Test]
        public void GetDirection01_from_to_Gets3DNormalizedVector()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetDirection(from, to);

            // Assert
            var length = Mathf.Sqrt(3 * 3 + 4 * 4 + 6 * 6);
            Assert.AreEqual((to.x - from.x) / length, result.x, 0.001f);
            Assert.AreEqual((to.y - from.y) / length, result.y, 0.001f);
            Assert.AreEqual((to.z - from.z) / length, result.z, 0.001f);
        }

        [Test]
        public void GetDirection02_from_to_XY_Gets2DNormalizedVector()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetDirection(from, to, CalculationType.XY);

            // Assert
            var length = Mathf.Sqrt(3 * 3 + 4 * 4);
            Assert.AreEqual((to.x - from.x) / length, result.x, 0.001f);
            Assert.AreEqual((to.y - from.y) / length, result.y, 0.001f);
            Assert.AreEqual(0 / length, result.z, 0.001f);
        }

        [Test]
        public void GetDirection03_from_to_YZ_Gets2DNormalizedVector()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetDirection(from, to, CalculationType.YZ);

            // Assert
            var length = Mathf.Sqrt(4 * 4 + 6 * 6);
            Assert.AreEqual(0 / length, result.x, 0.001f);
            Assert.AreEqual((to.y - from.y) / length, result.y, 0.001f);
            Assert.AreEqual((to.z - from.z) / length, result.z, 0.001f);
        }

        [Test]
        public void GetDirection04_from_to_ZX_Gets2DNormalizedVector()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetDirection(from, to, CalculationType.ZX);

            // Assert
            var length = Mathf.Sqrt(3 * 3 + 6 * 6);
            Assert.AreEqual((to.x - from.x) / length, result.x, 0.001f);
            Assert.AreEqual(0 / length, result.y, 0.001f);
            Assert.AreEqual((to.z - from.z) / length, result.z, 0.001f);
        }

        [Test]
        public void GetDirection05_from_to_Normal_false_Gets3DUnnormalizedVector()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetDirection(from, to, CalculationType.Normal, false);

            // Assert
            Assert.AreEqual((to.x - from.x), result.x, 0.001f);
            Assert.AreEqual((to.y - from.y), result.y, 0.001f);
            Assert.AreEqual((to.z - from.z), result.z, 0.001f);
        }

        [Test]
        public void GetDirection06_from_to_XY_false_Gets2DUnnormalizedVector()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetDirection(from, to, CalculationType.XY, false);

            // Assert
            Assert.AreEqual((to.x - from.x), result.x, 0.001f);
            Assert.AreEqual((to.y - from.y), result.y, 0.001f);
            Assert.AreEqual(0, result.z, 0.001f);
        }

        // GetAngle

        [Test]
        public void GetAngle01_from_to_GetsXYDegAngle()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetAngle(from, to);

            // Assert
            Assert.AreEqual(Mathf.Atan2(to.y - from.y, to.x - from.x) * Mathf.Rad2Deg, result, 0.001f);
        }

        [Test]
        public void GetAngle02_from_to_normal_ThrowsException()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            // -

            // Assert
            Assert.Throws<Exception>(() => { Vector3Util.GetAngle(from, to, CalculationType.Normal); });
        }

        [Test]
        public void GetAngle03_from_to_YZ_GetsYZDegAngle()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetAngle(from, to, CalculationType.YZ);

            // Assert
            Assert.AreEqual(Mathf.Atan2(to.z - from.z, to.y - from.y) * Mathf.Rad2Deg, result, 0.001f);
        }

        [Test]
        public void GetAngle04_from_to_ZX_GetsZXDegAngle()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetAngle(from, to, CalculationType.ZX);

            // Assert
            Assert.AreEqual(Mathf.Atan2(to.x - from.x, to.z - from.z) * Mathf.Rad2Deg, result, 0.001f);
        }

        [Test]
        public void GetAngle05_from_to_ZX_true_GetsZXRadAngle()
        {
            // Arrange
            var (from, to) = Arrange();

            // Act
            var result = Vector3Util.GetAngle(from, to, CalculationType.ZX, true);

            // Assert
            Assert.AreEqual(Mathf.Atan2(to.x - from.x, to.z - from.z), result, 0.001f);
        }
    }
}