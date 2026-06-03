using MarketFlowCLI.Commands;
using MarketFlowCLI.Factory;
using MarketFlowCLI.Market;
using MarketFlowCLI.Model;
using MarketFlowCLI.Reports;
using MarketFlowCLI.Strategy;
using MarketFlowCLI.Util;


namespace MarketFlowCLI.Cli;


// Główna aplikacja konsolowa. Obsługuje pętlę menu i deleguje akcje do odpowiednich klas
public sealed class ConsoleApp
{
    private readonly MarketEngine _market;
    private readonly Portfolio _portfolio;
    private readonly List<PriceAlert> _alerts = new();
    private IRiskStrategy _riskStrategy = new BalancedRiskStrategy();

    // Token i task do zarządzania asynchroniczną symulacją rynku (opcja 9)
    private CancellationTokenSource? _marketCancellation;
    private Task? _marketTask;



    public ConsoleApp()
    {
        _market = new MarketEngine(AssetFactory.CreateDemoAssets());
        _portfolio = new Portfolio(new Investor("Student", "Balanced"), Money.From(10000m));

        // Subskrypcja zdarzenia - wyświetla zmiany cen w trakcie symulacji
        _market.PriceChanged += ShowMarketPriceChange;
    }


    // Główna pętla aplikacji: czyta wybór z konsoli i wywołuje odpowiednią metodę
    public async Task RunAsync()
    {
        Console.Title = "MarketFlow CLI";
        Console.WriteLine("MarketFlow CLI: system zarządzania portfelem inwestycyjnym");
        Console.WriteLine("Portfel demo startuje z gotówką 10000,00 PLN.");

        var running = true;
        while (running)
        {
            PrintMenu();
            Console.Write("Wybierz opcję: ");
            var choice = Console.ReadLine()?.Trim();
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1": ShowAssets(); break;
                    case "2": ShowPortfolio(); break;
                    case "3": DepositCash(); break;
                    case "4": ExecuteBuy(); break;
                    case "5": ExecuteSell(); break;
                    case "6": ShowTransactions(); break;
                    case "7": AddPriceAlert(); break;
                    case "8": ShowAlerts(); break;
                    case "9": await ToggleMarketSimulationAsync(); break;
                    case "10": GeneratePortfolioReport(); break;
                    case "11": ShowReflectionDemo(); break;
                    case "12": ChangeRiskStrategy(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Nieznana opcja."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

            Console.WriteLine();
        }

        await StopMarketSimulationAsync();
        Console.WriteLine("Zamknięto MarketFlow CLI.");
    }



    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("***** MENU *****");
        Console.WriteLine("1. Pokaż dostępne aktywa");
        Console.WriteLine("2. Pokaż portfel");
        Console.WriteLine("3. Wpłać gotówkę");
        Console.WriteLine("4. Kup aktywa");
        Console.WriteLine("5. Sprzedaj aktywa");
        Console.WriteLine("6. Pokaż historię transakcji");
        Console.WriteLine("7. Dodaj alert cenowy");
        Console.WriteLine("8. Pokaż alerty cenowe");
        Console.WriteLine("9. Uruchom/zatrzymaj asynchroniczną symulację rynku");
        Console.WriteLine("10. Wygeneruj raport portfela");
        Console.WriteLine("11. Pokaż opis techniczny przez refleksję");
        Console.WriteLine("12. Zmień strategię oceny ryzyka");
        Console.WriteLine("0. Wyjście");
    }


    private void ShowAssets()
    {
        Console.WriteLine("Dostępne aktywa:");
        foreach (var asset in _market.Assets)
        {
            Console.WriteLine(asset.ToReportLine());
            Console.WriteLine($"       {asset.GetRiskDescription()}");
        }
    }

    
    // Handler zdarzenia PriceChanged
    private void ShowMarketPriceChange(object? sender, PriceChangedEventArgs e)
    {
        if (!_market.IsRunning)
        {
            return;
        }

        Console.WriteLine(
            $"[RYNEK] {e.Asset.Symbol}: {e.OldPrice} -> {e.NewPrice}");
    }


    private void ShowPortfolio()
    {
        Console.WriteLine(_portfolio.ToReportLine());
        Console.WriteLine();

        if (!_portfolio.Positions.Any())
        {
            Console.WriteLine("Brak pozycji w portfelu.");
            return;
        }

        Console.WriteLine("Pozycje:");
        foreach (var position in _portfolio.Positions)
        {
            Console.WriteLine(position.ToReportLine());
        }

        Console.WriteLine();
        Console.WriteLine("Przykład indeksatora: pierwsza pozycja portfela, jeśli istnieje:");
        Console.WriteLine(_portfolio[0].ToReportLine());
    }


    private void DepositCash()
    {
        var amount = InputReader.ReadPositiveDecimal("Podaj kwotę wpłaty: ");
        _portfolio.Deposit(Money.From(amount));
        Console.WriteLine($"Wpłacono {Money.From(amount)}.");
    }


