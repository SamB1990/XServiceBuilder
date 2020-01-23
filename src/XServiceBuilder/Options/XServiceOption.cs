using System.Dynamic;
using XServiceBuilderLibrary.Abstractions.Options;

namespace XServiceBuilderLibrary.Options
{
    public class XServiceOption : IXServiceOption
    {
        public string Type { get; set; }
        public ExpandoObject Data { get; set; }
    }
}
