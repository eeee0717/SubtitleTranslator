using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SubtitleTranslator.Helpers;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class TranslatorControlViewModel : ObservableRecipient,
  IRecipient<ValueChangedMessage<List<string>>>
{
  private static readonly TranslationLanguage TranslationLanguage = new();
  private static readonly TranslationProvider TranslationProvider = new();
  [ObservableProperty] private IEnumerable<string>? _translationSourceList = TranslationProvider.TranslatorMap.Keys;

  [ObservableProperty] private string _selectedTranslationSource = TranslationProvider.TranslatorMap.Keys.First();

  [ObservableProperty] private IEnumerable<string> _sourceLanguageList = TranslationLanguage.SourceLanguageList.Keys;

  [ObservableProperty] private string _selectedSourceLanguage = TranslationLanguage.SourceLanguageList.Keys.First();

  [ObservableProperty] private IEnumerable<string> _targetLanguageList = TranslationLanguage.TargetLanguageList.Keys;

  [ObservableProperty] private string _selectedTargetLanguage = TranslationLanguage.TargetLanguageList.Keys.First();

  private readonly Dictionary<string, ITranslator>? _translatorMap = TranslationProvider.TranslatorMap;
  private List<string> ToBeTranslatedPaths { get; set; } = new();


  [RelayCommand]
  private async Task TranslateClicked()
  {
    ITranslator currentTranslator = this._translatorMap![SelectedTranslationSource];
    foreach (var toBeTranslatedPath in ToBeTranslatedPaths)
    {
      var translatedResult = await TranslateFile(currentTranslator, toBeTranslatedPath);
      var fileWriter = new SubtitleFileWriter();
      await fileWriter.WriteFile(toBeTranslatedPath, translatedResult, TranslationLanguage.TargetLanguageList[SelectedTargetLanguage]);

      WeakReferenceMessenger.Default.Send(
        new ValueChangedMessage<string>(toBeTranslatedPath)
      );
    }

    var completedMessageBox = MessageBoxManager
      .GetMessageBoxStandard("翻译结果", "翻译完成", ButtonEnum.Ok, Icon.Info);
    await completedMessageBox.ShowAsync();
  }

  private async Task<string?> TranslateFile(ITranslator currentTranslator, string toBeTranslatedPath)
  {
    var translatedContents = "";
    var srtContentSplitHelper = new SrtContentSplitHelper();
    var (originalContentList, toBeTranslatedList) =
      await srtContentSplitHelper.SplitContent(toBeTranslatedPath);
    foreach (var toBeTranslatedContent in toBeTranslatedList)
    {
      var apiCount = 0;
      var translatedContent =
        await currentTranslator.Translate(toBeTranslatedContent,
          TranslationLanguage.SourceLanguageList[SelectedSourceLanguage],
          TranslationLanguage.TargetLanguageList[SelectedTargetLanguage]);
      apiCount++;
      if (apiCount == 4)
        await Task.Delay(1000);
      translatedContents += translatedContent;
    }

    return srtContentSplitHelper.CombineContent(originalContentList, translatedContents);
  }


  public void Receive(ValueChangedMessage<List<string>> message)
  {
    ToBeTranslatedPaths = message.Value;
  }
}