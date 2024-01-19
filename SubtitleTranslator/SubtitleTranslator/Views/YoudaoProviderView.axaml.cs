using Avalonia.Controls;
using Avalonia.Interactivity;
using SubtitleTranslator.Helpers;

namespace SubtitleTranslator.Views;

public partial class YoudaoProviderView : UserControl
{
  public YoudaoProviderView()
  {
    InitializeComponent();
  }

  private async void SecretIdTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
  {
    TextBox textBox = (TextBox)sender!;
    if (textBox.Text!.Length == 0) return;
    ConfigFileHelper configFileHelper = new();
    configFileHelper.ProviderOptions!.YoudaoProviderOptions.AppId = textBox.Text;
    await configFileHelper.Save();
  }

  private async void SecretKeyTextBox_OnLostFocus(object? sender, RoutedEventArgs e)
  {
    TextBox textBox = (TextBox)sender!;
    if (textBox.Text!.Length == 0) return;
    ConfigFileHelper configFileHelper = new();
    configFileHelper.ProviderOptions!.YoudaoProviderOptions.AppKey = textBox.Text;
    await configFileHelper.Save();
  }
}