namespace MonkeModManager;

public class Registry
{
    public static string GetValue(string name, string key)
    {
        return (string)Microsoft.Win32.Registry.GetValue(name, key, null);
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