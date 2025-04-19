using NUnit.Framework;
using System;
using Uft.UnityUtils.Common;

namespace Uft.UnityUtils.Tests.Editor.Common
{
    public class Reflec_Test
    {
        class DummyClass
        {
#pragma warning disable IDE0052 // 読み取られていないプライベート メンバーを削除
#pragma warning disable IDE0044 // 読み取り専用修飾子を追加します
            string privateField;
#pragma warning restore IDE0044 // 読み取り専用修飾子を追加します
#pragma warning restore IDE0052 // 読み取られていないプライベート メンバーを削除
            string _privateProperty;
#pragma warning disable IDE0052 // 読み取られていないプライベート メンバーを削除
            string PrivateProperty { get { return this._privateProperty; } set { this._privateProperty = value; } }
#pragma warning restore IDE0052 // 読み取られていないプライベート メンバーを削除
            public string AsymmetricProperty { get; private set; }

            public DummyClass(string privateFieldValue, string privatePropertyValue, string asymmetricPropertyValue)
            {
                this.privateField = privateFieldValue;
                this.PrivateProperty = privatePropertyValue;
                this.AsymmetricProperty = asymmetricPropertyValue;
            }
        }

        // GetProperty

        [Test]
        public void GetProperty_CorrectMemberName_GetValueOfProperty()
        {
            // Arrange
            var value = new DummyClass("foo", "bar", "baz");
            // Act
            var result = Reflec.GetProperty(value, "PrivateProperty");
            // Assert
            Assert.AreEqual("bar", result);
        }

        [Test]
        public void GetProperty_InvalidMemberName_ThrowsNullReferenceException()
        {
            // Arrange
            var value = new DummyClass("foo", "bar", "baz");
            // Act
            // Assert
            Assert.Throws<NullReferenceException>(() => Reflec.GetProperty(value, "UnknownProperty"));
        }

        // -

        [Test]
        public void SetProperty_CorrectValue_SetValueToProperty()
        {
            // Arrange
            var value = new DummyClass("foo", "bar", "baz");
            // Act
            var result1 = value.AsymmetricProperty;
            Reflec.SetProperty(value, () => value.AsymmetricProperty, "qux");
            var result2 = value.AsymmetricProperty;
            // Assert
            Assert.AreEqual("baz", result1);
            Assert.AreEqual("qux", result2);
        }

        // GetField

        [Test]
        public void GetField_CorrectMemberName_GetValueOfField()
        {
            // Arrange
            var value = new DummyClass("foo", "bar", "baz");
            // Act
            var result = Reflec.GetField(value, "privateField");
            // Assert
            Assert.AreEqual("foo", result);
        }

        [Test]
        public void GetField_InvalidMemberName_ThrowsNullReferenceException()
        {
            // Arrange
            var value = new DummyClass("foo", "bar", "baz");
            // Act
            // Assert
            Assert.Throws<NullReferenceException>(() => Reflec.GetField(value, "UnknownProperty"));
        }

        // -

        [Test]
        public void SetField_CorrectValue_SetValueToField()
        {
            // Arrange
            var value = new DummyClass("foo", "bar", "baz");
            // Act
            var result1 = Reflec.GetField(value, "privateField");
            Reflec.SetField(value, "privateField", "qux");
            var result2 = Reflec.GetField(value, "privateField");
            // Assert
            Assert.AreEqual("foo", result1);
            Assert.AreEqual("qux", result2);
        }

        [Test]
        public void ToFieldName_FromCamel_ToLowerCamelWithUnderBarPrefix()
        {
            // Arrange
            var input1 = "fooBarBaz";
            var input2 = "FooBarBaz";
            var input3 = "FOO_BAR_BAZ";
            var input4 = "_foo_bar_baz";
            // Act
            var output1 = Reflec.ToFieldName(input1);
            var output2 = Reflec.ToFieldName(input2);
            var output3 = Reflec.ToFieldName(input3);
            var output4 = Reflec.ToFieldName(input4);
            // Assert
            Assert.AreEqual("_fooBarBaz", output1);
            Assert.AreEqual("_fooBarBaz", output2);
            Assert.AreEqual("_fOO_BAR_BAZ", output3);
            Assert.AreEqual("__foo_bar_baz", output4);
        }

        // UNDONE:テストコード
        [Test]
        public void SetFieldForProperty_TestNotImplemented()
        {
            Assert.Ignore("Unimplemented test.");
        }

        [Test]
        public void InvokePrivateMethod_TestNotImplemented()
        {
            Assert.Ignore("Unimplemented test.");
        }

        [Test]
        public void GetInstanceFieldByType_TestNotImplemented()
        {
            Assert.Ignore("Unimplemented test.");
        }

        [Test]
        public void GetInstanceFieldInfosFromType_TestNotImplemented()
        {
            Assert.Ignore("Unimplemented test.");
        }

        [Test]
        public void GetTypeFromName_TestNotImplemented()
        {
            Assert.Ignore("Unimplemented test.");
        }

        [Test]
        public void GetAllTypes_TestNotImplemented()
        {
            Assert.Ignore("Unimplemented test.");
        }
    }
}