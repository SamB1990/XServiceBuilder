using System.Dynamic;

namespace XServiceBuilderLibrary.Abstractions.Options
{
    public interface IXServiceOption
    {
        string Type { get; set; }

        ExpandoObject Data { get; set; }
    }
}
