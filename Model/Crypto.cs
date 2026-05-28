using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;


public sealed class Crypto : Asset
{

    [ReportField("Blockchain")]
    public string Blockchain { get; }

    public override AssetType Type => AssetType.Crypto;
    public override RiskLevel BaseRisk => RiskLevel.Extreme;


    public Crypto(string symbol, string name, Money currentPrice, string blockchain) : base(symbol, name, currentPrice)
    {
        Blockchain = blockchain;
    }


    public override string GetRiskDescription() => "Kryptowaluta: bardzo wysoka zmienność i ryzyko płynności.";
}
