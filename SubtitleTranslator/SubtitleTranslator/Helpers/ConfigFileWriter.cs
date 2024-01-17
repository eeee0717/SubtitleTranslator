
using System.Threading.Tasks;


namespace SubtitleTranslator.Helpers;

public class ConfigFileWriter: IFileWriter
{
  public Task WriteFile(string content, string path = "avares://SubtitleTranslator/Assets/config.json")
  {
    
    return Task.FromResult("ok");
  }
}