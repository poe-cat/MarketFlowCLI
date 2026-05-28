namespace MarketFlowCLI.Util;


public static class InputReader
{
    
    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("Wartość nie może być pusta.");
        }
    }


    public static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (int.TryParse(input, out var number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Podaj dodatnią liczbę całkowitą.");
        }
    }


    public static decimal ReadPositiveDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Replace(',', '.');

            if (decimal.TryParse(input, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out var number) && number > 0)
            {
                return number;
            }

            Console.WriteLine("Podaj dodatnią kwotę.");
        }
    }
}
