using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SubtitleTranslator.Models;
using System.Collections.Specialized;
using System.Linq;

namespace SubtitleTranslator.ViewModels;

public partial class FileListViewModel :
  ObservableRecipient,
  IRecipient<ValueChangedMessage<ToBeTranslatedItem>>,
  IRecipient<ValueChangedMessage<string>>
{
  public ObservableCollection<ToBeTranslatedItem> ToBeTranslatedItems { get; } = new();
  public FileListViewModel()
  {
    ToBeTranslatedItems.CollectionChanged += ToBeTranslatedItems_CollectionChanged;
    
  }

  private void ToBeTranslatedItems_CollectionChanged(object? sender,
    NotifyCollectionChangedEventArgs e)
  {
    var toBeTranslatedPaths = new List<string>();
    
    foreach (var item in sender as ObservableCollection<ToBeTranslatedItem>)
    {
      if (item is ToBeTranslatedItem toBeTranslatedItem)
      {
        toBeTranslatedPaths.Add(toBeTranslatedItem.FilePath);
        
      }
    }
    WeakReferenceMessenger.Default.Send(
      new ValueChangedMessage<List<string>>(toBeTranslatedPaths)
    );
  }

  [RelayCommand]
  private void DeleteItem(ToBeTranslatedItem currentSelectedItem)
  {
    ToBeTranslatedItems.Remove(currentSelectedItem);
    Console.WriteLine(ToBeTranslatedItems.Count);
  }

  public void Receive(ValueChangedMessage<ToBeTranslatedItem> message)
  {
    ToBeTranslatedItems.Add(message.Value);
  }

  public void Receive(ValueChangedMessage<string> message)
  {
    var translatedItem = ToBeTranslatedItems.FirstOrDefault(item => item.FilePath == message.Value);
    ToBeTranslatedItems.Remove(translatedItem!);
  }
}
