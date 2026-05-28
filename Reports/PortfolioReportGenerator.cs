using System.Text;
using MarketFlowCLI.Model;
using MarketFlowCLI.Strategy;

namespace MarketFlowCLI.Reports;


public sealed class PortfolioReportGenerator
{

    private readonly IRiskStrategy _riskStrategy;


    public PortfolioReportGenerator(IRiskStrategy riskStrategy)
    {
        _riskStrategy = riskStrategy;
    }


    public string Generate(Portfolio portfolio)
    {
        var builder = new StringBuilder();
        builder.AppendLine("=== RAPORT PORTFELA ===");
        builder.AppendLine(portfolio.ToReportLine());
        builder.AppendLine();
        builder.AppendLine("Pozycje:");

        if (!portfolio.Positions.Any())
        {
            builder.AppendLine("Brak aktywów w portfelu.");
        }
        else
        {
            foreach (var position in portfolio.Positions)
            {
                builder.AppendLine(position.ToReportLine());
            }
        }


        builder.AppendLine();
        builder.AppendLine($"Strategia ryzyka: {_riskStrategy.Name}");
        builder.AppendLine($"Ocena ryzyka: {_riskStrategy.CalculateRisk(portfolio)}");
        builder.AppendLine(_riskStrategy.Explain(portfolio));
        return builder.ToString();
    }
}
