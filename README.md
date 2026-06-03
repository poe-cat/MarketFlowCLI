# MarketFlow CLI

MarketFlow CLI to konsolowa aplikacja napisana w języku C#. Projekt symuluje prosty system zarządzania portfelem inwestycyjnym: użytkownik może przeglądać aktywa, wpłacać gotówkę, kupować i sprzedawać instrumenty, ustawiać alerty cenowe, uruchamiać asynchroniczną symulację rynku oraz generować raporty. Temat systemu zarządzania portfelem inwestycyjnym z symulacją rynku został wybrany, ponieważ dobrze odwzorowuje rzeczywisty problem biznesowy, w którym występuje wiele powiązanych ze sobą obiektów, m.in. aktywa finansowe, portfel, inwestor, transakcje, zlecenia,alerty cenowe... 


---

## Uwagi

Ten projekt nie jest próbą stworzenia realnej aplikacji inwestycyjnej. Przygotowałam go jako projekt akademicki, którego głównym celem jest pokazanie mechanizmów programowania obiektowego w praktycznym kontekście. Dlatego część rozwiązań uprościłam.

Najważniejsze uproszczenia to brak połączenia z prawdziwą giełdą, brak bazy danych, brak trwałego zapisu portfela, uproszczony model transakcji oraz losowa symulacja cen. Program nie uwzględnia prowizji, podatków, spreadów, kursów walut ani rzeczywistego ryzyka inwestycyjnego. Ceny aktywów są zmieniane lokalnie, żeby można było pokazać działanie asynchroniczności, zdarzeń i reakcji systemu na zmianę stanu rynku.

Interfejs ograniczyłam do konsoli, ponieważ celem projektu nie było tworzenie aplikacji okienkowej, tylko pokazanie struktury kodu i zależności między obiektami. Z tego samego powodu projekt jest podzielony na kilka modułów. Podział na `Model`, `Market`, `Commands`, `Strategy`, `Reports` i `Cli` ma pokazać separację odpowiedzialności.

Refleksję, strategię ryzyka, komendy i zdarzenia wykorzystałam w ograniczonym zakresie. Każdy z tych elementów ma konkretne miejsce w działaniu programu: refleksja służy do technicznego opisu obiektów, strategie do oceny portfela, komendy do operacji kupna i sprzedaży, a zdarzenia do obsługi zmian cen.

Projekt nie jest użyteczny w sensie komercyjnym. Jest to przykład tego, jak można zaprojektować większą strukturę obiektową wokół prostego problemu domenowego i jednocześnie pokazać mechanizmy języka C# w działającym programie.

---

## Spis treści

