using System.Text;

namespace XtractPro.Utils.JsonQueryGenerator
{
    public class QueryOptions
    {
        public QueryOptions() { }

        public bool callFlatten { get; set; }
        public bool retElement { get; set; }
        public bool callListAgg { get; set; }
        public bool retProperty { get; set; }
        public bool twoArrays { get; set; }
        public bool callTable { get; set; }
        public bool useRecursive { get; set; }
    }
}
