#nullable enable

#if UNITYUTILS_CSVHELPER_SUPPORT
using CsvHelper;

namespace Uft.UnityUtils.Csv
{
    public readonly struct CsvRow
    {
        readonly CsvReader reader;

        public CsvRow(CsvReader reader)
        {
            this.reader = reader;
        }
        public string? GetString(int index) => index < 0 ? null : this.reader.GetField(index);
        public int GetInt32(int index) => index < 0 ? 0 : this.reader.GetField<int>(index);
        public float GetSingle(int index) => index < 0 ? 0 : this.reader.GetField<float>(index);
        public bool GetBoolean(int index) => index >= 0 && this.reader.GetField<bool>(index);
    }
}
#endif
