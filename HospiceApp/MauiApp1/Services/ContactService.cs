using MauiApp1.Models;
using System.Collections.ObjectModel;

namespace MauiApp1.Services
{
    public class ContactService
    {
        public ObservableCollection<ContactItem> ContactItemList { get; set; }

        public void SetContactList(ObservableCollection<ContactItem> contactItemList)
        {
            ContactItemList = contactItemList;
        }


    }
}
