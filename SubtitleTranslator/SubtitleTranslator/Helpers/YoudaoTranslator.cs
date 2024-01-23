using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.Helpers;

public class YoudaoTranslator : ITranslator
{
  private Dictionary<string, string> _postBody;
  private const string Url = "https://openapi.youdao.com/api";
  private readonly ConfigFileHelper _configFileHelper = new();


  private void Initialize(string text)
  {
    var appId = _configFileHelper.ProviderOptions!.YoudaoProviderOptions.AppId;
    var appKey = _configFileHelper.ProviderOptions!.YoudaoProviderOptions.AppKey;
    TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    long millis = (long)ts.TotalMilliseconds;

    string curtime = Convert.ToString(millis / 1000);
    string salt = DateTime.Now.Millisecond.ToString();

    string signStr = appId + Truncate(text) + salt + curtime + appKey;
    string sign = ComputeHash(signStr, SHA256.Create());
    _postBody = new Dictionary<string, string>
    {
      { "appKey", appId },
      { "salt", salt },
      { "sign", sign },
      { "signType", "v3" },
      { "curtime", curtime },
    };
  }

  public async Task<string?> Translate(string text, string sourceLanguage, string targetLanguage)
  {
    if( CheckApi() != true || CheckContent(text) != true)
      return null!;
    if (sourceLanguage == "zh") sourceLanguage = "zh-CHS";
    if (sourceLanguage == "zh-TW") sourceLanguage = "zh-CHT";
    if (targetLanguage == "zh") sourceLanguage = "zh-CHS";
    if (targetLanguage == "zh-TW") sourceLanguage = "zh-CHT";
    Initialize(text);
    _postBody.Add("from", sourceLanguage);
    _postBody.Add("to", targetLanguage);
    _postBody.Add("q", System.Web.HttpUtility.UrlEncode(text));
    string resp = await Post();
    JObject jObject = JObject.Parse(resp);
    YoudaoResponse youdaoResponse = new();
    youdaoResponse.Translation = jObject["translation"]?[0]?.ToString();
    youdaoResponse.ErrorCode = jObject["errorCode"]!.ToString();
    if (youdaoResponse.ErrorCode != "0")
      throw new Exception($"有道云API错误,错误码{youdaoResponse.ErrorCode}");
    return youdaoResponse.Translation;
  }

  public bool CheckApi()
  {
    if (_configFileHelper.ProviderOptions!.YoudaoProviderOptions.AppId != "" &&
        _configFileHelper.ProviderOptions!.YoudaoProviderOptions.AppKey != "") return true;
    throw new Exception("有道云API未配置");
  }
  public bool CheckContent(string text)
  {
    if(text.Length !=0)
      return true;
    throw new Exception("字幕文件内容为空");
  }
  private static string ComputeHash(string input, HashAlgorithm algorithm)
  {
    Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
    Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
    return BitConverter.ToString(hashedBytes).Replace("-", "");
  }

  private async Task<string> Post()
  {
    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
    req.Method = "POST";
    req.ContentType = "application/x-www-form-urlencoded";
    StringBuilder builder = new StringBuilder();
    int i = 0;
    foreach (var item in _postBody)
    {
      if (i > 0)
        builder.Append("&");
      builder.AppendFormat("{0}={1}", item.Key, item.Value);
      i++;
    }

    var data = Encoding.UTF8.GetBytes(builder.ToString());
    req.ContentLength = data.Length;

    using var reqStream = req.GetRequestStream();
    reqStream.Write(data, 0, data.Length);
    reqStream.Close();


    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
    Stream stream = resp.GetResponseStream();
    using var reader = new StreamReader(stream, Encoding.UTF8);
    var result = await reader.ReadToEndAsync();

    return result;
  }

  private static string? Truncate(string q)
  {
    if (q == null)
    {
      return null;
    }

    int len = q.Length;
    return len <= 20 ? q : q.Substring(0, 10) + len + q.Substring(len - 10, 10);
  }
}