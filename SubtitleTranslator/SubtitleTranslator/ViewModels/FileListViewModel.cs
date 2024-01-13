using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class FileListViewModel :
  ObservableRecipient,
  IRecipient<ValueChangedMessage<ToBeTranslatedItem>>
{
  public ObservableCollection<ToBeTranslatedItem> ToBeTranslatedItems { get; } = new();


  public FileListViewModel()
  {
    ToBeTranslatedItems.CollectionChanged += ToBeTranslatedItems_CollectionChanged;
  }
  private static void ToBeTranslatedItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
  {
    if (e.NewItems is null)
      return;
    foreach (var item in e.NewItems)
    {
      if (item is ToBeTranslatedItem toBeTranslatedItem)
      {
        WeakReferenceMessenger.Default.Send(
          new ValueChangedMessage<Uri>(toBeTranslatedItem.Path)
        );
      }
    }
  }

  public void Receive(ValueChangedMessage<ToBeTranslatedItem> message)
  {
    ToBeTranslatedItems.Add(message.Value);
  }
}