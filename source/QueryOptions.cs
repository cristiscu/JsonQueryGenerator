using System.Text;
using Newtonsoft.Json.Linq;

namespace XtractPro.Utils.JsonQueryGenerator
{
    public class QueryOptions
    {
        public QueryOptions() { }

        public bool callFlatten { get; set; }
        public bool retElement { get; set; }
        public bool callArrayAgg { get; set; }
        public bool retProperty { get; set; }
        public bool twoArrays { get; set; }
        public bool callTable { get; set; }
        public bool useRecursive { get; set; }
        public bool useNested { get; set; }
        public bool keepNulls { get; set; }
        public bool hasValues { get; set; }

        public string getObjectConstruct()
            => keepNulls ? "object_construct_keep_null" : "object_construct";
        public string getArrayConstruct()
            => keepNulls ? "array_construct" : "array_construct_compact";

        public string ToSqlValue(JValue value)
            => value.Type == JTokenType.String ? $"'{value.ToString().Replace("'", "''")}'"
            : value.ToString() == "" ? "null" : value.ToString();

        public string GetAltNotation(string notation)
            => notation.Replace("\".\"", "']['").Replace(":\"", "['")
            .Replace(".\"", "['").Replace("\"", "']");

        public string MakePath(string pathJson, int skip = 0)
        {
            var sb = new StringBuilder();
            var parts = pathJson.Split('.');
            for (var i = 0; i < parts.Length; i++)
                if (i >= skip)
                {
                    var j = parts[i].IndexOf('[');
                    var quoted = (j < 0 ? $"\"{parts[i]}\"" : $"\"{parts[i][..j]}\"{parts[i][j..]}");
                    sb.Append($"{(sb.Length == 0 ? "" : ".")}{quoted}");
                }
            return sb.ToString();
        }
    }
}
