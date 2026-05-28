using MarketFlowCLI.Model;

namespace MarketFlowCLI.Strategy;


public sealed class BalancedRiskStrategy : IRiskStrategy
{

    public string Name => "Balanced";


    public RiskLevel CalculateRisk(Portfolio portfolio)
    {
        if (!portfolio.Positions.Any()) return RiskLevel.Low;
        var cryptoShare = PortfolioMath.ShareOfType(portfolio, AssetType.Crypto);
        var etfShare = PortfolioMath.ShareOfType(portfolio, AssetType.ETF);

        if (cryptoShare > 0.35m) return RiskLevel.High;
        if (etfShare > 0.50m) return RiskLevel.Low;
        return RiskLevel.Medium;
    }


    public string Explain(Portfolio portfolio)
    {
        return "Strategia zrównoważona ocenia portfel przez udział klas aktywów i dywersyfikację.";
    }
}
