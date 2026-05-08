namespace MonkeModManager;

public class Program
{
    public const string Version = "2.0.0";

    public const string BepInExVersion = "5.4.23.5";
    public const string BepInExURL = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip";

    public const string MelonLoaderVersion = "0.7.2";
    public const string MelonLoaderURL = "https://github.com/LavaGang/MelonLoader/releases/download/v0.7.2/MelonLoader.x64.zip";

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
