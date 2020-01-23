using XServiceBuilderLibrary.Abstractions.Options;

// ReSharper disable CoVariantArrayConversion
namespace XServiceBuilderLibrary.Options
{
    public class XServiceOptionsCollection : IXServiceOptionsCollection
    {
        public IXServiceOption[] Options { get; set; } = new XServiceOption [] { };
    }
}
