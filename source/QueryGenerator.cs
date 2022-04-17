﻿using System.Text;
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
            "LATERAL FLATTEN first two arrays",
            "TABLE FLATTEN first array",
            "TABLE FLATTEN first array RECURSIVE",
        };

        public QueryGenerator() { }

        public string GetQuery(string json, string queryType)
            => string.IsNullOrEmpty(json) ? "-- [ERROR] must use non-empty JSON!"
            : queryType == QueryTypes[1] ? GetSingleParseJsonQuery(json)
            : queryType == QueryTypes[2] ? GetSingleObjectConstructQuery(JToken.Parse(json))
            : queryType == QueryTypes[3] ? GetMultiObjectConstructQuery(JToken.Parse(json))
            : queryType == QueryTypes[4] ? GetArrayQuery(json, new QueryOptions())
            : queryType == QueryTypes[5] ? GetArrayQuery(json, new QueryOptions() { retElement = true })
            : queryType == QueryTypes[6] ? GetArrayQuery(json, new QueryOptions() { retElement = true, retProperty = true })
            : queryType == QueryTypes[7] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true })
            : queryType == QueryTypes[8] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, callListAgg = true })
            : queryType == QueryTypes[9] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, twoArrays = true })
            : queryType == QueryTypes[10] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, callTable = true })
            : queryType == QueryTypes[11] ? GetArrayQuery(json, new QueryOptions() { callFlatten = true, callTable = true, useRecursive = true })
            : "-- [ERROR] query type not found!";

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

        private string MakePath(string pathJson, int skip = 0)
        {
            var sb = new StringBuilder();
            var parts = pathJson.Split('.');
            for (var i = 0; i < parts.Length; i++)
                if (i >= skip)
                    sb.Append($"{(sb.Length == 0 ? "" : ".")}\"{parts[i]}\"");
            return sb.ToString();
        }

        private string GetArrayQuery(string json, QueryOptions opt)
        {
            // find first array, with path
            var array = GetFirstArray(JToken.Parse(json)) as JArray;
            if (array == null)
                return "-- [ERROR] must use JSON with at least one array!";
            if (opt.retElement && array.Count == 0)
                return "-- [ERROR] must use JSON with at least one non-empty array!";
            if (opt.retProperty && (array[0] is not JObject obj || obj.Properties().ToList().Count == 0))
                return "-- [ERROR] must use JSON with at least one non-empty object array!";

            var path = MakePath(array.Path);
            var alias = path.Split('.')[^1];
            var index = opt.retElement ? "[0]" : "";
            var rec = opt.useRecursive ? ", recursive => true" : "";

            // generate SQL query
            var sb = new StringBuilder();
            if (opt.callFlatten && opt.callTable)
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
                sb.AppendLine($"  select parse_json('{json.Replace("'", "''")}') as json){(opt.callListAgg ? "," : "")}");
                sb.AppendLine();

                if (opt.callFlatten && opt.callListAgg)
                {
                    sb.AppendLine("-- CTE with one row for each element of the first JSON array");
                    sb.AppendLine($"arr as (");
                    sb.AppendLine($"  select elem.value as {alias}");
                    sb.AppendLine($"  from src,");
                    sb.AppendLine($"    lateral flatten(input => json:{path}{index}{rec}) as elem)");
                    sb.AppendLine();

                    sb.AppendLine("-- LISTAGG combines all rows in a single comma-separated list");
                    sb.AppendLine($"select '[ ' || listagg({alias}, ', ') || ' ]' as agg");
                    sb.Append($"from arr;");
                }
                else if (opt.callFlatten && opt.twoArrays)
                {
                    // find second array, with path
                    var array2 = GetFirstArray(array, array) as JArray;
                    if (array2 == null)
                        return "-- [ERROR] must use JSON with at least two nested arrays!";

                    var path2 = MakePath(array2.Path, path.Split('.').Length);
                    var alias2 = path2.Split('.')[^1];

                    sb.AppendLine("-- returns one row for each element of the first two JSON arrays");
                    sb.AppendLine($"select elem.value as {alias}, elem2.value as {alias2}");
                    sb.AppendLine($"from src,");
                    sb.AppendLine($"  lateral flatten(input => json:{path}{rec}) as elem,");
                    sb.Append($"  lateral flatten(input => elem.value:{path2}{rec}) as elem2;");
                }
                else if (opt.callFlatten)
                {
                    sb.AppendLine("-- returns one row for each element of the first JSON array");
                    sb.AppendLine($"select elem.value as {alias}");
                    sb.AppendLine($"from src,");
                    sb.Append($"  lateral flatten(input => json:{path}{index}{rec}) as elem;");
                }
                else if (opt.retProperty)
                {
                    var propName = $"\"{(array[0] as JObject).Properties().ToList()[0].Name}\"";
                    sb.AppendLine("-- returns the first property value of the first JSON object of the first JSON array");
                    sb.AppendLine($"select json:{path}{index}.{propName} as {propName}");
                    sb.Append($"from src;");
                }
                else
                {
                    sb.AppendLine("-- returns the first JSON object of the first JSON array");
                    sb.AppendLine($"select json:{path}{index} as {alias}");
                    sb.Append($"from src;");
                }
            }
            return sb.ToString();
        }

        // find first JArray array
        private JToken GetFirstArray(JToken token, JToken skip = null)
        {
            if (token != skip && token is JArray)
                return token;

            JToken tok;
            if (token is JProperty prop
                && (tok = GetFirstArray(prop.Value, skip)) != null)
                return tok;
            else if (token is JObject obj)
                foreach (var property in obj.Properties())
                {
                    if ((tok = GetFirstArray(property, skip)) != null)
                        return tok;
                }
            else if (token is JArray arr)
                for (var i = 0; i < arr.Count; i++)
                {
                    if ((tok = GetFirstArray(arr[i], skip)) != null)
                        return tok;
                }
            return null;
        }
    }
}