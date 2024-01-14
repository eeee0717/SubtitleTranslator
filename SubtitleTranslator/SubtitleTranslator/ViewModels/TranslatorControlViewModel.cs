using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class TranslatorControlViewModel : ObservableRecipient,
  IRecipient<ValueChangedMessage<string>>
{
  [ObservableProperty] private ObservableCollection<string> _translationSourceList = null!;
  [ObservableProperty] private string _selectedTranslationSource = "腾讯云";
  private Dictionary<string, ITranslator>? _translatorMap;
  private ObservableCollection<string> ToBeTranslatedPaths { get; } = new();


  public TranslatorControlViewModel()
  {
    this.InitializeTranslation();
  }

  private void InitializeTranslation()
  {
    this._translatorMap = new Dictionary<string, ITranslator>
    {
      { "腾讯云", new TencentcloudTranslator() },
    };
    this.TranslationSourceList = new ObservableCollection<string>(this._translatorMap.Keys);
  }

  [RelayCommand]
  private async Task TranslateClicked()
  {
    ITranslator currentTranslator = this._translatorMap![SelectedTranslationSource];
    foreach (var toBeTranslatedPath in ToBeTranslatedPaths)
    {
      var translatedResult = await TranslateFile(currentTranslator, toBeTranslatedPath);
      var fileWriter = new FileWriter();
      await fileWriter.WriteFile(toBeTranslatedPath, translatedResult);
    }
  }

  private async Task<string> TranslateFile(ITranslator currentTranslator, string toBeTranslatedPath)
  {
    var numberPattern = new Regex(@"^\d+$");
    var timelinePattern = new Regex(@"^\d{2}:\d{2}:\d{2},\d{3} --> \d{2}:\d{2}:\d{2},\d{3}$");
    using StreamReader streamReader = new StreamReader(toBeTranslatedPath, Encoding.UTF8);
    string? line;
    string translatedResult="";
    while ((line = await streamReader.ReadLineAsync()) != null)
    {
      if (numberPattern.IsMatch(line) || timelinePattern.IsMatch(line)||string.IsNullOrWhiteSpace(line))
      {
        translatedResult += line + "\r\n";
        continue;
      }

      // This line is a subtitle, translate it
      string translatedLine =  await currentTranslator.Translate(line, "en", "zh");
      translatedResult += translatedLine + "\r\n";
      Console.WriteLine(translatedResult);
    }

    return translatedResult;
  }


  public void Receive(ValueChangedMessage<string> message)
  {
    ToBeTranslatedPaths.Add(message.Value);
  }
}