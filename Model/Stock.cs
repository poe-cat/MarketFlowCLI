using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;

public sealed class Stock : Asset
{

    [ReportField("Giełda")]
    public string Exchange { get; }


    public override AssetType Type => AssetType.Stock;
    public override RiskLevel BaseRisk => RiskLevel.Medium;


    public Stock(string symbol, string name, Money currentPrice, string exchange) : base(symbol, name, currentPrice)
    {
        Exchange = exchange;
    }

    public override string GetRiskDescription() => "Akcja spółki publicznej: ryzyko zależne od wyników firmy i rynku.";
}
