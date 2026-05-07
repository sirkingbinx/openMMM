using System.IO.Compression;
using AwesomeTxt;
using Spectre.Console;

namespace MonkeModManager;

public class Installer
{
    public const string VerifiedApiUrl = "https://raw.githubusercontent.com/sirkingbinx/openMMM/refs/heads/master/api/Verified.txt";

    private static AwesomeTxtFile VerifiedCache;

    public static async Task Invoke(List<(string, string)> mods, string path = "")
    {
        if (path == "")
            path = Pathing.GetGamePath();
        
        if (VerifiedCache == null)
        {
            try { VerifiedCache = new AwesomeTxtFile(await Network.GetString(VerifiedApiUrl)); }
            catch { }
        }

        string primaryName = mods[0].Item1;
        string primaryUrl = mods[0].Item2;

        string dependencyText = "";

        foreach (var dependency in mods[1..])
        {
            string depName = dependency.Item1;
            string depUrl = dependency.Item2;

            bool depVerified = VerifiedCache.Results.Any(verifiedUrl => depUrl.StartsWith(verifiedUrl));
            string depVerifiedText = depVerified ? " [green](Verified)[/]" : "";

            dependencyText += $"{depName}{depVerifiedText}{(dependency == mods.Last() ? "" : ",")} ";
        }

        bool install;

        bool verified = VerifiedCache.Results.Any(verifiedUrl => primaryUrl.StartsWith(verifiedUrl));
        string verifiedText = verified ? "[green](Verified)[/]" : "";
        
        if (dependencyText == "")
        {
            install = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"Installing {primaryName} {verifiedText}\n\nDo you want to install this?")
                .AddChoices("Yes", "No")) == "Yes";
        }
        else
        {
            install = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title($"Installing {primaryName} {verifiedText}\nDependencies: {dependencyText}\n\nDo you want to install this?")
                .AddChoices("Yes", "No")) == "Yes";
        }

        if (!install)
            return;

        foreach (var mod in mods.AsEnumerable().Reverse())
            await InstallMod(mod.Item1, mod.Item2, path);
    }

    public static async Task InstallMod(string name, string url, string path)
    {
        Console.Write($"\rDownloading: {name}");
        var fileData = await Network.GetStream(url);
        Console.Write($"\rInstalling : {name}");

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

        Console.Write($"\rInstalled  : {name}\n");
    }

    public static async Task InstallBepInEx(string path)
    {
        await Invoke([("BepInEx v5.4.23.5", "https://github.com/BepInEx/BepInEx/releases/download/v5.4.23.5/BepInEx_win_x64_5.4.23.5.zip")], path);
        File.Delete(Path.Combine(path, "version.dll"));
    }

    public static async Task InstallMelonLoader(string path)
    {
        await Invoke([("MelonLoader v0.7.2", "https://github.com/LavaGang/MelonLoader/releases/download/v0.7.2/MelonLoader.x64.zip")], path);
        File.Delete(Path.Combine(path, "winhttp.dll"));
    }
}