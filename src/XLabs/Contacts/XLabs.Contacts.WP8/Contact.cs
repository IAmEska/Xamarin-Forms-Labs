using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Contacts;
using Xamarin.Forms;
using System.IO;
using Windows.Storage;

[assembly: Dependency(typeof(XLabs.Contacts.WP.Contact))]
namespace XLabs.Contacts.WP
{
    public class Contact : IContact
    {
        private readonly Microsoft.Phone.UserData.Contact contact;
        public Contact() { }

        internal Contact (Microsoft.Phone.UserData.Contact contact)
        {
            this.contact = contact;
        }

        public string Id { get; set; }
        public bool IsAggregate { get; set; }
        public string DisplayName { get; set; }
        public string Prefix { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Suffix { get; set; }

        internal List<Website> websites = new List<Website>();

        public IEnumerable<Website> Websites
        {
            get
            {
                return websites;
            }
            set
            {
                websites = new List<Website>(value);
            }
        }

        internal List<Organization> organizations = new List<Organization>();

        public IEnumerable<Organization> Organizations
        {
            get
            {
                return Organizations;
            }
            set
            {
                organizations = new List<Organization>(value);
            }
        }

        internal List<Note> notes = new List<Note>();

        public IEnumerable<Note> Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = new List<Note>(value);
            }
        }

        internal List<Email> emails = new List<Email>();

        public IEnumerable<Email> Emails
        {
            get
            {
                return emails;
            }
            set
            {
                emails = new List<Email>(value);
            }
        }

        internal List<Phone> phones = new List<Phone>();

        public IEnumerable<Phone> Phones
        {
            get
            {
                return phones;
            }
            set
            {
                phones = new List<Phone>(value);
            }
        }

        internal List<Address> addresses = new List<Address>();

        public IEnumerable<Address> Addresses
        {
            get
            {
                return addresses;
            }
            set
            {
                addresses = new List<Address>(value);
            }
        }

        public async Task<ImageSource> GetThumbnailAsync()
        {
            return await Task<ImageSource>.Run(() =>
                {
                    ImageSource image = ImageSource.FromStream(() =>
                    {
                        lock (contact)
                        {
                            return this.contact.GetPicture();
                        }
                    });
                   return image;
                });
        }

        internal List<Relationship> relationships = new List<Relationship>();

        public IEnumerable<Relationship> Relationships
        {
            get
            {
                return relationships;
            }
            set
            {
                relationships = new List<Relationship>(value);
            }
        }

        //TODO finish GetPhotoAsync
        public async Task<ImageSource> GetPhotoAsync()
        {
            return await GetThumbnailAsync();
        }

    }
}
