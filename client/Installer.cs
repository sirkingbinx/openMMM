using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using Spectre.Console;

namespace MonkeModManager;

public class Installer
{
    public const string VerifiedApi = "https://api.sirkingbinx.dev/";

    public static async Task Invoke(string displayName, string url, ProgressContext progress = null, string path = "")
    {
        if (path == "")
            path = Registry.GetGamePath();

        const int steps = 2;
        var task = progress?.AddTask($"Installing {displayName}");

        var updateStep = (int now) => {
            task?.Increment(now * 100 / steps);
        };

        updateStep(1);

        using var httpClient = new HttpClient();
        var fileData = await httpClient.GetStreamAsync(url);

        if (Path.GetExtension(url) == ".zip")
        {
            using var archive = new ZipArchive(fileData);
            archive.ExtractToDirectory(path, overwriteFiles: true);
        }
        else if (Path.GetExtension(url) == ".dll")
        {
            
        }
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