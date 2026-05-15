namespace MonkeModManager;

public class Program
{
    public const string Version = "2.0.0";

    [STAThread]
    static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        
        Registry.RegisterProtocol(Environment.ProcessPath);

        if (args.Length > 0 && args[0].StartsWith("mmm://"))
        {
            Protocol.Handle(args[0]);
        }
        
        if (args.Length == 0)
        {
            Console.WriteLine($"MonkeModManager v{Version}");
            Console.WriteLine($"(C) 2026 SirKingBinx / MIT License");
        }
    }
}
