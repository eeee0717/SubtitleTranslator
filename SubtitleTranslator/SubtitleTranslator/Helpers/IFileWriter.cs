using System.Threading.Tasks;

namespace SubtitleTranslator.Helpers;

public interface IFileWriter
{
  Task WriteFile(string path, string? content, string targetLanguage);
}