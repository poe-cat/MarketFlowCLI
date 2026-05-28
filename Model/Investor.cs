using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Model;


public sealed class Investor : IEntity
{

    public Guid Id { get; } = Guid.NewGuid();


    [ReportField("Imię inwestora")]
    public string Name { get; }

    [ReportField("Profil ryzyka")]
    public string RiskProfile { get; private set; }

    
    public Investor(string name, string riskProfile = "Balanced")
    {
        Name = string.IsNullOrWhiteSpace(name) ? "Demo Investor" : name.Trim();
        RiskProfile = riskProfile;
    }

    
    public void ChangeRiskProfile(string riskProfile)
    {
        if (string.IsNullOrWhiteSpace(riskProfile)) throw new ArgumentException("Risk profile cannot be empty.");
        RiskProfile = riskProfile;
    }
}
