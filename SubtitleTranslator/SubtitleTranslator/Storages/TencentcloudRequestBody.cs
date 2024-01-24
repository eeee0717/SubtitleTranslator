namespace SubtitleTranslator.Storages;

public class TencentcloudRequestBody
{
  public required string SourceText;
  public string Source = "en";
  public string Target = "zh";
  public int ProjectId = 1;
}