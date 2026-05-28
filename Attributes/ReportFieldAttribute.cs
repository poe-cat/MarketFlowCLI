namespace MarketFlowCLI.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ReportFieldAttribute : Attribute
{
    public string Label { get; }

    public ReportFieldAttribute(string label)
    {
        Label = label;
    }
}
