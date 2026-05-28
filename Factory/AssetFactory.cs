using MarketFlowCLI.Model;

namespace MarketFlowCLI.Factory;


public static class AssetFactory
{
    
    public static Asset Create(AssetType type, string symbol, string name, Money price)
    {
        return type switch
        {
            AssetType.Stock => new Stock(symbol, name, price, "GPW"),
            AssetType.Crypto => new Crypto(symbol, name, price, "Public blockchain"),
            AssetType.ETF => new ETF(symbol, name, price, "Broad market index"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unsupported asset type.")
        };
    }

    
    public static IReadOnlyList<Asset> CreateDemoAssets()
    {
        return new List<Asset>
        {
            new Stock("CDR", "CD Projekt", Money.From(126.40m), "GPW"),
            new Stock("PKO", "PKO BP", Money.From(58.20m), "GPW"),
            new Stock("PZU", "PZU", Money.From(47.90m), "GPW"),
            new Crypto("BTC", "Bitcoin", Money.From(265000m), "Bitcoin"),
            new Crypto("ETH", "Ethereum", Money.From(14200m), "Ethereum"),
            new ETF("SP500", "S&P 500 ETF", Money.From(480m), "S&P 500"),
            new ETF("WIG20", "WIG20 ETF", Money.From(86.50m), "WIG20")
        };
    }
}
