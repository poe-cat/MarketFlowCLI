using System.Globalization;
using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;

// Value object reprezentujący kwotę pieniężną z walutą
public readonly struct Money : IComparable<Money>, IEquatable<Money>
{

    [ReportField("Kwota")]
    public decimal Amount { get; }

    [ReportField("Waluta")]
    public string Currency { get; }

    public static string DefaultCurrency { get; } = "PLN";
    public static Money Zero => new(0m, DefaultCurrency);

    
    public Money(decimal amount, string currency = "PLN")
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be empty.", nameof(currency));
        }

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        Currency = currency.ToUpperInvariant();
    }


    // Fabryka: tworzy Money w domyślnej walucie PLN
    public static Money From(decimal amount) => new(amount, DefaultCurrency);


    // Parsuje string z kwotą (przecinek lub kropka dziesiętna)
    public static Money Parse(string input)
    {
        if (!decimal.TryParse(input.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
        {
            throw new FormatException("Invalid money value.");
        }

        return From(amount);
    }

    // Operatory arytmetyczne: rzucają wyjątek przy mieszaniu walut
    public static Money operator +(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    
    public static Money operator -(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount - right.Amount, left.Currency);
    }


    public static Money operator *(Money money, decimal multiplier) => new(money.Amount * multiplier, money.Currency);
    public static Money operator *(decimal multiplier, Money money) => money * multiplier;
    public static Money operator /(Money money, decimal divisor) => divisor == 0 ? throw new DivideByZeroException() : new Money(money.Amount / divisor, money.Currency);

    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;
    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;


    public int CompareTo(Money other)
    {
        EnsureSameCurrency(this, other);
        return Amount.CompareTo(other.Amount);
    }


    public bool Equals(Money other) => Amount == other.Amount && Currency == other.Currency;
    public override bool Equals(object? obj) => obj is Money other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Amount:0.00} {Currency}";


    private static void EnsureSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException($"Currency mismatch: {left.Currency} vs {right.Currency}.");
        }
    }
}
