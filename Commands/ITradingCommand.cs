namespace MarketFlowCLI.Commands;


public interface ITradingCommand
{
    string Name { get; }
    void Execute();
}
