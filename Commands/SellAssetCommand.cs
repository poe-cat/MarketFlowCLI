using MarketFlowCLI.Model;

namespace MarketFlowCLI.Commands;


public sealed class SellAssetCommand : ITradingCommand
{

    private readonly Portfolio _portfolio;
    private readonly Asset _asset;
    private readonly int _quantity;

    
    public string Name => $"Sprzedaj {_quantity} x {_asset.Symbol}";

    
    public SellAssetCommand(Portfolio portfolio, Asset asset, int quantity)
    {
        _portfolio = portfolio;
        _asset = asset;
        _quantity = quantity;
    }

    public void Execute()
    {
        _portfolio.Sell(_asset, _quantity);
    }
}
