namespace MonkeModManager;

public class Network
{
    private static HttpClient client;

    private static void InitializeHttpClient()
    {
        if (client != null)
            return;
        
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", $"MonkeModManager/{Program.Version}");
    }

    public static async Task<Stream> GetStream(string url)
    {
        InitializeHttpClient();

        var response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show($"The remote server at {url} returned status code {response.StatusCode}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        var byteStream = await response.Content.ReadAsStreamAsync();
        return byteStream;
    }

    public static async Task<string> GetString(string url)
    {
        InitializeHttpClient();

        var response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode)
        {
            MessageBox.Show($"The remote server at {url} returned status code {response.StatusCode}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
        

        var contentStream = await response.Content.ReadAsStringAsync();
        return contentStream;
    }
}