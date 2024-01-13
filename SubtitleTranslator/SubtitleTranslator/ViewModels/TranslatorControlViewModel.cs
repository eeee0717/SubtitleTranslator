using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class TranslatorControlViewModel: ObservableObject
{
  [ObservableProperty] private ObservableCollection<string> _translationSourceList;
  [ObservableProperty] private string _selectedTranslationSource;
  private Dictionary<string, ITranslator> _translatorMap;
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
  private void TranslateClicked()
  {
    ITranslator currentTranslator = this._translatorMap[SelectedTranslationSource];
    string s = currentTranslator.Translate("Hello World", "en", "zh");
    Console.WriteLine(s);
  }
}