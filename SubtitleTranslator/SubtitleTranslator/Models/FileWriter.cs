using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTranslator.Models;

public class FileWriter: IFileWriter
{
  public async Task WriteFile(string path, string content)
  {
    string toBeWriteFileName = Path.GetFileNameWithoutExtension(path) + "_zh.srt";
    string toBeWriteFilePath = Path.Combine(Path.GetDirectoryName(path)!, toBeWriteFileName);
    await using StreamWriter streamWriter = new StreamWriter(toBeWriteFilePath, false, Encoding.Default);
    await streamWriter.WriteLineAsync(content);
  }
}