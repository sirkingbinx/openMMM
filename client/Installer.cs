using System.IO.Compression;
using AwesomeTxt;

namespace MonkeModManager;

public class Installer
{
    public const string VerifiedApiUrl = "https://raw.githubusercontent.com/sirkingbinx/openMMM/refs/heads/master/api/Verified.txt";

    private static AwesomeTxtFile VerifiedCache;

    public static async Task Invoke(List<(string, string)> mods, string path)
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
            string depVerifiedText = depVerified ? " (Verified)" : "";

            dependencyText += $"{depName}{depVerifiedText}{(dependency == mods.Last() ? "" : ",")} ";
        }

        if (dependencyText != "")
            dependencyText = $"Dependencies: {dependencyText}\n\n";

        bool verified = VerifiedCache.Results.Any(verifiedUrl => primaryUrl.StartsWith(verifiedUrl));
        string verifiedText = verified ? "This mod is verified.\n" : "";

        if (!Pathing.IsLoaderInstalled(path) && !mods.Select(m => m.Item1.ToLower()).Any(m => m.Contains("bepinex") || m.Contains("melonloader")))
            mods.Add(($"BepInEx v{Program.BepInExVersion}", Program.BepInExURL));

        var installConfirmation = new TaskDialogPage()
        {
            Caption = "MonkeModManager",
            Heading = $"Install {primaryName}?",
            Text = $"{verifiedText}{dependencyText}Do you want to install this?",
            Icon = verified ? TaskDialogIcon.ShieldSuccessGreenBar : TaskDialogIcon.ShieldWarningYellowBar,
            Buttons = { TaskDialogButton.Cancel, TaskDialogButton.Yes }
        };

        if (TaskDialog.ShowDialog(installConfirmation) != TaskDialogButton.Yes)
            return;

        var progressBar = new TaskDialogProgressBar()
        {
            State = TaskDialogProgressBarState.Normal,
            Minimum = 0,
            Maximum = mods.Count
        };

        var page = new TaskDialogPage()
        {
            Caption = "MonkeModManager",
            Heading = $"Loading..",
            Text = "Your mods are being installed.",
            ProgressBar = progressBar,
            Buttons = { TaskDialogButton.OK }
        };

        page.Created += (s, e) =>
        {
            foreach (var mod in mods.AsEnumerable().Reverse())
            {
                page.Heading = $"Installing {mod.Item1}";
                InstallMod(mod.Item2, path).GetAwaiter().GetResult();
                progressBar.Value++;
            }

            page.Heading = "Installed";
            page.Text = $"The following mods have been installed:\n{string.Join("\n", mods.Select(s => "- " + s.Item1).ToArray())}";
        };

        TaskDialog.ShowDialog(page);
    }

    public static async Task InstallMod(string url, string path)
    {
        var fileData = await Network.GetStream(url);

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
    }

    public static async Task InstallBepInEx(string path)
    {
        await Invoke([("BepInEx v" + Program.BepInExVersion, Program.BepInExURL)], path);
        File.Delete(Path.Combine(path, "version.dll"));
    }

    public static async Task InstallMelonLoader(string path)
    {
        await Invoke([("MelonLoader v" + Program.MelonLoaderVersion, Program.MelonLoaderURL)], path);
        File.Delete(Path.Combine(path, "winhttp.dll"));
    }
}