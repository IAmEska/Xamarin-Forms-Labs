using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using MonoTouch.AddressBook;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(XLabs.Contacts.iOS.Contact))]
namespace XLabs.Contacts.iOS
{
    public class Contact : IContact
    {
        #region Platform specific

        internal List<InstantMessagingAccount> instantMessagingAccounts = new List<InstantMessagingAccount>();

        public IEnumerable<InstantMessagingAccount> InstantMessagingAccounts
        {
            get
            {
                return instantMessagingAccounts;
            }
            set
            {
                instantMessagingAccounts = new List<InstantMessagingAccount>(value);
            }
        }

        #endregion

        public Contact()
        {
        }

        internal Contact(ABPerson person)
        {
            Id = person.Id.ToString();
            this.person = person;
        }

        #region IContact implementation

        public async Task<ImageSource> GetThumbnailAsync()
        {
            return await GetPhotoAsync(ABPersonImageFormat.Thumbnail);
        }

        public async Task<ImageSource> GetPhotoAsync()
        {
            return await GetPhotoAsync(ABPersonImageFormat.OriginalSize);
        }

        private async Task<ImageSource> GetPhotoAsync(ABPersonImageFormat imageFormat)
        {
            return await Task<ImageSource>.Run(() =>
                {
                    if (!this.person.HasImage)
                        return null;

                    IntPtr data;
                    lock (this.person)
                    {
                        data = ABPersonCopyImageDataWithFormat(person.Handle, imageFormat);
                    }

                    if (data == IntPtr.Zero)
                        return null;

                    return ImageSource.FromStream(() =>
                        {
                            return new NSData(data).AsStream();
                        });
                });
        }

        public string Id
        {
            get;
            set;
        }

        public bool IsAggregate
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public string Prefix
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string NickName
        {
            get;
            set;
        }

        public string Suffix
        {
            get;
            set;
        }

        internal List<Website> websites;

        public IEnumerable<Website> Websites
        {
            get;
            set;
        }

        internal List<Organization> organizations;

        public IEnumerable<Organization> Organizations
        {
            get;
            set;
        }

        internal List<Note> notes;

        public IEnumerable<Note> Notes
        {
            get;
            set;
        }

        internal List<Email> emails;

        public IEnumerable<Email> Emails
        {
            get;
            set;
        }

        internal List<Phone> phones;

        public IEnumerable<Phone> Phones
        {
            get;
            set;
        }

        internal List<Address> addresses;

        public IEnumerable<Address> Addresses
        {
            get;
            set;
        }

        internal List<Relationship> relationships;

        public IEnumerable<Relationship> Relationships
        {
            get;
            set;
        }

        #endregion

        private readonly ABPerson person;

        [DllImport("/System/Library/Frameworks/AddressBook.framework/AddressBook")]
        private static extern IntPtr ABPersonCopyImageDataWithFormat(IntPtr handle, ABPersonImageFormat format);
    }
}

