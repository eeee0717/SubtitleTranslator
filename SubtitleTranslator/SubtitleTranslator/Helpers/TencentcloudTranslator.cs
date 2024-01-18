using System.Threading.Tasks;
using Newtonsoft.Json;
using SubtitleTranslator.Models;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Tmt.V20180321;
using TencentCloud.Tmt.V20180321.Models;
using Task = System.Threading.Tasks.Task;

namespace SubtitleTranslator.Helpers;

public class TencentcloudTranslator : ITranslator
{
  private Credential? _credential;
  private ClientProfile? _clientProfile;
  private HttpProfile? _httpProfile;
  private TmtClient? _client;

  private void Initialize()
  {
    ConfigFileHelper configFileHelper = new();

    _credential = new Credential
    {
      SecretId = configFileHelper.ProviderOptions.TencentProviderOptions.SecretId,
      SecretKey = configFileHelper.ProviderOptions.TencentProviderOptions.SecretKey
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

  public Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
  {
    Initialize();
    var req = new TextTranslateRequest
    {
      SourceText = text,
      Source = sourceLanguage,
      Target = targetLanguage,
      ProjectId = 1
    };
    TextTranslateResponse resp = _client!.TextTranslateSync(req);
    var jsonString = AbstractModel.ToJsonString(resp);
    TencentcloudResponse? tencentcloudResponse = JsonConvert.DeserializeObject<TencentcloudResponse>(jsonString);
    return Task.FromResult(tencentcloudResponse!.TargetText);
  }
}