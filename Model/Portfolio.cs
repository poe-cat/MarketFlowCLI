using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;

// Portfel inwestora przechowuje gotówkę, pozycje i historię transakcji
public sealed class Portfolio : IEntity, IReportable
{

    // Pozycje indeksowane po symbolu; transakcje w kolejności chronologicznej
    private readonly Dictionary<string, Position> _positions = new();
    private readonly List<Transaction> _transactions = new();


    public Guid Id { get; } = Guid.NewGuid();


    [ReportField("Właściciel")]
    public Investor Owner { get; }

    [ReportField("Gotówka")]
    public Money Cash { get; private set; }

    
    public IReadOnlyCollection<Position> Positions => _positions.Values;
    public IReadOnlyList<Transaction> Transactions => _transactions;

    // Łączna wartość wszystkich otwartych pozycji (wg bieżących cen)
    public Money PositionsValue => Positions.Aggregate(Money.Zero, (sum, position) => sum + position.Value);
    public Money TotalValue => Cash + PositionsValue;
    

    // Indeksator po symbolu (null jeśli brak) i po indeksie numerycznym
    public Position? this[string symbol] => _positions.TryGetValue(symbol.ToUpperInvariant(), out var position) ? position : null;
    public Position this[int index] => _positions.Values.ElementAt(index);

    
    public Portfolio(Investor owner) : this(owner, Money.Zero){ }

    
    public Portfolio(Investor owner, Money initialCash) {
        Owner = owner;
        Cash = initialCash;
    }

    // Wpłata gotówki, rejestruje transakcję typu Deposit
    public void Deposit(Money amount)
    {
        if (amount.Amount <= 0) throw new ArgumentException("Deposit must be positive.");
        Cash += amount;
        _transactions.Add(new Transaction(TransactionType.Deposit, "CASH", 1, amount));
    }

    // Zakup aktywów, pobiera koszt z gotówki, zwiększa lub tworzy pozycję
    public void Buy(Asset asset, int quantity)
    {
        ValidateQuantity(quantity);
        var totalCost = asset.CurrentPrice * quantity;
        if (totalCost > Cash) throw new InvalidOperationException("Insufficient cash.");

        Cash -= totalCost;

        if (_positions.TryGetValue(asset.Symbol, out var position))
        {
            position.Increase(quantity);
        }
        else
        {
            _positions[asset.Symbol] = new Position(asset, quantity);
        }

        _transactions.Add(new Transaction(TransactionType.Buy, asset.Symbol, quantity, asset.CurrentPrice));
    }


    // Sprzedaż aktywów, zmniejsza pozycję, usuwa ją gdy wyzerowana
    public void Sell(Asset asset, int quantity)
    {
        ValidateQuantity(quantity);
        if (!_positions.TryGetValue(asset.Symbol, out var position))
        {
            throw new InvalidOperationException("This asset is not present in the portfolio.");
        }

        position.Decrease(quantity);
        Cash += asset.CurrentPrice * quantity;

        if (position.Quantity == 0)
        {
            _positions.Remove(asset.Symbol);
        }

        _transactions.Add(new Transaction(TransactionType.Sell, asset.Symbol, quantity, asset.CurrentPrice));
    }

    public string ToReportLine() => $"Portfel: {Owner.Name}, gotówka: {Cash}, wartość pozycji: {PositionsValue}, razem: {TotalValue}";

    
    private static void ValidateQuantity(int quantity) {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.");
    }
}
