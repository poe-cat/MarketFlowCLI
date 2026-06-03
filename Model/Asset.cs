using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;

// Klasa bazowa dla wszystkich aktywów (akcje, krypto, ETF)
// Implementuje IEntity (unikalny GUID), ITradable (symbol + cena) i IReportable (linia raportu)
public abstract class Asset : IEntity, ITradable, IReportable
{

    public Guid Id { get; } = Guid.NewGuid();

    
    [ReportField("Symbol")]
    public string Symbol { get; }

    [ReportField("Nazwa")]
    public string Name { get; }

    [ReportField("Cena bieżąca")]
    public Money CurrentPrice { get; private set; }

    public abstract AssetType Type { get; }
    public abstract RiskLevel BaseRisk { get; }

    
    protected Asset(string symbol, string name, Money currentPrice)
    {
        if (string.IsNullOrWhiteSpace(symbol)) throw new ArgumentException("Symbol cannot be empty.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
        if (currentPrice.Amount <= 0) throw new ArgumentException("Current price must be positive.");

        Symbol = symbol.ToUpperInvariant();
        Name = name;
        CurrentPrice = currentPrice;
    }

   // Aktualizuje bieżącą cenę - wywoływane przez MarketEngine przy każdym ticku
    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Amount <= 0) throw new ArgumentException("New price must be positive.");
        CurrentPrice = newPrice;
    }


    // Zwraca opis ryzyka specyficzny dla podtypu (Stock/Crypto/ETF)
    public abstract string GetRiskDescription();

    
    public virtual string ToReportLine()
    {
        return $"{Symbol,-6} | {Name,-24} | {Type,-6} | {CurrentPrice,12} | ryzyko: {BaseRisk}";
    }

    
    public override string ToString() => ToReportLine();
}
