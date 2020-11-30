using Faelyn.Framework.Components;

namespace Faelyn.Framework.WPF.Samples.ViewModels
{
    public class ContactElement : NotifyPropertyChanges
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value); 
        }
        
        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }
        
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
    }
}