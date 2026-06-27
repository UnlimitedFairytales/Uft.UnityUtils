#nullable enable

using System;
using System.IO;
using UnityEngine;

namespace Uft.UnityUtils.Asset
{
    public class AssetLoadProxy
    {
        public static string TrimExtension(string path)
        {
            var dirName = Path.GetDirectoryName(path) ?? "";
            var fileName = Path.GetFileNameWithoutExtension(path);
            return Path.Combine(dirName, fileName).Replace(@"\", "/");
        }

        readonly Func<string, Type, UnityEngine.Object?>? _externalLoader;

        public AssetLoadProxy(Func<string, Type, UnityEngine.Object?>? externalLoader) => this._externalLoader = externalLoader;

        /// <summary>pathは拡張子付きで渡してOK。フォールバック時に <see cref="Resources.Load"/> する場合は除去される。見つからない場合は例外。</summary>
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            if (this._externalLoader != null)
            {
                var asset = (T?)this._externalLoader(path, typeof(T));
                // NOTE: where制約 を認識しないため、抑制
#pragma warning disable IDE0270 // coalesce 式を使用します
                if (asset == null) throw new InvalidOperationException($"(External) Failed to load: {path}");
#pragma warning restore IDE0270 // coalesce 式を使用します
                return asset;
            }
            else
            {
                var trimmedPath = TrimExtension(path);
                var asset = Resources.Load<T>(trimmedPath);
                // NOTE: where制約 を認識しないため、抑制
#pragma warning disable IDE0270 // coalesce 式を使用します
                if (asset == null) throw new InvalidOperationException($"(Resources) Failed to load: {trimmedPath}");
#pragma warning restore IDE0270 // coalesce 式を使用します
                return asset;
            }
        }
    }
}
