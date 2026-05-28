using MarketFlowCLI.Model;

namespace MarketFlowCLI.Strategy;


public sealed class ConservativeRiskStrategy : IRiskStrategy
{

    public string Name => "Conservative";

    
    public RiskLevel CalculateRisk(Portfolio portfolio)
    {
        if (!portfolio.Positions.Any()) return RiskLevel.Low;
        var cryptoShare = PortfolioMath.ShareOfType(portfolio, AssetType.Crypto);
        return cryptoShare > 0.10m ? RiskLevel.High : RiskLevel.Low;
    }

    
    public string Explain(Portfolio portfolio)
    {
        return "Strategia konserwatywna mocno karze ekspozycję na kryptowaluty i koncentrację ryzyka.";
    }
}
