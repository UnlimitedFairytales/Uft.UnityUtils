using NUnit.Framework;
using System;
using Uft.UnityUtils.Common;

namespace Uft.UnityUtils.Tests.Editor.Common
{
    internal class ThrowIf_Test
    {
        class Foo
        {
            public string _wrapperProperty = null;
            public string WrapperProperty => ThrowIf.Unassigned(this._wrapperProperty);
        }

        [Test]
        public void Unassigned_Test()
        {
            // Arrange
            // -
            // Act
            // -
            // Assert
            try
            {
                Foo val = new();
                _ = val.WrapperProperty;
                Assert.Fail("Expected InvalidOperationException to be thrown.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsTrue(ex.Message.Contains("_wrapperProperty"));
            }

            Assert.DoesNotThrow(() =>
            {
                Foo val = new()
                {
                    _wrapperProperty = ""
                };
                _ = val.WrapperProperty;
            });
        }

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
