using NUnit.Framework;
using System;
using System.Data;
using System.Text.RegularExpressions;
using Uft.UnityUtils.Common;

namespace Uft.UnityUtils.Tests.Editor.Common
{
    internal class Eval_Test
    {
        [Test]
        public void IDENTIFIER_Matchs()
        {
            // Arrange
            // -
            // Act
            var results = Regex.Matches("(var1+1)*2*var2+var3var3", Eval.RegexPattern.IDENTIFIER);
            // Assert
            Assert.AreEqual("var1", results[0].Value);
            Assert.AreEqual("var2", results[1].Value);
            Assert.AreEqual("var3var3", results[2].Value);
        }

        [Test]
        public void EvaluateBooleanOrNull_Test()
        {
            // Arrange
            var eval = new Eval();
            // Act
            // -
            // Assert
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" 1+2*3  = 7"));
            Assert.AreEqual(false, eval.EvaluateBooleanOrNull("1+2*3  = 10"));
            Assert.AreEqual(false, eval.EvaluateBooleanOrNull("1+2*3 <> 7"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" 1+2*3 <> 10"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" 1/2    = 0.5"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" 0.5*3 <= 1.6"));
            Assert.AreEqual(false, eval.EvaluateBooleanOrNull("0.5*3 >= 1.6"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" 8%3    = 2"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" TruE AnD 2=2"));
            Assert.AreEqual(false, eval.EvaluateBooleanOrNull("TruE AnD 1=2"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" TruE OR 2=2"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" TruE OR 1=2"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" TruE AnD NoT 1=2"));
            Assert.AreEqual(false, eval.EvaluateBooleanOrNull("TruE AnD NoT 2=2"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" ((2+3)*4 = 20 AnD 2*3 <> 2+3) OR FalsE"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" 'foobar' LikE 'foo%'"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" 'foobar' IN ('foo', 'bar', 'foobar')"));
            Assert.AreEqual(null, eval.EvaluateBooleanOrNull(" 'foobar' = NulL"));
            Assert.AreEqual(false, eval.EvaluateBooleanOrNull("'foobar' IS NulL"));
            Assert.AreEqual(null, eval.EvaluateBooleanOrNull(" NulL = NulL"));
            Assert.AreEqual(true, eval.EvaluateBooleanOrNull(" NulL IS NulL"));

            Assert.AreEqual(null, eval.EvaluateBooleanOrNull(null));
            Assert.AreEqual(null, eval.EvaluateBooleanOrNull(""));
            Assert.Throws<SyntaxErrorException>(() => eval.EvaluateBooleanOrNull("1+2 *"));
            Assert.Throws<FormatException>(() => eval.EvaluateBooleanOrNull("1+2"));
        }

        [Test]
        public void EvaluateDoubleOrNull_Test()
        {
            // Arrange
            var eval = new Eval();
            // Act
            // -
            // Assert
            Assert.AreEqual(7.0d, eval.EvaluateDoubleOrNull(" 1+2*3"), double.Epsilon);
            Assert.AreEqual(1.5d, eval.EvaluateDoubleOrNull(" 0.5*3"), double.Epsilon);
            Assert.AreEqual(0.5d, eval.EvaluateDoubleOrNull(" 1/2"), double.Epsilon);
            Assert.AreEqual(2.0d, eval.EvaluateDoubleOrNull(" 8%3"), double.Epsilon);

            Assert.AreEqual(null, eval.EvaluateDoubleOrNull(null));
            Assert.AreEqual(null, eval.EvaluateDoubleOrNull(""));
            Assert.Throws<SyntaxErrorException>(() => eval.EvaluateDoubleOrNull("1+2 <>"));
            Assert.Throws<FormatException>(() => eval.EvaluateDoubleOrNull("1+2 = 3"));
        }


        [Test]
        public void IsKeyword_Test()
        {
            // Arrange
            // -
            // Act
            // -
            // Assert
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("TruE"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("FalsE"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("IS"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("NulL"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("AnD"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("OR"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("NoT"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("LikE"));
            Assert.AreEqual(true, Eval.Keyword.IsKeyword("IN"));
            Assert.AreEqual(false, Eval.Keyword.IsKeyword("Foo"));
        }
    }
}
