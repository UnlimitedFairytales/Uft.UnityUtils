using CsvHelper;

namespace Uft.UnityUtils.Samples.ScriptSample
{
    public class CsvDtoSample
    {
        public static CsvDtoSample Map(CsvReader reader)
        {
            return new CsvDtoSample
            {
                Name = reader.GetField("name"),
                MetaVar = reader.GetField("meta-var"),
                MetaVarJp = reader.GetField("meta-var-jp")
            };
        }

        public string Name { get; set; }
        public string MetaVar { get; set; }
        public string MetaVarJp { get; set; }

        public override string ToString() => $"{this.Name},{this.MetaVar},{this.MetaVarJp}";
    }
}
