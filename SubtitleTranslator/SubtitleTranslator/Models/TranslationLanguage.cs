using System.Collections.Generic;

namespace SubtitleTranslator.Models;

public class TranslationLanguage
{
  public Dictionary<string, string> SourceLanguageList{ get; }


  public Dictionary<string, string> TargetLanguageList { get; }

  public TranslationLanguage()
  {
    SourceLanguageList = new Dictionary<string, string>
    {
      {"自动识别","auto"},
      {"简体中文","zh"},
      {"繁体中文","zh-TW"},
      {"英语","en"},
      {"日语","ja"},
      {"韩语","ko"},
      {"法语","fr"},
      {"西班牙语","es"},
      {"意大利语","it"},
      {"德语","de"},
      {"土耳其语","tr"},
      {"俄语","ru"},
      {"葡萄牙语","pt"}
    };
    TargetLanguageList = new Dictionary<string, string>(SourceLanguageList);
  }
}