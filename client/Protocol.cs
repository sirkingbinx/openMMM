using System.Web;
using Spectre.Console;

namespace MonkeModManager;

public class Protocol
{
    public static void Handle(string uriString)
    {
        if (!Uri.TryCreate(uriString, UriKind.Absolute, out Uri uri))
            return;

        Console.Title = $"MonkeModManager v{Program.Version}";

        var query = HttpUtility.ParseQueryString(uri.Query);

        if (uri.Host == "ping")
        {
            Console.WriteLine($"Pong");
            Console.WriteLine($"\nPress any key to continue.");

            Console.ReadKey();
            Environment.Exit(0);
        }

        if (uri.Host == "install")
        {
            string gamePath = Pathing.GetGamePath();

            if (query["name"] is not null && query["url"] is not null)
            {
                AnsiConsole.Progress()
                    .StartAsync(progress =>
                    Installer.Invoke(query["name"], query["url"], progress, gamePath));
                
                Console.WriteLine($"\n{query["name"]} was installed.");
                Console.ReadKey();

                Environment.Exit(0);
            }

            if (query["loader"] is not null)
            {
                string loader = query["loader"].ToLower();

                if (loader == "bepinex")
                    Installer.InstallBepInEx(gamePath);
                else
                    Installer.InstallMelonLoader(gamePath);
                
                Console.WriteLine($"\n{query["loader"]} was installed.");
                Console.ReadKey();

                Environment.Exit(0);
            }
        }

        Console.WriteLine($"Invalid URI {uri.Host}.");
        Console.WriteLine($"Please contact the developers of the application that invoked MonkeModManager with this error.");
        
        Console.WriteLine($"\nPress any key to continue.");
        Console.ReadKey();
        Environment.Exit(1);
    }
}
