#nullable enable

#if UNITYUTILS_CSVHELPER_SUPPORT
namespace Uft.UnityUtils.Csv
{
    public delegate T CsvRowMapper<T>(CsvRow csvRow);
    public delegate CsvRowMapper<T> CsvRowMapperFactory<T>(string[] csvHeaders);
}
#endif
