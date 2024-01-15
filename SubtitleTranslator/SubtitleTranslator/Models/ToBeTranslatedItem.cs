namespace SubtitleTranslator.Models;

public class ToBeTranslatedItem(bool isTranslated, string fileName, string fileSize, string filePath)
{
  public bool IsTranslated { get; set; } = isTranslated;
  public string FileName { get; set; } = fileName;
  public string FileSize { get; set; } = fileSize;
  public string FilePath { get; set; } = filePath;
  
  
  
}
