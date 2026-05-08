using System.ComponentModel.DataAnnotations;

namespace MonkeModManager;

public class Pathing
{
    public static string GetGamePath(bool reselect = false)
    {
        if ((!reselect) && (Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\SirKingBinx\MonkeModManager", "InstallPath", null) != null))
        {
            string path = Registry.GetValue(@"HKEY_CURRENT_USER\Software\SirKingBinx\MonkeModManager", "InstallPath");
            Console.WriteLine(path);
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
        string p = "";

        if (File.Exists(Path.Combine(gamePath, "winhttp.dll")))
            p = Path.Combine(gamePath, "BepInEx", "plugins");
        if (File.Exists(Path.Combine(gamePath, "version.dll")))
            p = Path.Combine(gamePath, "Mod");

        Directory.CreateDirectory(p);
        return p;
    }

    public static bool IsLoaderInstalled(string gamePath) =>
        File.Exists(Path.Combine(gamePath, "winhttp.dll")) || File.Exists(Path.Combine(gamePath, "version.dll"));
}