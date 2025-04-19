using Uft.UnityUtils.Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Uft.UnityUtils.Tests.Editor.Common
{
    public class DictionaryExtension_Test
    {
        // ToDictionary

        [Test]
        public void ToDictionary01_null_null_IsLength0()
        {
            // Arrange
            // -

            // Act
            var nullAndNull = DictionaryExtension.ToDictionary<int, int>(null, null);

            // Assert
            Assert.AreEqual(0, nullAndNull.Count);
        }

        [Test]
        public void ToDictionary02_keys_null_IsLength3()
        {
            // Arrange
            var keys = new List<int>(new int[] { 10, 20, 30 });

            // Act
            var keysAndNull = DictionaryExtension.ToDictionary<int, int>(keys, null);

            // Assert
            Assert.AreEqual(3, keysAndNull.Count);
        }

        [Test]
        public void ToDictionary03_null_vals_IsLength0()
        {
            // Arrange
            var vals = new List<int>(new int[] { 10, 20, 30 });

            // Act
            var nullAndVals = DictionaryExtension.ToDictionary<int, int>(null, vals);

            // Assert
            Assert.AreEqual(0, nullAndVals.Count);
        }

        [Test]
        public void ToDictionary04_3keys_4vals_IsLength3()
        {
            // Arrange
            var keys = new List<int>(new int[] { 10, 20, 30 });
            var vals = new List<int>(new int[] { 10, 20, 30, 40 });

            // Act
            var keysAndVals = DictionaryExtension.ToDictionary<int, int>(keys, vals);

            // Assert
            Assert.AreEqual(3, keysAndVals.Count);
        }

        [Test]
        public void ToDictionary05_4keys_3vals_IsLength4()
        {
            // Arrange
            var keys = new List<int>(new int[] { 10, 20, 30, 40 });
            var vals = new List<int>(new int[] { 10, 20, 30 });

            // Act
            var keysAndVals = DictionaryExtension.ToDictionary<int, int>(keys, vals);

            // Assert
            Assert.AreEqual(4, keysAndVals.Count);
        }

        [Test]
        public void ToDictionary06_duplicatedKeys_ThrowsArgumentException()
        {
            // Arrange
            var keys = new List<int>(new int[] { 10, 20, 30, 40, 10 });
            var vals = new List<int>(new int[] { 10, 20, 30 });

            // Act
            // -

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                DictionaryExtension.ToDictionary<int, int>(keys, vals);
            });
        }

        [Test]
        public void ToDictionary07_duplicatedVals_IsOK()
        {
            // Arrange
            var keys = new List<int>(new int[] { 10, 20, 30 });
            var vals = new List<int>(new int[] { 10, 10, 10 });

            // Act
            // -

            // Assert
            Assert.DoesNotThrow(() =>
            {
                DictionaryExtension.ToDictionary<int, int>(keys, vals);
            });
        }

        [Test]
        public void ToDictionary08_stringList_IsOK()
        {
            // Arrange
            var keys = new List<string>(new string[] { "10", "20", "30", "40" });
            var vals = new List<string>(new string[] { "11", "22", "33" });

            // Act
            var keysAndVals = DictionaryExtension.ToDictionary<string, string>(keys, vals);

            // Assert
            Assert.AreEqual("11", keysAndVals["10"]);
            Assert.AreEqual("33", keysAndVals["30"]);
            Assert.AreEqual(null, keysAndVals["40"]);
        }

        // ToListPair

        [Test]
        public void ToListPair01_null_IsLength0()
        {
            // Arrange
            // -

            // Act
            var keysAndVals = DictionaryExtension.ToListPair<string, int>(null);

            // Assert
            Assert.AreEqual(0, keysAndVals.Key.Count);
            Assert.AreEqual(0, keysAndVals.Value.Count);
        }

        [Test]
        public void ToListPair02_3dictionary_Is3ListPair()
        {
            // Arrange
            var dic = new Dictionary<string, int>
            {
                { "1st", 10 },
                { "2nd", 20 },
                { "3rd", 30 }
            };

            // Act
            var keysAndVals = DictionaryExtension.ToListPair<string, int>(dic);

            // Assert
            Assert.AreEqual(3, keysAndVals.Key.Count);
            Assert.AreEqual("1st", keysAndVals.Key[0]);
            Assert.AreEqual("3rd", keysAndVals.Key[2]);
            Assert.AreEqual(3, keysAndVals.Value.Count);
            Assert.AreEqual(10, keysAndVals.Value[0]);
            Assert.AreEqual(30, keysAndVals.Value[2]);
        }

        // AddOrSet

        [Test]
        public void AddOrSet01_null_int_int_ThrowsNullReferenceException()
        {
            // Arrange
            // -

            // Act
            // -

            // Assert
            Assert.Throws<NullReferenceException>(() => DictionaryExtension.AddOrSet(null, 1, 10));
        }

        [Test]
        public void AddOrSet02_dictionary_int_int_IsOK()
        {
            // Arrange
            var dic = new Dictionary<int, int>
            {
                { 1, 10 },
                { 2, 20 },
                { 3, 30 }
            };

            // Act
            DictionaryExtension.AddOrSet(dic, 4, 40);

            // Assert
            Assert.AreEqual(4, dic.Count);
            Assert.AreEqual(10, dic[1]);
            Assert.AreEqual(20, dic[2]);
            Assert.AreEqual(30, dic[3]);
            Assert.AreEqual(40, dic[4]);
        }

        [Test]
        public void AddOrSet03_emptyDictionary_int_int_IsOK()
        {
            // Arrange
            var dic = new Dictionary<int, int>();

            // Act
            DictionaryExtension.AddOrSet(dic, 2, 10);

            // Assert
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(10, dic[2]);
        }

        [Test]
        public void AddOrSet04_dictionary_int_int_IsOKForSet()
        {
            // Arrange
            var dic = new Dictionary<int, int>();

            // Act
            DictionaryExtension.AddOrSet(dic, 2, 10);
            DictionaryExtension.AddOrSet(dic, 2, 20);

            // Assert
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(20, dic[2]);
        }

        [Test]
        public void AddOrSet05_dictionary_string_string_IsOK()
        {
            // Arrange
            var dic = new Dictionary<string, string>
            {
                { "1st", "apple" },
                { "2nd", "banana" },
                { "3rd", "cherry" }
            };

            // Act
            DictionaryExtension.AddOrSet(dic, "4th", "durian");

            // Assert
            Assert.AreEqual(4, dic.Count);
            Assert.AreEqual("apple", dic["1st"]);
            Assert.AreEqual("banana", dic["2nd"]);
            Assert.AreEqual("cherry", dic["3rd"]);
            Assert.AreEqual("durian", dic["4th"]);
        }

        [Test]
        public void AddOrSet06_emptyDictionary_string_string_IsOK()
        {
            // Arrange
            var dic = new Dictionary<string, string>();

            // Act
            DictionaryExtension.AddOrSet(dic, "1st", "apple");

            // Assert
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual("apple", dic["1st"]);
        }

        [Test]
        public void AddOrSet07_dictionary_string_string_IsOKForSet()
        {
            // Arrange
            var dic = new Dictionary<string, string>();

            // Act
            DictionaryExtension.AddOrSet(dic, "1st", "apple");
            DictionaryExtension.AddOrSet(dic, "1st", "antelope");

            // Assert
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual("antelope", dic["1st"]);
        }

        // ContainsAndEquals

        [Test]
        public void ContainsAndEquals01_null_int_int_ThrowsNullReferenceException()
        {
            // Arrange
            // -
            // Act
            // -
            // Assert
            Assert.Throws<NullReferenceException>(() => DictionaryExtension.ContainsAndEquals(null, 1, 10));
        }

        [Test]
        public void ContainsAndEquals02_dictionary_int_int_ExistingKeyAndEqualingValues_IsOK()
        {
            // Arrange
            var dic = new Dictionary<int, int>
            {
                { 1, 10 },
                { 2, 20 },
                { 3, 30 }
            };

            // Act
            // -

            // Assert
            Assert.AreEqual(3, dic.Count);
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, 1, 10));
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, 2, 20));
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, 3, 30));
        }

        [Test]
        public void ContainsAndEquals03_dictionary_int_int_NotExistingKey_IsFalse()
        {
            // Arrange
            var dic = new Dictionary<int, int>();

            // Act
            // -

            // Assert
            Assert.AreEqual(0, dic.Count);
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, 1, 0));
        }

        [Test]
        public void ContainsAndEquals04_dictionary_int_int_ExistingKeyButNotEqualingValues_IsFalse()
        {
            // Arrange
            var dic = new Dictionary<int, int>
            {
                { 1, 10 }
            };

            // Act
            // -

            // Assert
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, 1, 0));
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, 1, 10));
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, 1, 11));
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, 0, 10));
        }

        [Test]
        public void ContainsAndEquals05_dictionary_string_string_ExistingKeyAndEqualingValues_IsOK()
        {
            // Arrange
            var dic = new Dictionary<string, string>
            {
                { "1st", "apple" },
                { "2nd", "banana" },
                { "3rd", "cherry" }
            };

            // Act
            // -

            // Assert
            Assert.AreEqual(3, dic.Count);
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, "1st", "apple"));
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, "2nd", "banana"));
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, "3rd", "cherry"));
        }

        [Test]
        public void ContainsAndEquals06_dictionary_string_string_NotExistingKey_IsFalse()
        {
            // Arrange
            var dic = new Dictionary<string, string>();

            // Act
            // -

            // Assert
            Assert.AreEqual(0, dic.Count);
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, "1st", null));
        }

        [Test]
        public void ContainsAndEquals07_dictionary_string_string_ExistingKeyButNotEqualingValues_IsFalse()
        {
            // Arrange
            var dic = new Dictionary<string, string>
            {
                { "1st", "apple" }
            };

            // Act
            // -

            // Assert
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, "1st", null));
            Assert.AreEqual(true, DictionaryExtension.ContainsAndEquals(dic, "1st", "apple"));
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, "1st", "APPLE"));
            Assert.AreEqual(false, DictionaryExtension.ContainsAndEquals(dic, "0", "ZERO"));
        }

        [Test]
        public void ContainsAndEquals08_dictionary_null_string_ThrowsArgumentNullException()
        {
            // Arrange
            var dic = new Dictionary<string, string>
            {
                { "1st", "apple" }
            };

            // Act
            // -

            // Assert
            Assert.Throws<ArgumentNullException>(() => DictionaryExtension.ContainsAndEquals(dic, null, "apple"));
        }
    }
}