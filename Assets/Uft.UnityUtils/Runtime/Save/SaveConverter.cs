#nullable enable

using System;
using System.Text;
using UnityEngine;

namespace Uft.UnityUtils.Save
{
    /// <summary><see cref="JsonUtility"/> 経由コンバータ</summary>
    /// <remarks>
    /// 1. 速度が気になる場合は、MemoryPack や MessagePack の導入を検討してください<br/>
    /// 2. <see cref="JsonUtility"/> は、<see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> をシリアライズできません<br/>
    /// 3. <see cref="JsonUtility"/> は、プロパティ をシリアライズできません
    /// </remarks>
    public class SaveConverter<T> : ISaveConverter<T>
    {
        const int LENGTH_PREFIX_SIZE = sizeof(int);

        readonly int fixedSize;

        /// <param name="fixedSize">出力するbyte[]の固定長(予約領域サイズ)</param>
        public SaveConverter(int fixedSize)
        {
            this.fixedSize = fixedSize;
        }

        public byte[] Serialize(T data)
        {
            var json = JsonUtility.ToJson(data);
            var payload = Encoding.UTF8.GetBytes(json);
            if (payload.Length + LENGTH_PREFIX_SIZE > this.fixedSize)
            {
                throw new InvalidOperationException(
                    $"Serialized data ({payload.Length + LENGTH_PREFIX_SIZE} bytes) exceeds fixed size ({this.fixedSize} bytes).");
            }

            // NOTE: 先頭 LENGTH_PREFIX_SIZE へ 正味のlengthを書き込んでから、続けて正味を書き込む
            var buffer = new byte[this.fixedSize];
            Array.Copy(BitConverter.GetBytes(payload.Length), 0, buffer, 0, LENGTH_PREFIX_SIZE);
            payload.CopyTo(buffer, LENGTH_PREFIX_SIZE);
            return buffer;
        }

        public T Deserialize(byte[] buffer)
        {
            // NOTE: 先頭 LENGTH_PREFIX_SIZE が 正味のlengthを返す
            var length = BitConverter.ToInt32(buffer, 0);
            var json = Encoding.UTF8.GetString(buffer, LENGTH_PREFIX_SIZE, length);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
