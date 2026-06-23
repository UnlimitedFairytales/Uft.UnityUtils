#nullable enable

using UnityEngine;
using UnityEngine.Profiling;

namespace Uft.UnityUtils
{
    public static class ProfilerUtil
    {
        public static (float fps, string text)? FpsAndMemory(ref float timer, ref int frameCount, float interval)
        {
            timer += Time.unscaledDeltaTime;
            frameCount++;
            if (timer >= interval)
            {
                var fps = frameCount / timer;
                timer -= interval;
                frameCount = 0;

                long totalReserved = Profiler.GetTotalReservedMemoryLong() / (1024 * 1024);
                long totalUsed = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);
                long monoUsed = Profiler.GetMonoUsedSizeLong() / (1024 * 1024);

                var text = $"{fps:F1} FPS" + "\n" +
                    $"Total Reserved: {totalReserved} MB\n" +
                    $"Used: {totalUsed} MB\n" +
                    $"Mono: {monoUsed} MB";
                return (fps, text);
            }
            return null;
        }
    }
}
