using SmartCall.Services;
using System.IO;
using System.Text.Json;

namespace SmartCall
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configurar URL da API
            LoadConfiguration();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
        }

        private static void LoadConfiguration()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                
                if (File.Exists(configPath))
                {
                    string json = File.ReadAllText(configPath);
                    var config = JsonSerializer.Deserialize<AppConfig>(json);
                    
                    if (config != null && !string.IsNullOrEmpty(config.ApiBaseUrl))
                    {
                        ApiService.SetBaseUrl(config.ApiBaseUrl);
                    }
                }
                else
                {
                    // URL padrão se o arquivo não existir
                    ApiService.SetBaseUrl("http://localhost:5000");
                }
            }
            catch
            {
                // Em caso de erro, usar URL padrão
                ApiService.SetBaseUrl("http://localhost:5000");
            }
        }
    }

    public class AppConfig
    {
        public string ApiBaseUrl { get; set; } = "http://localhost:5000";
    }
}