namespace MarketFlowCLI.Model;

public interface ITradable
{
    string Symbol { get; }
    string Name { get; }
    Money CurrentPrice { get; }
}
