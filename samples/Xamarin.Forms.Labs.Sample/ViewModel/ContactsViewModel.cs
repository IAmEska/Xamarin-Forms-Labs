using System;
using Xamarin.Forms.Labs.Mvvm;
using Xamarin.Forms.Labs.Sample.Pages.Services;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xamarin.Forms.Labs.Sample
{
    [ViewType(typeof(ContactsPage))]
    public class ContactsViewModel : Xamarin.Forms.Labs.Mvvm.ViewModel
    {
        public ContactsViewModel()
        {
            LoadData();
        }

        private async void LoadData()
        {
            contacts = new ObservableCollection<ContactListModel>();
            var book = DependencyService.Get<XLabs.Contacts.IAddressBook>();
            await book.RequestPermission();
            foreach (XLabs.Contacts.IContact contact in book.OrderBy(x => x.LastName))
            {
                XLabs.Contacts.Phone phone = contact.Phones.FirstOrDefault();
                XLabs.Contacts.Email email = contact.Emails.FirstOrDefault();
                string number = phone != null ? phone.Number : String.Empty;
                string emailAddress = email != null ? email.Address : String.Empty;
                contacts.Add(new ContactListModel(){ DisplayName = contact.DisplayName, Number = number, Email = emailAddress, Thumbnail = await contact.GetThumbnailAsync() });
            }
        }

        private ObservableCollection<ContactListModel> contacts;

        public ObservableCollection<ContactListModel> Contacts
        {
            get { return contacts; }
            set
            {
                SetProperty(ref contacts, value, "Contacts");
            }
        }

        public class ContactListModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            private string displayName;
            private string number;
            private string email;
            private ImageSource thumbnail;

            public string DisplayName
            {
                get
                {
                    return displayName;
                }
                set
                {
                    if (displayName != value)
                    {
                        displayName = value;
                        OnPropertyChanged("DisplayName");
                    }
                }
            }

            public string ContactInfo
            {
                get
                {
                    return string.Format("{0} {1}", Number, Email);
                }
            }

            public string Email
            {
                get
                {
                    return email;
                }
                set
                {
                    if (email != value)
                    {
                        email = value;
                        OnPropertyChanged("Email");
                    }
                }
            }

            public string Number
            {
                get
                {
                    return number;
                }
                set
                {
                    if (number != value)
                    {
                        number = value;
                        OnPropertyChanged("Number");
                    }
                }
            }

            public ImageSource Thumbnail
            {
                get
                {
                    return thumbnail;
                }
                set
                {
                    if (thumbnail != value)
                    {
                        thumbnail = value;
                        OnPropertyChanged("Thumbnail");
                    }
                }
            }
        }
    }
}

