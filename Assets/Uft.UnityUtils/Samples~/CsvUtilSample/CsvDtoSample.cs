using Uft.UnityUtils.Csv;

namespace Uft.UnityUtils.Samples.CsvUtilSample
{
    public class CsvDtoSample
    {
        public static CsvRowMapper<CsvDtoSample> MapperFactory(string[] csvHeaders)
        {
            int iName = CsvUtil.FindColumnIndex(csvHeaders, "name");
            int iMetaVar = CsvUtil.FindColumnIndex(csvHeaders, "meta-var");
            int iMetaVarJp = CsvUtil.FindColumnIndex(csvHeaders, "meta-var-jp");
            CsvDtoSample mapper(CsvRow csvRow)
            {
                return new CsvDtoSample(
                    csvRow.GetString(iName),
                    csvRow.GetString(iMetaVar),
                    csvRow.GetString(iMetaVarJp));
            }
            return mapper;
        }

        public string Name { get; set; }
        public string MetaVar { get; set; }
        public string MetaVarJp { get; set; }

        public CsvDtoSample(string name, string metaVar, string metaVarJp)
        {
            this.Name = name;
            this.MetaVar = metaVar;
            this.MetaVarJp = metaVarJp;
        }

        public override string ToString() => $"{this.Name},{this.MetaVar},{this.MetaVarJp}";
    }
}
