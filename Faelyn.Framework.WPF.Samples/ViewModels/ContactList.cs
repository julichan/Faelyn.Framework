using System.Collections.ObjectModel;
using Faelyn.Framework.Components;

namespace Faelyn.Framework.WPF.Samples.ViewModels
{
    public class ContactList : NotifyPropertyChanges
    {
        private ObservableCollection<ContactElement> _contacts;
        public ObservableCollection<ContactElement> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }
        
        public ContactList()
        {
            Contacts = new ObservableCollection<ContactElement>();
            Contacts.Add(new ContactElement() 
            { 
                Username = "Mike", 
                FullName = "Mickey Mouse", 
                Email = "mmouse@disney.com"
                
            });
            
            Contacts.Add(new ContactElement() 
            { 
                Username = "The queen of Arendelle", 
                FullName = "Elsa Árnadalr", 
                Email = "elsa@disney.com"
                
            });
            
            Contacts.Add(new ContactElement() 
            { 
                Username = "The snow man", 
                FullName = "Olaf", 
                Email = "olaf@disney.com"
            });
            
        }
    }
}