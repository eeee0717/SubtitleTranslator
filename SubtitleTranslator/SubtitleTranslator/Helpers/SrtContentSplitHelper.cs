using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitleTranslator.Helpers;

public class SrtContentSplitHelper : IContentSplitHelper
{
  private readonly Regex _numberPattern;
  private readonly Regex _timelinePattern;

  public SrtContentSplitHelper()
  {
    this._numberPattern = new Regex(@"^\d+$");
    this._timelinePattern = new Regex(@"^\d{2}:\d{2}:\d{2},\d{3} --> \d{2}:\d{2}:\d{2},\d{3}$");
  }

  public async Task<Tuple<List<string>, List<string>>> SplitContent(string filePath)
  {
    var originalContentList = new List<string>();
    var toBeTranslatedList = new List<string>();
    var toBeTranslatedContent = "";
    
    using var streamReader = new StreamReader(filePath, Encoding.UTF8);
    while (await streamReader.ReadLineAsync() is { } line)
    {
      if (_numberPattern.IsMatch(line) || _timelinePattern.IsMatch(line) || string.IsNullOrWhiteSpace(line))
      {
        originalContentList.Add(line);
        continue;
      }
      originalContentList.Add("");
      if (toBeTranslatedContent.Length > 5000)
      {
        toBeTranslatedList.Add(toBeTranslatedContent);
        toBeTranslatedContent = "";
      }
      toBeTranslatedContent += $"{line}\n";
    }
    toBeTranslatedList.Add(toBeTranslatedContent);

    return new Tuple<List<string>, List<string>>(originalContentList, toBeTranslatedList);
  }
  

  public string? CombineContent(List<string> originalContentList, string? translatedContents)
  {
    var translatedSplitStrings = translatedContents.Split("\n");
    for (int i = 0; i < originalContentList.Count; i++)
    {
      // i=2,6,10...时处理
      if ((i - 2) % 4 == 0)
      {
        originalContentList[i] = translatedSplitStrings[(i - 2) / 4];
      }
    }

    return string.Join("\r\n", originalContentList);
  }
}