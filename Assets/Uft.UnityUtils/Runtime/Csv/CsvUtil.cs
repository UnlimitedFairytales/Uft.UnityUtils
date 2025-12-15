#nullable enable

#if UNITYUTILS_CSVHELPER_SUPPORT
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Uft.UnityUtils.Csv
{
    public static class CsvUtil
    {
        // public static readonly Encoding SJIS_WIN;
        // public static readonly Encoding EUC_JP_WIN;
        // public static readonly Encoding ISO_2022_JP_WIN;
        public static readonly Encoding UTF8;

        static CsvUtil()
        {
            // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // SJIS_WIN = Encoding.GetEncoding(932);
            // EUC_JP_WIN = Encoding.GetEncoding(51932);
            // ISO_2022_JP_WIN = Encoding.GetEncoding(50220);
            UTF8 = Encoding.GetEncoding(65001);
        }
        public static CsvConfiguration GetCsvConfiguration(Encoding enc, string? delimiter = null, BadDataFound? badDataFound = null)
        {
            // https://github.com/JoshClose/CsvHelper/blob/33.1.0/src/CsvHelper/Configuration/CsvConfiguration.cs
            var cultureInfo = CultureInfo.InvariantCulture;
            var config = new CsvConfiguration(cultureInfo)
            {
                // var 33.1.0
                //AllowComments = false,
                AllowComments = true,
                //BadDataFound = ConfigurationFunctions.BadDataFound,
                //BufferSize = 0x1000, // 4096
                //CacheFields = false,
                //Comment = '#',
                //CountBytes = false,
                //CultureInfo
                //Delimiter = cultureInfo.TextInfo.ListSeparator, // ","
                //DetectDelimiter = false,
                //GetDelimiter = ConfigurationFunctions.GetDelimiter,
                //DetectDelimiterValues = new[] { ",", ";", "|", "\t" },
                //DetectColumnCountChanges = false,
                //DynamicPropertySort = null,
                //Encoding = Encoding.UTF8,
                Encoding = enc,
                //Escape = '"',
                //ExceptionMessagesContainRawData = true,
                //GetConstructor = ConfigurationFunctions.GetConstructor,
                //GetDynamicPropertyName = ConfigurationFunctions.GetDynamicPropertyName,
                //HasHeaderRecord = true,
                //HeaderValidated = ConfigurationFunctions.HeaderValidated,
                //IgnoreBlankLines = true,
                //IgnoreReferences = false,
                //IncludePrivateMembers = false, // true
                //InjectionCharacters = new char[] { '=', '@', '+', '-', '\t', '\r' },
                //InjectionEscapeCharacter = '\'',
                //InjectionOptions = 0,
                //IsNewLineSet
                //LineBreakInQuotedFieldIsBadData = false,
                //MaxFieldSize = 0,
                //MemberTypes = MemberTypes.Properties,
                //MissingFieldFound = ConfigurationFunctions.MissingFieldFound,
                //Mode = 0,
                //NewLine // private string newLine = "\r\n";
                //PrepareHeaderForMatch = ConfigurationFunctions.PrepareHeaderForMatch,
                //ProcessFieldBufferSize = 1024,
                //Quote = '"',
                //ReadingExceptionOccurred = ConfigurationFunctions.ReadingExceptionOccurred,
                //ReferenceHeaderPrefix = null,
                //ShouldQuote = ConfigurationFunctions.ShouldQuote,
                //ShouldSkipRecord = null,
                //ShouldUseConstructorParameters = ConfigurationFunctions.ShouldUseConstructorParameters,
                //TrimOptions = 0,
                //UseNewObjectForNullReferenceMembers = true,
                //WhiteSpaceChars = new char[] { ' ' },
            };
            if (delimiter != null)
            {
                config.Delimiter = delimiter;
            }
            if (badDataFound != null)
            {
                config.BadDataFound = badDataFound;
            }
            return config;
        }

        public static List<T> ReadCsv<T>(this FileInfo fileInfo, CsvConfiguration config, Func<CsvReader, T> manualMapping)
        {
            using var sr = new StreamReader(fileInfo.FullName, config.Encoding);
            return ReadCsvInner(sr, config, manualMapping);
        }

        public static List<T> ReadCsv<T>(this string csvText, CsvConfiguration config, Func<CsvReader, T> manualMapping)
        {
            using var sr = new StringReader(csvText);
            return ReadCsvInner(sr, config, manualMapping);
        }

        static List<T> ReadCsvInner<T>(TextReader tr, CsvConfiguration config, Func<CsvReader, T> manualMapping)
        {
            using var reader = new CsvReader(tr, config);
            // HACK: Do the below instead of GetRecords<T> due to incompatibility with IL2CPP
            // https://github.com/JoshClose/CsvHelper/issues/1337
            // return reader.GetRecords<T>().ToList();
            var records = new List<T>();
            reader.Read();
            reader.ReadHeader();
            while (reader.Read())
            {
                var record = manualMapping(reader);
                records.Add(record);
            }
            return records;
        }
    }
}
#endif
