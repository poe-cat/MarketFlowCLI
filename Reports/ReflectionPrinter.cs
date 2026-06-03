using System.Reflection;
using MarketFlowCLI.Attributes;

namespace MarketFlowCLI.Reports;


// Narzędzie do opisu obiektów przez refleksję: wypisuje właściwości oznaczone [ReportField]
public static class ReflectionPrinter
{

    public static string DescribeObject(object target)
    {
        var type = target.GetType();
        var lines = new List<string>
        {
            $"Typ obiektu: {type.FullName}",
            "Pola raportowane przez refleksję:"
        };

        var members = type
            .GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(member => member.GetCustomAttribute<ReportFieldAttribute>() is not null);

        foreach (var member in members)
        {
            var attribute = member.GetCustomAttribute<ReportFieldAttribute>()!;
            var value = member switch
            {
                PropertyInfo property => property.GetValue(target),
                FieldInfo field => field.GetValue(target),
                _ => null
            };

            lines.Add($"- {attribute.Label}: {value}");
        }

        return string.Join(Environment.NewLine, lines);
    }
}
