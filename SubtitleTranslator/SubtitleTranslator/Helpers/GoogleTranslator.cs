using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GTranslate.Results;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.Helpers;

public class GoogleTranslator : ITranslator
{
  public async Task<string?> Translate(string text, string sourceLanguage, string targetLanguage)
  {
    if (CheckContent(text) != true)
      return null!;
    if (targetLanguage == "auto")
    {
      await MessageBoxManager.GetMessageBoxStandard("错误", "请选择目标语言", ButtonEnum.Ok, Icon.Error).ShowAsync();
      return null;
    }

    var translator = new GTranslate.Translators.GoogleTranslator();
    // Translation: 'Hola Mundo', TargetLanguage: 'Spanish (es)', SourceLanguage: 'English (en)', Service: GoogleTranslator
    GoogleTranslationResult result;
    if (sourceLanguage == "auto")
    {
      result = await translator.TranslateAsync(text, targetLanguage);
    }
    else
    {
      result = await translator.TranslateAsync(text, targetLanguage, sourceLanguage);
    }

    // 匹配''中间的内容
    MatchCollection matches = Regex.Matches(result.ToString(), @"'(.*?)'", RegexOptions.Singleline);
    GoogleResponse googleResponse = new GoogleResponse
    {
      Translation = matches[0].Groups[1].Value,
      Target = matches[1].Groups[1].Value,
      Source = matches[2].Groups[1].Value,
    };
    return googleResponse.Translation;
  }

  public bool CheckApi()
  {
    throw new NotImplementedException();
  }

  public bool CheckContent(string text)
  {
    if (text.Length != 0)
      return true;
    throw new Exception("字幕文件内容为空");
  }
}