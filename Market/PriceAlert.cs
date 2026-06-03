using MarketFlowCLI.Model;

namespace MarketFlowCLI.Market;


// Alert cenowy, jednorazowo powiadamia gdy cena aktywa przekroczy (lub spadnie poniżej) progu
public sealed class PriceAlert
{

    public string Symbol { get; }
    public Money TargetPrice { get; }
    public bool TriggerWhenAbove { get; }
    public bool IsTriggered { get; private set; }

    
    public PriceAlert(string symbol, Money targetPrice, bool triggerWhenAbove)
    {
        Symbol = symbol.ToUpperInvariant();
        TargetPrice = targetPrice;
        TriggerWhenAbove = triggerWhenAbove;
    }

    
    // Handler podpinany pod MarketEngine.PriceChanged - sprawdza warunek i wyswietla komunikat
    public void HandlePriceChanged(object? sender, PriceChangedEventArgs args)
    {
        if (IsTriggered || args.Asset.Symbol != Symbol)
        {
            return;
        }

        var conditionMet = TriggerWhenAbove
            ? args.NewPrice >= TargetPrice
            : args.NewPrice <= TargetPrice;

        if (!conditionMet)
        {
            return;
        }

        IsTriggered = true;
        var direction = TriggerWhenAbove ? "osiągnął lub przekroczył" : "spadł do lub poniżej";
        Console.WriteLine();
        Console.WriteLine($"ALERT CENOWY: {Symbol} {direction} {TargetPrice}. Cena aktualna: {args.NewPrice}.");
        Console.Write("Wybierz opcję: ");
    }


    public override string ToString()
    {
        var condition = TriggerWhenAbove ? ">=" : "<=";
        var status = IsTriggered ? "wykonany" : "aktywny";
        return $"{Symbol} {condition} {TargetPrice} [{status}]";
    }
}
