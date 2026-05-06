using Spectre.Console;

namespace MonkeModManager;

public class Registry
{
    public static string GetValue(string name, string key, string def = "")
    {
        if (!OperatingSystem.IsWindows())
            return "";

        return (string)Microsoft.Win32.Registry.GetValue(name, key, def);
    }

    public static string GetGamePath()
    {
        string steamInstallPath = GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 1533390", "InstallLocation");

        string oculusLibraryId = GetValue(@"HKEY_CURRENT_USER\Software\Oculus VR, LLC\Oculus\Libraries", "DefaultLibrary");
        string oculusLibraryPath = GetValue(@$"HKEY_CURRENT_USER\Software\Oculus VR, LLC\Oculus\Libraries\{oculusLibraryId}", "OriginalPath");
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

    public static void RegisterProtocol(string appPath)
    {
        if (!OperatingSystem.IsWindows())
            return;

        using var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey($@"Software\Classes\mmm");
        
        key.SetValue("", $"URL:mmm Protocol");
        key.SetValue("URL Protocol", "");

        using var commandKey = key.CreateSubKey(@"shell\open\command");
        
        commandKey.SetValue("", $"\"{appPath}\" \"%1\"");
    }
}