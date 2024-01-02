using DbUp;
using System.Reflection;

namespace DbSetup;

public static class DbUpSetup
{
    public static void DbSetup()
    {
        EnsureDatabase.For.PostgresqlDatabase("User ID=postgres;Password=115711;Host=localhost;Port=5432;Database=BankData;");

        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase("User ID=postgres;Password=115711;Host=localhost;Port=5432;Database=BankData;")
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
