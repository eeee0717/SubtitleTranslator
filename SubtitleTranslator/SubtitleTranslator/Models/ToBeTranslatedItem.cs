using System;

namespace SubtitleTranslator.Models;

public record ToBeTranslatedItem(string State, string FileName, string Description, string FileContent);
