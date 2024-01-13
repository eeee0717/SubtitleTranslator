namespace SubtitleTranslator.Models;

public interface ITranslator
{
  string Translate(string text, string sourceLanguage, string targetLanguage);
}