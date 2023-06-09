using Microsoft.Extensions.Configuration;

namespace ConfigurationProviders
{
    public class FileConfigurationProvider : IConfigurationProvider
    {
        private readonly string _filePath;

        public FileConfigurationProvider(string filePath)
        {
            _filePath = filePath;
        }

        public string GetValue(string settingName)
        {
            var settings = File.ReadAllLines(_filePath)
                .Select(line => line.Split('='))
                .ToDictionary(parts => parts[0], parts => parts[1]);

            var res = settings.TryGetValue(settingName, out var value) ? value : null;

            return res;
        }

        public void SetValue(string settingName, string value)
        {
            var settings = File.ReadAllLines(_filePath)
                .Select(line => line.Split('='))
                .ToDictionary(parts => parts[0], parts => parts[1]);

            settings[settingName] = value;

            File.WriteAllLines(_filePath, settings.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }

    public class ConfigurationManagerConfigurationProvider : IConfigurationProvider
    {
        public string GetValue(string settingName)
        {
            Configurations _config = new Configurations();
            IConfiguration config = _config.GetBuilder();
            var res = config.GetSection("AppSettings")[settingName];
            return res;
        }

        public void SetValue(string settingName, string value)
        {
            Configurations _config = new Configurations();
            IConfiguration config = _config.GetBuilder();
            config.GetSection("AppSettings")[settingName] = value;
        }
    }
}
