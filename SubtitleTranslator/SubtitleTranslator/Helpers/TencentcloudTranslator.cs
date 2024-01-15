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
    _credential = new Credential
    {
      SecretId = "AKIDMdmJo9JMI4e7VuSvLbdR0LRJRL1UjkX6",
      SecretKey = "mU2Riv6Sg1PfirtkbHxTnu7aKCB4rUu7"
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
      Source = "en",
      Target = "zh",
      ProjectId = 1
    };
    TextTranslateResponse resp = _client!.TextTranslateSync(req);
    var jsonString = AbstractModel.ToJsonString(resp);
    TencentcloudResponse? tencentcloudResponse = JsonConvert.DeserializeObject<TencentcloudResponse>(jsonString);
    return Task.FromResult(tencentcloudResponse!.TargetText);
  }
}