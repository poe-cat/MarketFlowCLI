using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;


public sealed class Transaction : IEntity, IReportable
{

    public Guid Id { get; } = Guid.NewGuid();

    [ReportField("Typ")]
    public TransactionType Type { get; }

    [ReportField("Symbol")]
    public string Symbol { get; }

    [ReportField("Liczba")]
    public int Quantity { get; }

    [ReportField("Cena jednostkowa")]
    public Money UnitPrice { get; }

    [ReportField("Wartość")]
    public Money TotalValue => UnitPrice * Quantity;

    [ReportField("Data")]
    public DateTime Timestamp { get; }


    public Transaction(TransactionType type, string symbol, int quantity, Money unitPrice)
    {
        Type = type;
        Symbol = symbol.ToUpperInvariant();
        Quantity = quantity;
        UnitPrice = unitPrice;
        Timestamp = DateTime.Now;
    }

    public string ToReportLine()
    {
        return $"{Timestamp:yyyy-MM-dd HH:mm:ss} | {Type,-7} | {Symbol,-6} | {Quantity,4} x {UnitPrice,12} | {TotalValue,12}";
    }
}
