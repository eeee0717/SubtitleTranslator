using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubtitleTranslator.Helpers;

public interface IContentSplitHelper
{ 
  Task<Tuple<List<string>, List<string>>> SplitContent(string filePath);
  string? CombineContent(List<string> originalContentList, string? translatedContents);
}