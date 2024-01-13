using System.Threading.Tasks;

namespace SubtitleTranslator.Models;

public interface ITranslator
{
  Task<string> Translate(string text, string sourceLanguage, string targetLanguage);
}