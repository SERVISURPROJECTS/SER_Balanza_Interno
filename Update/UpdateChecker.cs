using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace SER_Balanza_Interno.Update
{
    public static class UpdateChecker
    {
        private const int ShareTimeoutMs = 4000;

        public static void CheckAndUpdate()
        {
            try
            {
                RunWithTimeout(CheckAndUpdateCore, ShareTimeoutMs);
            }
            catch
            {
                // La actualizacion es "best effort": cualquier fallo (red, permisos, archivo
                // ausente) no debe impedir que la app arranque normalmente.
            }
        }

        private static void RunWithTimeout(Action action, int timeoutMs)
        {
            var thread = new Thread(() => action()) { IsBackground = true };
            thread.Start();
            thread.Join(timeoutMs);
        }

        private static void CheckAndUpdateCore()
        {
            var sharePath = GetUpdateSharePath();
            if (string.IsNullOrWhiteSpace(sharePath))
                return;

            var versionFilePath = Path.Combine(sharePath, "version.json");
            if (!File.Exists(versionFilePath))
                return;

            using var stream = File.OpenRead(versionFilePath);
            using var doc = JsonDocument.Parse(stream);

            var remoteVersionText = doc.RootElement.GetProperty("version").GetString();
            var installerName = doc.RootElement.GetProperty("installer").GetString();

            if (string.IsNullOrWhiteSpace(remoteVersionText) || string.IsNullOrWhiteSpace(installerName))
                return;

            if (!Version.TryParse(remoteVersionText, out var remoteVersion))
                return;

            var localVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0, 0);

            if (remoteVersion <= localVersion)
                return;

            var remoteInstallerPath = Path.Combine(sharePath, installerName);
            if (!File.Exists(remoteInstallerPath))
                return;

            var localInstallerPath = Path.Combine(Path.GetTempPath(), installerName);
            File.Copy(remoteInstallerPath, localInstallerPath, overwrite: true);

            Process.Start(new ProcessStartInfo
            {
                FileName = localInstallerPath,
                Arguments = "/VERYSILENT /NORESTART /SUPPRESSMSGBOXES",
                UseShellExecute = true
            });

            Environment.Exit(0);
        }

        private static string? GetUpdateSharePath()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(path))
                return null;

            using var stream = File.OpenRead(path);
            using var doc = JsonDocument.Parse(stream);

            return doc.RootElement.TryGetProperty("UpdateSharePath", out var value)
                ? value.GetString()
                : null;
        }
    }
}
