using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SubtitleTranslator.Helpers;

namespace SubtitleTranslator.ViewModels;

public partial class TranslatorControlViewModel : ObservableRecipient,
  IRecipient<ValueChangedMessage<List<string>>>
{
  [ObservableProperty] private ObservableCollection<string> _translationSourceList = null!;
  [ObservableProperty] private string _selectedTranslationSource = "腾讯云";
  private Dictionary<string, ITranslator>? _translatorMap;
  private List<string> ToBeTranslatedPaths { get; set; } = new();


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
      var fileWriter = new SubtitleFileWriter();
      await fileWriter.WriteFile(toBeTranslatedPath, translatedResult);
      
      WeakReferenceMessenger.Default.Send(
        new ValueChangedMessage<string>(toBeTranslatedPath)
      );
    }
    var completedMessageBox = MessageBoxManager
      .GetMessageBoxStandard("翻译结果", "翻译完成", ButtonEnum.Ok, Icon.Info);
    await completedMessageBox.ShowAsync();
  }

  private static async Task<string> TranslateFile(ITranslator currentTranslator, string toBeTranslatedPath)
  {
    var translatedContents = "";
    var srtContentSplitHelper = new SrtContentSplitHelper();
    var (originalContentList, toBeTranslatedList) =
      await srtContentSplitHelper.SplitContent(toBeTranslatedPath);
    foreach (var toBeTranslatedContent in toBeTranslatedList)
    {
      var apiCount = 0;
      var translatedContent =
        await currentTranslator.Translate(toBeTranslatedContent, "en", "zh");
      apiCount++;
      if(apiCount == 4)
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
