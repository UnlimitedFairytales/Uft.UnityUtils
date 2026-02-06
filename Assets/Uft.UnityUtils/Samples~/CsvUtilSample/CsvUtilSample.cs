using System;
using System.IO;
using TMPro;
using Uft.UnityUtils.Common;
using Uft.UnityUtils.Csv;
using UnityEngine;

namespace Uft.UnityUtils.Samples.CsvUtilSample
{
    public class CsvUtilSample : MonoBehaviour
    {
        public const string PATH_PART1 = "Assets/Samples/Uft.UnityUtils";
        public const string PATH_PART2 = "CsvUtilSample/Scripts";
        const string csvName = "csv-sample.csv";

        [SerializeField] TMP_Text txtText1;

        void Start()
        {
            var srcPath = DirectoryUtil.GetLatestSampleSourceDirectory(PATH_PART1, PATH_PART2);
            var resourcesPath = DirectoryUtil.ToResourcesPath(srcPath);
            var streamingAssetsPath = DirectoryUtil.ToStreamingAssetsPath(srcPath);

            try
            {
                var fileInfo = new FileInfo($"{streamingAssetsPath}/{csvName}");
                var result = fileInfo.ReadCsv(CsvUtil.GetCsvConfiguration(CsvUtil.UTF8), CsvDtoSample.Map);
                this.txtText1.text = string.Join("\n", result);
            }
            catch (Exception ex)
            {
                var message = "Please click \"Tools > Uft.UnityUtils.Samples > CsvUtilSample > ..., and restart";
                DevLog.LogWarning(ex.Message);
                this.txtText1.text = message;
            }
        }
    }
}
