using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Intrinsics.Arm;
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

  public YoudaoTranslator()
  {
  }

  private void Initialize(string text)
  {
    string appKey = "3d8ed64d0bbcb811";
    var appSecret = "8lLzxZkd5oeT93pSMmf5UbvTclvSoLFg";
    TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    long millis = (long)ts.TotalMilliseconds;

    string curtime = Convert.ToString(millis / 1000);
    string salt = DateTime.Now.Millisecond.ToString();

    string signStr = appKey + Truncate(text) + salt + curtime + appSecret;
    string sign = ComputeHash(signStr, SHA256.Create());
    _postBody = new Dictionary<string, string>
    {
      { "appKey", appKey },
      { "salt", salt },
      { "sign", sign },
      { "signType", "v3" },
      { "curtime", curtime },
    };
  }

  public async Task<string> Translate(string text, string sourceLanguage, string targetLanguage)
  {
    if (sourceLanguage == "zh") sourceLanguage = "zh-CHS";
    if (sourceLanguage == "zh-TW") sourceLanguage = "zh-CHT";    
    if (targetLanguage == "zh") sourceLanguage = "zh-CHS";
    if (targetLanguage == "zh-TW") sourceLanguage = "zh-CHT";
    Initialize(text);
    _postBody.Add("from", sourceLanguage);
    _postBody.Add("to", "zh-CHS");
    _postBody.Add("q", System.Web.HttpUtility.UrlEncode(text));
    string resp = await Post();
    JObject jObject = JObject.Parse(resp);
    YoudaoResponse youdaoResponse = new();
    youdaoResponse.Translation = jObject["translation"][0].ToString();
    youdaoResponse.ErrorCode = jObject["errorCode"].ToString();
    return youdaoResponse.Translation;
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

  private static bool SaveBinaryFile(WebResponse response, string FileName)
  {
    string filePath = FileName + DateTime.Now.Millisecond + ".mp3";
    bool value = true;
    byte[] buffer = new byte[1024];

    try
    {
      if (File.Exists(filePath))
        File.Delete(filePath);
      Stream outStream = File.Create(filePath);
      Stream inStream = response.GetResponseStream();

      int l;
      do
      {
        l = inStream.Read(buffer, 0, buffer.Length);
        if (l > 0)
          outStream.Write(buffer, 0, l);
      } while (l > 0);

      outStream.Close();
      inStream.Close();
    }
    catch
    {
      value = false;
    }

    return value;
  }
}