using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class TranslatorControlViewModel : ObservableRecipient,
  IRecipient<ValueChangedMessage<Uri>>
{
  [ObservableProperty] private ObservableCollection<string> _translationSourceList = null!;
  [ObservableProperty] private string _selectedTranslationSource = "腾讯云";
  private Dictionary<string, ITranslator>? _translatorMap;
  public ObservableCollection<Uri> ToBeTranslatedPaths { get; } = new();


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
    string s = await currentTranslator.Translate("Hello World", "en", "zh");
    Console.WriteLine(s);
  }
  
  public void Receive(ValueChangedMessage<Uri> message)
  {
    ToBeTranslatedPaths.Add(message.Value);
  }
}