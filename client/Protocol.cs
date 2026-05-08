using System.Web;

namespace MonkeModManager;

public class Protocol
{
    public static void Handle(string uriString)
    {
        if (!Uri.TryCreate(uriString, UriKind.Absolute, out Uri uri))
            return;

        var query = HttpUtility.ParseQueryString(uri.Query);

        if (uri.Host == "install")
        {
            string gamePath = Pathing.GetGamePath();

            if (query["mods"] is not null)
            {
                List<(string, string)> mods = new();
                foreach (string dependencyString in query["mods"].Split("-"))
                {
                    string[] split = dependencyString.Split("~");

                    if (split.Length == 2)
                    {
                        mods.Add((split[0], split[1]));
                    }
                    else
                    {
                        MessageBox.Show($"Invalid URI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                }

                Installer.Invoke(mods, gamePath).GetAwaiter().GetResult();
                Application.Exit();
                return;
            }

            if (query["loader"] is not null)
            {
                string loader = query["loader"].ToLower();

                if (loader == "bepinex")
                    Installer.InstallBepInEx(gamePath).GetAwaiter().GetResult();
                else
                    Installer.InstallMelonLoader(gamePath).GetAwaiter().GetResult();

                Application.Exit();
                return;
            }

            MessageBox.Show($"Invalid URI parameters for /install. Please see documentation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        if (uri.Host == "open")
        {
            string gamePath = Pathing.GetGamePath();

            if (query["mode"] is not null)
            {
                switch (query["mode"].ToLower()) {
                    case "game":
                        Pathing.OpenGameFolder(gamePath);
                        break;
                    case "mods":
                        Pathing.OpenPluginsFolder(gamePath);
                        break;
                    case "appdata":
                        Pathing.OpenAppDataFolder();
                        break;
                    case "userdata":
                        Pathing.OpenUserDataFolder(gamePath);
                        break;
                    default:
                        MessageBox.Show($"Invalid URL parameters for /open. Please see documentation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                Application.Exit();
                return;
            }

            if (query["path"] is not null)
            {
                Pathing.OpenGameRelativeFolder(gamePath, query["path"].ToLower());

                Application.Exit();
                return;
            }

            MessageBox.Show($"Invalid URL parameters for /open. Please see documentation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            return;
        }

        if (uri.Host == "select")
        {
            Pathing.GetGamePath(true);
            Application.Exit();
            return;
        }
        
        MessageBox.Show($"Invalid URI.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        Application.Exit();
    }
}
