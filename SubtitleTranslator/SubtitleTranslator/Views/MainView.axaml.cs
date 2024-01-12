using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace SubtitleTranslator.Views;

public partial class MainView : UserControl
{
  public MainView()
  {
    InitializeComponent();
  }
  //
  // private async void Button_OnClick(object? sender, RoutedEventArgs e)
  // {
  //   // 从当前控件获取 TopLevel。或者，您也可以使用 Window 引用。
  //   var topLevel = TopLevel.GetTopLevel(this);
  //
  //   // 启动异步操作以打开对话框。
  //   var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
  //   {
  //     Title = "Open Text File",
  //     AllowMultiple = true
  //   });
  //
  //   if (files.Count >= 1)
  //   {
  //     // 打开第一个文件的读取流。
  //     await using var stream = await files[0].OpenReadAsync();
  //     using var streamReader = new StreamReader(stream);
  //     // 将文件的所有内容作为文本读取。
  //     var fileContent = await streamReader.ReadToEndAsync();
  //   }
  // }
}