    // Tworzy i wykonuje komendę BuyAssetCommand (wzorzec Command)
    private void ExecuteBuy()
    {
        var asset = ReadAssetBySymbol();
        var quantity = InputReader.ReadPositiveInt("Podaj liczbę jednostek: ");
        ITradingCommand command = new BuyAssetCommand(_portfolio, asset, quantity);
        command.Execute();
        Console.WriteLine($"Wykonano komendę: {command.Name}.");
    }


    // Tworzy i wykonuje komendę SellAssetCommand (wzorzec Command)
    private void ExecuteSell()
    {
        var asset = ReadAssetBySymbol();
        var quantity = InputReader.ReadPositiveInt("Podaj liczbę jednostek: ");
        ITradingCommand command = new SellAssetCommand(_portfolio, asset, quantity);
        command.Execute();
        Console.WriteLine($"Wykonano komendę: {command.Name}.");
    }


    private void ShowTransactions()
    {
        if (!_portfolio.Transactions.Any())
        {
            Console.WriteLine("Brak transakcji.");
            return;
        }

        Console.WriteLine("Historia transakcji:");
        foreach (var transaction in _portfolio.Transactions)
        {
            Console.WriteLine(transaction.ToReportLine());
        }
    }


    private void AddPriceAlert()
    {
        var asset = ReadAssetBySymbol();
        var target = InputReader.ReadPositiveDecimal("Podaj cenę docelową: ");
        Console.Write("Alert po przekroczeniu ceny w górę? [t/n]: ");
        var answer = Console.ReadLine();
        var triggerWhenAbove = answer?.Trim().Equals("t", StringComparison.OrdinalIgnoreCase) == true;


        var alert = new PriceAlert(asset.Symbol, Money.From(target), triggerWhenAbove);
        _alerts.Add(alert);
        _market.PriceChanged += alert.HandlePriceChanged;
        Console.WriteLine($"Dodano alert: {alert}");
    }


    private void ShowAlerts()
    {
        if (!_alerts.Any())
        {
            Console.WriteLine("Brak alertów.");
            return;
        }

        foreach (var alert in _alerts)
        {
            Console.WriteLine(alert);
        }
    }


    // Przełącza symulację rynku: start lub stop w zależności od bieżącego stanu
    private async Task ToggleMarketSimulationAsync()
    {
        if (_market.IsRunning)
        {
            await StopMarketSimulationAsync();
            Console.WriteLine("Zatrzymano symulację rynku.");
            return;
        }

        _marketCancellation = new CancellationTokenSource();
        _marketTask = _market.StartAsync(_marketCancellation.Token);
        Console.WriteLine("Uruchomiono asynchroniczną symulację rynku. Aby zatrzymać, wybierz 9");
    }


    private async Task StopMarketSimulationAsync()
    {
        if (_marketCancellation is null || _marketTask is null)
        {
            return;
        }

        _marketCancellation.Cancel();
        await _marketTask;
        _marketCancellation.Dispose();
        _marketCancellation = null;
        _marketTask = null;
    }


    private void GeneratePortfolioReport()
    {
        var generator = new PortfolioReportGenerator(_riskStrategy);
        Console.WriteLine(generator.Generate(_portfolio));
    }


    private void ShowReflectionDemo()
    {
        Console.WriteLine("Wybierz obiekt do opisu:");
        Console.WriteLine("1. Portfel");
        Console.WriteLine("2. Pierwsze aktywa rynku");
        Console.WriteLine("3. Gotówka jako obiekt Money");
        Console.Write("Opcja: ");
        var choice = Console.ReadLine();

        object target = choice switch
        {
            "1" => _portfolio,
            "2" => _market.Assets.First(),
            "3" => _portfolio.Cash,
            _ => _portfolio
        };

        Console.WriteLine(ReflectionPrinter.DescribeObject(target));
    }


    // Zmienia aktywną strategię ryzyka (wzorzec proj Strategy) i aktualizuje profil inwestora
    private void ChangeRiskStrategy()
    {
        Console.WriteLine("Wybierz strategię:");
        Console.WriteLine("1. Conservative");
        Console.WriteLine("2. Balanced");
        Console.WriteLine("3. Aggressive");
        Console.Write("Opcja: ");
        var choice = Console.ReadLine();


        _riskStrategy = choice switch
        {
            "1" => new ConservativeRiskStrategy(),
            "2" => new BalancedRiskStrategy(),
            "3" => new AggressiveRiskStrategy(),
            _ => _riskStrategy
        };

        _portfolio.Owner.ChangeRiskProfile(_riskStrategy.Name);
        Console.WriteLine($"Ustawiono strategię: {_riskStrategy.Name}.");
    }


    // Wczytuje symbol z konsoli i wyszukuje aktywa, wrzuca wyjątek jeśli nie istnieje
    private Asset ReadAssetBySymbol()
    {

        var symbol = InputReader.ReadRequiredString("Podaj symbol aktywa: ");
        var asset = _market.FindAsset(symbol);
        if (asset is null)
        {
            throw new InvalidOperationException("Nie znaleziono aktywa o podanym symbolu.");
        }

        return asset;
    }
}
