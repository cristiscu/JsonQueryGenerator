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
            "Single PARSE_JSON check types",
            "Single PARSE_JSON conversions",
            "Single OBJECT_CONSTRUCT with PARSE_JSON",
            "Multiple OBJECT_CONSTRUCT with ARRAY_CONSTRUCT_COMPACT",
            "Multiple OBJECT_CONSTRUCT_KEEP_NULL with ARRAY_CONSTRUCT",
            "First array",
            "First value array",
            "First array object",
            "First array object property",
            "LATERAL FLATTEN first array",
            "LATERAL FLATTEN first array + ARRAY_AGG/LISTAGG to compact",
            "LATERAL FLATTEN first two arrays",
            "TABLE FLATTEN first array",
            "TABLE FLATTEN first array nested",
            "TABLE FLATTEN first array RECURSIVE",
        };

        public QueryGenerator() { }

        public string GetQuery(string json, string queryType)
            => string.IsNullOrEmpty(json) ? "-- [ERROR] must use non-empty JSON!"
            : queryType == QueryTypes[1] ? GetSingleParseJsonQuery(json, new QueryOptions())
            : queryType == QueryTypes[2] ? GetTypesQuery(json, new QueryOptions() { checkTypes = true })
            : queryType == QueryTypes[3] ? GetTypesQuery(json, new QueryOptions())
            : queryType == QueryTypes[4] ? GetSingleObjectConstructQuery(JToken.Parse(json), new QueryOptions())
            : queryType == QueryTypes[5] ? GetMultiObjectConstructQuery(JToken.Parse(json), new QueryOptions())
            : queryType == QueryTypes[6] ? GetMultiObjectConstructQuery(JToken.Parse(json), new QueryOptions() { keepNulls = true })
            : queryType == QueryTypes[7] ? GetArrayQuery(json, new QueryOptions())
            : queryType == QueryTypes[8] ? GetArrayQuery(json, new QueryOptions() { hasValues = true })
            : queryType == QueryTypes[9] ? GetArrayQuery(json, new QueryOptions() { retElement = true })
            : queryType == QueryTypes[10] ? GetArrayQuery(json, new QueryOptions() { retElement = true, retProperty = true })
            : queryType == QueryTypes[11] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true })
            : queryType == QueryTypes[12] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, callArrayAgg = true })
            : queryType == QueryTypes[13] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, twoArrays = true })
            : queryType == QueryTypes[14] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, callTable = true })
            : queryType == QueryTypes[15] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, callTable = true, useNested = true })
            : queryType == QueryTypes[16] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, callTable = true, useRecursive = true })
            : "-- [ERROR] query type not found!";

        private string GetSingleParseJsonQuery(string json, QueryOptions opt)
        {
            var sb = new StringBuilder();
            sb.AppendLine("-- creates a single JSON object with a PARSE_JSON call");
            sb.Append($"select parse_json('{json.Replace("'", "''")}') as json;");
            return sb.ToString();
        }
        
        private string GetSingleObjectConstructQuery(JToken token, QueryOptions opt)
        {
            var sb = new StringBuilder();
            if (token is JObject obj)
                foreach (var property in obj.Properties())
                {
                    if (sb.Length == 0)
                    {
                        sb.AppendLine("-- creates a single JSON object only from properties");
                        sb.AppendLine($"select {opt.getObjectConstruct()}(");
                    }
                    else
                        sb.AppendLine(",");
                    sb.Append($"  '{property.Name}', ");
                    sb.Append(property.Value is JValue value
                        ? $"{opt.ToSqlValue(value)}" : $"parse_json('{property.Value}')");
                }
            else if (token is JArray array)
                for (var i = 0; i < array.Count; i++)
                {
                    if (sb.Length == 0)
                    {
                        sb.AppendLine("-- creates a single JSON object only from properties");
                        sb.AppendLine($"select {opt.getArrayConstruct()}(");
                    }
                    else
                        sb.AppendLine(",");
                    sb.Append(array[i] is JValue value
                        ? $"  {opt.ToSqlValue(value)}" : $"  parse_json('{array[i]}')");
                }

            sb.Append(") as v;");
            return sb.ToString();
        }

        private string GetMultiObjectConstructQuery(
            JToken token, QueryOptions opt, bool parentObj = true, string level = "")
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
                        sb.AppendLine($"{opt.getObjectConstruct()}(");
                    }
                    sb.Append($"{level}  '{property.Name}', ");
                    sb.Append(property.Value is JValue value ? opt.ToSqlValue(value)
                        : GetMultiObjectConstructQuery(property.Value, opt, true, $"{level}  "));
                }
            else if (token is JArray array)
                for (var i = 0; i < array.Count; i++)
                {
                    if (sb.Length > 0) sb.AppendLine(",");
                    else
                    {
                        if (level.Length == 0) sb.Append("select ");
                        else if (!parentObj) sb.Append(level);
                        sb.AppendLine($"{opt.getArrayConstruct()}(");
                    }
                    sb.Append(array[i] is JValue value ? $"{level}  {opt.ToSqlValue(value)}"
                        : GetMultiObjectConstructQuery(array[i], opt, false, $"{level}  "));
                }

            sb.Append(')');
            if (level.Length == 0) sb.Append(" as v;");
            return sb.ToString();
        }

        private string GetTypesQuery(string json, QueryOptions opt)
        {
            var sb = new StringBuilder();
            sb.AppendLine(opt.checkTypes ? "-- check data types on JSON string" : "-- conversions on JSON data");
            sb.AppendLine($"with src as (");
            sb.AppendLine($"  select parse_json('{json.Replace("'", "''")}') as v)");
            sb.AppendLine();

            if (opt.checkTypes)
            {
                sb.AppendLine($"select typeof(v), SYSTEM$TYPEOF(v),");
                sb.AppendLine($"  SYSTEM$TYPEOF(to_variant(v)), SYSTEM$TYPEOF(to_json(v)),");
                sb.AppendLine($"  is_object(v), is_array(v)");
                sb.Append($"from src;");
            }
            else
            {
                sb.AppendLine($"select to_object(v), as_object(v),");
                sb.AppendLine($"  to_array(v), as_array(v),");
                sb.AppendLine($"  to_json(v), to_xml(v)");
                sb.Append($"from src;");
            }
            return sb.ToString();
        }

        private string GetArrayQuery(string json, QueryOptions opt)
        {
            // find first array, with path
            var array = GetFirstArray(JToken.Parse(json), opt) as JArray;
            if (array == null && opt.hasValues)
                return "-- [ERROR] must use JSON with at least one array of values!";
            if (array == null)
                return "-- [ERROR] must use JSON with at least one array!";
            if (opt.retElement && array.Count == 0)
                return "-- [ERROR] must use JSON with at least one non-empty array!";
            if (opt.retProperty && (array[0] is not JObject obj || obj.Properties().ToList().Count == 0))
                return "-- [ERROR] must use JSON with at least one non-empty object array!";

            var path = opt.MakePath(array.Path);
            var alias = path.Split('.')[^1];
            var index = opt.retElement ? "[0]" : "";
            var rec = opt.useRecursive ? ", recursive => true" : "";

            // generate SQL query
            var sb = new StringBuilder();
            if (opt.callFlatten && opt.callTable && opt.useNested)
            {
                sb.AppendLine("-- returns one row for each element of the first JSON array");
                sb.AppendLine("-- (use embedded PARSE_JSON to call with TABLE instead of LATERAL!)");
                sb.AppendLine($"select elem.value as {alias}");
                sb.Append($"from table(flatten(input => parse_json('{json.Replace("'", "''")}'):{path}{index}{rec})) as elem;");
            }
            else
            {
                sb.AppendLine("-- top CTE with one single data entry as an inline JSON object");
                sb.AppendLine($"with src as (");
                sb.AppendLine($"  select parse_json('{json.Replace("'", "''")}') as v){(opt.callArrayAgg ? "," : "")}");
                sb.AppendLine();

                if (opt.callFlatten && opt.callArrayAgg)
                {
                    sb.AppendLine("-- CTE with one row for each element of the first JSON array");
                    sb.AppendLine($"arr as (");
                    sb.AppendLine($"  select elem.value as {alias}");
                    sb.AppendLine($"  from src,");
                    sb.AppendLine(opt.callTable
                        ? $"    table(flatten(input => v:{path}{index}{rec})) as elem)"
                        : $"    lateral flatten(input => v:{path}{index}{rec}) as elem)");
                    sb.AppendLine();

                    sb.AppendLine("-- ARRAY_AGG and LISTAGG combine row array elements into a single array");
                    sb.AppendLine($"select array_agg({alias}) as array_agg,");
                    sb.AppendLine($"  '[ ' || listagg({alias}, ', ') || ' ]' as listagg");
                    sb.Append($"from arr;");
                }
                else if (opt.callFlatten && opt.twoArrays)
                {
                    // find second array, with path
                    var array2 = GetFirstArray(array, opt, array) as JArray;
                    if (array2 == null)
                        return "-- [ERROR] must use JSON with at least two nested arrays!";

                    var path2 = opt.MakePath(array2.Path, path.Split('.').Length);
                    var alias2 = path2.Split('.')[^1];

                    sb.AppendLine("-- returns one row for each element of the first two JSON arrays");
                    sb.AppendLine($"select elem.value as {alias}, elem2.value as {alias2}");
                    sb.AppendLine($"from src,");
                    sb.AppendLine(opt.callTable
                        ? $"  table(flatten(input => v:{path}{rec})) as elem,"
                        : $"  lateral flatten(input => v:{path}{rec}) as elem,");
                    sb.Append(opt.callTable
                        ? $"  table(flatten(input => elem.value:{path2}{rec})) as elem2;"
                        : $"  lateral flatten(input => elem.value:{path2}{rec}) as elem2;");
                }
                else if (opt.callFlatten)
                {
                    sb.AppendLine("-- returns one row for each element of the first JSON array");
                    sb.AppendLine($"select elem.value as {alias}");
                    sb.AppendLine($"from src,");
                    sb.Append(opt.callTable
                        ? $"  table(flatten(input => v:{path}{index}{rec})) as elem;"
                        : $"  lateral flatten(input => v:{path}{index}{rec}) as elem;");
                }
                else if (opt.retProperty)
                {
                    var prop = (array[0] as JObject).Properties().ToList()[0];
                    var type = prop.Value.Type == JTokenType.String ? "::string" : "";
                    var altNotation = opt.GetAltNotation($"v:{path}{index}.\"{prop.Name}\"");

                    sb.AppendLine("-- returns the first property value of the first JSON object of the first JSON array");
                    sb.AppendLine($"select v:{path}{index}.\"{prop.Name}\"{type} as \"{prop.Name}\",");
                    sb.AppendLine($"  {altNotation}{type} as \"{prop.Name}\"");
                    sb.Append($"from src;");
                }
                else if (opt.retElement)
                {
                    var altNotation = opt.GetAltNotation($"v:{path}{index}");
                    var key = (array[0] as JObject)?.Properties().FirstOrDefault()?.Name;

                    sb.AppendLine("-- returns the first element in the first JSON array");
                    sb.AppendLine($"select v:{path}{index} as {alias},");
                    sb.AppendLine($"  {altNotation} as {alias},");
                    sb.AppendLine($"  get(v:{path}, 0) as get,");
                    if (!string.IsNullOrEmpty(key))
                        sb.AppendLine($"  object_pick(v:{path}{index}, '{key}') as object_pick,");
                    sb.AppendLine($"  object_keys(v:{path}{index}) as object_keys");
                    sb.Append($"from src;");
                }
                else
                {
                    var slice = (array.Count >= 1 ? array.Count - 1 : array.Count);

                    sb.AppendLine("-- returns the first JSON array");
                    sb.AppendLine($"select v:{path} as {alias},");
                    sb.AppendLine($"  array_size(v:{path}) as array_size,");
                    sb.AppendLine($"  array_slice(v:{path}, 0, {slice}) as array_slice,");
                    sb.AppendLine($"  array_to_string(v:{path}, ' ###### ') as array_to_string");
                    sb.Append($"from src;");
                }
            }
            return sb.ToString();
        }

        // find first JArray array
        private JToken GetFirstArray(JToken token, QueryOptions opt, JToken skip = null)
        {
            if (token != skip && token is JArray array
                && (!opt.hasValues || array.Count > 0 && array[0] is JValue))
                return token;

            JToken tok;
            if (token is JProperty prop
                && (tok = GetFirstArray(prop.Value, opt, skip)) != null)
                return tok;
            else if (token is JObject obj)
                foreach (var property in obj.Properties())
                {
                    if ((tok = GetFirstArray(property, opt, skip)) != null)
                        return tok;
                }
            else if (token is JArray arr)
                for (var i = 0; i < arr.Count; i++)
                {
                    if ((tok = GetFirstArray(arr[i], opt, skip)) != null)
                        return tok;
                }
            return null;
        }
    }
}
