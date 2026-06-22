#nullable enable

namespace Uft.UnityUtils.Save
{
    public interface ISaveConverter<T>
    {
        byte[] Serialize(T data);
        T Deserialize(byte[] buffer);
    }
}
