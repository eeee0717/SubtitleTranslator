using System;

namespace SubtitleTranslator.Models;

public class TencentcloudTranslator: ITranslator
{
  public string Translate(string text, string sourceLanguage, string targetLanguage)
  {
    return "TencentcloudTranslator.Translate";
  }
}