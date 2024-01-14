using System.Threading.Tasks;

namespace SubtitleTranslator.Models;

public interface IFileWriter
{
  Task WriteFile(string path, string content);
}