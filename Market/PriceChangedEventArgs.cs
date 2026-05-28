using MarketFlowCLI.Model;

namespace MarketFlowCLI.Market;


public sealed class PriceChangedEventArgs : EventArgs
{

    public Asset Asset { get; }
    public Money OldPrice { get; }
    public Money NewPrice { get; }
    public decimal ChangePercent { get; }

    
    public PriceChangedEventArgs(Asset asset, Money oldPrice, Money newPrice)
    {
        Asset = asset;
        OldPrice = oldPrice;
        NewPrice = newPrice;
        ChangePercent = oldPrice.Amount == 0 ? 0 : decimal.Round(((newPrice.Amount - oldPrice.Amount) / oldPrice.Amount) * 100, 2);
    }
}
