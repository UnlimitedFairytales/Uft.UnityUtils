#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Uft.UnityUtils.Common
{
    public static class DirectoryUtil
    {
        /// <summary>GetLatestSampleSourceDirectory (Assets/Samples/...で始まる想定) の指定した内容を Assets/Resources/Samples/... へ コピー</summary>
        public static void CopyLatestSampleToResources(string pathPart1, string pathPart2, bool excludesMetaFile = true)
        {
            var src = GetLatestSampleSourceDirectory(pathPart1, pathPart2);
            if (!Directory.Exists(src)) throw new DirectoryNotFoundException($"Source directory not found: {src}");
            var dst = ToResourcesPath(src);
            CopyDirectory(src, dst, true, true, excludesMetaFile);
        }

        /// <summary>GetLatestSampleSourceDirectory (Assets/Samples/...で始まる想定) の指定した内容を Assets/StreamingAssets/Samples/... へ コピー</summary>
        public static void CopyLatestSampleToStreamingAssets(string pathPart1, string pathPart2, bool excludesMetaFile = true)
        {
            var src = GetLatestSampleSourceDirectory(pathPart1, pathPart2);
            if (!Directory.Exists(src)) throw new DirectoryNotFoundException($"Source directory not found: {src}");
            var dst = ToStreamingAssetsPath(src);
            CopyDirectory(src, dst, true, true, excludesMetaFile);
        }

        /// <summary>pathPart1/x.x.x/pathPart2</summary>
        /// <param name="pathPart1">e.g. Assets/Samples/Uft.UnityUtils</param>
        /// <param name="pathPart2">e.g. ScriptSample/Scripts</param>
        public static string GetLatestSampleSourceDirectory(string pathPart1, string pathPart2)
        {
            var verDirs = EnumerateSubdirectories(pathPart1)
                .ToArray();
            if (verDirs.Length == 0)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {pathPart1}/<version>/{pathPart2}");
            }
            var verDirs2 = verDirs
                .Select(v => new
                {
                    OriginalString = v,
                    Version = ParseVersion(Path.GetFileName(v))
                })
                .Where(x => x.Version != null)
                .OrderByDescending(record => record.Version)
                .ThenByDescending(record => record.OriginalString, StringComparer.Ordinal)
                .ToArray();
            if (verDirs2.Length == 0) throw new DirectoryNotFoundException($"Source directory not found: {pathPart1}/<version>/{pathPart2}");
            var latestDir = verDirs2[0].OriginalString;

            return latestDir + "/" + pathPart2;
        }

        /// <summary>e.g. "1.2.3", "1.2", "v1.2.3"</summary>
        public static Version? ParseVersion(string versionString)
        {
            var m = Regex.Match(versionString, @"^v?(\d+)\.(\d+)(?:\.(\d+))?$");
            if (!m.Success) return null;
            var major = int.Parse(m.Groups[1].Value);
            var minor = int.Parse(m.Groups[2].Value);
            var patch = m.Groups[3].Success ? int.Parse(m.Groups[3].Value) : 0;
            return new Version(major, minor, patch);
        }

        /// <summary>Assets/xxx/... で始まるパスを Assets/Resources/xxx/... に変換する</summary>
        public static string ToResourcesPath(string copySourcePath)
        {
            if (!copySourcePath.StartsWith("Assets/")) throw new ArgumentException(copySourcePath, nameof(copySourcePath));

            return $"Assets/Resources/{copySourcePath["Assets/".Length..]}";
        }

        /// <summary>Assets/xxx/... で始まるパスを Assets/StreamingAssets/xxx/... に変換する</summary>
        public static string ToStreamingAssetsPath(string copySourcePath)
        {
            if (!copySourcePath.StartsWith("Assets/")) throw new ArgumentException(copySourcePath, nameof(copySourcePath));

            return $"Assets/StreamingAssets/{copySourcePath["Assets/".Length..]}";
        }

        public static IEnumerable<string> EnumerateSubdirectories(this string dir)
        {
            return Directory
                .EnumerateDirectories(dir)
                .Select(s => s.Replace(@"\", "/"));
        }

        public static void CopyDirectory(string srcDir, string dstDir, bool recursive, bool overwrite, bool excludesMetaFile = true)
        {
            var dirInfo = new DirectoryInfo(srcDir);
            if (!dirInfo.Exists) throw new DirectoryNotFoundException($"Source directory not found: {srcDir}");

            Directory.CreateDirectory(dstDir);
            foreach (FileInfo fileInfo in dirInfo.EnumerateFiles())
            {
                if (excludesMetaFile && fileInfo.Name.EndsWith(".meta", StringComparison.InvariantCultureIgnoreCase)) continue;
                string dstFile = Path.Combine(dstDir, fileInfo.Name).Replace(@"\", "/");
                fileInfo.CopyTo(dstFile, overwrite);
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirInfo.EnumerateDirectories())
                {
                    string newDstDir = Path.Combine(dstDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDstDir, true, overwrite, excludesMetaFile);
                }
            }
        }
    }
}
