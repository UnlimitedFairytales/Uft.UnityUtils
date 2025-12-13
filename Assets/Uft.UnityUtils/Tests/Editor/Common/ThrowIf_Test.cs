using NUnit.Framework;
using System;
using Uft.UnityUtils.Common;

namespace Uft.UnityUtils.Tests.Editor.Common
{
    internal class ThrowIf_Test
    {
        [Test]
        public void NullOrEmpty_Test1()
        {
            // Arrange
            string strVal = null;
            // Act
            // -
            // Assert
            Assert.Throws<ArgumentNullException>(() => { ThrowIf.NullOrEmpty(strVal, "foo"); });
            Assert.Throws<ArgumentException>(() => { ThrowIf.NullOrEmpty("", "foo"); });
            Assert.DoesNotThrow(() => { ThrowIf.NullOrEmpty(" ", "foo"); });
        }

        [Test]
        public void NullOrEmpty_Test2()
        {
            // Arrange
            int[] valList = null;
            // Act
            // -
            // Assert
            Assert.Throws<ArgumentNullException>(() => { ThrowIf.NullOrEmpty(valList, "foo"); });
            Assert.Throws<ArgumentException>(() => { ThrowIf.NullOrEmpty(new int[0], "foo"); });
            Assert.DoesNotThrow(() => { ThrowIf.NullOrEmpty(new int[1], "foo"); });
        }
    }
}
