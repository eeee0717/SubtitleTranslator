using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using SubtitleTranslator.Helpers;

namespace SubtitleTranslator.Views;

public partial class TencentProviderView : UserControl
{
  public TencentProviderView()
  {
    InitializeComponent();
  }


  private async void SecretIdTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
  {
    TextBox textBox = (TextBox)sender!;
    if(textBox.Text!.Length == 0) return;
    ConfigFileHelper configFileHelper = new();
    configFileHelper.ProviderOptions!.TencentProviderOptions.SecretId = textBox.Text;
    await configFileHelper.Save();
  }

  private async void SecretKeyTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
  {
    TextBox textBox = (TextBox)sender!;
    if(textBox.Text!.Length == 0) return;
    ConfigFileHelper configFileHelper = new();
    configFileHelper.ProviderOptions!.TencentProviderOptions.SecretKey = textBox.Text;
    await configFileHelper.Save();
  }
}