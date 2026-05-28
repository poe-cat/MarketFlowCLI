using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;


public sealed class Position : IReportable
{
    
    [ReportField("Aktywo")]
    public Asset Asset { get; }

    [ReportField("Liczba jednostek")]
    public int Quantity { get; private set; }

    
    public Money Value => Asset.CurrentPrice * Quantity;

    
    public Position(Asset asset, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        Asset = asset;
        Quantity = quantity;
    }

    
    public void Increase(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        Quantity += quantity;
    }

    
    public void Decrease(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
        if (quantity > Quantity) throw new InvalidOperationException("Cannot sell more units than owned.");
        Quantity -= quantity;
    }

   
    public string ToReportLine()
    {
        return $"{Asset.Symbol,-6} | {Asset.Name,-24} | ilość: {Quantity,4} | cena: {Asset.CurrentPrice,12} | wartość: {Value,12}";
    }
}
