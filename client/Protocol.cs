using System.Web;

namespace MonkeModManager;

public class Protocol
{
    public static void Handle(string uriString)
    {
        if (!Uri.TryCreate(uriString, UriKind.Absolute, out Uri uri))
            return;

        var query = HttpUtility.ParseQueryString(uri.Query);

        if (uri.Host == "ping")
        {
            Console.Title = $"MonkeModManager v{Program.Version}";

            Console.WriteLine($"Pong");
            Console.WriteLine($"\nPress any key to continue.");

            Console.ReadKey();
            Environment.Exit(0);
        }

        Console.WriteLine($"Invalid URI {uri.Host}.");
        Console.WriteLine($"Please contact the developers of the application that invoked MonkeModManager with this error.");
        
        Console.WriteLine($"\nPress any key to continue.");
        Console.ReadKey();
        Environment.Exit(1);
    }
}
