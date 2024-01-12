using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using SubtitleTranslator.Models;

namespace SubtitleTranslator.ViewModels;

public partial class FileListViewModel:
  ObservableRecipient,
  IRecipient<ValueChangedMessage<ToBeTranslatedItem>>
{
  public ObservableCollection<ToBeTranslatedItem> ToBeTranslatedItems { get;} = new();
  
  public void Receive(ValueChangedMessage<ToBeTranslatedItem> message)
  {
    ToBeTranslatedItems.Add(message.Value);
  }

  // [RelayCommand]
  // private void ButtonClicked()
  // {
  //   Console.WriteLine(ToBeTranslatedItems.Count);
  // }
}