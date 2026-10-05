using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace SER_Balanza_Interno.Update
{
    /// <summary>
    /// Revisa el ultimo Release de GitHub del repo configurado en appsettings.json (UpdateRepo) y,
    /// si hay una version mas nueva, descarga el instalador (asset .exe del release) y lo corre en
    /// silencio. Reemplaza el mecanismo anterior basado en una carpeta de red compartida (casillero),
    /// que dejo de usarse por no ser un medio de distribucion confiable/seguro.
    /// </summary>
    public static class UpdateChecker
    {
        private const int TimeoutMs = 60000;

        public static void CheckAndUpdate()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeoutMs);
                Task.Run(() => CheckAndUpdateCoreAsync(cts.Token), cts.Token).Wait(TimeoutMs);
            }
            catch
            {
                // La actualizacion es "best effort": cualquier fallo (sin internet, GitHub caido,
                // timeout, repo sin releases) no debe impedir que la app arranque normalmente.
            }
        }

        private static async Task CheckAndUpdateCoreAsync(CancellationToken ct)
        {
            var repo = GetUpdateRepo();
            if (string.IsNullOrWhiteSpace(repo))
                return;

            using var http = new HttpClient();
            http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("SER_Balanza_Interno-UpdateChecker", "1.0"));
            http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

            var json = await http.GetStringAsync($"https://api.github.com/repos/{repo}/releases/latest", ct);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var tag = root.TryGetProperty("tag_name", out var tagProp) ? tagProp.GetString() : null;
            if (string.IsNullOrWhiteSpace(tag))
                return;

            var versionTexto = tag.TrimStart('v', 'V');
            if (!Version.TryParse(versionTexto, out var remoteVersion))
                return;

            var localVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);
            if (remoteVersion <= localVersion)
                return;

            if (!root.TryGetProperty("assets", out var assets))
                return;

            string? downloadUrl = null;
            string? assetName = null;
            foreach (var asset in assets.EnumerateArray())
            {
                var nombre = asset.TryGetProperty("name", out var n) ? n.GetString() : null;
                if (nombre is null || !nombre.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    continue;

                downloadUrl = asset.TryGetProperty("browser_download_url", out var u) ? u.GetString() : null;
                assetName = nombre;
                break;
            }

            if (string.IsNullOrWhiteSpace(downloadUrl) || string.IsNullOrWhiteSpace(assetName))
                return;

            var localInstallerPath = Path.Combine(Path.GetTempPath(), assetName);
            var bytes = await http.GetByteArrayAsync(downloadUrl, ct);
            await File.WriteAllBytesAsync(localInstallerPath, bytes, ct);

            Process.Start(new ProcessStartInfo
            {
                FileName = localInstallerPath,
                Arguments = "/VERYSILENT /NORESTART /SUPPRESSMSGBOXES",
                UseShellExecute = true
            });

            Environment.Exit(0);
        }

        private static string? GetUpdateRepo()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(path))
                return null;

            using var stream = File.OpenRead(path);
            using var doc = JsonDocument.Parse(stream);

            return doc.RootElement.TryGetProperty("UpdateRepo", out var value)
                ? value.GetString()
                : null;
        }
    }
}
