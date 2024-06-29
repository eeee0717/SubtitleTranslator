using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class FileUploadViewModel : ObservableRecipient
{
  [RelayCommand]
  private async Task OpenFile(CancellationToken token)
  {
    try
    {
      var files = await DoOpenFilePickerAsync();
      var storageFiles = files as IStorageFile[] ?? files.ToArray();
      if (!storageFiles.Any())
        return;
      
      foreach (var file in storageFiles)
      {
        var filePath = file!.TryGetLocalPath();
        var fileSize = new FileInfo(filePath!).Length;
        var toBeTranslatedItem = new ToBeTranslatedItem(false, file!.Name, fileSize.ToString(), filePath!);
        WeakReferenceMessenger.Default.Send(
          new ValueChangedMessage<ToBeTranslatedItem>(toBeTranslatedItem)
        );
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  private async Task<IEnumerable<IStorageFile?>> DoOpenFilePickerAsync()
  {
    if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
        desktop.MainWindow?.StorageProvider is not { } provider)
      throw new NullReferenceException("Missing StorageProvider instance.");

    var files = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
    {
      Title = "选择字幕文件",
      FileTypeFilter = new[] { SubtitleFileAll },
      AllowMultiple = true
    });
    return files;
  }

  private static FilePickerFileType SubtitleFileAll { get; } = new("All Subtitle Files")
  {
    Patterns = new[] { "*.srt", "*.ass", "*.*" },
  };
}