#nullable enable

namespace Uft.UnityUtils.Common
{
    public interface IDeepCloneable<out T>
    {
        T DeepClone();
    }
}
