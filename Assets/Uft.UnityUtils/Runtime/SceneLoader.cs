#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Uft.UnityUtils
{
    public interface ISceneHandle
    {
        string SceneName { get; }
        UniTask SetupAsync(CancellationToken cancellationToken, Scene scene, ISceneSetupParameter? parameter);
        UniTask CleanupAsync(CancellationToken cancellationToken);
    }

    public interface ISceneSetupParameter { }

    public static class SceneLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetState()
        {
            _isLoading = false;
            _current = null;
        }

        static readonly DevLogWithTag DevLog = new("[" + nameof(SceneLoader) + "]");
        static bool _isLoading;
        static ISceneHandle? _current;

        public static async UniTask ChangeSceneAsync<T>(CancellationToken cancellationToken, ISceneSetupParameter? parameter) where T : ISceneHandle, new()
        {
            // NOTE: Unityシーン操作はメインスレッド専用。これで十分
            if (_isLoading) throw new InvalidOperationException("Multiple concurrent loads are not supported.");
            _isLoading = true;

            try
            {
                if (_current != null)
                {
                    var last = _current.SceneName;
                    await _current.CleanupAsync(cancellationToken);
                    DevLog.Log($"CleanupAsync done: {last}");
                    await SceneManager.UnloadSceneAsync(last).WithCancellation(cancellationToken);
                    DevLog.Log($"UnloadSceneAsync done: {last}");
                    _current = null;
                }
                _current = new T();
                var next = _current.SceneName;
                await SceneManager.LoadSceneAsync(next, LoadSceneMode.Additive).WithCancellation(cancellationToken);
                DevLog.Log($"LoadSceneAsync done: {next}");
                var scene = SceneManager.GetSceneByName(next);
                SceneManager.SetActiveScene(scene);
                await _current.SetupAsync(cancellationToken, scene, parameter);
                DevLog.Log($"SetupAsync done: {next}");
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
