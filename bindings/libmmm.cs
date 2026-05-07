// MonkeModManager/bindings/libmmm.cs
// Bindings for the MMM protocol in C#

// 
// This is free and unencumbered software released into the public domain.
// 
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
// 
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>
// 

using System.Diagnostics;

namespace MonkeModManager;

/// <summary>
/// Bindings for the MonkeModManager.
/// </summary>
public static class MonkeModManager
{
    internal static HttpClient httpClient;

    internal static void RunUri(string uri)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = uri,
            UseShellExecute = true
        });
    }

    internal static void SetupHttpClient()
    {
        if (httpClient != null)
            return;
        
        httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", $"MonkeModLib/1.0");
    }

    /// <summary>
    /// Install a mod.
    /// </summary>
    /// <param name="mod">The mod to install.</param>
    public static void Install(MonkeMod mod)
    {
        RunUri($"mmm://install{mod.ToInstallerQuery()}");
    }

    /// <summary>
    /// Install a mod.
    /// </summary>
    /// <param name="name">The display name of the mod.</param>
    /// <param name="url">The direct download link of the mod.</param>
    public static void Install(string name, string url)
    {
        string urlName = Uri.EscapeDataString(name);
        string urlDownload = Uri.EscapeDataString(url);

        RunUri($"mmm://install?name={urlName}&url={urlDownload}");
    }

    /// <summary>
    /// Install a mod loader.
    /// </summary>
    /// <param name="loader">The mod loader to install.</param>
    public static void Install(ModLoader loader)
    {
        RunUri($"mmm://install?loader={(loader == ModLoader.BepInEx ? "BepInEx" : "MelonLoader")}");
    }

    /// <summary>
    /// Open a folder in the File Explorer.
    /// </summary>
    /// <param name="folder">The folder to open.</param>
    public static void Open(ModFolder folder)
    {
        string folderString = folder switch
        {
            ModFolder.GameFolder => "game",
            ModFolder.Plugins => "mods",
            ModFolder.UserData => "userdata",
            ModFolder.AppData => "appdata"
        };

        RunUri($"mmm://open?mode={folderString}");
    }

    /// <summary>
    /// Opens a path in the File Explorer relative to the Gorilla Tag folder.
    /// </summary>
    /// <param name="relativePath">The path relative to the gorilla tag folder.</param>
    public static void Open(string relativePath)
    {
        RunUri($"mmm://open?path={Uri.EscapeDataString(relativePath)}");
    }
}

public class MonkeMod
{
    /// <summary>
    /// The display name of the mod.
    /// </summary>
    public string Name;

    /// <summary>
    /// The url of the mod's DLL/Zip file.
    /// </summary>
    public string Url;

    /// <summary>
    /// True if the mod is verified on MonkeModManager.
    /// </summary>
    public string Verified
    {
        get
        {
            var response = MonkeModManager.httpClient.GetAsync(url).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            var verifiedList = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().Split("\n");
            return verifiedList.Any(Url.StartsWith);
        }
    }

    /// <summary>
    /// Converts the MonkeMod into a valid query to be sent through mmm://install.
    /// </summary>
    /// <returns>A string with the mod parameters.</returns>
    public string ToInstallerQuery()
    {
        string name = Uri.EscapeDataString(Name);
        string url = Uri.EscapeDataString(Url);

        return $"?name={name}&url={url}";
    }
}

public enum ModLoader
{
    BepInEx = 0,
    MelonLoader = 1
}

public enum ModFolder
{
    // game folder (holds Gorilla Tag.exe)
    GameFolder,
    // plugins
    Plugins,
    // config files
    UserData,
    // log files
    AppData,
}