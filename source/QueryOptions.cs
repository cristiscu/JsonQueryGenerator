using System.Text;

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

        public string getObjectConstruct()
            => keepNulls ? "object_construct_keep_null" : "object_construct";
        public string getArrayConstruct()
            => keepNulls ? "array_construct" : "array_construct_compact";
    }
}
