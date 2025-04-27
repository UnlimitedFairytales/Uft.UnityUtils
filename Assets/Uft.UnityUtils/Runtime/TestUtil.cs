using NUnit.Framework;

namespace Uft.UnityUtils
{
    public static class TestUtil
    {
        public static void Ignore(string message)
        {
#if UNITY_EDITOR
            Assert.Ignore(message);
#else
            Assert.Inconclusive(message);
#endif
        }
    }
}
