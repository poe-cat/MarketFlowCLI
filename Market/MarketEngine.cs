using MarketFlowCLI.Model;

namespace MarketFlowCLI.Market;


public sealed class MarketEngine
{
    
    private readonly List<Asset> _assets;
    private readonly Random _random = new();
    private bool _isRunning;

    
    public event MarketPriceChangedHandler? PriceChanged;

    public IReadOnlyList<Asset> Assets => _assets.AsReadOnly();
    public bool IsRunning => _isRunning;

    
    public MarketEngine(IEnumerable<Asset> assets)
    {
        _assets = assets.ToList();
    }

    
    public Asset? FindAsset(string symbol)
    {
        return _assets.FirstOrDefault(asset => asset.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
    }

    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_isRunning)
        {
            return;
        }

        _isRunning = true;

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                UpdateRandomPrices();
                await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            // Normal shutdown path.
        }
        finally
        {
            _isRunning = false;
        }
    }

    
    public void UpdateRandomPrices()
    {
        foreach (var asset in _assets)
        {
            var oldPrice = asset.CurrentPrice;
            var volatility = asset.Type switch
            {
                AssetType.Crypto => 0.08m,
                AssetType.Stock => 0.035m,
                AssetType.ETF => 0.018m,
                _ => 0.02m
            };

            var movement = ((decimal)_random.NextDouble() * 2 - 1) * volatility;
            var newAmount = Math.Max(0.01m, oldPrice.Amount * (1 + movement));
            var newPrice = Money.From(newAmount);

            asset.UpdatePrice(newPrice);
            PriceChanged?.Invoke(this, new PriceChangedEventArgs(asset, oldPrice, newPrice));
        }
    }
}
