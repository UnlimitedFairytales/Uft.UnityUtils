#if !DISABLE_UNITY_UTILS_SCRIPTED_IMPORTER
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Uft.UnityUtils.Editor
{
    [ScriptedImporter(1, new[] { "jsonl", "toml" })]
    public class CustomTextImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string content = File.ReadAllText(ctx.assetPath);
            var textAsset = new TextAsset(content);
            ctx.AddObjectToAsset("text", textAsset);
            ctx.SetMainObject(textAsset);
        }
    }
}
#endif
