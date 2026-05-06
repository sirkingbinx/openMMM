using Spectre.Console;

namespace MonkeModManager;

public class Pathing
{
    public static string GetGamePath()
    {
        string steamInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1533390", "InstallLocation");

        string oculusLibraryId = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Oculus VR, LLC\Oculus\Libraries", "DefaultLibrary");
        string oculusLibraryPath = Registry.GetValue(@$"HKEY_CURRENT_USER\Software\Oculus VR, LLC\Oculus\Libraries\{oculusLibraryId}", "OriginalPath");
        string oculusInstallPath = Path.Combine(oculusLibraryPath, "Software", "another-axiom-gorilla-tag");

        string gamePath = "";

        if (Directory.Exists(steamInstallPath) && Directory.Exists(oculusInstallPath))
        {
            string option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Select game to mod")
                .AddChoices("Steam", "Oculus"));

            if (option == "Steam")
                gamePath = steamInstallPath;
            else
                gamePath = oculusInstallPath;
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
            Console.WriteLine("Gorilla Tag is not installed on your machine. Please install Gorilla Tag via Steam or Oculus.");
            Environment.Exit(1);
        }

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
}