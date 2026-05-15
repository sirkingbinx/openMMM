using System.Diagnostics;

namespace MonkeModManager;

public class Pathing
{
    public static string GetGamePath(bool reselect = false)
    {
        if ((!reselect) && (Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\SirKingBinx\MonkeModManager", "InstallPath", null) != null))
        {
            string path = Registry.GetValue(@"HKEY_CURRENT_USER\Software\SirKingBinx\MonkeModManager", "InstallPath");
            return path;
        }
        
        string steamInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1533390", "InstallLocation");

        string oculusLibraryId = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Oculus VR, LLC\Oculus\Libraries", "DefaultLibrary");
        string oculusLibraryPath = Registry.GetValue(@$"HKEY_CURRENT_USER\Software\Oculus VR, LLC\Oculus\Libraries\{oculusLibraryId}", "OriginalPath");
        string oculusInstallPath = Path.Combine(oculusLibraryPath, "Software", "another-axiom-gorilla-tag");

        string gamePath;

        if (Directory.Exists(steamInstallPath) && Directory.Exists(oculusInstallPath))
        {
            var steamBtn = new TaskDialogButton("Steam");
            var oculusBtn = new TaskDialogButton("Oculus");
            var selectGame = new TaskDialogPage()
            {
                Caption = "MonkeModManager",
                Heading = "Select Gorilla Tag version",
                Text = "Multiple installations of Gorilla Tag were found on your computer. Please select the one that MonkeModManager should install mods to. You can change this at any time by opening mmm://select in a browser.",
                Buttons = { steamBtn, oculusBtn }
            };

            bool useSteam = TaskDialog.ShowDialog(selectGame) == steamBtn;
            gamePath = useSteam ? steamInstallPath : oculusInstallPath;
        }
        else if (Directory.Exists(steamInstallPath))
        {
            gamePath = steamInstallPath;
        }
        else if (Directory.Exists(oculusInstallPath))
        {
            gamePath = oculusInstallPath;
        }
        else
        {
            MessageBox.Show("No installation of Gorilla Tag was detected. Please install the game via Steam or Oculus.", "MonkeModManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            return "";
        }

        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\SirKingBinx\MonkeModManager", "InstallPath", gamePath);
        return gamePath;
    }

    public static string GetModLoaderPluginsPath(string gamePath)
    {
        string p = Path.Combine(gamePath, "BepInEx", "plugins");
        Directory.CreateDirectory(p);

        return p;
    }

    public static void OpenPluginsFolder(string gamePath)
    {
        if (!IsLoaderInstalled(gamePath))
            return;
        
        LaunchFolder(GetModLoaderPluginsPath(gamePath));
    }

    public static void OpenGameFolder(string gamePath)
    {
        LaunchFolder(gamePath);
    }

    public static void OpenGameRelativeFolder(string gamePath, string relativePath)
    {
        LaunchFolder(Path.Combine(gamePath, relativePath));
    }

    public static void OpenAppDataFolder()
    {
        var path = Path.Combine(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).FullName, "LocalLow", "Another Axiom", "Gorilla Tag");

        if (!Directory.Exists(path))
            return;

        LaunchFolder(path);
    }

    public static void OpenUserDataFolder(string gamePath)
    {
        string path = @"C:\TEstewsuidzgvoyuhfgbyuvhsdf12412512542"; // invalidate if no mod loader is installed

        path = Path.Combine(gamePath, "BepInEx", "config");

        if (!Directory.Exists(path))
            return;

        LaunchFolder(path);
    }

    private static void LaunchFolder(string folder)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = "explorer.exe",
            Arguments = folder,
            UseShellExecute = true
        };
        Process.Start(startInfo);
    }

    public static bool IsLoaderInstalled(string gamePath) =>
        File.Exists(Path.Combine(gamePath, "winhttp.dll"));
}