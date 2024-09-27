//Building and Configuring Class Libraries

using Ch11;
using Microsoft.Extensions.Configuration;
using extensions = Ch11.MyExtensions;

namespace Ch16;

public class Chapter16
{
    public static void ConfigurationExample()
    {
        var configure = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.dev.json", true, true)
            .Build();
        var devDetailsSection = configure.GetSection("dev-details");
        DevConfiguration devConfiguration = new();
        devDetailsSection.Bind(devConfiguration);
        $"My Car name is {configure["CarName"]}, is it fast? {configure.GetValue<bool>("isFast")}".Print();
        devConfiguration.Print();

        DevConfiguration? anotherSection = configure.GetSection("dev-details").Get<DevConfiguration>();
        anotherSection.Print();
    }

    public record DevConfiguration
    {
        public DevConfiguration() { }

        public DevConfiguration(string environment, string version, string apiKey)
        {
            Environment = environment;
            Version = version;
            APIKey = apiKey;
        }

        public string Environment { get; set; }
        public string Version { get; set; }
        public string APIKey { get; set; }
    }
}
