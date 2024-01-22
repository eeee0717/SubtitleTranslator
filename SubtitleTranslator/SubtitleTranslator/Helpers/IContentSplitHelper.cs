using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubtitleTranslator.Helpers;

public interface IContentSplitHelper
{ 
  Task<Tuple<List<string>, List<int>, List<string>>> SplitContent(string filePath);
  string CombineContent(List<string> originalContentList, List<int>toBeTranslatedContentIndexList, string? translatedContents);
}