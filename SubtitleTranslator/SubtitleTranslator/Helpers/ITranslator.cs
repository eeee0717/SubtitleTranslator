using System.Threading.Tasks;

namespace SubtitleTranslator.Helpers;

public interface ITranslator
{
  Task<string?> Translate(string text, string sourceLanguage, string targetLanguage);
}