1. [Cel projektu](#cel-projektu)
2. [Zakres funkcjonalny](#zakres-funkcjonalny)
3. [Uruchomienie](#uruchomienie-projektu)
4. [Struktura katalogów](#struktura-katalogów)
5. [Model działania aplikacji](#model-działania-aplikacji)
6. [Omówienie wymagań z programowania obiektowego](#omówienie-wymagań-z-programowania-obiektowego)
7. [Dodatkowe elementy zaawansowanej obiektowości](#dodatkowe-elementy-zaawansowanej-obiektowości)
8. [Opis menu aplikacji](#opis-menu-aplikacji)
9. [Przykładowy scenariusz użycia](#przykładowy-scenariusz-użycia)
10. [Możliwe rozszerzenia](#możliwe-rozszerzenia)

---

## Cel projektu

Celem projektu jest implementacja terminalowego systemu zarządzania portfelem inwestycyjnym, który wykorzystuje wszystkie wskazane zagadnienia z zaawansowanego programowania obiektowego:

1. Klasy
2. Konstruktory
3. Właściwości/indeksatory
4. Elementy statyczne
5. Dziedziczenie
6. Polimorfizm
7. Interfejsy/abstrakcja
8. Typy ogólne/kolekcje
9. Delegacje/zdarzenia
10. Przeciążanie operatorów
11. Programowanie asynchroniczne
12. Refleksja

---

## Zakres funkcjonalny

Aplikacja pozwala użytkownikowi na:

- przeglądanie dostępnych aktywów inwestycyjnych,
- wyświetlanie aktualnego stanu portfela,
- wpłacanie gotówki do portfela,
- kupowanie aktywów,
- sprzedawanie aktywów,
- przeglądanie historii transakcji,
- dodawanie alertów cenowych,
- uruchamianie i zatrzymywanie symulacji rynku,
- przeglądanie ostatnich zmian cen,
- generowanie raportu portfela,
- zmianę strategii oceny ryzyka,
- uruchomienie demonstracji refleksji.

Aplikacja działa w terminalu i nie wymaga bazy danych ani połączenia z zewnętrznym API.

---

## Uruchomienie projektu

### Wymagania

- Visual Studio 2022 lub nowszy,
- .NET SDK/Runtime zgodny z wersją wskazaną w pliku `.csproj`, domyślnie `net8.0`.

### Uruchomienie w Visual Studio

1. Otwórz Visual Studio.
2. Wybierz opcję `Open a project or solution`.
3. Wskaż plik `MarketFlowCLI.csproj`.

### Uruchomienie z terminala

W katalogu zawierającym plik `MarketFlowCLI.csproj` wykonaj:

```bash
dotnet restore
dotnet run
```

Jeżeli pojawi się komunikat o braku runtime'u `.NET 8`, można go doinstalować poleceniem:

```powershell
winget install -e --id Microsoft.DotNet.Runtime.8
```

albo zainstalować pełne SDK:

```powershell
winget install -e --id Microsoft.DotNet.SDK.8
```

---

## Struktura katalogów

```text
MarketFlowCLI/
├── Attributes/
│   └── ReportFieldAttribute.cs
├── Cli/
│   └── ConsoleApp.cs
├── Commands/
│   ├── BuyAssetCommand.cs
│   ├── ITradingCommand.cs
│   └── SellAssetCommand.cs
├── Factory/
│   └── AssetFactory.cs
├── Market/
│   ├── MarketEngine.cs
│   ├── MarketPriceChangedHandler.cs
│   ├── PriceAlert.cs
│   └── PriceChangedEventArgs.cs
├── Model/
│   ├── Asset.cs
│   ├── AssetType.cs
│   ├── Crypto.cs
│   ├── ETF.cs
│   ├── IEntity.cs
│   ├── Investor.cs
│   ├── IReportable.cs
│   ├── ITradable.cs
│   ├── Money.cs
│   ├── Portfolio.cs
│   ├── Position.cs
│   ├── RiskLevel.cs
│   ├── Stock.cs
│   ├── Transaction.cs
│   └── TransactionType.cs
├── Repository/
│   └── Repository.cs
├── Reports/
│   ├── PortfolioReportGenerator.cs
│   └── ReflectionPrinter.cs
├── Strategy/
│   ├── AggressiveRiskStrategy.cs
│   ├── BalancedRiskStrategy.cs
│   ├── ConservativeRiskStrategy.cs
│   ├── IRiskStrategy.cs
│   └── PortfolioMath.cs
├── Util/
│   └── InputReader.cs
├── Program.cs
└── MarketFlowCLI.csproj
```

---

## Model działania aplikacji

Głównym obiektem aplikacji jest `ConsoleApp`. Klasa ta odpowiada za obsługę menu, przyjmowanie danych od użytkownika i wywoływanie odpowiednich elementów logiki biznesowej.

Najważniejsze obiekty domenowe:

| Obiekt | Rola w projekcie |
|---|---|
| `Portfolio` | Reprezentuje portfel użytkownika: gotówkę, pozycje i historię transakcji. |
| `Investor` | Reprezentuje właściciela portfela i jego profil ryzyka. |
| `Asset` | Abstrakcyjna klasa bazowa dla aktywów inwestycyjnych. |
| `Stock` | Akcja notowana na giełdzie. |
| `Crypto` | Kryptowaluta o wysokiej zmienności. |
| `ETF` | Fundusz ETF reprezentujący koszyk aktywów. |
| `Money` | Typ wartości reprezentujący kwotę i walutę. |
| `Position` | Pozycja w portfelu, czyli posiadane aktywo i liczba jednostek. |
| `Transaction` | Pojedyncza operacja kupna, sprzedaży lub wpłaty. |
| `MarketEngine` | Silnik rynku odpowiedzialny za listę aktywów i symulację zmian cen. |
| `PriceAlert` | Alert reagujący na zmianę ceny aktywa. |
| `PortfolioReportGenerator` | Generator raportu portfela. |
| `ReflectionPrinter` | Klasa wykorzystująca refleksję do opisu obiektów. |

---

# Omówienie wymagań z programowania obiektowego

## 1. Klasy

Klasa jest podstawową jednostką organizacji kodu w programowaniu obiektowym. W projekcie klasy reprezentują konkretne pojęcia domenowe, a nie przypadkowe kontenery na dane.

Przykłady klas:

- `Portfolio` - portfel użytkownika,
- `Asset` - abstrakcyjna definicja aktywa,
- `Stock`, `Crypto`, `ETF` - konkretne typy aktywów,
- `Money` - kwota pieniędzy,
- `Transaction` - transakcja,
- `MarketEngine` - silnik rynku,
- `PriceAlert` - alert cenowy,
- `PortfolioReportGenerator` - generator raportów.

Przykład:

```csharp
public sealed class Investor : IEntity
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public string RiskProfile { get; private set; }
}
```

`Investor` jest klasą, ponieważ opisuje obiekt posiadający własne dane i zachowanie. Nie jest to tylko luźna struktura danych, ponieważ inwestor może zmieniać profil ryzyka przez metodę `ChangeRiskProfile`.

---

## 2. Konstruktory

Konstruktor odpowiada za poprawne utworzenie obiektu. W projekcie konstruktory są wykorzystywane do inicjalizacji wymaganych danych oraz walidacji argumentów.

Przykład z klasy `Asset`:

```csharp
protected Asset(string symbol, string name, Money currentPrice)
{
    if (string.IsNullOrWhiteSpace(symbol)) throw new ArgumentException("Symbol cannot be empty.");
    if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.");
    if (currentPrice.Amount <= 0) throw new ArgumentException("Current price must be positive.");

    Symbol = symbol.ToUpperInvariant();
    Name = name;
    CurrentPrice = currentPrice;
}
```

Ten konstruktor wymusza, aby każde aktywo miało symbol, nazwę i cenę (z wartością dodatnią). Dzięki temu obiekt `Asset` nie może zostać utworzony jeśli nie spełnia określonych wymagań.

Konstruktory są również wykorzystywane w klasach pochodnych:

```csharp
public Stock(string symbol, string name, Money currentPrice, string exchange)
    : base(symbol, name, currentPrice)
{
    Exchange = exchange;
}
```

Klasa `Stock` przekazuje wspólne dane do konstruktora klasy bazowej `Asset`, a sama dopisuje informację specyficzną dla akcji, czyli giełdę.

---

## 3. Właściwości/indeksatory

### Właściwości

Właściwości w C# umożliwiają kontrolowany dostęp do danych obiektu. W projekcie są używane praktycznie w każdej klasie modelu.

Przykład:

```csharp
public string Symbol { get; }
public string Name { get; }
public Money CurrentPrice { get; private set; }
```

`Symbol` i `Name` można odczytać, ale nie można ich zmienić po utworzeniu aktywa. `CurrentPrice` można odczytać publicznie, ale ustawić tylko wewnątrz klasy `Asset`, ponieważ setter jest prywatny. Cena zmieniana jest przez metodę `UpdatePrice`, która dodatkowo waliduje dane.

### Indeksatory

Indeksator pozwala używać obiektu podobnie jak kolekcji. W projekcie indeksatory znajdują się w klasie `Portfolio`.

Przykład użycia w aplikacji:

```csharp
_portfolio[0]
```

To odwołanie zwraca pierwszą pozycję w portfelu. Dzięki temu `Portfolio` udostępnia dostęp do swoich pozycji bez ujawniania wewnętrznej struktury kolekcji.

W projekcie znajduje się również indeksator po symbolu aktywa, np. koncepcyjnie:

```csharp
_portfolio["CDR"]
```

Taki mechanizm pokazuje, że obiekt może udostępniać dostęp do danych w sposób bardziej naturalny niż przez zwykłe metody typu `GetById`.

---

## 4. Elementy statyczne

Elementy statyczne należą do klasy, a nie do konkretnego obiektu. Są używane tam, gdzie dana operacja nie wymaga stanu instancji.

Przykłady w projekcie:

| Element | Znaczenie |
|---|---|
| `Money.Zero` | Reprezentuje wartość zero w domyślnej walucie. |
| `Money.From(decimal amount)` | Tworzy obiekt `Money` z podanej kwoty. |
| `AssetFactory.CreateDemoAssets()` | Tworzy demonstracyjną listę aktywów. |
| `PortfolioMath.ShareOfType(...)` | Oblicza udział danego typu aktywów w portfelu. |

Przykład:

```csharp
public static Money From(decimal amount) => new(amount, DefaultCurrency);
```

Nie trzeba tworzyć obiektu pomocniczego, żeby zbudować wartość pieniężną. Metoda `From` działa jako czytelna metoda fabrykująca.

---

## 5. Dziedziczenie

Dziedziczenie pozwala utworzyć klasę bazową z częścią wspólną oraz klasy pochodne ze szczegółową implementacją.

W projekcie klasą bazową jest:

```csharp
public abstract class Asset : IEntity, ITradable, IReportable
```

Dziedziczą po niej:

```csharp
public sealed class Stock : Asset
public sealed class Crypto : Asset
public sealed class ETF : Asset
```

Wspólne cechy aktywów znajdują się w `Asset`:

- identyfikator,
- symbol,
- nazwa,
- cena bieżąca,
- metoda aktualizacji ceny,
- metoda raportowania.

Cechy szczegółowe znajdują się w klasach pochodnych:

| Klasa | Cecha specyficzna |
|---|---|
| `Stock` | giełda, np. `GPW` |
| `Crypto` | blockchain, np. `Bitcoin` |
| `ETF` | indeks bazowy, np. `S&P 500` |

Dziedziczenie jest tutaj naturalne, ponieważ akcja, kryptowaluta i ETF są różnymi rodzajami aktywów inwestycyjnych.

---

## 6. Polimorfizm

Polimorfizm oznacza, że różne obiekty mogą być traktowane przez wspólny typ bazowy, ale zachowywać się inaczej.

W projekcie lista rynku przechowuje różne aktywa jako `Asset`:

```csharp
IReadOnlyList<Asset> Assets
```

W tej samej kolekcji mogą znajdować się obiekty:

- `Stock`,
- `Crypto`,
- `ETF`.

Każdy z nich ma własną implementację opisu ryzyka:

```csharp
public override string GetRiskDescription()
```

Przykładowo:

- `Stock` opisuje ryzyko akcji,
- `Crypto` opisuje bardzo wysoką zmienność kryptowaluty,
- `ETF` opisuje dywersyfikację koszyka aktywów.

Kod aplikacji może wywołać:

```csharp
asset.GetRiskDescription()
```

nie wiedząc, czy `asset` jest akcją, kryptowalutą czy ETF-em. To jest praktyczny polimorfizm.

---

## 7. Interfejsy/abstrakcja

Interfejs opisuje kontrakt: mówi, co klasa musi udostępniać, ale nie narzuca, jak ma to zrobić.

W projekcie użyto kilku interfejsów:

| Interfejs | Rola |
|---|---|
| `IEntity` | Wymaga identyfikatora `Id`. |
| `ITradable` | Oznacza obiekt możliwy do handlu. |
| `IReportable` | Oznacza obiekt możliwy do przedstawienia w raporcie. |
| `ITradingCommand` | Reprezentuje komendę transakcyjną. |
| `IRiskStrategy` | Reprezentuje strategię oceny ryzyka. |

Przykład:

```csharp
public interface ITradingCommand
{
    string Name { get; }
    void Execute();
}
```

Dzięki temu aplikacja może wykonać dowolną komendę transakcyjną przez wspólny interfejs:

```csharp
ITradingCommand command = new BuyAssetCommand(_portfolio, asset, quantity);
command.Execute();
```

To jest abstrakcja, ponieważ kod menu nie musi znać szczegółów kupowania. Wystarczy, że obiekt spełnia kontrakt `ITradingCommand`.

---

## 8. Typy ogólne/kolekcje

### Typy ogólne

Typy ogólne pozwalają pisać kod działający dla różnych typów przy zachowaniu bezpieczeństwa typów.

W projekcie przykładem jest:

```csharp
public class Repository<T> where T : IEntity
```

Repozytorium może przechowywać dowolny typ `T`, ale tylko taki, który implementuje `IEntity`. Dzięki temu wiadomo, że każdy przechowywany obiekt ma `Id`.

Przykład:

```csharp
private readonly Dictionary<Guid, T> _items = new();
```

### Kolekcje

Projekt korzysta z kolekcji w naturalnych miejscach:

| Kolekcja | Zastosowanie |
|---|---|
| `List<Asset>` | Lista aktywów dostępnych na rynku. |
| `List<Position>` | Pozycje w portfelu. |
| `List<Transaction>` | Historia transakcji. |
| `Dictionary<Guid, T>` | Przechowywanie obiektów w generycznym repozytorium. |
| `Queue<string>` | Ostatnie zmiany rynku zapisywane jako kolejka komunikatów. |
| `IReadOnlyList<T>` / `IReadOnlyCollection<T>` | Bezpieczne udostępnianie kolekcji bez pozwalania na modyfikację z zewnątrz. |

Kolekcje nie są tu użyte tylko formalnie. System portfela inwestycyjnego naturalnie wymaga listy aktywów, listy pozycji i historii transakcji.

---

## 9. Delegacje/zdarzenia

Delegat w C# określa sygnaturę metody, która może zostać przypisana jako obsługa zdarzenia. Zdarzenie pozwala obiektowi powiadamiać inne obiekty, że coś się wydarzyło.

W projekcie delegat znajduje się w pliku `MarketPriceChangedHandler.cs`:

```csharp
public delegate void MarketPriceChangedHandler(object? sender, PriceChangedEventArgs args);
```

Silnik rynku udostępnia zdarzenie:

```csharp
public event MarketPriceChangedHandler? PriceChanged;
```

Gdy cena aktywa zmienia się w `MarketEngine`, wywoływane jest zdarzenie `PriceChanged`. Reagują na nie między innymi:

- `ConsoleApp`, która zapisuje ostatnie zmiany rynku,
- `PriceAlert`, który sprawdza, czy cena osiągnęła wskazany poziom.

Przykład podpięcia obsługi zdarzenia:

```csharp
_market.PriceChanged += TrackMarketPriceChange;
_market.PriceChanged += alert.HandlePriceChanged;
```

Dzięki temu `MarketEngine` nie musi znać szczegółów alertów ani interfejsu użytkownika. On tylko publikuje informację o zmianie ceny. Inne obiekty same decydują, jak na nią zareagować.

To jest zastosowanie modelu Observer przez mechanizm zdarzeń C#.

---

## 10. Przeciążanie operatorów

Przeciążanie operatorów pozwala zdefiniować, jak operatory takie jak `+`, `-`, `*`, `/`, `>` lub `<` mają działać dla własnych typów.

W projekcie przeciążanie operatorów znajduje się w strukturze `Money`.

Przykłady:

```csharp
public static Money operator +(Money left, Money right)
public static Money operator -(Money left, Money right)
public static Money operator *(Money money, decimal multiplier)
public static bool operator >(Money left, Money right)
public static bool operator <(Money left, Money right)
```

Dzięki temu w kodzie można pisać:

```csharp
Cash -= totalCost;
Cash += income;
position.Value > Money.Zero;
```

Zamiast tworzyć sztuczne metody typu `AddMoney` i `SubtractMoney`, klasa `Money` zachowuje się jak rzeczywista wartość pieniężna. Jednocześnie typ pilnuje waluty i nie pozwala dodawać kwot w różnych walutach.

Przykład zabezpieczenia:

```csharp
private static void EnsureSameCurrency(Money left, Money right)
```

W projekcie przeciążanie operstorów jest powiązane z realną logiką finansową.

---

## 11. Programowanie asynchroniczne

Programowanie asynchroniczne pozwala wykonywać operacje w tle bez blokowania całej aplikacji.

W projekcie mechanizm ten służy do symulacji rynku. Po uruchomieniu opcji menu `9` aplikacja uruchamia zadanie asynchroniczne, które cyklicznie zmienia ceny aktywów.

Najważniejsze elementy:

| Element | Rola |
|---|---|
| `Task` | Reprezentuje operację wykonywaną asynchronicznie. |
| `async` / `await` | Pozwala uruchamiać i kontrolować operacje asynchroniczne. |
| `CancellationToken` | Pozwala bezpiecznie zatrzymać symulację. |
| `CancellationTokenSource` | Obiekt wysyłający sygnał anulowania. |

W `ConsoleApp` znajduje się metoda:

```csharp
private async Task ToggleMarketSimulationAsync()
```

Jeżeli symulacja nie działa, metoda ją uruchamia. Jeżeli już działa, metoda ją zatrzymuje.

Mechanizm zatrzymania opiera się na:

```csharp
_marketCancellation.Cancel();
await _marketTask;
```

Zmiany cen nie są wypisywane automatycznie w każdej sekundzie, ponieważ powodowałoby to chaos w terminalu (mieszałoby tekst menu z komunikatami tła). Zamiast tego aplikacja zapisuje ostatnie zmiany do kolejki, a użytkownik może je zobaczyć przez opcję `13`.

To pokazuje asynchroniczność: proces rynku działa w tle, a użytkownik nadal może korzystać z menu.

---

## 12. Refleksja

Refleksja pozwala analizować typy, właściwości, pola i atrybuty w czasie działania programu.

W projekcie refleksja znajduje się w klasie:

```csharp
ReflectionPrinter
```

Klasa ta odczytuje publiczne elementy obiektu oznaczone własnym atrybutem:

```csharp
[ReportField("Symbol")]
public string Symbol { get; }
```

Atrybut zdefiniowano jako:

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ReportFieldAttribute : Attribute
{
    public string Label { get; }
}
```

`ReflectionPrinter` wyszukuje elementy oznaczone `ReportFieldAttribute`:

```csharp
var members = type
    .GetMembers(BindingFlags.Public | BindingFlags.Instance)
    .Where(member => member.GetCustomAttribute<ReportFieldAttribute>() is not null);
```

Następnie pobiera ich wartości i generuje opis obiektu.

W menu aplikacji odpowiada za to opcja:

```text
11. Pokaż opis techniczny przez refleksję
```

Refleksja służy tutaj do automatycznego raportowania obiektów bez ręcznego wypisywania każdej właściwości.

---

# Dodatkowe elementy zaawansowanej obiektowości

## 1. Wzorzec Strategy

Wzorzec Strategy pozwala wymieniać algorytm działania bez zmiany klasy, która z niego korzysta.

W projekcie strategia dotyczy oceny ryzyka portfela.

Interfejs:

```csharp
public interface IRiskStrategy
{
    string Name { get; }
    RiskLevel CalculateRisk(Portfolio portfolio);
    string Explain(Portfolio portfolio);
}
```

Implementacje:

- `ConservativeRiskStrategy`,
- `BalancedRiskStrategy`,
- `AggressiveRiskStrategy`.

Użytkownik może zmienić strategię przez opcję menu `12`. Ten sam portfel może zostać oceniony inaczej zależnie od wybranego profilu ryzyka.

---

## 2. Wzorzec Factory

Fabryka odpowiada za tworzenie obiektów bez rozrzucania konstruktorów po całym kodzie aplikacji.

W projekcie służy do tworzenia aktywów:

```csharp
public static Asset Create(AssetType type, string symbol, string name, Money price)
```

oraz aktywów demo:

```csharp
public static IReadOnlyList<Asset> CreateDemoAssets()
```

Dzięki temu logika tworzenia obiektów `Stock`, `Crypto` i `ETF` jest skupiona w jednym miejscu.

---

## 3. Wzorzec Command

Wzorzec Command opakowuje operację jako osobny obiekt.

W projekcie komendami są:

- `BuyAssetCommand`,
- `SellAssetCommand`.

Obie implementują:

```csharp
public interface ITradingCommand
{
    string Name { get; }
    void Execute();
}
```

Dzięki temu menu nie wykonuje bezpośrednio całej logiki kupna i sprzedaży. Tworzy komendę i uruchamia ją przez `Execute()`.

---

## 4. Observer przez zdarzenia

Mechanizm zdarzeń C# realizuje wzorzec Observer. `MarketEngine` publikuje zdarzenie zmiany ceny, a inne obiekty reagują.

Obserwatorami są:

- aplikacja konsolowa zapisująca historię zmian,
- alerty cenowe.

To zmniejsza zależności między klasami. Silnik rynku nie musi wiedzieć, ile alertów istnieje ani co robi interfejs użytkownika.

---

## 5. Value Object

`Money` jest przykładem obiektu wartości. Oznacza to, że reprezentuje wartość, a nie osobny byt z tożsamością.

Cechy `Money`:

- jest strukturą `readonly struct`,
- zawiera kwotę i walutę,
- zaokrągla wartość do dwóch miejsc po przecinku,
- przeciąża operatory arytmetyczne i porównania,
- sprawdza zgodność walut.

Dzięki temu operacje finansowe są bardziej czytelne i bezpieczne niż przy używaniu samego typu `decimal`.

---

## 6. Custom Attribute

Projekt definiuje własny atrybut:

```csharp
ReportFieldAttribute
```

Atrybut oznacza właściwości, które mają zostać pokazane przez mechanizm refleksji.

Przykład:

```csharp
[ReportField("Cena bieżąca")]
public Money CurrentPrice { get; private set; }
```

Metadane zapisane w kodzie mogą zostać odczytane podczas działania programu.

---

# Opis menu aplikacji

Po uruchomieniu program pokazuje menu:

```text
1. Pokaż dostępne aktywa
2. Pokaż portfel
3. Wpłać gotówkę
4. Kup aktywo
5. Sprzedaj aktywo
6. Pokaż historię transakcji
7. Dodaj alert cenowy
8. Pokaż alerty cenowe
9. Uruchom / zatrzymaj asynchroniczną symulację rynku
10. Wygeneruj raport portfela
11. Pokaż opis techniczny przez refleksję
12. Zmień strategię oceny ryzyka
13. Pokaż ostatnie zmiany rynku
0. Wyjście
```

## 1. Pokaż dostępne aktywa

Wyświetla listę aktywów dostępnych na rynku. Każde aktywo jest obiektem klasy pochodnej po `Asset`.

Pokazywane są między innymi:

- symbol,
- nazwa,
- typ aktywa,
- cena,
- poziom ryzyka,
- opis ryzyka.

Opcja demonstruje polimorfizm, ponieważ różne klasy aktywów są obsługiwane przez wspólny typ `Asset`.

## 2. Pokaż portfel

Wyświetla gotówkę, wartość pozycji i łączną wartość portfela. Jeżeli portfel zawiera aktywa, pokazuje też przykład użycia indeksatora:

```csharp
_portfolio[0]
```

## 3. Wpłać gotówkę

Pozwala zwiększyć ilość gotówki w portfelu. Operacja tworzy transakcję typu `Deposit`.

## 4. Kup aktywo

Użytkownik podaje symbol aktywa i liczbę jednostek. Program tworzy obiekt `BuyAssetCommand`, a następnie wykonuje go przez `Execute()`.

## 5. Sprzedaj aktywo

Działa analogicznie do kupna, ale używa klasy `SellAssetCommand`. Program sprawdza, czy użytkownik posiada wystarczającą liczbę jednostek aktywa.

## 6. Pokaż historię transakcji

Wyświetla historię transakcji zapisanych w portfelu. Każda transakcja jest osobnym obiektem `Transaction`.

## 7. Dodaj alert cenowy

Tworzy alert dla wskazanego aktywa. Alert zostaje podpięty pod zdarzenie `PriceChanged` silnika rynku.

Gdy cena spełni warunek alertu, obiekt `PriceAlert` reaguje na zdarzenie.

## 8. Pokaż alerty cenowe

Wyświetla aktywne i wykonane alerty cenowe.

## 9. Uruchom/zatrzymaj asynchroniczną symulację rynku

Uruchamia albo zatrzymuje działanie rynku w tle.

Pierwsze wybranie opcji `9` startuje symulację. Drugie wybranie opcji `9` zatrzymuje ją.

Symulacja działa asynchronicznie. Oznacza to, że ceny mogą zmieniać się w tle, a użytkownik nadal może korzystać z menu.

## 10. Wygeneruj raport portfela

Tworzy raport portfela z użyciem klasy `PortfolioReportGenerator`. Raport zawiera:

- stan portfela,
- listę pozycji,
- wybraną strategię ryzyka,
- ocenę ryzyka,
- opis zastosowanej strategii.

## 11. Pokaż opis techniczny przez refleksję

Uruchamia mechanizm `ReflectionPrinter`, który analizuje wybrany obiekt w czasie działania programu.

Użytkownik może opisać między innymi:

- portfel,
- pierwsze aktywo rynku,
- gotówkę jako obiekt `Money`.

## 12. Zmień strategię oceny ryzyka

Pozwala wybrać jedną z trzech strategii:

- `Conservative`,
- `Balanced`,
- `Aggressive`.

Zmiana strategii wpływa na wynik raportu portfela.

## 13. Pokaż ostatnie zmiany rynku

Pokazuje ostatnie zmiany cen zapisane podczas działania symulacji. Dane są przechowywane w kolejce `Queue<string>`.

## 0. Wyjście

Kończy działanie aplikacji. Przed zamknięciem program zatrzymuje symulację rynku, jeżeli była uruchomiona.

---

# Przykładowy scenariusz użycia

1. Uruchom aplikację.
2. Wybierz opcję `1`, aby zobaczyć dostępne aktywa.
3. Wybierz opcję `3`, aby wpłacić gotówkę.
4. Wybierz opcję `4`, aby kupić aktywo, np. `CDR`.
5. Wybierz opcję `2`, aby sprawdzić stan portfela.
6. Wybierz opcję `7`, aby ustawić alert cenowy.
7. Wybierz opcję `9`, aby uruchomić symulację rynku.
8. Poczekaj kilka sekund.
9. Wybierz opcję `13`, aby zobaczyć ostatnie zmiany cen.
10. Wybierz opcję `10`, aby wygenerować raport portfela.
11. Wybierz opcję `11`, aby zobaczyć demonstrację refleksji.
12. Wybierz opcję `9`, aby zatrzymać symulację.
13. Wybierz opcję `0`, aby zakończyć program.

---

# Możliwe rozszerzenia

Projekt można rozbudować o:

- zapis portfela do pliku JSON,
- odczyt portfela z pliku,
- import większej listy aktywów,
- dodatkowe typy aktywów, np. obligacje lub surowce,
- testy jednostkowe,
- logowanie zdarzeń do pliku,
- prostą warstwę konfiguracji,
- obsługę kilku portfeli,
- bardziej rozbudowany system zleceń,
- mechanizm cofania komend transakcyjnych.

---

# Podsumowanie

MarketFlow CLI pokazuje zaawansowane programowanie obiektowe na przykładzie spójnego systemu domenowego. Najważniejsze mechanizmy języka C# zostały użyte w konkretnych miejscach logiki programu: dziedziczenie opisuje hierarchię aktywów, polimorfizm pozwala traktować różne aktywa wspólnie, zdarzenia obsługują reakcję na zmiany cen, przeciążanie operatorów upraszcza operacje na pieniądzach, asynchroniczność umożliwia działanie symulacji rynku w tle, a refleksja automatyzuje opis wybranych obiektów.

