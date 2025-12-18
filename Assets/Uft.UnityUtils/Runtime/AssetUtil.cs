// #define UNITY_ANDROID
// #undef UNITY_EDITOR

#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.IO;
using UnityEngine;

namespace Uft.UnityUtils
{
    public static class AssetUtil
    {
        const string NAME = "[" + nameof(AssetUtil) + "]";

        static bool _defaultModeIsResources;
        public static bool DefaultModeIsResources
        {
            get => _defaultModeIsResources;
            set
            {
                _defaultModeIsResources = value;
                DevLog.LogWarning($"{NAME} {nameof(DefaultModeIsResources)} = {value}");
            }
        }

        /// <summary>このメソッドは、Android/WebGL の StreamingAssets に対して使用できません。非同期版を使用してください。</summary>
        public static string LoadText(string relativePath, bool? isResources = null)
        {
            isResources ??= DefaultModeIsResources;

            var supportsSync =
                (Application.platform != RuntimePlatform.Android) &&
                (Application.platform != RuntimePlatform.WebGLPlayer);
            if (!supportsSync && !isResources.Value) throw new NotSupportedException($"{Application.platform} + StreamingAssets is not supported for sync");

            if (isResources.Value)
            {
                var path = FixPath(relativePath);
                var asset = Resources.Load<TextAsset>(path);
                if (asset == null) throw new InvalidOperationException($"[Resources] Failed to load: {path}");
                return asset.text;
            }
            else
            {
                var path = Path.Combine(Application.streamingAssetsPath, relativePath).Replace(@"\", "/");
                try
                {
                    return File.ReadAllText(path);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"[StreamingAssets] Failed to load: {path}", ex);
                }
            }
        }

        public static async UniTask<string> LoadTextAsync(string relativePath, bool? isResources = null)
        {
            isResources ??= DefaultModeIsResources;

            if (isResources.Value)
            {
                var path = FixPath(relativePath);
                var asyncOperation = Resources.LoadAsync<TextAsset>(path);
                await asyncOperation;
                var asset = asyncOperation.asset as TextAsset;
                if (asset == null) throw new InvalidOperationException($"[Resources] Failed to load: {path}");
                return asset.text;
            }
            else
            {
                var path = Path.Combine(Application.streamingAssetsPath, relativePath).Replace(@"\", "/");
#if (UNITY_ANDROID && !UNITY_EDITOR) || (UNITY_WEBGL && !UNITY_EDITOR)
                try
                {
                    using var request = UnityEngine.Networking.UnityWebRequest.Get(path);
                    await request.SendWebRequest();
                    if (request.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
                    {
                        throw new Exception($"responseCode={request.responseCode}, error={request.error}");
                    }
                    return request.downloadHandler.text;
                }
#else
                try
                {
                    return await File.ReadAllTextAsync(path);
                }
#endif
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"[StreamingAssets] Failed to load: {path}", ex);
                }
            }
        }

        static string FixPath(string relativePath)
        {
            var dirName = Path.GetDirectoryName(relativePath) ?? "";
            var fileName = Path.GetFileNameWithoutExtension(relativePath);
            var path = Path.Combine(dirName, fileName).Replace(@"\", "/");
            return path;
        }
    }
}
