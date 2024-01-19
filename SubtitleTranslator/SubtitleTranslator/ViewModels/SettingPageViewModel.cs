using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace SubtitleTranslator.ViewModels;

public partial class SettingPageViewModel: ViewModelBase
{
  [ObservableProperty] private ViewModelBase _currentPage = new BaseProviderViewModel();
  [ObservableProperty] private ProviderListTemplate? _selectedListProviderItem;


  partial void OnSelectedListProviderItemChanged(ProviderListTemplate? value)
  {
    if (value is null) return;

    var instance = Design.IsDesignMode
      ? Activator.CreateInstance(value.ModelType)
      : Ioc.Default.GetService(value.ModelType);

    if (instance is null) return;
    CurrentPage = (ViewModelBase)instance;
  }
  public ObservableCollection<ProviderListTemplate> Providers { get; } = new()
  {
    new ProviderListTemplate(typeof(BaseProviderViewModel), "翻译服务商"),
    new ProviderListTemplate(typeof(TencentProviderViewModel), "腾讯云"),
    new ProviderListTemplate(typeof(YoudaoProviderViewModel), "有道智云"),
  };
  
  

}
public class ProviderListTemplate
{
  public ProviderListTemplate(Type type,  string label)
  {
    ModelType = type;
    Label = label;
  }

  public string Label { get; }
  public Type ModelType { get; }
}

