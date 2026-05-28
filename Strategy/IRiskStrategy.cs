using MarketFlowCLI.Model;

namespace MarketFlowCLI.Strategy;


public interface IRiskStrategy
{
    string Name { get; }
    RiskLevel CalculateRisk(Portfolio portfolio);
    string Explain(Portfolio portfolio);
}
