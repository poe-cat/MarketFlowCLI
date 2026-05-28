using MarketFlowCLI.Model;

namespace MarketFlowCLI.Strategy;


public static class PortfolioMath
{

    public static decimal ShareOfType(Portfolio portfolio, AssetType type)
    {
        if (portfolio.PositionsValue.Amount <= 0) return 0m;

        var value = portfolio.Positions
            .Where(position => position.Asset.Type == type)
            .Aggregate(Money.Zero, (sum, position) => sum + position.Value);

        return decimal.Round(value.Amount / portfolio.PositionsValue.Amount, 4);
    }
}
