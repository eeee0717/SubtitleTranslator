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

  public async Task<Tuple<List<string>, List<int>, List<string>>> SplitContent(string filePath)
  {
    var originalContentList = new List<string>();
    var toBeTranslatedList = new List<string>();
    var toBeTranslatedContentIndexList = new List<int>();
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
      toBeTranslatedContentIndexList.Add(originalContentList.Count - 1);
      toBeTranslatedContent += $"{line}\n";
      if (toBeTranslatedContent.Length > 5000)
      {
        toBeTranslatedList.Add(toBeTranslatedContent);
        toBeTranslatedContent = "";
      }
    }

    toBeTranslatedList.Add(toBeTranslatedContent);

    return new Tuple<List<string>, List<int>, List<string>>(originalContentList, toBeTranslatedContentIndexList,
      toBeTranslatedList);
  }


  public string CombineContent(List<string> originalContentList, List<int> toBeTranslatedContentIndexList,
    string? translatedContents)
  {
    translatedContents = translatedContents.Trim();
    var translatedSplitStrings = translatedContents.Split("\n");
    for (int i = 0; i < translatedSplitStrings.Length; i++)
    {
      originalContentList[toBeTranslatedContentIndexList[i]] = translatedSplitStrings[i];
    }

    return string.Join("\r\n", originalContentList);
  }
  
}