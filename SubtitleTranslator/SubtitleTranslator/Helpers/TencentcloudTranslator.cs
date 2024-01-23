using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SubtitleTranslator.Models;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;

namespace SubtitleTranslator.Helpers;

public class TencentcloudTranslator : ITranslator
{
  private Credential? _credential;
  private ClientProfile? _clientProfile;
  private HttpProfile? _httpProfile;
  private TmtClient? _client;
  private readonly ConfigFileHelper _configFileHelper = new();

  public TencentcloudTranslator()
  {
    Initialize();
  }

  private void Initialize()
  {
    _credential = new Credential
    {
      SecretId = _configFileHelper.ProviderOptions!.TencentProviderOptions.SecretId,
      SecretKey = _configFileHelper.ProviderOptions!.TencentProviderOptions.SecretKey
    };
    _httpProfile = new HttpProfile
    {
      Endpoint = "tmt.tencentcloudapi.com"
    };
    _clientProfile = new ClientProfile
    {
      HttpProfile = _httpProfile
    };
    _client = new TmtClient(_credential, "ap-beijing", _clientProfile);
  }

  public async Task<string?> Translate(string text, string sourceLanguage, string targetLanguage)
  {
    if( CheckApi() != true|| CheckContent(text) != true)
      return null!;
    var req = new TextTranslateRequest
    {
      SourceText = text,
      Source = sourceLanguage,
      Target = targetLanguage,
      ProjectId = 1
    };
    TextTranslateResponse resp = await _client!.TextTranslate(req);
    var jsonString = AbstractModel.ToJsonString(resp);
    TencentcloudResponse? tencentcloudResponse = JsonConvert.DeserializeObject<TencentcloudResponse>(jsonString);
    return tencentcloudResponse!.TargetText;
  }

  public bool CheckApi()
  {
    if (_configFileHelper.ProviderOptions!.TencentProviderOptions.SecretId != "" &&
        _configFileHelper.ProviderOptions!.TencentProviderOptions.SecretKey != "") return true;
    throw new Exception("腾讯云API未配置");
  }
  public bool CheckContent(string text)
  {
    if(text.Length !=0)
      return true;
    throw new Exception("字幕文件内容为空");
  }
}