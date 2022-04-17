using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace XtractPro.Utils.JsonQueryGenerator
{
    public class QueryGenerator
    {
        public static readonly string[] QueryTypes = new[] {
            "(select a query type)",
            "Single PARSE_JSON",
            "Single OBJECT_CONSTRUCT with PARSE_JSON",
            "Multiple OBJECT_CONSTRUCT with ARRAY_CONSTRUCT",
            "First array",
            "First array object",
            "First array object property",
            "LATERAL FLATTEN first array",
            "LATERAL FLATTEN first array + LISTAGG to combine back",
        };

        public QueryGenerator() { }

        public string GetQuery(string json, string queryType)
            => string.IsNullOrEmpty(json) ? ""
            : queryType == QueryTypes[1] ? GetSingleParseJsonQuery(json)
            : queryType == QueryTypes[2] ? GetSingleObjectConstructQuery(JToken.Parse(json))
            : queryType == QueryTypes[3] ? GetMultiObjectConstructQuery(JToken.Parse(json))
            : queryType == QueryTypes[4] ? GetFirstArrayQuery(json, false)
            : queryType == QueryTypes[5] ? GetFirstArrayQuery(json, false, true)
            : queryType == QueryTypes[6] ? GetFirstArrayQuery(json, false, true, false, true)
            : queryType == QueryTypes[7] ? GetFirstArrayQuery(json, true)
            : queryType == QueryTypes[8] ? GetFirstArrayQuery(json, true, false, true)
            : "";

        private string ToSqlValue(JValue value)
            => value.Type == JTokenType.String ? $"'{value.ToString().Replace("'", "''")}'"
            : value.ToString() == "" ? "null" : value.ToString();

        private string GetSingleParseJsonQuery(string json)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- creates a single JSON object with a PARSE_JSON call");
            sb.Append($"select parse_json('{json.Replace("'", "''")}') as json;");
            return sb.ToString();
        }
        
        private string GetSingleObjectConstructQuery(JToken token)
        {
            var sb = new StringBuilder();
            if (token is JObject obj)
                foreach (var property in obj.Properties())
                {
                    if (sb.Length == 0)
                    {
                        sb.AppendLine("-- creates a single JSON object only from properties");
                        sb.AppendLine("select object_construct(");
                    }
                    else
                        sb.AppendLine(",");
                    sb.Append($"  '{property.Name}', ");
                    sb.Append(property.Value is JValue value
                        ? $"{ToSqlValue(value)}" : $"parse_json('{property.Value}')");
                }
            sb.Append(") as json;");
            return sb.ToString();
        }

        private string GetMultiObjectConstructQuery(
            JToken token, bool parentObj = true, string level = "")
        {
            var sb = new StringBuilder();
            if (token is JObject obj)
                foreach (var property in obj.Properties())
                {
                    if (sb.Length > 0) sb.AppendLine(",");
                    else
                    {
                        if (level.Length == 0)
                        {
                            sb.AppendLine("-- creates a single JSON object from objects, properties, and arrays");
                            sb.Append("select ");
                        }
                        else if (!parentObj) sb.Append(level);
                        sb.AppendLine("object_construct(");
                    }
                    sb.Append($"{level}  '{property.Name}', ");
                    sb.Append(property.Value is JValue value ? ToSqlValue(value)
                        : GetMultiObjectConstructQuery(property.Value, true, $"{level}  "));
                }
            else if (token is JArray array)
                for (var i = 0; i < array.Count; i++)
                {
                    if (sb.Length > 0) sb.AppendLine(",");
                    else
                    {
                        if (level.Length == 0) sb.Append("select ");
                        else if (!parentObj) sb.Append(level);
                        sb.AppendLine("array_construct(");
                    }
                    sb.Append(array[i] is JValue value ? $"{level}  {ToSqlValue(value)}"
                        : GetMultiObjectConstructQuery(array[i], false, $"{level}  "));
                }

            sb.Append(')');
            if (level.Length == 0) sb.Append(" as json;");
            return sb.ToString();
        }

        private string GetFirstArrayQuery(string json, bool flatten, 
            bool element = false, bool agg = false, bool prop = false)
        {
            // find path to first array
            var array = GetFirstArray(JToken.Parse(json)) as JArray;
            if (array == null
                || element && array.Count == 0
                || prop && (array[0] is not JObject obj || obj.Properties().ToList().Count == 0))
                return "";

            // quote names
            var parts = array.Path.Split('.');
            for (var i = 0; i < parts.Length; i++) parts[i] = $"\"{parts[i]}\"";

            // make full path
            var field = "";
            for (var i = 0; i < parts.Length; i++) field += (i == 0 ? "" : ".") + parts[i];

            // return query
            var sb = new StringBuilder();
            sb.AppendLine("-- top CTE with one single data entry as an inline JSON object");
            sb.AppendLine($"with src as (");
            sb.AppendLine($"  select parse_json('{json.Replace("'", "''")}') as json){(agg ? "," : "")}");
            sb.AppendLine();

            var elem = element ? "[0]" : "";
            if (flatten && agg)
            {
                sb.AppendLine("-- CTE with one row for each element of the first JSON array");
                sb.AppendLine($"arr as (");
                sb.AppendLine($"  select elem.value as {parts[^1]}");
                sb.AppendLine($"  from src, lateral flatten(");
                sb.AppendLine($"    input => json:{field}{elem}) as elem)");
                sb.AppendLine();

                sb.AppendLine("-- LISTAGG combines all rows in a single comma-separated list");
                sb.AppendLine($"select '[ ' || listagg({parts[^1]}, ', ') || ' ]' as agg");
                sb.Append($"from arr;");
            }
            else if (flatten)
            {
                sb.AppendLine("-- returns one row for each element of the first JSON array");
                sb.AppendLine($"select elem.value as {parts[^1]}");
                sb.AppendLine($"from src, lateral flatten(");
                sb.Append($"  input => json:{field}{elem}) as elem;");
            }
            else if (prop)
            {
                var propName = $"\"{(array[0] as JObject).Properties().ToList()[0].Name}\"";
                sb.AppendLine("-- returns the first property value of the first JSON object of the first JSON array");
                sb.AppendLine($"select json:{field}{elem}.{propName} as {propName}");
                sb.Append($"from src;");
            }
            else
            {
                sb.AppendLine("-- returns the first JSON object of the first JSON array");
                sb.AppendLine($"select json:{field}{elem} as {parts[^1]}");
                sb.Append($"from src;");
            }
            return sb.ToString();
        }

        // find first JArray array
        private JToken GetFirstArray(JToken token)
        {
            JToken tok;
            if (token is JArray)
                return token;
            else if (token is JProperty prop
                && (tok = GetFirstArray(prop.Value)) != null)
                return tok;
            else if (token is JObject obj)
                foreach (var property in obj.Properties())
                    if ((tok = GetFirstArray(property)) != null)
                        return tok;
            return null;
        }
    }
}
