using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SubtitleTranslator.Providers;

namespace SubtitleTranslator.Helpers;

public class ConfigFileHelper
{
  public ProviderOptions? ProviderOptions { get; set; }
  private readonly string _configPath;
  public ConfigFileHelper()
  {
    _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

    using StreamReader streamReader = new StreamReader(_configPath);
    var json = streamReader.ReadToEnd();
    ProviderOptions = JsonConvert.DeserializeObject<ProviderOptions>(json);
  }
  public async Task Save()
  {
    string providerOptionsJson = JsonConvert.SerializeObject(ProviderOptions, Formatting.Indented);
    await File.WriteAllTextAsync(_configPath, providerOptionsJson);
  }
}