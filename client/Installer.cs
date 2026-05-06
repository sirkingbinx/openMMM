using System.IO.Compression;
using Spectre.Console;

namespace MonkeModManager;

public class Installer
{
    public const string VerifiedApiUrl = "https://raw.githubusercontent.com/sirkingbinx/openMMM/refs/heads/master/api/Verified.txt";

    public static string[] VerifiedCache = [];

    public static async Task Invoke(string displayName, string url, ProgressContext progress = null, string path = "")
    {
        if (path == "")
            path = Pathing.GetGamePath();
        
        if (VerifiedCache.Length == 0)
        {
            try { VerifiedCache = new HttpClient().GetStringAsync(VerifiedApiUrl).GetAwaiter().GetResult().Split("\n"); }
            catch { }
        }

        string host = new Uri(url).Host;
        bool verified = VerifiedCache.Any(verifiedUrl => url.StartsWith(verifiedUrl));

        string verifiedText = verified ? "[green]Verified[/]" : "";

        bool install = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title($"{displayName} {verifiedText}\n\nDo you want to install this?")
            .AddChoices("Yes", "No")) == "Yes";

        const int steps = 2;
        var task = progress?.AddTask($"Installing {displayName}");

        var updateStep = (int now) => {
            task?.Increment(now * 100 / steps);
        };

        updateStep(1);

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", $"MonkeModManager/{Program.Version}");

        var fileData = await httpClient.GetStreamAsync(url);

        if (Path.GetExtension(url) == ".zip")
        {
            using var archive = new ZipArchive(fileData);
            archive.ExtractToDirectory(path, overwriteFiles: true);
        }
        else if (Path.GetExtension(url) == ".dll")
        {
            using var fileStream = new FileStream(Path.Combine(Pathing.GetModLoaderPluginsPath(path), Path.GetFileName(url)), FileMode.Create);
            fileData.CopyTo(fileStream);
            fileStream.Flush();

            fileStream.Close();
        }

        updateStep(2);
    }

    public static void InstallBepInEx(string path) =>
        AnsiConsole.Progress()
            .StartAsync(progress =>
            Invoke("BepInEx v5.4.23.5", "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip", progress, path));
    
    public static void InstallMelonLoader(string path) =>
        AnsiConsole.Progress()
            .StartAsync(progress =>
            Invoke("MelonLoader v0.7.2", "https://github.com/LavaGang/MelonLoader/releases/download/v0.7.2/MelonLoader.x64.zip", progress, path));
}