using MarketFlowCLI.Model;

namespace MarketFlowCLI.Commands;


public sealed class BuyAssetCommand : ITradingCommand
{

    private readonly Portfolio _portfolio;
    private readonly Asset _asset;
    private readonly int _quantity;

   
    public string Name => $"Kup {_quantity} x {_asset.Symbol}";

    
    public BuyAssetCommand(Portfolio portfolio, Asset asset, int quantity)
    {
        _portfolio = portfolio;
        _asset = asset;
        _quantity = quantity;
    }

    public void Execute()
    {
        _portfolio.Buy(_asset, _quantity);
    }
}
