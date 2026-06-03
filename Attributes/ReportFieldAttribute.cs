namespace MarketFlowCLI.Attributes;

// Atrybut oznaczający właściwości/pola, które mają być wyświetlane przez ReflectionPrinter
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ReportFieldAttribute : Attribute {

    public string Label { get; }

    public ReportFieldAttribute(string label) {
        Label = label;
    }
}
