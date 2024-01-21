using System;

namespace SubtitleTranslator.Exceptions;

public class ApiNotFoundException: ApplicationException
{
  public ApiNotFoundException(string message) : base(message)
  {
  }
}