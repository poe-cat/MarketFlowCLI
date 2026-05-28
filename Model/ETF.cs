using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;


public sealed class ETF : Asset
{

    [ReportField("Indeks bazowy")]
    public string Benchmark { get; }

    public override AssetType Type => AssetType.ETF;
    public override RiskLevel BaseRisk => RiskLevel.Low;

    
    public ETF(string symbol, string name, Money currentPrice, string benchmark) : base(symbol, name, currentPrice)
    {
        Benchmark = benchmark;
    }

    public override string GetRiskDescription() => "ETF: ekspozycja na koszyk aktywów, zwykle niższe ryzyko koncentracji.";
}
