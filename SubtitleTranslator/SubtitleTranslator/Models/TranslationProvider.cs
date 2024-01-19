using System.Collections.Generic;
using SubtitleTranslator.Helpers;

namespace SubtitleTranslator.Models;

public class TranslationProvider
{
  public Dictionary<string, ITranslator> TranslatorMap;

  public TranslationProvider()
  {
    TranslatorMap = new Dictionary<string, ITranslator>
    {
      { "腾讯云", new TencentcloudTranslator() },
      { "有道智云", new YoudaoTranslator() }
    };
  }
}