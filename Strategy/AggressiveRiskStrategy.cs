using MarketFlowCLI.Model;

namespace MarketFlowCLI.Strategy;


public sealed class AggressiveRiskStrategy : IRiskStrategy
{

    public string Name => "Aggressive";


    public RiskLevel CalculateRisk(Portfolio portfolio)
    {
        if (!portfolio.Positions.Any()) return RiskLevel.Low;
        var cryptoShare = PortfolioMath.ShareOfType(portfolio, AssetType.Crypto);
        return cryptoShare > 0.60m ? RiskLevel.Extreme : RiskLevel.Medium;
    }


    public string Explain(Portfolio portfolio)
    {
        return "Strategia agresywna toleruje większą zmienność, ale wykrywa skrajną ekspozycję na aktywa wysokiego ryzyka.";
    }
}
