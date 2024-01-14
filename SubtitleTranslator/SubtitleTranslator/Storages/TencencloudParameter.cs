namespace SubtitleTranslator.Storages;

public record TencencloudParameter(
  string SecretId,
  string SecretKey,
  string Token,
  string Service,
  string Version,
  string Action,
  string Body,
  string Region
  );