using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTranslator.Helpers;

public class SubtitleFileWriter: IFileWriter
{
  public async Task WriteFile(string path, string content, string targetLanguage)
  {
    string toBeWriteFileName = Path.GetFileNameWithoutExtension(path) + $"_{targetLanguage}.srt";
    string toBeWriteFilePath = Path.Combine(Path.GetDirectoryName(path)!, toBeWriteFileName);
    await using StreamWriter streamWriter = new StreamWriter(toBeWriteFilePath, false, Encoding.Default);
    await streamWriter.WriteLineAsync(content);
  }
  
}